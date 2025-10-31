using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Nara
{
    public class NaraAreaLineHandler
    {
        private readonly LineRenderer _fullMoveArea;
        private readonly LineRenderer _resultMoveArea;
        private readonly int _segments;
        private Vector3 _center;
        private float _radius;

        public NaraAreaLineHandler(GameObject fullMoveAreaHost, GameObject resultMoveArea_resultMoveAreaHost, int segments = 100)
        {
            _segments = Mathf.Max(8, segments);

            _fullMoveArea = fullMoveAreaHost.AddComponent<LineRenderer>();
            _fullMoveArea.useWorldSpace = true;
            _fullMoveArea.loop = false;
            _fullMoveArea.material = new Material(Shader.Find("Sprites/Default"));
            _fullMoveArea.startWidth = 1f;
            _fullMoveArea.endWidth = 1f;
            _fullMoveArea.startColor = Color.blue;
            _fullMoveArea.endColor = Color.blue;
            _fullMoveArea.enabled = false;

            _resultMoveArea = resultMoveArea_resultMoveAreaHost.AddComponent<LineRenderer>();
            _resultMoveArea.useWorldSpace = true;
            _resultMoveArea.loop = false;
            _resultMoveArea.material = new Material(Shader.Find("Sprites/Default"));
            _resultMoveArea.startWidth = 1f;
            _resultMoveArea.endWidth = 1f;
            _resultMoveArea.startColor = Color.blue;
            _resultMoveArea.endColor = Color.blue;
            _resultMoveArea.enabled = false;
        }

        public void SetFullMoveArea(Vector3 center, float radius)
        {
            _center = center;
            _radius = Mathf.Max(0f, radius);
            DrawCircle(_fullMoveArea, center, _radius, center.y + 0.25f);
        }

        public void SetResultMoveArea(Vector3 playerPos)
        {
            float dist = Vector3.Distance(playerPos, _center);
            float r2 = Mathf.Max(0f, _radius - dist);
            if (r2 <= 0f)
            {
                _resultMoveArea.positionCount = 0;
                return;
            }
            DrawCircle(_resultMoveArea, playerPos, r2, _center.y + 0.25f);
        }

        public void Refresh(Vector3 center, float radius, Vector3 playerPos)
        {
            SetFullMoveArea(center, radius);
            SetResultMoveArea(playerPos);
        }

        public void SetVisible(bool visible)
        {
            _fullMoveArea.enabled = visible;
            _resultMoveArea.enabled = visible;
        }

        private void DrawCircle(LineRenderer lr, Vector3 center, float radius, float y)
        {
            int perCircle = _segments + 1;
            int total = perCircle;
            lr.positionCount = total;
            for (int i = 0; i <= _segments; i++)
            {
                float t = (float)i / _segments;
                float ang = t * 2f * Mathf.PI;
                float x = Mathf.Cos(ang) * radius + center.x;
                float z = Mathf.Sin(ang) * radius + center.z;
                lr.SetPosition(i, new Vector3(x, y, z));
            }
        }
    }
}
