using System;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Nara
{
    public class NaraView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        public Transform SkillSpawnSpot;

        private Action<Collision> _onCollisionEnter;
        private Action<Collider> _onTriggerEnter;
        private Action<ParticleSystem> _onParticleCollisionEnter;

        private Vector3 movementCenter;
        private int movementRadius;

        private LineRenderer _lineRenderer;
        private int segments = 100;

        public void SetupCallbacks(Action<Collision> onCollisionEnter, Action<Collider> onTriggerEnter,
            Action<ParticleSystem> onParticleCollisionEnter)
        {
            _onCollisionEnter = onCollisionEnter;
            _onTriggerEnter = onTriggerEnter;
            _onParticleCollisionEnter = onParticleCollisionEnter;
        }

        public void RemoveAllCallbacks()
        {
            _onCollisionEnter = null;
            _onTriggerEnter = null;
            _onParticleCollisionEnter = null;
        }

        private void OnCollisionEnter(Collision collision)
        {
            _onCollisionEnter?.Invoke(collision);
        }

        private void OnParticleCollision(GameObject particleSystemGO)
        {
            //Provavelmente sera usado para habilidades
            _onParticleCollisionEnter?.Invoke(particleSystemGO.GetComponent<ParticleSystem>());
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            _onTriggerEnter?.Invoke(otherCollider);
        }

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

        void Update()
        {
            if (_rigidbody == null || movementCenter == null) return;

            float distance = Vector3.Distance(transform.position, movementCenter);

            if (distance > movementRadius)
            {
                Debug.Log("Passou Raio");
                Vector3 directionFromCenter = (transform.position - movementCenter).normalized;
                Vector3 radiusLimit = movementCenter + directionFromCenter * movementRadius;
                _rigidbody.MovePosition(new Vector3(radiusLimit.x, transform.position.y, radiusLimit.z));
            }
        }

        

        
    }
    
}