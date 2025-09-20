using Zenject;

namespace Logic.Scripts.Turns
{
    public class TurnFlowController : IInitializable, System.IDisposable
    {
        private readonly ITurnEventBus _eventBus;
        private readonly IActionPointsService _actionPointsService;
        private readonly IEchoService _echoService;
        private readonly IBossActionService _bossActionService;
        private readonly IEnviromentActionService _enviromentActionService;

        private bool _active;
        private int _turnNumber;
        private bool _waitingBoss;
        private bool _waitingPlayer;
        private TurnPhase _phase;

        public TurnFlowController(
            ITurnEventBus eventBus,
            IActionPointsService actionPointsService,
            IEchoService echoService,
            IBossActionService bossActionService,
            IEnviromentActionService enviromentActionService)
        {
            _eventBus = eventBus;
            _actionPointsService = actionPointsService;
            _echoService = echoService;
            _bossActionService = bossActionService;
            _enviromentActionService = enviromentActionService;
        }

        public void Initialize()
        {
            _eventBus.Subscribe<RequestEnterTurnModeSignal>(OnRequestEnter);
            _eventBus.Subscribe<RequestExitTurnModeSignal>(OnRequestExit);
            _eventBus.Subscribe<BossActionCompletedSignal>(OnBossCompleted);
            _eventBus.Subscribe<PlayerActionCompletedSignal>(OnPlayerCompleted);
            _eventBus.Subscribe<TurnSkippedSignal>(OnTurnSkipped);
            _eventBus.Subscribe<EchoesResolutionCompletedSignal>(OnEchoesCompleted);
            _eventBus.Subscribe<EnviromentActionCompletedSignal>(OnEnviromentCompleted);
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<RequestEnterTurnModeSignal>(OnRequestEnter);
            _eventBus.Unsubscribe<RequestExitTurnModeSignal>(OnRequestExit);
            _eventBus.Unsubscribe<BossActionCompletedSignal>(OnBossCompleted);
            _eventBus.Unsubscribe<PlayerActionCompletedSignal>(OnPlayerCompleted);
            _eventBus.Unsubscribe<TurnSkippedSignal>(OnTurnSkipped);
            _eventBus.Unsubscribe<EchoesResolutionCompletedSignal>(OnEchoesCompleted);
            _eventBus.Unsubscribe<EnviromentActionCompletedSignal>(OnEnviromentCompleted);
        }

        private void OnRequestEnter(RequestEnterTurnModeSignal _)
        {
            StartTurns();
        }

        private void OnRequestExit(RequestExitTurnModeSignal _)
        {
            StopTurns();
        }

        public void StartTurns()
        {
            if (_active) return;
            _active = true;
            _turnNumber = 0;
            _phase = TurnPhase.None;
            _actionPointsService.Reset();
            _eventBus.Publish(new EnterTurnModeSignal());
            AdvanceTurn();
        }

        public void StopTurns()
        {
            if (!_active) return;
            _active = false;
            _waitingBoss = false;
            _waitingPlayer = false;
            _phase = TurnPhase.None;
            _eventBus.Publish(new ExitTurnModeSignal());
            _actionPointsService.Reset();
        }

        private void AdvanceTurn()
        {
            if (!_active) return;
            _turnNumber += 1;
            _phase = TurnPhase.BossAct;
            _eventBus.Publish(new TurnAdvancedSignal { TurnNumber = _turnNumber, Phase = _phase });
            _waitingBoss = true;
            _eventBus.Publish(new BossActionRequestedSignal());
            _bossActionService.ExecuteBossTurn();
        }

        private void OnBossCompleted(BossActionCompletedSignal _)
        {
            if (!_active || !_waitingBoss) return;
            _waitingBoss = false;
            StartPlayerPhase();
        }

        private void StartPlayerPhase()
        {
            _actionPointsService.GainTurnPoints();
            _phase = TurnPhase.PlayerAct;
            _eventBus.Publish(new TurnAdvancedSignal { TurnNumber = _turnNumber, Phase = _phase });
            _waitingPlayer = true;
            _eventBus.Publish(new RequestPlayerActionSignal());
        }

        private void OnTurnSkipped(TurnSkippedSignal _)
        {
            if (!_active || !_waitingPlayer) return;
            _waitingPlayer = false;
            StartEchoPhase();
        }

        private void OnPlayerCompleted(PlayerActionCompletedSignal _)
        {
            if (!_active || !_waitingPlayer) return;
            _waitingPlayer = false;
            StartEchoPhase();
        }

        private void StartEchoPhase()
        {
            _phase = TurnPhase.EchoesAct;
            _eventBus.Publish(new TurnAdvancedSignal { TurnNumber = _turnNumber, Phase = _phase });
            _eventBus.Publish(new EchoesResolutionRequestedSignal());
            _echoService.ResolveDueEchoes();
        }

        private void OnEchoesCompleted(EchoesResolutionCompletedSignal _)
        {
            StartEnviromentPhase();
        }

        private void StartEnviromentPhase()
        {
            _phase = TurnPhase.EnviromentAct;
            _eventBus.Publish(new TurnAdvancedSignal { TurnNumber = _turnNumber, Phase = _phase });
            _eventBus.Publish(new EnviromentActionRequestedSignal());
            _enviromentActionService.ExecuteEnviromentTurn();
        }

        private void OnEnviromentCompleted(EnviromentActionCompletedSignal _)
        {
            AdvanceTurn();
        }
    }
}
