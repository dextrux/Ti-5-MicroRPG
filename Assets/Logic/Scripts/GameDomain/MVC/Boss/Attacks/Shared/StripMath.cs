using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Boss.Attacks.Shared
{
    public static class StripMath
    {
        public static Vector3[] GenerateStripVertices(Vector3 start, Vector3 end, float width)
        {
            Vector3 a = new Vector3(start.x, 0f, start.z);
            Vector3 b = new Vector3(end.x, 0f, end.z);
            Vector3 dir = b - a;
            float len = dir.magnitude;
            if (len < 1e-6f)
            {
                Vector3 right = Vector3.right * (width * 0.5f);
                return new Vector3[] { a - right, a + right, a + right, a - right };
            }
            Vector3 n = new Vector3(-dir.z, 0f, dir.x).normalized * (width * 0.5f);
            Vector3 v0 = a - n;
            Vector3 v1 = a + n;
            Vector3 v2 = b + n;
            Vector3 v3 = b - n;
            return new Vector3[] { v0, v1, v2, v3 };
        }

        public static bool IsPointInsideStrip(Vector3 start, Vector3 end, float width, Vector3 point)
        {
            Vector3 a = new Vector3(start.x, 0f, start.z);
            Vector3 b = new Vector3(end.x, 0f, end.z);
            Vector3 p = new Vector3(point.x, 0f, point.z);
            Vector3 ab = b - a;
            float abLenSq = ab.sqrMagnitude;
            if (abLenSq < 1e-6f)
            {
                return (p - a).magnitude <= (width * 0.5f);
            }
            float t = Vector3.Dot(p - a, ab) / abLenSq;
            t = Mathf.Clamp01(t);
            Vector3 closest = a + ab * t;
            float dist = (p - closest).magnitude;
            return dist <= (width * 0.5f);
        }

        public static float DistancePointToSegment(Vector3 start, Vector3 end, Vector3 point)
        {
            Vector3 a = new Vector3(start.x, 0f, start.z);
            Vector3 b = new Vector3(end.x, 0f, end.z);
            Vector3 p = new Vector3(point.x, 0f, point.z);
            Vector3 ab = b - a;
            float abLenSq = ab.sqrMagnitude;
            if (abLenSq < 1e-6f) return (p - a).magnitude;
            float t = Vector3.Dot(p - a, ab) / abLenSq;
            t = Mathf.Clamp01(t);
            Vector3 closest = a + ab * t;
            return (p - closest).magnitude;
        }
    }
}


