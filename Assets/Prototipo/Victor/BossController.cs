using System;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float minDistance = 3f;
    public float speed;
    public float rotSpeed;
    public Transform Target;
    private Vector3 dir;

    private void FixedUpdate() {
        UpdateDirection(Target);
        SmoothRotate();
        Move();
    }

    private void Move() {
        transform.position += dir * Time.fixedDeltaTime;
    }

    public void UpdateDirection(Transform target) {
        dir = target.position - transform.position;
        if (dir.sqrMagnitude > (minDistance * minDistance)) {
            dir = dir.normalized * speed;
        }
        else {
            dir = Vector3.zero;
        }
    }
    public void SmoothRotate() {
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * rotSpeed);
    }
}
