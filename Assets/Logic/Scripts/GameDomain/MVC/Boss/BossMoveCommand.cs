using Logic.Scripts.Services.CommandFactory;
using UnityEngine;
using Zenject;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    public class BossMoveCommand : ICommandVoid
    {
        private BossView _bossView;
        private UnityEngine.Vector3 _direction;
        private float _distance;

        public void SetObjectResolver(DiContainer diContainer) { }
        public void ResolveDependencies() { _bossView = UnityEngine.Object.FindFirstObjectByType<BossView>(); }

        public BossMoveCommand SetStep(UnityEngine.Vector3 direction, float distance)
        {
            _direction = direction;
            _distance = distance;
            return this;
        }

        public void Execute() {
            if (_bossView == null) return;
            Transform t = _bossView.transform;
            t.position = t.position + _direction.normalized * _distance;
        }
    }
}
