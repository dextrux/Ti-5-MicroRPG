using Logic.Scripts.GameDomain.MVC.Nara;
using UnityEngine;
using Logic.Scripts.Turns;

public class NaraMovementController : MonoBehaviour, IMovement
{
    private Rigidbody _rigidbody;
    private Vector3 _movement;
    private Vector3 movementCenter;
    private int movementRadius;
    private ActionPointsService _actionPointsService;
    private int extraMovementSpaceCost = 2;

    public NaraMovementController(NaraConfigurationSO naraSO, Rigidbody rigidbody)
    {
        _rigidbody = rigidbody;
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        movementRadius = naraSO.InitialMovementDistance;
    }

    public void SetNara(NaraView naraView, Vector3 naraPosition)
    {

    }

    public void Jump(float jumpForce, float gravity)
    {
        throw new System.NotImplementedException();
    }

    public void Move(Vector2 direction, float velocity, float rotation)
    {
        float distance = Vector3.Distance(transform.position, movementCenter);

        if(distance < movementRadius)
        {
            if (distance >= movementRadius / 2)
            {
                _actionPointsService.Spend(extraMovementSpaceCost);
            }
            Vector3 _direction = new Vector3(direction.x, 0f, direction.y);
            _movement = _direction * velocity;
            _rigidbody.linearVelocity = new Vector3(_movement.x, 0f, _movement.z);
            Rotate(rotation);
        }
    }

    public void MoveToPoint(Vector3 endPosition, float velocity, float rotation)
    {
        float distance = Vector3.Distance(endPosition, movementCenter);

        if(distance < movementRadius)
        {
            if(distance >= movementRadius/2)
            {
                _actionPointsService.Spend(extraMovementSpaceCost);
            }
            Vector3 direction = endPosition - transform.position;
            direction.y = 0f;
            Vector3 n = direction.magnitude > 0.0001f ? direction.normalized : Vector3.zero;
            _movement = n * velocity;
            _rigidbody.linearVelocity = new Vector3(_movement.x, 0f, _movement.z);
            Rotate(rotation);
        }
    }

    private void Rotate(float rotationForce)
    {
        Vector3 rotate = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);
        if (rotate.sqrMagnitude > 0.0001f)
        {
            Quaternion finalRotation = Quaternion.LookRotation(rotate.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.fixedDeltaTime * rotationForce);
        }
    }

    public void SetMovementRadiusCenter()
    {
        movementCenter = transform.position;
    }
}
