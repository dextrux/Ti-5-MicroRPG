using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Echo {
    public class EchoView : MonoBehaviour {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _rotationSpeed;
        private Vector3 dir;
        public AbilityView AbilityToCast { get; private set; }
        public int TimeToCast { get; private set; }
        [field: SerializeField] public Transform CastPosition { get; private set; }

        public void SetAbilityToCast(AbilityView ability) {
            AbilityToCast = ability;
        }

        public void CastTime(int castTime) {
            TimeToCast = castTime;
        }

        public void LookAt(Vector3 lookpoint) {
            dir = lookpoint - transform.position;
            Quaternion rotate = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.fixedDeltaTime * _rotationSpeed);
        }
    }
}