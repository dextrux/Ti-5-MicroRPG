using UnityEngine;

public interface ICameraController
{
    void Follow(Transform target);
    void Rotate(Vector2 input);
}
