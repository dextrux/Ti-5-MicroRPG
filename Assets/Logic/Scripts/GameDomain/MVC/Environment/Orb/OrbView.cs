using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Shared;

namespace Logic.Scripts.GameDomain.MVC.Environment.Orb
{
    public class OrbView : MonoBehaviour
    {
        private GameObject _telegraphGO;
        private LineRenderer _line;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Mesh _mesh;
        [SerializeField] private int _segments = 64;

        public void PrepareTelegraph()
        {
            if (_telegraphGO != null) return;
            _telegraphGO = new GameObject("OrbTelegraph");
            _telegraphGO.transform.SetParent(transform, false);

            _line = _telegraphGO.AddComponent<LineRenderer>();
            _line.material = new Material(Shader.Find("Sprites/Default"));
            _line.useWorldSpace = true;
            _line.loop = true;
            _line.widthMultiplier = 0.1f;
            _line.startColor = Color.cyan;
            _line.endColor = Color.cyan;

            _meshFilter = _telegraphGO.AddComponent<MeshFilter>();
            _meshRenderer = _telegraphGO.AddComponent<MeshRenderer>();
            _meshRenderer.material = new Material(Shader.Find("Sprites/Default")) { color = new Color(0f, 1f, 1f, 0.2f) };
            _mesh = new Mesh();
            _mesh.name = "OrbDiscMesh";
            _meshFilter.sharedMesh = _mesh;
        }

        public void UpdateRadius(float radius)
        {
            if (_telegraphGO == null) PrepareTelegraph();
            Vector3 center = transform.position; center.y = 0.2f;
            Vector3[] ring = DiscMath.GenerateDiscVertices(center, radius, _segments);
            for (int i = 0; i < ring.Length; i++) ring[i].y = 0.2f;
            _line.positionCount = ring.Length;
            _line.SetPositions(ring);

            Transform t = _meshFilter.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;

            int seg = ring.Length;
            Vector3[] verts = new Vector3[seg + 1];
            verts[0] = t.InverseTransformPoint(center);
            for (int i = 0; i < seg; i++) verts[i + 1] = t.InverseTransformPoint(ring[i]);
            int[] tris = new int[seg * 3];
            int ti = 0;
            for (int i = 1; i <= seg; i++)
            {
                int next = (i == seg) ? 1 : i + 1;
                tris[ti++] = 0; tris[ti++] = i; tris[ti++] = next;
            }
            _mesh.Clear();
            _mesh.vertices = verts;
            _mesh.triangles = tris;
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            _meshRenderer.enabled = true;
            _line.enabled = true;
        }
    }
}
