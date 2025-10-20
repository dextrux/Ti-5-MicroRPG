using System;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Boss {
    public class BossView : MonoBehaviour, IEffectable {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;

        private Action<int> _onPreviewHeal;
        private Action<int> _onPreviewDamage;
        private Action<int> _onTakeDamage;
        private Action<int> _onHeal;

        public void SetupCallbacks(Action<int> onPreviewHeal, Action<int> onPreviewDamage,
            Action<int> onTakeDamage, Action<int> onHeal) {
            _onPreviewHeal = onPreviewHeal;
            _onPreviewDamage = onPreviewDamage;
            _onTakeDamage = onTakeDamage;
            _onHeal = onHeal;
        }

        public void RemoveAllCallbacks() {
        }

        public Rigidbody GetRigidbody() {
            return _rigidbody;
        }

        public Transform GetReferenceTransform() {
            return transform;
        }

        public void PreviewHeal(int healAmound) {
            _onPreviewHeal?.Invoke(healAmound);
        }

        public void PreviewDamage(int damageAmound) {
            _onPreviewDamage?.Invoke(damageAmound);
        }

        public void ResetPreview() {
            throw new NotImplementedException();
        }

        public void TakeDamage(int damageAmount) {
            Debug.Log("Take damage bossView");
            _onTakeDamage?.Invoke(damageAmount);
        }

        public void TakeDamagePerTurn(int damageAmount, int duration) {
            throw new NotImplementedException();
        }

        public void Heal(int healAmount) {
            _onHeal?.Invoke(healAmount);
        }

        public void HealPerTurn(int healAmount, int duration) {
            throw new NotImplementedException();
        }
    }
}


