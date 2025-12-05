using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.StartLevel;
using Logic.Scripts.GameDomain.GameInputActions;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.StateMachineService;
using System.Threading;
using UnityEngine;

public class PortalEnterCommand : BaseCommand, ICommandAsync {
    private IStateMachineService _stateMachineService;
    private GamePlayState.Factory _gameplayStateFactory;

    private PortalEnterCommandData _portalEnterCommandData;

    public PortalEnterCommand SetData(PortalEnterCommandData portalEnterCommandData) {
        _portalEnterCommandData = portalEnterCommandData;
        return this;
    }

    public override void ResolveDependencies() {
        _stateMachineService = _diContainer.Resolve<IStateMachineService>();
        _gameplayStateFactory = _diContainer.Resolve<GamePlayState.Factory>();
    }

    public async Awaitable Execute(CancellationTokenSource cancellationTokenSource) {
        _stateMachineService.SwitchState(_gameplayStateFactory.Create(new GamePlayInitatorEnterData(1)));
    }

}
