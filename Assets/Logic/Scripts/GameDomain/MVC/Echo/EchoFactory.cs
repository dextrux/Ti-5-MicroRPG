using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Echo {
    public class EchoFactory {
        private readonly EchoView _echoViewPrefab;

        public EchoFactory(EchoView echoViewPrefab) {
            _echoViewPrefab = echoViewPrefab;
        }

        public EchoView CreateEcho(AbilityView abilityView, int castTime, Transform referenceTransform) {
            EchoView echo = Object.Instantiate(_echoViewPrefab, referenceTransform.position, referenceTransform.rotation);
            echo.SetAbilityToCast(abilityView);
            echo.CastTime(castTime);
            return echo;
        }
    }
}