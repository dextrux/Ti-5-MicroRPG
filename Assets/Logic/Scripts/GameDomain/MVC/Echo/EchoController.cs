using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Turns;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Echo {
    public class EchoController: IEchoController{
        private readonly List<EchoView> _echoViewList;
        private readonly EchoFactory _echoFactory;
        private readonly ICommandFactory _commandFactory;

        private const int ONE_INT = 1;
        private const int TWO_INT = 2;

        public EchoController(ICommandFactory commandFactory, EchoView echoViewPrefab, IEchoService echoService) {
            _commandFactory = commandFactory;
            _echoFactory = new EchoFactory(echoViewPrefab, echoService);
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