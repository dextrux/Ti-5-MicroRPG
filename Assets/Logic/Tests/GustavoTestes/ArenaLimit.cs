using System.Reflection;
using UnityEngine;
using Zenject;
using Logic.Scripts.Services.UpdateService;
using Logic.Scripts.Turns;

namespace Logic.Scripts.GameDomain.MVC.Environment
{
    [RequireComponent(typeof(MeshCollider))]
    public class ArenaLimit : MonoBehaviour, IUpdatable
    {
        private TurnStateService _turnStateService;
        private IUpdateSubscriptionService _updateSvc;
        private MeshCollider _meshCollider;
        private bool _lastEnabled;

        [Inject]
        public void Construct(TurnStateService turnStateService, IUpdateSubscriptionService updateSubscriptionService)
        {
            _turnStateService = turnStateService;
            _updateSvc = updateSubscriptionService;

            _meshCollider = GetComponent<MeshCollider>();
            _meshCollider.enabled = false;
            _lastEnabled = _meshCollider.enabled;

            _updateSvc.RegisterUpdatable(this);
        }

        private void OnDestroy()
        {
            _updateSvc?.UnregisterUpdatable(this);
        }

        public void ManagedUpdate()
        {
            if (_meshCollider == null || _turnStateService == null) return;

            bool shouldEnable = IsPlayerPhase(_turnStateService);
            if (shouldEnable != _lastEnabled)
            {
                _meshCollider.enabled = shouldEnable;
                _lastEnabled = shouldEnable;
            }
        }

        private static bool IsPlayerPhase(TurnStateService service)
        {
            var t = service.GetType();

            var prop = t.GetProperty("Phase", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                       ?? t.GetProperty("CurrentPhase", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null)
            {
                object v = prop.GetValue(service, null);
                if (v is TurnPhase p) return p == TurnPhase.PlayerAct;
            }

            var field = t.GetField("_phase", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null)
            {
                object v = field.GetValue(service);
                if (v is TurnPhase p) return p == TurnPhase.PlayerAct;
            }

            return false;
        }
    }
}
