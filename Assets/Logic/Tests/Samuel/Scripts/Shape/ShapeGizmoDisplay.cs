using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ShapeGizmoDisplay : MonoBehaviour
{
    [SerializeField] private ArenaPosReference _arena;
    public bool Runtime;

    [SerializeField] private AreaShapeFactory _areaShapeFactory;
    private AreaShape _areaShape;   

    public void UpdateShape()
    {
        _areaShape = _areaShapeFactory.CreateAreaShape();
    }

    private void OnDrawGizmosSelected()
    {
        if (Runtime)
            UpdateShape();

        if (_areaShape != null && _arena != null)
            _areaShape.VisualGizmo(_arena.RealPositionToRelativeArenaPosition(transform), new Vector2(transform.forward.x, transform.forward.z), _arena.GetPlayerArenaPosition(), _arena);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ShapeGizmoDisplay))]
public class ShapeGizmoDisplayEditor : Editor
{
    private ShapeGizmoDisplay _target;

    private void OnEnable()
    {
        _target = target as ShapeGizmoDisplay;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(8);

        if (GUILayout.Button("Apply"))
            _target.UpdateShape();
    }
}
#endif
