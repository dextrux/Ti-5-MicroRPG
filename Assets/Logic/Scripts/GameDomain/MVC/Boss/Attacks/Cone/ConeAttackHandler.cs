using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Core;

namespace Logic.Scripts.GameDomain.MVC.Boss.Attacks.Cone
{
    public class ConeAttackHandler : IBossAttackHandler
    {
        private readonly float _radius;
        private readonly float _angleDeg;
        private readonly int _sides;
        private readonly float[] _yaws;
        private LineRenderer[] _renderers;

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
            _renderers = new LineRenderer[_yaws.Length];
            for (int i = 0; i < _yaws.Length; i++)
            {
                GameObject go = new GameObject("ConeSubActionView");
                go.transform.SetParent(parentTransform, false);
                LineRenderer lr = go.AddComponent<LineRenderer>();
                lr.material = new Material(Shader.Find("Sprites/Default"));
                lr.useWorldSpace = true;
                lr.loop = true;
                lr.widthMultiplier = 0.1f;
                _renderers[i] = lr;

                Vector3 origin = parentTransform.position;
                Vector3 forward = Quaternion.Euler(0f, _yaws[i], 0f) * parentTransform.forward;
                Vector3[] pts = ConeArea.GenerateConeOutlinePolygon(origin, forward, _radius, _angleDeg, _sides);
                for (int p = 0; p < pts.Length; p++) pts[p].y = 1f;
                lr.positionCount = pts.Length;
                lr.SetPositions(pts);
            }
        }

        public bool ComputeHits(ArenaPosReference arenaReference, Transform originTransform, IEffectable caster)
        {
            if (_yaws == null || _yaws.Length == 0) return false;
            Vector3 playerWorld = arenaReference.RelativeArenaPositionToRealPosition(arenaReference.GetPlayerArenaPosition());
            for (int i = 0; i < _yaws.Length; i++)
            {
                Vector3 origin = originTransform.position;
                Vector3 forward = Quaternion.Euler(0f, _yaws[i], 0f) * originTransform.forward;
                if (ConeArea.IsPointInsideCone(origin, forward, _radius, _angleDeg, playerWorld)) return true;
            }
            return false;
        }

        public void Cleanup()
        {
            if (_renderers == null) return;
            for (int i = 0; i < _renderers.Length; i++)
            {
                if (_renderers[i] != null)
                {
                    Object.Destroy(_renderers[i].gameObject);
                }
            }
            _renderers = null;
        }
    }
}


