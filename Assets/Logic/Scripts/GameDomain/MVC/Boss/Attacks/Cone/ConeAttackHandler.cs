using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Core;
using System.Collections.Generic;
using Logic.Scripts.GameDomain.MVC.Abilitys;

namespace Logic.Scripts.GameDomain.MVC.Boss.Attacks.Cone
{
    public class ConeAttackHandler : IBossAttackHandler
    {
        private readonly float _radius;
        private readonly float _angleDeg;
        private readonly int _sides;
        private readonly float[] _yaws;
        private class ConeSubView
        {
            public LineRenderer Line;
            public MeshFilter MeshFilter;
            public MeshRenderer MeshRenderer;
            public Mesh Mesh;
        }
        private ConeSubView[] _views;

        public ConeAttackHandler(float radius, float angleDeg, int sides, float[] yaws)
        {
            _radius = radius;
            _angleDeg = angleDeg;
            _sides = sides;
            _yaws = yaws;
        }

        public void PrepareTelegraph(Transform parentTransform)
        {
            if (_yaws == null || _yaws.Length == 0) return;
            _views = new ConeSubView[_yaws.Length];
            for (int i = 0; i < _yaws.Length; i++)
            {
                GameObject go = new GameObject("ConeSubActionView");
                go.transform.SetParent(parentTransform, false);

                ConeSubView v = new ConeSubView();
                v.Line = go.AddComponent<LineRenderer>();
                v.Line.material = new Material(Shader.Find("Sprites/Default"));
                v.Line.useWorldSpace = true;
                v.Line.loop = true;
                v.Line.widthMultiplier = 0.1f;
                v.Line.startColor = Color.yellow;
                v.Line.endColor = Color.yellow;

                v.MeshFilter = go.AddComponent<MeshFilter>();
                v.MeshRenderer = go.AddComponent<MeshRenderer>();
                v.MeshRenderer.material = new Material(Shader.Find("Sprites/Default")) { color = new Color(1f, 1f, 0f, 0.2f) };
                v.Mesh = new Mesh();
                v.Mesh.name = "ConeMesh";
                v.MeshFilter.sharedMesh = v.Mesh;

                Vector3 origin = parentTransform.position;
                Vector3 forward = Quaternion.Euler(0f, _yaws[i], 0f) * Vector3.forward;

                Vector3[] outline = ConeArea.GenerateConeOutlinePolygon(origin, forward, _radius, _angleDeg, _sides);
                for (int p = 0; p < outline.Length; p++) outline[p].y = 0.2f;
                v.Line.positionCount = outline.Length;
                v.Line.SetPositions(outline);

                Vector3[] arc = ConeArea.GenerateConeArcVertices(origin, forward, _radius, _angleDeg, _sides);
                for (int p = 0; p < arc.Length; p++) arc[p].y = 0.2f;

                Transform mT = v.MeshFilter.transform;
                mT.localPosition = new Vector3(0f, 0.2f, 0f);
                mT.localRotation = Quaternion.identity;

                // Build triangle fan: vertex 0 = origin, then arc points
                Vector3[] worldVerts = new Vector3[arc.Length + 1];
                worldVerts[0] = new Vector3(origin.x, 0.2f, origin.z);
                for (int a = 0; a < arc.Length; a++) worldVerts[a + 1] = arc[a];

                Vector3[] localVerts = new Vector3[worldVerts.Length];
                for (int a = 0; a < worldVerts.Length; a++) localVerts[a] = mT.InverseTransformPoint(worldVerts[a]);

                int triCount = (worldVerts.Length - 1) * 3;
                int[] tris = new int[triCount];
                int t = 0;
                for (int a = 1; a < worldVerts.Length - 1; a++)
                {
                    tris[t++] = 0; tris[t++] = a; tris[t++] = a + 1;
                }

                v.Mesh.Clear();
                v.Mesh.vertices = localVerts;
                v.Mesh.triangles = tris;
                v.Mesh.RecalculateNormals();
                v.Mesh.RecalculateBounds();

                _views[i] = v;
            }
        }

        public bool ComputeHits(ArenaPosReference arenaReference, Transform originTransform, IEffectable caster)
        {
            if (_yaws == null || _yaws.Length == 0) return false;
            Vector3 playerWorld = arenaReference.RelativeArenaPositionToRealPosition(arenaReference.GetPlayerArenaPosition());
            for (int i = 0; i < _yaws.Length; i++)
            {
                Vector3 origin = originTransform.position;
                Vector3 forward = Quaternion.Euler(0f, _yaws[i], 0f) * Vector3.forward;
                if (ConeArea.IsPointInsideCone(origin, forward, _radius, _angleDeg, playerWorld)) return true;
            }
            return false;
        }

        public System.Collections.IEnumerator ExecuteEffects(List<AbilityEffect> effects, ArenaPosReference arenaReference, Transform originTransform, IEffectable caster)
        {
            if (effects == null || effects.Count == 0) yield break;
            IEffectable target = arenaReference.NaraController as IEffectable;
            if (target == null) yield break;

            // Apply all effects if any cone hits
            Vector3 playerWorld = arenaReference.RelativeArenaPositionToRealPosition(arenaReference.GetPlayerArenaPosition());
            bool anyHit = false;
            for (int i = 0; i < _yaws.Length; i++)
            {
                Vector3 origin = originTransform.position;
                Vector3 forward = Quaternion.Euler(0f, _yaws[i], 0f) * Vector3.forward;
                if (ConeArea.IsPointInsideCone(origin, forward, _radius, _angleDeg, playerWorld)) { anyHit = true; break; }
            }
            if (!anyHit) yield break;

            for (int i = 0; i < effects.Count; i++)
            {
                AbilityEffect fx = effects[i];
                fx?.Execute(caster, target);
            }
            yield break;
        }

        public void Cleanup()
        {
            if (_views == null) return;
            for (int i = 0; i < _views.Length; i++)
            {
                if (_views[i] != null)
                {
                    Object.Destroy(_views[i].Line?.gameObject);
                }
            }
            _views = null;
        }
    }
}


