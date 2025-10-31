using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Nara
{
    public class NaraView : MonoBehaviour
    {
        public Transform CastPoint;
        public LineRenderer CastLineRenderer;
        public GameObject TargetPrefab;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private Animator _animator;

        private Vector3 movementCenter;
        private int movementRadius;

        private int segments = 100;
        private Camera _camera;

        private NaraAreaLineHandler _areaHandler;
        private GameObject _primaryHost;
        private GameObject _secondaryHost;

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
                Vector3 directionFromCenter = (transform.position - movementCenter).normalized;
                Vector3 radiusLimit = movementCenter + directionFromCenter * movementRadius;
                _rigidbody.MovePosition(new Vector3(radiusLimit.x, transform.position.y, radiusLimit.z));
            }

            if (_areaHandler != null)
                _areaHandler.Refresh(movementCenter, movementRadius, transform.position);
        }

        public void SetNaraMovementAreaAgain(int radius, Vector3 moveCenter)
        {
            movementCenter = moveCenter;
            movementRadius = radius;
            if (_areaHandler != null)
                _areaHandler.Refresh(movementCenter, movementRadius, transform.position);
        }

        public void CreateLineRenderer()
        {
            if (_primaryHost == null)
            {
                _primaryHost = new GameObject("NaraArea_PrimaryCircle");
                _primaryHost.transform.SetParent(transform, false);
            }
            if (_secondaryHost == null)
            {
                _secondaryHost = new GameObject("NaraArea_SecondaryCircle");
                _secondaryHost.transform.SetParent(transform, false);
            }
            _areaHandler = new NaraAreaLineHandler(_primaryHost, _secondaryHost, segments);
            _areaHandler.Refresh(movementCenter, movementRadius, transform.position);
            _areaHandler.SetVisible(false);
        }

        public void SetMovementCircleVisible(bool visible)
        {
            if (_areaHandler == null) return;
            _areaHandler.SetVisible(visible);
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

        public LineRenderer GetPointLineRenderer()
        {
            return CastLineRenderer;
        }

        public void SetPosition()
        {
            _rigidbody.position = movementCenter;
        }
    }
}
