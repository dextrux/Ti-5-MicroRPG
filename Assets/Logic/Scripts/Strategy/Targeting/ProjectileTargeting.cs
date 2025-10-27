using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Nara;
using System;
using UnityEngine;

public enum AimingMode {
    StraightAim,
    ParabolicArc
}

[Serializable]
public class ProjectileTargeting : TargetingStrategy {
    public ProjectileController ProjectilePrefab;
    [HideInInspector] public Transform hitMarker;
    public int maxPoints = 50;
    public float increment = 0.025f;
    public float rayOverlap = 1.1f;
    private LineRenderer trajectoryLine;

    [Header("Aiming Logic")]
    public AimingMode aimingMode = AimingMode.StraightAim;
    public LayerMask GroundLayerMask;

    [Header("Parabolic Arc Settings")]
    [Tooltip("A altura máxima que o arco atingirá na distância máxima.")]
    public float parabolicMaxHeight = 10f;
    [Tooltip("A distância máxima para calcular a escala da altura do arco.")]
    public float parabolicMaxRange = 50f;

    private float currentLaunchSpeed;


    public override void Initialize(AbilityData data, IEffectable caster) {
        base.Initialize(data, caster);
        if (ProjectilePrefab != null) {
            if (Caster is NaraController) {
                NaraController controller = (NaraController)Caster;
                trajectoryLine = controller.GetPointLineRenderer();
            }
            hitMarker = UnityEngine.Object.Instantiate(Caster.GetReferenceTargetPrefab()).transform;
            SetTrajectoryVisible(true);

            currentLaunchSpeed = ProjectilePrefab.InitialSpeed;

            SubscriptionService.RegisterUpdatable(this);
        }
    }

    public override void ManagedUpdate() {
        base.ManagedUpdate();

        UpdateNaraViewRotation();

        PredictTrajectory();
    }

    private void UpdateNaraViewRotation() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, float.MaxValue, GroundLayerMask)) {
            switch (aimingMode) {
                case AimingMode.StraightAim:
                    AimStraight(hit.point);
                    break;

                case AimingMode.ParabolicArc:
                    AimParabolic(hit.point);
                    break;
            }
        }
    }

    private void AimStraight(Vector3 targetPoint) {
        Vector3 startPos = Caster.GetTransformCastPoint().position;
        Vector3 direction = targetPoint - startPos;

        Caster.GetTransformCastPoint().rotation = Quaternion.LookRotation(direction);

        currentLaunchSpeed = ProjectilePrefab.InitialSpeed;
    }

    private void AimParabolic(Vector3 targetPoint) {
        Vector3 startPos = Caster.GetTransformCastPoint().position;
        float g = Physics.gravity.y * -1;

        Vector3 deltaXZ_vec = new Vector3(targetPoint.x - startPos.x, 0, targetPoint.z - startPos.z);
        float deltaX = deltaXZ_vec.magnitude;
        float deltaY = targetPoint.y - startPos.y;

        float distanceRatio = Mathf.Clamp01(deltaX / parabolicMaxRange);
        float h = parabolicMaxHeight * distanceRatio;
        if (h < 0.1f) h = 0.1f;

        float Vy = Mathf.Sqrt(2 * g * h);

        float a = 0.5f * g;
        float b = -Vy;
        float c = deltaY;

        float discriminant = (b * b) - (4 * a * c);

        if (discriminant < 0) {
            AimStraight(targetPoint);
            return;
        }

        float t_total = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
        if (t_total <= 0) {
            AimStraight(targetPoint);
            return;
        }

        float Vx = deltaX / t_total;

        Vector3 launchVelocity = (deltaXZ_vec.normalized * Vx) + (Vector3.up * Vy);

        Caster.GetTransformCastPoint().rotation = Quaternion.LookRotation(launchVelocity.normalized);

        currentLaunchSpeed = launchVelocity.magnitude;
    }


    public override void LockAim(out IEffectable[] targets) {
        base.LockAim(out targets);
        Rigidbody thrownObject = UnityEngine.Object.Instantiate(ProjectilePrefab, Caster.GetTransformCastPoint().position, Quaternion.identity).GetComponent<Rigidbody>();
        if (thrownObject.TryGetComponent<ProjectileController>(out ProjectileController controller)){
            controller.Initialize(Caster.GetTransformCastPoint(), Caster, Ability);
        }
        thrownObject.AddForce(Caster.GetTransformCastPoint().forward * currentLaunchSpeed, ForceMode.Impulse);
    }

    public override void Cancel() {
        SetTrajectoryVisible(false);
        UnityEngine.Object.Destroy(hitMarker.gameObject);
        base.Cancel();
    }

    #region TrajectoryPrediction_Helpers
    private void UpdateLineRender(int count, (int point, Vector3 pos) pointPos) {
        if (trajectoryLine == null) return;
        trajectoryLine.positionCount = count;
        trajectoryLine.SetPosition(pointPos.point, pointPos.pos);
    }

    private Vector3 CalculateNewVelocity(Vector3 velocity, float drag, float increment) {
        velocity += Physics.gravity * increment;
        velocity *= Mathf.Clamp01(1f - drag * increment);
        return velocity;
    }

    private void MoveHitMarker(RaycastHit hit) {
        if (hitMarker == null) return;
        hitMarker.gameObject.SetActive(true);

        float offset = 0.025f;
        hitMarker.position = hit.point + hit.normal * offset;
        hitMarker.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
    }

    public void SetTrajectoryVisible(bool visible) {
        if (trajectoryLine != null)
            trajectoryLine.enabled = visible;
        if (hitMarker != null)
            hitMarker.gameObject.SetActive(visible);
    }
    #endregion

    public void PredictTrajectory() {
        Vector3 velocity = Caster.GetTransformCastPoint().forward * (currentLaunchSpeed / ProjectilePrefab.GetRigidbody.mass);
        Vector3 position = Caster.GetTransformCastPoint().position;
        Vector3 nextPosition;
        float overlap;

        UpdateLineRender(maxPoints, (0, position));

        for (int i = 1; i < maxPoints; i++) {
            velocity = CalculateNewVelocity(velocity, ProjectilePrefab.GetRigidbody.linearDamping, increment);
            nextPosition = position + velocity * increment;

            overlap = Vector3.Distance(position, nextPosition) * rayOverlap;

            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, overlap)) {
                UpdateLineRender(i, (i - 1, hit.point));
                MoveHitMarker(hit);
                break;
            }

            if (hitMarker != null)
                hitMarker.gameObject.SetActive(false);

            position = nextPosition;
            UpdateLineRender(maxPoints, (i, position));
        }
    }
}