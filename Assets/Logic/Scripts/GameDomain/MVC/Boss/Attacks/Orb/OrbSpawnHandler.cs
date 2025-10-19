using UnityEngine;
using System.Collections.Generic;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Core;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Shared;
using Logic.Scripts.GameDomain.MVC.Abilitys;

namespace Logic.Scripts.GameDomain.MVC.Boss.Attacks.Orb
{
    public class OrbSpawnHandler : IBossAttackHandler
    {
        private readonly float _radius;
        private readonly int _segments;
        private GameObject _telegraphGO;
        private LineRenderer _line;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Mesh _mesh;

        public OrbSpawnHandler(float radius, int segments = 48)
        {
            _radius = radius;
            _segments = segments < 12 ? 12 : segments;
        }

        public void PrepareTelegraph(Transform parentTransform)
        {
            _telegraphGO = new GameObject("OrbSpawnTelegraph");
            _telegraphGO.transform.SetParent(parentTransform, false);

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
            _mesh.name = "OrbSpawnDisc";
            _meshFilter.sharedMesh = _mesh;

            Vector3 center = parentTransform.position; center.y = 0.2f;
            Vector3[] ring = DiscMath.GenerateDiscVertices(center, _radius, _segments);
            for (int i = 0; i < ring.Length; i++) ring[i].y = 0.2f;
            _line.positionCount = ring.Length;
            _line.SetPositions(ring);

            Transform t = _meshFilter.transform;
            t.localPosition = new Vector3(0f, 0.2f, 0f);
            t.localRotation = Quaternion.identity;
            Vector3[] verts = new Vector3[ring.Length + 1];
            verts[0] = t.InverseTransformPoint(center);
            for (int i = 0; i < ring.Length; i++) verts[i + 1] = t.InverseTransformPoint(ring[i]);
            int[] tris = new int[ring.Length * 3];
            int ti = 0;
            for (int i = 1; i < verts.Length; i++)
            {
                int next = (i + 1) < verts.Length ? (i + 1) : 1;
                tris[ti++] = 0; tris[ti++] = i; tris[ti++] = next;
            }
            _mesh.Clear();
            _mesh.vertices = verts;
            _mesh.triangles = tris;
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
        }

        public bool ComputeHits(ArenaPosReference arenaReference, Transform originTransform, IEffectable caster)
        {
            return false;
        }

        public System.Collections.IEnumerator ExecuteEffects(List<AbilityEffect> effects, ArenaPosReference arenaReference, Transform originTransform, IEffectable caster)
        {
            yield break;
        }

        public void Cleanup()
        {
            if (_telegraphGO != null)
            {
                Object.Destroy(_telegraphGO);
                _telegraphGO = null;
            }
        }
    }
}


