using Logic.Scripts.Turns;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Echo {
    public class EchoFactory {
        private readonly EchoView _echoViewPrefab;
        private readonly IEchoService _echoService;

        public EchoFactory(EchoView echoViewPrefab, IEchoService echoService) {
            _echoViewPrefab = echoViewPrefab;
            _echoService = echoService;
        }

        public EchoView CreateEcho(AbilityView abilityView, int castTime, Transform referenceTransform) {
            EchoView echo = Object.Instantiate(_echoViewPrefab, referenceTransform.position, referenceTransform.rotation);
            echo.SetAbilityToCast(abilityView);
            _echoService.EnqueueEcho(echo, castTime);
            return echo;
        }
    }
}