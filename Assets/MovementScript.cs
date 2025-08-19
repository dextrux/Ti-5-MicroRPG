using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;

    private void Update() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
