using UnityEngine;
using Zenject;

public class RigidbodyMovement : MonoBehaviour, IMovement
{
    private Rigidbody rb;

    private Vector3 movement;
    private float verticalVelocity;
    private float rotationForce;
    private float gravityForce;
    private Transform platform;

    [Inject]
    public void Construct(Rigidbody rb)
    {
        this.rb = rb;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public void Move(Vector2 direction, float velocity, float rotation)
    {
        Vector3 dir = new Vector3(direction.x, 0f, direction.y);
        movement = dir * velocity;
        rotationForce = rotation;
    }

    public void MoveToPoint(Vector3 endPosition, float velocity, float rotation)
    {
        Vector3 dir = endPosition - transform.position;
        dir.y = 0f;
        Vector3 n = dir.magnitude > 0.0001f ? dir.normalized : Vector3.zero;
        movement = n * velocity;
        rotationForce = rotation;
    }

    public void Jump(float jumpForce, float gravity)
    {
        if (IsGrounded())
        {
            verticalVelocity = jumpForce;
        }
        gravityForce = gravity;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private void FixedUpdate()
    {
        if (!IsGrounded())
        {
            verticalVelocity += gravityForce * Time.fixedDeltaTime;
        }
        else if (verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        rb.linearVelocity = new Vector3(movement.x, 0f, movement.z);

        Vector3 rot = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if(rot.sqrMagnitude > 0.0001f)
        {
            Quaternion finalRotation = Quaternion.LookRotation(rot.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.fixedDeltaTime * rotationForce);
        }
    }
}
