using System;
using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Common;

namespace Logic.Scripts.GameDomain.MVC.Nara {
    public class NaraView : MonoBehaviour, IDamageable {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;

        [SerializeField] private NaraConfigurationSO _naraConfig;
        private NaraData _naraData;

        private Action<Collision> _onCollisionEnter;
        private Action<Collider> _onTriggerEnter;
        private Action<ParticleSystem> _onParticleCollisionEnter;

        public void SetupCallbacks(Action<Collision> onCollisionEnter, Action<Collider> onTriggerEnter, Action<ParticleSystem> onParticleCollisionEnter) {
            _onCollisionEnter = onCollisionEnter;
            _onTriggerEnter = onTriggerEnter;
            _onParticleCollisionEnter = onParticleCollisionEnter;
            if (_naraData == null && _naraConfig != null) {
                _naraData = new NaraData(_naraConfig);
            }
        }

        public void RemoveAllCallbacks() {
            _onCollisionEnter = null;
            _onTriggerEnter = null;
            _onParticleCollisionEnter = null;
        }

        private void OnCollisionEnter(Collision collision) {
            _onCollisionEnter?.Invoke(collision);
        }

        private void OnParticleCollision(GameObject particleSystemGO) {
            //Provavelmente sera usado para habilidades
            _onParticleCollisionEnter?.Invoke(particleSystemGO.GetComponent<ParticleSystem>());
        }

        private void OnTriggerEnter(Collider otherCollider) {
            _onTriggerEnter?.Invoke(otherCollider);
        }

        public void TakeDamage(int amount) {
            if (_naraData == null && _naraConfig != null) _naraData = new NaraData(_naraConfig);
            _naraData?.TakeDamage(amount);
        }

        public void Heal(int amount) {
            if (_naraData == null && _naraConfig != null) _naraData = new NaraData(_naraConfig);
            _naraData?.Heal(amount);
        }

        public void AddShield(int amount) {
            if (_naraData == null && _naraConfig != null) _naraData = new NaraData(_naraConfig);
            _naraData?.AddShield(amount);
        }

        public bool IsAlive() {
            if (_naraData == null && _naraConfig != null) _naraData = new NaraData(_naraConfig);
            return _naraData != null && _naraData.IsAlive();
        }
    }
}