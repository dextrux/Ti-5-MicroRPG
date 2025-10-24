using System;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Nara
{
    public class NaraView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;

        private Vector3 movementCenter;
        private int movementRadius;

        private LineRenderer _lineRenderer;
        private LineRenderer _lineRenderer2;

        private int segments = 100;
        private Camera _camera;

        public Rigidbody GetRigidbody() => _rigidbody;

        public void SetNaraCenterView(Vector3 moveCenter)
        {
            movementCenter = moveCenter;
        }

        public void SetNaraRadiusView(int radius)
        {
            movementRadius = radius;
        }

        public void SetCamera()
        {
            _camera = Camera.main;
        }

        public Camera GetCamera() => _camera;

        void Update()
        {
            if (_rigidbody == null) return;

            float distance = Vector3.Distance(transform.position, movementCenter);

            if (distance > movementRadius)
            {
                Vector3 directionFromCenter = (transform.position - movementCenter).normalized;
                Vector3 radiusLimit = movementCenter + directionFromCenter * movementRadius;
                _rigidbody.MovePosition(new Vector3(radiusLimit.x, transform.position.y, radiusLimit.z));
            }
        }

        public void SetNaraMovementAreaAgain(int radius, Vector3 moveCenter)
        {
            movementCenter = moveCenter;
            movementRadius = radius;

            if (_lineRenderer != null)
                DrawCircle(_lineRenderer, movementCenter, movementRadius);
        }

        public void CreateLineRenderer()
        {
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
            InitLineRenderer(_lineRenderer, Color.blue);
            DrawCircle(_lineRenderer, movementCenter, movementRadius);
            _lineRenderer.enabled = false;
        }

        public void SetMovementCircleVisible(bool visible)
        {
            if (_lineRenderer == null) return;
            _lineRenderer.enabled = visible;
        }

        public void CreateSecondCircleRenderer(int radius, Vector3 center, Color? color = null)
        {
            if (_lineRenderer2 == null)
                _lineRenderer2 = gameObject.AddComponent<LineRenderer>();

            InitLineRenderer(_lineRenderer2, color ?? Color.red);
            DrawCircle(_lineRenderer2, center, radius);
            _lineRenderer2.enabled = true;
        }

        public void SetSecondMovementCircleVisible(bool visible)
        {
            if (_lineRenderer2 == null) return;
            _lineRenderer2.enabled = visible;
        }

        private void InitLineRenderer(LineRenderer lr, Color color)
        {
            lr.positionCount = segments;
            lr.useWorldSpace = true;
            lr.loop = true;
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = color;
            lr.endColor = color;
        }

        private void DrawCircle(LineRenderer lr, Vector3 center, float radius, float yOffset = 0.25f)
        {
            if (lr == null) return;

            if (lr.positionCount != segments)
                lr.positionCount = segments;

            Vector3[] points = new Vector3[segments];

            for (int i = 0; i < segments; i++)
            {
                float angle = (float)i / segments * 2f * Mathf.PI;
                float x = Mathf.Cos(angle) * radius + center.x;
                float z = Mathf.Sin(angle) * radius + center.z;
                points[i] = new Vector3(x, center.y + yOffset, z);
            }

            lr.SetPositions(points);
        }
    }
}
