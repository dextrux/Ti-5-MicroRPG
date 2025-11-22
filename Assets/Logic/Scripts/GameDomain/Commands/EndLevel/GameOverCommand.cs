using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.StartLevel;
using Logic.Scripts.GameDomain.GameInputActions;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.GameDomain.MVC.Ui;
using Logic.Scripts.Services.CommandFactory;
using System.Threading;
using UnityEngine;

public class GameOverCommand : BaseCommand, ICommandAsync {
    private IGamePlayUiController _gamePlayUiController;
    private IGameInputActionsController _gameInputActionsController;
    private INaraController _naraController;
    private ICommandFactory _commandFactory;
    private ILevelsDataService _levelsDataService;

    public override void ResolveDependencies() {
        _gamePlayUiController = _diContainer.Resolve<IGamePlayUiController>();
        _gameInputActionsController = _diContainer.Resolve<IGameInputActionsController>();
        _commandFactory = _diContainer.Resolve<ICommandFactory>();
        _naraController = _diContainer.Resolve<INaraController>();
        _levelsDataService = _diContainer.Resolve<ILevelsDataService>();
    }

    public async Awaitable Execute(CancellationTokenSource cancellationTokenSource) {
        //_gamePlayUiController.ShowGameOverPanel();
        _gameInputActionsController.UnregisterAllInputListeners();
        //_naraController.DisableCallbacks();

        _commandFactory.CreateCommandVoid<DisposeLevelCommand>().Execute();
        await _commandFactory.CreateCommandAsync<LoadLevelCommand>().SetEnterData(new LoadLevelCommandData(0)).Execute(cancellationTokenSource);
        await _commandFactory.CreateCommandAsync<StartLevelCommand>().Execute(cancellationTokenSource);
    }
}
