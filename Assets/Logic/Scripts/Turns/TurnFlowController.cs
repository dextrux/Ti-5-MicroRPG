using Zenject;
using Logic.Scripts.Services.Logger.Base;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.GameDomain.MVC.Nara;

namespace Logic.Scripts.Turns {
    public class TurnFlowController : IInitializable, System.IDisposable {
        private readonly IActionPointsService _actionPointsService;
        private readonly IEchoService _echoService;
        private readonly IBossActionService _bossActionService;
        private readonly IEnviromentActionService _enviromentActionService;
        private readonly TurnStateService _turnStateService;
        private readonly ICommandFactory _commandFactory;
        private readonly NaraTurnMovementController _turnMovement;

        private bool _active;
        private int _turnNumber;
        private bool _waitingBoss;
        private bool _waitingPlayer;
        private TurnPhase _phase;

        public TurnFlowController(
            IActionPointsService actionPointsService,
            IEchoService echoService,
            IBossActionService bossActionService,
            IEnviromentActionService enviromentActionService,
            TurnStateService turnStateService,
            ICommandFactory commandFactory,
            INaraController naraController) {
            _actionPointsService = actionPointsService;
            _echoService = echoService;
            _bossActionService = bossActionService;
            _enviromentActionService = enviromentActionService;
            _turnStateService = turnStateService;
            _commandFactory = commandFactory;
            if (naraController.NaraMove is NaraTurnMovementController naraTurnMovement) _turnMovement = naraTurnMovement;
        }

        public void Initialize() {
        }

        public void Dispose() {
        }

        public void StartTurns() {
            if (_active) return;
            _active = true;
            _turnNumber = 0;
            _phase = TurnPhase.None;
            _actionPointsService.Reset();
            _turnStateService.EnterTurnMode();
            AdvanceTurnAsync();
        }

        public void StopTurns() {
            if (!_active) return;
            _active = false;
            _waitingBoss = false;
            _waitingPlayer = false;
            _phase = TurnPhase.None;
            _actionPointsService.Reset();
            _turnStateService.ExitTurnMode();
        }

        private async void AdvanceTurnAsync() {
            if (!_active) return;
            _turnNumber += 1;
            _phase = TurnPhase.BossAct;
            _turnStateService.AdvanceTurn(_turnNumber, _phase);
            LogService.Log($"Turno {_turnNumber} - Fase: BossAct");
            _waitingBoss = true;
            await _bossActionService.ExecuteBossTurnAsync();
            await System.Threading.Tasks.Task.Delay(1500);
            OnBossCompleted();
        }

        private void OnBossCompleted() {
            if (!_active || !_waitingBoss) return;
            _waitingBoss = false;
            StartPlayerPhase();
        }

        private void StartPlayerPhase() {
            _actionPointsService.GainTurnPoints();
            _phase = TurnPhase.PlayerAct;
            //_turnMovement.ResetMovementArea();
            _turnStateService.AdvanceTurn(_turnNumber, _phase);
            LogService.Log($"Turno {_turnNumber} - Fase: PlayerAct");
            _waitingPlayer = true;
            _commandFactory.CreateCommandVoid<Logic.Scripts.GameDomain.Commands.RecenterNaraMovementOnPlayerTurnCommand>().Execute();
            //_turnMovement?.LineHandlerController.SetVisible(true);
            _turnStateService.RequestPlayerAction();
        }

        public void SkipTurn() {
            if (!_active || !_waitingPlayer) return;
            _waitingPlayer = false;
            StartEchoPhaseAsync();
        }

        public void CompletePlayerAction() {
            if (!_active || !_waitingPlayer) return;
            _waitingPlayer = false;
            StartEchoPhaseAsync();
        }

        private async void StartEchoPhaseAsync() {
            _phase = TurnPhase.EchoesAct;
            _turnStateService.AdvanceTurn(_turnNumber, _phase);
            LogService.Log($"Turno {_turnNumber} - Fase: EchoesAct");
            await _echoService.ResolveDueEchoesAsync();
            OnEchoesCompleted();
        }

        private void OnEchoesCompleted() {
            StartEnviromentPhaseAsync();
        }

        private async void StartEnviromentPhaseAsync() {
            _phase = TurnPhase.EnviromentAct;
            _turnStateService.AdvanceTurn(_turnNumber, _phase);
            LogService.Log($"Turno {_turnNumber} - Fase: EnviromentAct");
            //_turnMovement?.LineHandlerController.SetVisible(false);
            await _enviromentActionService.ExecuteEnviromentTurnAsync();
            OnEnviromentCompleted();
        }

        private void OnEnviromentCompleted() {
            AdvanceTurnAsync();
        }
    }
}
