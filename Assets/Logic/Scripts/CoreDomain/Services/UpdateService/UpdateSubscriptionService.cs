using System.Collections.Generic;
using Logic.Scripts.Extensions;
using UnityEngine;

namespace Logic.Scripts.Services.UpdateService {
    public class UpdateSubscriptionService : MonoBehaviour, IUpdateSubscriptionService {
        private static readonly List<IUpdatable> _updateObservers = new List<IUpdatable>();

        private static readonly List<IFixedUpdatable> _fixedUpdateObservers = new List<IFixedUpdatable>();
        private static readonly List<IFixedUpdatable> _pendingAddFixedUpdateObservers = new List<IFixedUpdatable>();
        private static readonly List<IFixedUpdatable> _pendingRemoveFixedUpdateObservers = new List<IFixedUpdatable>();

        private static readonly List<ILateUpdatable> _lateUpdateObservers = new List<ILateUpdatable>();
        private static readonly List<ILateUpdatable> _pendingAddLateUpdateObservers = new List<ILateUpdatable>();
        private static readonly List<ILateUpdatable> _pendingRemoveLateUpdateObservers = new List<ILateUpdatable>();
        private static int _currentUpdateIndex;
        private void Update() {
            for (_currentUpdateIndex = _updateObservers.Count - 1; _currentUpdateIndex >= 0; _currentUpdateIndex--) {
                var observer = _updateObservers[_currentUpdateIndex];
                observer.ManagedUpdate();
            }
        }

        private void LateUpdate() {
            _lateUpdateObservers.AddRange(_pendingAddLateUpdateObservers);
            _pendingAddLateUpdateObservers.Clear();
            _lateUpdateObservers.RemoveElements(_pendingRemoveLateUpdateObservers);
            _pendingRemoveLateUpdateObservers.Clear();

            foreach (var observer in _lateUpdateObservers) {
                observer.ManagedLateUpdate();
            }
        }

        private void FixedUpdate() {
            _fixedUpdateObservers.AddRange(_pendingAddFixedUpdateObservers);
            _pendingAddFixedUpdateObservers.Clear();
            _fixedUpdateObservers.RemoveElements(_pendingRemoveFixedUpdateObservers);
            _pendingRemoveFixedUpdateObservers.Clear();

            foreach (var observer in _fixedUpdateObservers) {
                observer.ManagedFixedUpdate();
            }
        }

        public void RegisterUpdatable(IUpdatable observer) {
            var isCurrentlyIterating = _currentUpdateIndex > 0;
            if (isCurrentlyIterating) {
                _updateObservers.Insert(0, observer);
                _currentUpdateIndex++;
            }
            else {
                _updateObservers.Add(observer);
            }
        }

        public void UnregisterUpdatable(IUpdatable observer) {
            var isCurrentlyIterating = _currentUpdateIndex > 0;
            if (isCurrentlyIterating) {
                var indexOfObserver = _updateObservers.IndexOf(observer);
                _updateObservers.Remove(observer);

                var wasObserverAlreadyIteratedThisFrame = indexOfObserver >= _currentUpdateIndex;
                if (!wasObserverAlreadyIteratedThisFrame) {
                    _currentUpdateIndex--;
                }
            }
            else {
                _updateObservers.Remove(observer);
            }
        }

        public void RegisterLateUpdatable(ILateUpdatable observer) {
            _pendingAddLateUpdateObservers.Add(observer);
        }

        public void UnregisterLateUpdatable(ILateUpdatable observer) {
            _pendingRemoveLateUpdateObservers.Add(observer);
        }

        public void RegisterFixedUpdatable(IFixedUpdatable updatable) {
            _pendingAddFixedUpdateObservers.Add(updatable);
        }

        public void UnregisterFixedUpdatable(IFixedUpdatable updatable) {
            _pendingRemoveFixedUpdateObservers.Add(updatable);
        }
    }
}