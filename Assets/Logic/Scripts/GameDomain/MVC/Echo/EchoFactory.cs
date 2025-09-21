using Logic.Scripts.GameDomain.MVC.Abilitys;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Echo {
    public class EchoFactory {
        private readonly EchoView _echoViewPrefab;

        public EchoFactory(EchoView echoViewPrefab) {
            _echoViewPrefab = echoViewPrefab;
        }

        public EchoView CreateEcho(AbilityData abilityData) {
            EchoView echo = Object.Instantiate(_echoViewPrefab);
            echo.SetAbilityToCast(abilityData);
            return echo;
        }
    }
}