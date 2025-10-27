using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Nara;
using System;
using UnityEngine;

[Serializable]
public class ProjectileTargeting : TargetingStrategy {
    public ProjectileController ProjectilePrefab;
    [HideInInspector] public Transform hitMarker;
    public int maxPoints = 50;
    public float increment = 0.025f;
    public float rayOverlap = 1.1f;
    private LineRenderer trajectoryLine;
    public override void Initialize(AbilityData data, IEffectable caster) {
        base.Initialize(data, caster);
        if (ProjectilePrefab != null) {
            if (Caster is NaraController) {
                NaraController controller = (NaraController)Caster;
                trajectoryLine = controller.GetPointLineRenderer();
            }
            hitMarker = UnityEngine.Object.Instantiate(Caster.GetReferenceTargetPrefab()).transform;
            SetTrajectoryVisible(true);

            targetDirection = Caster.GetTransformCastPoint().localRotation.eulerAngles;

            SubscriptionService.RegisterUpdatable(this);
        }
    }

    public override void ManagedUpdate() {
        base.ManagedUpdate();
        UpdateNaraViewRotation();
        PredictTrajectory();
    }

    private void UpdateNaraViewRotation() {
        if (lockCursor)
            Cursor.lockState = CursorLockMode.Locked;

        mouseFinal += ScaleAndSmooth(Input.mousePositionDelta);

        ClampValues();
        AlignToBody();
    }

    public override void LockAim(out IEffectable[] targets) {
        base.LockAim(out targets);
        Rigidbody thrownObject = UnityEngine.Object.Instantiate(ProjectilePrefab, Caster.GetTransformCastPoint().position, Quaternion.identity).GetComponent<Rigidbody>();
        thrownObject.AddForce(Caster.GetTransformCastPoint().forward * ProjectilePrefab.InitialSpeed, ForceMode.Impulse);
    }

    public override void Cancel() {
        SetTrajectoryVisible(false);
        UnityEngine.Object.Destroy(hitMarker.gameObject);
        base.Cancel();
    }

    private void UpdateLineRender(int count, (int point, Vector3 pos) pointPos) {
        trajectoryLine.positionCount = count;
        trajectoryLine.SetPosition(pointPos.point, pointPos.pos);
    }

    private Vector3 CalculateNewVelocity(Vector3 velocity, float drag, float increment) {
        velocity += Physics.gravity * increment;
        velocity *= Mathf.Clamp01(1f - drag * increment);
        return velocity;
    }

    private void MoveHitMarker(RaycastHit hit) {
        hitMarker.gameObject.SetActive(true);

        float offset = 0.025f;
        hitMarker.position = hit.point + hit.normal * offset;
        Debug.Log("Moveu o hit: " + hitMarker.position);
        hitMarker.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
    }

    public void SetTrajectoryVisible(bool visible) {
        trajectoryLine.enabled = visible;
        hitMarker.gameObject.SetActive(visible);
    }

    public void PredictTrajectory() {
        Vector3 velocity = Caster.GetTransformCastPoint().forward * (ProjectilePrefab.InitialSpeed / ProjectilePrefab.GetRigidbody.mass);
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

            hitMarker.gameObject.SetActive(false);
            position = nextPosition;
            UpdateLineRender(maxPoints, (i, position));
        }
    }


    Vector2 mouseFinal;
    Vector2 smoothMouse;
    Vector2 targetDirection;
    Vector2 targetCharacterDirection;

    public Vector2 clampInDegrees = new Vector2(360f, 180f);
    public Vector2 sensitivity = new Vector2(0.1f, 0.1f);
    public Vector2 smoothing = new Vector2(1f, 1f);

    public bool lockCursor;

    Vector2 ScaleAndSmooth(Vector2 delta) {
        //Apply sensetivity
        delta = Vector2.Scale(delta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        //Lerp from last frame
        smoothMouse.x = Mathf.Lerp(smoothMouse.x, delta.x, 1f / smoothing.x);
        smoothMouse.y = Mathf.Lerp(smoothMouse.y, delta.y, 1f / smoothing.y);

        return smoothMouse;
    }

    void ClampValues() {
        if (clampInDegrees.x < 360)
            mouseFinal.x = Mathf.Clamp(mouseFinal.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        if (clampInDegrees.y < 360)
            mouseFinal.y = Mathf.Clamp(mouseFinal.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        var targetOrientation = Quaternion.Euler(targetDirection);
        Caster.GetTransformCastPoint().localRotation = Quaternion.AngleAxis(-mouseFinal.y, targetOrientation * Vector3.right) * targetOrientation;

    }

    void AlignToBody() {
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);
        Quaternion yRotation = Quaternion.identity;

        yRotation = Quaternion.AngleAxis(mouseFinal.x, Caster.GetTransformCastPoint().InverseTransformDirection(Vector3.up));
        Caster.GetTransformCastPoint().localRotation *= yRotation;
    }



}
