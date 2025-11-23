using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.StartLevel;
using Logic.Scripts.GameDomain.GameInputActions;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;
using System.Threading;
using UnityEngine;

public class PortalEnterCommand : BaseCommand, ICommandAsync {
    private ICommandFactory _commandFactory;
    private INaraController _naraController;
    private IGameInputActionsController _gameInputActionsController;

    private PortalEnterCommandData _portalEnterCommandData;

    public PortalEnterCommand SetData(PortalEnterCommandData portalEnterCommandData) {
        _portalEnterCommandData = portalEnterCommandData;
        return this;
    }

    public override void ResolveDependencies() {
        _commandFactory = _diContainer.Resolve<ICommandFactory>();
        _naraController = _diContainer.Resolve<INaraController>();
        _gameInputActionsController = _diContainer.Resolve<IGameInputActionsController>();
    }

    public async Awaitable Execute(CancellationTokenSource cancellationTokenSource) {
        _gameInputActionsController.UnregisterAllInputListeners();
        //_naraController.DisableCallbacks();
        _commandFactory.CreateCommandVoid<DisposeLevelCommand>().SetShouldReleaseAssetsFromMemory(true).Execute();
        await _commandFactory.CreateCommandAsync<LoadLevelCommand>().SetEnterData(new LoadLevelCommandData(_portalEnterCommandData.LevelToEnter)).Execute(cancellationTokenSource);
        await _commandFactory.CreateCommandAsync<StartLevelCommand>().Execute(cancellationTokenSource);
    }

}
