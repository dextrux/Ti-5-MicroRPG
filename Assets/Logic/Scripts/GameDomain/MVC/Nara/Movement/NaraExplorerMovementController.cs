using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.UpdateService;
using UnityEngine;

public class NaraExplorerMovementController : NaraMovementController {
    public NaraExplorerMovementController(GameInputActions inputActions, IUpdateSubscriptionService updateSubscriptionService,
        NaraConfigurationSO naraConfiguration) : base(inputActions, updateSubscriptionService, naraConfiguration) {
    }

    public override void Move(Vector2 direction, float velocity, float rotation) {
        if (NaraRigidbody == null) return;

        Vector3 camF = Cam.transform.forward;
        camF.y = 0f; camF.Normalize();
        Vector3 camR = Cam.transform.right;
        camR.y = 0f; camR.Normalize();

        Vector3 worldDir = camF * direction.y + camR * direction.x;
        if (worldDir.sqrMagnitude > 1e-6f) worldDir.Normalize();

        Vector3 vel = worldDir * velocity;
        NaraRigidbody.linearVelocity = new Vector3(vel.x, NaraRigidbody.linearVelocity.y, vel.z);

        Rotate(rotation);
    }

    public override void MoveToPoint(Vector3 endPosition, float velocity, float rotation) {
        if (NaraRigidbody == null) return;

        Vector3 camF = Cam.transform.forward;
        camF.y = 0f; camF.Normalize();
        Vector3 camR = Cam.transform.right;
        camR.y = 0f; camR.Normalize();

        Vector3 direction = endPosition - NaraTransform.position;

        Vector3 worldDir = camF * direction.y + camR * direction.x;
        if (worldDir.sqrMagnitude > 1e-6f) worldDir.Normalize();

        Vector3 vel = worldDir * velocity;
        NaraRigidbody.linearVelocity = new Vector3(vel.x, NaraRigidbody.linearVelocity.y, vel.z);

        Rotate(rotation);
    }
}
