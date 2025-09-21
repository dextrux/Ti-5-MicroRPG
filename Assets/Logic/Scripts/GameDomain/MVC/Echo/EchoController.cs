using Logic.Scripts.Services.CommandFactory;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Echo {
    public class EchoController: IEchoController{
        private readonly List<EchoView> _echoViewList;
        private readonly EchoFactory _echoFactory;
        private readonly CommandFactory _commandFactory;

        private const int ONE_INT = 1;
        private const int TWO_INT = 2;

        public EchoController(CommandFactory commandFactory, EchoView echoViewPrefab) {
            _commandFactory = commandFactory;
            _echoFactory = new EchoFactory(echoViewPrefab);
            _echoViewList = new List<EchoView>();
        }

        public void CreateFastEcho(AbilityView castingAbility, Transform referenceTransform) {
            _echoFactory.CreateEcho(castingAbility, ONE_INT, referenceTransform);
        }

        public void CreateSlowEcho(AbilityView castingAbility, Transform referenceTransform) {
            _echoFactory.CreateEcho(castingAbility, TWO_INT, referenceTransform);
        }
    }
}