using UnityEngine;

public class ShapeLIneRender : MonoBehaviour
{
    [SerializeField] private ArenaPosReference _arena;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Color _mainGizmoColor = Color.yellow;
    [SerializeField, Min(0.1f)] private float _width;

    [Space]
    [SerializeField] private CompositedAreaShapeFactory _areaShapeFactory;
    private AreaShape _areaShape;

    private void Start()
    {
        _areaShape = _areaShapeFactory.CreateAreaShape();
        Vector3[] points = _areaShape.GetPoints(_arena.RealPositionToRelativeArenaPosition(transform), new Vector2(transform.forward.x, transform.forward.z), _arena);

        foreach (var point in points )
        {
            Debug.Log(point);
        }

        _lineRenderer.useWorldSpace = true;
        _lineRenderer.loop = true;
        _lineRenderer.startWidth = _width;
        _lineRenderer.endWidth = _width;
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.startColor = Color.blue;
        _lineRenderer.endColor = _mainGizmoColor;

        _lineRenderer.positionCount = points.Length;
        _lineRenderer.SetPositions(points);
    }

    [ContextMenu("UpdateShape")]
    public void UpdateShape()
    {
        _areaShape = _areaShapeFactory.CreateAreaShape();
        Vector3[] points = _areaShape.GetPoints(_arena.RealPositionToRelativeArenaPosition(transform), new Vector2(transform.forward.x, transform.forward.z), _arena);

        _lineRenderer.positionCount = points.Length;
        _lineRenderer.SetPositions(points);
    }
}
