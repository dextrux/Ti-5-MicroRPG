using UnityEngine;
using UnityEngine.InputSystem;

namespace Proto_Samuel
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private PlayerMoveData _data;

        private float _turnSmoothVelocity;

        private CharacterController _controller;
        private Vector2 _direction;
        private Vector3 _velocity;

        void Start()
        {
            _controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            _direction.x = Input.GetAxis("Horizontal");
            _direction.y = Input.GetAxis("Vertical");

            Move();
        }

        private void FixedUpdate()
        {
            _controller.Move(_velocity * Time.deltaTime);
        }

        private void Move()
        {
            if (_direction.magnitude >= 0.1f)
            {
                Vector3 moveDir = Vector3.zero;
                float currentSpeed = _data.moveSpeed;

                float targetAngle = Mathf.Atan2(_direction.x, _direction.y) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _data.turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                _controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
            }
        }
    }
}
