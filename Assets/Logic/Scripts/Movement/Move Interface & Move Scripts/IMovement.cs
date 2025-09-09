using UnityEngine;

public interface IMovement
{
    void Move(Vector2 direction, float velocity, float rotation);

    void MoveToPoint(Vector3 endPosition, float velocity, float rotation);

    void Jump(float jumpForce, float gravity);
}
