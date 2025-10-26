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
            if (caster is NaraController) {
                NaraController controller = (NaraController)caster;
                trajectoryLine = controller.GetPointLineRenderer();
            }
            hitMarker = UnityEngine.Object.Instantiate(caster.GetReferenceTargetPrefab()).transform;
            SetTrajectoryVisible(true);
            SubscriptionService.RegisterUpdatable(this);
        }
    }

    public override void ManagedUpdate() {
        base.ManagedUpdate();
        PredictTrajectory();
        Debug.Log("TesteUpdate");
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



}
