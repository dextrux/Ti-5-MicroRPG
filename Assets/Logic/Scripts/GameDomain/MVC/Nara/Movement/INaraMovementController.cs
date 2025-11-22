using UnityEngine;

public interface INaraMovementController {
    void Move(Vector2 direction, float velocity, float rotation);

    void MoveToPoint(Vector3 endPosition, float velocity, float rotation);
}
