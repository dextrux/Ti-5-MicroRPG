using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Turns;

namespace Logic.Scripts.GameDomain.Commands {
    public class EnterTurnModeCommand : BaseCommand, ICommandVoid {
        private TurnFlowController _turnFlowController;
        private IBossActionService _bossActionService;
        private IEnviromentActionService _enviromentActionService;
        private INaraController _naraController;

        public override void ResolveDependencies() {
            _turnFlowController = _diContainer.Resolve<TurnFlowController>();
            _bossActionService = _diContainer.Resolve<IBossActionService>();
            _enviromentActionService = _diContainer.Resolve<IEnviromentActionService>();
            _naraController = _diContainer.Resolve<INaraController>();
        }

        public void Execute() {
            _turnFlowController.Initialize(_bossActionService, _enviromentActionService, (NaraTurnMovementController)_naraController.NaraMove);
        }
    }
}
