using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Boss.Attacks.Shared
{
    public static class DiscMath
    {
        public static Vector3[] GenerateDiscVertices(Vector3 center, float radius, int segments)
        {
            int seg = segments < 3 ? 3 : segments;
            Vector3[] verts = new Vector3[seg];
            float step = Mathf.PI * 2f / seg;
            float y = center.y;
            for (int i = 0; i < seg; i++)
            {
                float a = i * step;
                float x = center.x + Mathf.Cos(a) * radius;
                float z = center.z + Mathf.Sin(a) * radius;
                verts[i] = new Vector3(x, y, z);
            }
            return verts;
        }

        public static bool IsPointInsideDisc(Vector3 center, float radius, Vector3 point)
        {
            Vector2 c = new Vector2(center.x, center.z);
            Vector2 p = new Vector2(point.x, point.z);
            return (p - c).sqrMagnitude <= radius * radius;
        }
    }
}


