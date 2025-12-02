using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.StartLevel;
using Logic.Scripts.Core.Mvc.WorldCamera;
using Logic.Scripts.GameDomain.Commands;
using Logic.Scripts.GameDomain.GameInputActions;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.GameDomain.MVC.Ui;
using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.CommandFactory;
using System.Threading;
using UnityEngine;

public class StartExplorationStateCommand : BaseCommand, ICommandAsync {
    private INaraController _naraController;
    private ICommandFactory _commandFactory;
    private IGameInputActionsController _gameInputActionsController;

    private ExplorationInitiatorEnterData _enterData;

    public StartExplorationStateCommand SetEnterData(ExplorationInitiatorEnterData enterData) {
        _enterData = enterData;
        return this;
    }

    public override void ResolveDependencies() {
        //_gamePlayAudioClipsScriptableObject = _diContainer.Resolve<GamePlayAudioClipsScriptableObject>();
        _naraController = _diContainer.Resolve<INaraController>();
        _commandFactory = _diContainer.Resolve<ICommandFactory>();
        _gameInputActionsController = _diContainer.Resolve<IGameInputActionsController>();
    }

    public async Awaitable Execute(CancellationTokenSource cancellationTokenSource) {
        _gameInputActionsController.EnableInputs();
        _gameInputActionsController.RegisterAllInputListeners();
        await _commandFactory.CreateCommandAsync<StartLevelCommand>().Execute(cancellationTokenSource);
        _naraController.InitEntryPointExploration();
    }
}
