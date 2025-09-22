using System;
using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Common;
using Logic.Scripts.GameDomain.MVC.Ui;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    public class BossView : MonoBehaviour, IEffectable
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;

        [SerializeField] private BossConfigurationSO _bossConfig;
        private BossData _bossData;

        private Action<Collision> _onCollisionEnter;
        private Action<Collider> _onTriggerEnter;
        private Action<ParticleSystem> _onParticleCollisionEnter;

        public void SetupCallbacks(Action<Collision> onCollisionEnter, Action<Collider> onTriggerEnter, Action<ParticleSystem> onParticleCollisionEnter)
        {
            _onCollisionEnter = onCollisionEnter;
            _onTriggerEnter = onTriggerEnter;
            _onParticleCollisionEnter = onParticleCollisionEnter;
            if (_bossData == null && _bossConfig != null)
            {
                _bossData = new BossData(_bossConfig);
            }
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

        public void TakeDamage(int amount)
        {
            if (_bossData == null && _bossConfig != null) _bossData = new BossData(_bossConfig);
            _bossData?.TakeDamage(amount);
        }

        public void Heal(int amount)
        {
            if (_bossData == null && _bossConfig != null) _bossData = new BossData(_bossConfig);
            _bossData?.Heal(amount);
        }

        public void AddShield(int amount)
        {
            if (_bossData == null && _bossConfig != null) _bossData = new BossData(_bossConfig);
            _bossData?.AddShield(amount);
        }

        public bool IsAlive()
        {
            if (_bossData == null && _bossConfig != null) _bossData = new BossData(_bossConfig);
            return _bossData != null && _bossData.IsAlive();
        }

        public void TakeDamagePerTurn(int damageAmount, int duration) {
            throw new NotImplementedException();
        }

        public void HealPerTurn(int healAmount, int duration) {
            throw new NotImplementedException();
        }

        public void AddShieldPerTurn(int value, int duration) {
            throw new NotImplementedException();
        }

        public void Stun(int value) {
            throw new NotImplementedException();
        }

        public void SubtractActionPoints(int value) {
            throw new NotImplementedException();
        }

        public void SubtractAllActionPoints(int value) {
            throw new NotImplementedException();
        }

        public void ReduceActionPointsGain(int value) {
            throw new NotImplementedException();
        }

        public void ReduceActionPointsGainPerTurn(int valueToSubtract, int duration) {
            throw new NotImplementedException();
        }

        public void IncreaseActionPointsGainPerTurn(int valueToIncrease, int duration) {
            throw new NotImplementedException();
        }

        public void AddActionPoints(int valueToIncrease) {
            throw new NotImplementedException();
        }

        public void ReduceMovementPerTurn(int valueToSubtract, int duration) {
            throw new NotImplementedException();
        }

        public void LimitActionPointUse(int value, int duration) {
            throw new NotImplementedException();
        }
    }
}


