// NaraMovementController.cs  (substituir conteúdo atual)
using Logic.Scripts.GameDomain.MVC.Nara;
using UnityEngine;
using Logic.Scripts.Turns;

public class NaraMovementController : IMovement
{
    private Rigidbody _rigidbody;
    private Transform _transform;

    private Vector3 _movement;
    private Vector3 movementCenter;
    private int movementRadius;

    private ActionPointsService _actionPointsService;
    private int extraMovementSpaceCost = 2;

    public NaraMovementController(NaraConfigurationSO naraSO)
    {
        movementRadius = naraSO.InitialMovementDistance;
    }

    public void SetActionPointsService(ActionPointsService aps)
    {
        _actionPointsService = aps;
    }

    public void SetNara(NaraView naraView, Vector3 naraPosition)
    {
        // (mantido vazio conforme seu original)
    }

    public void SetNaraRigidbody(Rigidbody rigidbody)
    {
        _rigidbody = rigidbody;
        _transform = rigidbody != null ? rigidbody.transform : null;

        if (_rigidbody == null) return;

        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public Vector3 GetNaraCenter()
    {
        return movementCenter;
    }

    public int GetNaraRadius()
    {
        return movementRadius;
    }

    public void Jump(float jumpForce, float gravity)
    {
        throw new System.NotImplementedException();
    }

    public void CheckRadiusLimit()
    {
        float distance = Vector3.Distance(_transform.position, movementCenter);

        if (distance > movementRadius)
        {
            Debug.Log("Passou Raio");
            Vector3 directionFromCenter = (_transform.position - movementCenter).normalized;
            Vector3 radiusLimit = movementCenter + directionFromCenter * movementRadius;
            _rigidbody.MovePosition(new Vector3(radiusLimit.x, _transform.position.y, radiusLimit.z));
        }
    }

    public void Move(Vector2 direction, float velocity, float rotation)
    {
        if (_rigidbody == null || _transform == null) return;

        float distance = Vector3.Distance(_transform.position, movementCenter);

        if (distance > movementRadius)
        {
            Debug.Log("Passou Raio");
            Vector3 directionFromCenter = (_transform.position - movementCenter).normalized;
            Vector3 radiusLimit = movementCenter + directionFromCenter * movementRadius;
            _rigidbody.MovePosition(new Vector3(radiusLimit.x, _transform.position.y, radiusLimit.z));
        }

        Debug.Log("Andando");

        Vector3 moveDirection = new Vector3(direction.x, 0f, direction.y);
        _movement = moveDirection * velocity;

        _rigidbody.linearVelocity = new Vector3(_movement.x, 0f, _movement.z);
        Rotate(rotation);
        Debug.Log("Rotacionou");
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
            Vector3 direction = endPosition - _transform.position;
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
            _transform.rotation = Quaternion.Slerp(_transform.rotation, finalRotation, Time.fixedDeltaTime * rotationForce);
        }
    }

    public void SetMovementRadiusCenter()
    {
        if (_transform != null)
            movementCenter = _transform.position;
    }
}
