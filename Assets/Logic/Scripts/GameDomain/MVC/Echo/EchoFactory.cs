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

        public EchoView CreateEcho(int castTime, Transform referenceTransform) {
            Debug.LogWarning("Is null refTransform: " + (referenceTransform == null));
            Debug.LogWarning("Is null echoprefab: " + (_echoViewPrefab == null));
            EchoView echo = Object.Instantiate(_echoViewPrefab, referenceTransform.position, referenceTransform.rotation);
            //_echoService.EnqueueEcho(echo, castTime);
            return echo;
        }
    }
}