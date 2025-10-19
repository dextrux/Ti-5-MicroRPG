using Unity.VisualScripting;
using UnityEngine;

public class HabilidadeEmpurarBossTeste : MonoBehaviour
{
    public Rigidbody _rb;
    public Vector3 thisPosition;
    public float _forca = 10f;

    public float _stopDistance = 0.05f;

    Vector3 dir;

    void Start()
    {
        thisPosition = transform.position;
    }

    public void Push(Rigidbody rb, float force)
    {
        dir = (rb.position - thisPosition).normalized;
        dir.y = 0f;
        dir = dir.normalized;

        Vector3 target = rb.position + dir * force;
        rb.MovePosition(target);
    }

    public void Pull(Rigidbody rb, float force, float stopDistance)
    {
        Vector3 toTarget = thisPosition - rb.position;
        toTarget.y = 0f;

        float dist = toTarget.magnitude;
        if (dist <= stopDistance) return;

        Vector3 dir = toTarget / dist;

        float step = Mathf.Min(force, Mathf.Max(0f, dist - stopDistance));
        if (step <= 0f) return;

        Vector3 target = rb.position + dir * step;
        rb.MovePosition(target);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Push(_rb, _forca);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Pull(_rb, _forca, _stopDistance);
        }
    }
}
