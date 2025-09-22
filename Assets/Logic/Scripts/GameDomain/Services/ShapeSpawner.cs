using UnityEngine;

[ExecuteAlways]
public class ShapeSpawner : MonoBehaviour {

    public static ShapeSpawner Instance { get; private set; }

    [Header("Configuração")]
    public ShapeType shape = ShapeType.Circle;
    public int quantidade = 10;
    public float raio = 5f;
    public Vector2 elipse = new Vector2(5f, 3f);
    public float spacing = 2f;
    public int squarePerSide = 4;
    public float spiralSpacing = 0.5f;
    public int polygonSides = 6;
    public bool olharParaCentro = false;

    [Header("Visualização")]
    public bool drawGizmos = true;
    public Color gizmoColor = Color.yellow;
    public float gizmoSize = 0.3f;

    private void Start() {
        Instance = this;
    }


    public void Spawn(GameObject prefab, Transform referemceTransform, ShapeType type) {
        shape = type;
        Vector3[] points = GetPoints();

        foreach (var pos in points) {
            GameObject obj = Instantiate(prefab, referemceTransform.position + pos, Quaternion.identity);

            if (olharParaCentro)
                obj.transform.LookAt(transform.position);
        }
    }

    public Vector3[] GetPoints() {
        switch (shape) {
            case ShapeType.Circle:
                return GenerateCircle();

            case ShapeType.Ellipse:
                return GenerateEllipse();

            case ShapeType.Line:
                return GenerateLine();

            case ShapeType.Square:
                return GenerateSquare();

            case ShapeType.Spiral:
                return GenerateSpiral();

            case ShapeType.Polygon:
                return GeneratePolygon();

            default:
                return new Vector3[0];
        }
    }

    Vector3[] GenerateCircle() {
        Vector3[] points = new Vector3[quantidade];
        float angleStep = 360f / quantidade;

        for (int i = 0; i < quantidade; i++) {
            float angle = i * angleStep * Mathf.Deg2Rad;

            points[i] = new Vector3(
                Mathf.Cos(angle) * raio,
                0,
                Mathf.Sin(angle) * raio
            );
        }

        return points;
    }

    Vector3[] GenerateEllipse() {
        Vector3[] points = new Vector3[quantidade];
        float angleStep = 360f / quantidade;

        for (int i = 0; i < quantidade; i++) {
            float angle = i * angleStep * Mathf.Deg2Rad;

            points[i] = new Vector3(
                Mathf.Cos(angle) * elipse.x,
                0,
                Mathf.Sin(angle) * elipse.y
            );
        }

        return points;
    }

    Vector3[] GenerateLine() {
        Vector3[] points = new Vector3[quantidade];
        float totalLength = (quantidade - 1) * spacing;
        float startX = -totalLength / 2f;

        for (int i = 0; i < quantidade; i++) {
            points[i] = new Vector3(
                startX + i * spacing,
                0,
                0
            );
        }

        return points;
    }

    Vector3[] GenerateSquare() {
        Vector3[] points = new Vector3[squarePerSide * squarePerSide];
        int index = 0;

        for (int x = 0; x < squarePerSide; x++) {
            for (int z = 0; z < squarePerSide; z++) {
                points[index++] = new Vector3(
                    (x - squarePerSide / 2f) * spacing,
                    0,
                    (z - squarePerSide / 2f) * spacing
                );
            }
        }

        return points;
    }

    Vector3[] GenerateSpiral() {
        Vector3[] points = new Vector3[quantidade];
        float angleStep = 30f;

        for (int i = 0; i < quantidade; i++) {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float currentRadius = i * spiralSpacing;

            points[i] = new Vector3(
                Mathf.Cos(angle) * currentRadius,
                0,
                Mathf.Sin(angle) * currentRadius
            );
        }

        return points;
    }

    Vector3[] GeneratePolygon() {
        Vector3[] points = new Vector3[polygonSides];
        float angleStep = 360f / polygonSides;

        for (int i = 0; i < polygonSides; i++) {
            float angle = i * angleStep * Mathf.Deg2Rad;

            points[i] = new Vector3(
                Mathf.Cos(angle) * raio,
                0,
                Mathf.Sin(angle) * raio
            );
        }

        return points;
    }

    void OnDrawGizmos() {
        if (!drawGizmos) return;

        Gizmos.color = gizmoColor;
        Vector3[] points = GetPoints();

        foreach (var pos in points) {
            Gizmos.DrawSphere(transform.position + pos, gizmoSize);
        }

        if (points.Length > 1) {
            for (int i = 0; i < points.Length; i++) {
                Vector3 current = transform.position + points[i];
                Vector3 next = transform.position + points[(i + 1) % points.Length];
                Gizmos.DrawLine(current, next);
            }
        }
    }
}
