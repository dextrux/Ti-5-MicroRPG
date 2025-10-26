using System;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Nara
{
    public class NaraView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private Animator _animator;

        private Vector3 movementCenter;
        private int movementRadius;

        private LineRenderer _lineRenderer;
        private int segments = 100;

        private Camera _camera;

        public Rigidbody GetRigidbody()
        {
            return _rigidbody;
        }

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

        public Camera GetCamera()
        {
            return _camera;
        }

        void Update()
        {
            if (_rigidbody == null) return;

            float distance = Vector3.Distance(transform.position, movementCenter);

            if (distance > movementRadius)
            {
                Debug.Log("Passou Raio");
                Vector3 directionFromCenter = (transform.position - movementCenter).normalized;
                Vector3 radiusLimit = movementCenter + directionFromCenter * movementRadius;
                _rigidbody.MovePosition(new Vector3(radiusLimit.x, transform.position.y, radiusLimit.z));
            }

            if (_lineRenderer != null)
                DrawCircle();
        }

        public void SetNaraMovementAreaAgain(int radius, Vector3 moveCenter)
        {
            movementCenter = moveCenter;
            movementRadius = radius;
            DrawCircle();
        }

        private void DrawCircle()
        {
            if (_lineRenderer == null) return;

            int perCircle = segments + 1;
            int totalPoints = perCircle + 1 + perCircle;

            Vector3[] points = new Vector3[totalPoints];

            FillCircle(points, 0, movementCenter, movementRadius);
            DrawSecondCircle(points, perCircle);

            _lineRenderer.loop = false;
            _lineRenderer.positionCount = totalPoints;
            _lineRenderer.SetPositions(points);
        }

        private void DrawSecondCircle(Vector3[] buffer, int startIndex)
        {
            Vector3 playerCenter = transform.position;

            float dist = Vector3.Distance(playerCenter, movementCenter);
            float r2 = Mathf.Max(0.01f, (float)movementRadius - dist);

            Vector3 first2 = CirclePoint(playerCenter, r2, 0f, movementCenter.y + 0.25f);
            buffer[startIndex] = first2;

            int start2 = startIndex + 1;
            for (int i = 0; i <= segments; i++)
            {
                float angle = (float)i / segments * 2f * Mathf.PI;
                buffer[start2 + i] = CirclePoint(playerCenter, r2, angle, movementCenter.y + 0.25f);
            }
        }

        private void FillCircle(Vector3[] buffer, int startIndex, Vector3 center, float radius)
        {
            float y = center.y + 0.25f;
            for (int i = 0; i <= segments; i++)
            {
                float angle = (float)i / segments * 2f * Mathf.PI;
                buffer[startIndex + i] = CirclePoint(center, radius, angle, y);
            }
        }

        private Vector3 CirclePoint(Vector3 center, float radius, float angle, float y)
        {
            float x = Mathf.Cos(angle) * radius + center.x;
            float z = Mathf.Sin(angle) * radius + center.z;
            return new Vector3(x, y, z);
        }

        public void CreateLineRenderer()
        {
            _lineRenderer = gameObject.AddComponent<LineRenderer>();

            _lineRenderer.positionCount = segments + 1;
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.loop = false;

            _lineRenderer.startWidth = 1f;
            _lineRenderer.endWidth = 1;
            _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            _lineRenderer.startColor = Color.blue;
            _lineRenderer.endColor = Color.blue;

            DrawCircle();
            _lineRenderer.enabled = false;
        }

        public void SetMovementCircleVisible(bool visible)
        {
            if (_lineRenderer == null) return;
            _lineRenderer.enabled = visible;
        }
        
        public void SetMoving(bool isMoving)
        {
            if (_animator != null)
            {
                _animator.SetBool("Moving", isMoving);
            }
        }

        public void PlayDeath()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("Dead");
            }
        }

        public void SetAttackType(int type)
        {
            if (_animator != null)
            {
                _animator.SetInteger("AKY_AttackType", type);
            }
        }

        public void ResetAttackType()
        {
            if (_animator != null)
            {
                _animator.SetInteger("AKY_AttackType", 0);
            }
        }

        public void TriggerExecute()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("Execute");
            }
        }

        public void ResetExecuteTrigger()
        {
            if (_animator != null)
            {
                _animator.ResetTrigger("Execute");
            }
        }
        
    }
}
