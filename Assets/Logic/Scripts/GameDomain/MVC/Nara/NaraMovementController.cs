using Logic.Scripts.GameDomain.MVC.Nara;
using UnityEngine;

public class NaraMovementController : IMovement {
    private PlayerData _data;
    private Rigidbody _rigidbody;
    public NaraMovementController(PlayerData data, Rigidbody rigidbody) {
        _data = data;
        _rigidbody = rigidbody;
    }

    public void SetNara(NaraView naraView, Vector3 naraPosition) {

    }

    public void Jump(float jumpForce, float gravity) {
        throw new System.NotImplementedException();
    }

    public void Move(Vector2 direction, float velocity, float rotation) {
        throw new System.NotImplementedException();
    }

    public void MoveToPoint(Vector3 endPosition, float velocity, float rotation) {
        throw new System.NotImplementedException();
    }
}
