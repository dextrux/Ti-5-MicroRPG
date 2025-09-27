using System;
using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Common;
using Logic.Scripts.GameDomain.MVC.Ui;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    public class BossView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;

        private Action<Collision> _onCollisionEnter;
        private Action<Collider> _onTriggerEnter;
        private Action<ParticleSystem> _onParticleCollisionEnter;

        public void SetupCallbacks(Action<Collision> onCollisionEnter, Action<Collider> onTriggerEnter, Action<ParticleSystem> onParticleCollisionEnter)
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
            _onParticleCollisionEnter?.Invoke(particleSystemGO.GetComponent<ParticleSystem>());
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            _onTriggerEnter?.Invoke(otherCollider);
        }
    }
}


