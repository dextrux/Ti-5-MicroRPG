using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CircleAttack : BossAttack
{
    [SerializeField, Min(0)] private float _radius;
    [SerializeField, Range(-1f, 1f)] private float _angle;

    private void Awake()
    {
        effectArea = new ConeArea(_radius, _angle);
    }

    protected override void Effect()
    {
        Debug.Log("CIRCLE!!!");
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;

        Handles.DrawWireDisc(transform.position, Vector3.up, _radius);

        Handles.color = Color.white;
    }
#endif
}
