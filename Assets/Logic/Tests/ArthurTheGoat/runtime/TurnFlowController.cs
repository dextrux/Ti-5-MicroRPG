using Zenject;

namespace Logic.Tests.ArthurTheGoat.Turns
{
    public class TurnFlowController : IInitializable, System.IDisposable
    {
        readonly ITurnEventBus _eventBus;
        readonly IActionPointsService _actionPointsService;
        readonly IEchoService _echoService;
        readonly IBossActionService _bossActionService;

        bool _active;
        int _turnNumber;
        bool _waitingBoss;
        bool _waitingPlayer;
        TurnPhase _phase;

        public TurnFlowController(
            ITurnEventBus eventBus,
            IActionPointsService actionPointsService,
            IEchoService echoService,
            IBossActionService bossActionService)
        {
            _eventBus = eventBus;
            _actionPointsService = actionPointsService;
            _echoService = echoService;
            _bossActionService = bossActionService;
        }

        public void Initialize()
        {
            _eventBus.Subscribe<RequestEnterTurnModeSignal>(OnRequestEnter);
            _eventBus.Subscribe<RequestExitTurnModeSignal>(OnRequestExit);
            _eventBus.Subscribe<BossActionCompletedSignal>(OnBossCompleted);
            _eventBus.Subscribe<PlayerActionCompletedSignal>(OnPlayerCompleted);
            _eventBus.Subscribe<TurnSkippedSignal>(OnTurnSkipped);
            _eventBus.Subscribe<EchoesResolutionCompletedSignal>(OnEchoesCompleted);
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<RequestEnterTurnModeSignal>(OnRequestEnter);
            _eventBus.Unsubscribe<RequestExitTurnModeSignal>(OnRequestExit);
            _eventBus.Unsubscribe<BossActionCompletedSignal>(OnBossCompleted);
            _eventBus.Unsubscribe<PlayerActionCompletedSignal>(OnPlayerCompleted);
            _eventBus.Unsubscribe<TurnSkippedSignal>(OnTurnSkipped);
            _eventBus.Unsubscribe<EchoesResolutionCompletedSignal>(OnEchoesCompleted);
        }

        void OnRequestEnter(RequestEnterTurnModeSignal _)
        {
            StartTurns();
        }

        void OnRequestExit(RequestExitTurnModeSignal _)
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

        void AdvanceTurn()
        {
            if (!_active) return;
            _turnNumber += 1;
            _phase = TurnPhase.BossAct;
            _eventBus.Publish(new TurnAdvancedSignal { TurnNumber = _turnNumber, Phase = _phase });
            _waitingBoss = true;
            _eventBus.Publish(new BossActionRequestedSignal());
            _bossActionService.ExecuteBossTurn();
        }

        void OnBossCompleted(BossActionCompletedSignal _)
        {
            if (!_active || !_waitingBoss) return;
            _waitingBoss = false;
            StartPlayerPhase();
        }

        void StartPlayerPhase()
        {
            _actionPointsService.GainTurnPoints();
            _phase = TurnPhase.PlayerAct;
            _eventBus.Publish(new TurnAdvancedSignal { TurnNumber = _turnNumber, Phase = _phase });
            _waitingPlayer = true;
            _eventBus.Publish(new RequestPlayerActionSignal());
        }

        void OnTurnSkipped(TurnSkippedSignal _)
        {
            if (!_active || !_waitingPlayer) return;
            _waitingPlayer = false;
            StartEchoPhase();
        }

        void OnPlayerCompleted(PlayerActionCompletedSignal _)
        {
            if (!_active || !_waitingPlayer) return;
            _waitingPlayer = false;
            StartEchoPhase();
        }

        void StartEchoPhase()
        {
            _phase = TurnPhase.EchoesAct;
            _eventBus.Publish(new TurnAdvancedSignal { TurnNumber = _turnNumber, Phase = _phase });
            _eventBus.Publish(new EchoesResolutionRequestedSignal());
            _echoService.ResolveDueEchoes();
        }

        void OnEchoesCompleted(EchoesResolutionCompletedSignal _)
        {
            AdvanceTurn();
        }
    }
}


