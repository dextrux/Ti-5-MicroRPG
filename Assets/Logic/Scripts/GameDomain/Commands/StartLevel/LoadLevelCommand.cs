using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.GameDomain.MVC.Ui;
using Logic.Scripts.Services.CommandFactory;
using System;
using System.Threading;
using UnityEngine;

public class LoadLevelCommand : BaseCommand, ICommandAsync {

    private IGamePlayUiController _gamePlayUiController;
    private ILevelScenarioController _levelScenarioController;
    private INaraController _naraController;
    private ILevelCancellationTokenService _levelCancellationTokenService;
    private ILevelsDataService _levelsDataService;
    private INaraMovementControllerFactory _naraMovementControllerFactory;
    private IGamePlayDataService _gamePlayDataService;
    private IPortalController _portalController;

    //To-Do adicionar efeitos do cenario

    private LoadLevelCommandData _commandData;

    public LoadLevelCommand SetEnterData(LoadLevelCommandData commandData) {
        _commandData = commandData;
        return this;
    }

    public override void ResolveDependencies() {
        Debug.Log("LoadLevelCommand Dependecies");
        _gamePlayUiController = _diContainer.Resolve<IGamePlayUiController>();
        _levelScenarioController = _diContainer.Resolve<ILevelScenarioController>();
        _naraController = _diContainer.Resolve<INaraController>();
        _levelCancellationTokenService = _diContainer.Resolve<ILevelCancellationTokenService>();
        _levelsDataService = _diContainer.Resolve<ILevelsDataService>();
        _gamePlayDataService = _diContainer.Resolve<IGamePlayDataService>();
        _naraMovementControllerFactory = _diContainer.Resolve<INaraMovementControllerFactory>();
        _portalController = _diContainer.Resolve<IPortalController>();
    }

    public async Awaitable Execute(CancellationTokenSource cancellationTokenSource) {
        _levelCancellationTokenService.InitCancellationToken();
        //To-Do pregame da Ui
        int levelNumber = _commandData.LevelNumber;
        _gamePlayDataService.SetCurrentLevelNumber(levelNumber);
        await CreateLevelScenario(levelNumber, cancellationTokenSource);
        NaraMovementController movementController = _naraMovementControllerFactory.Create(_levelsDataService.GetLevelData(levelNumber).ControllerType, _levelsDataService.GetLevelData(levelNumber).NaraLevelConfiguration);
        _naraController.CreateNara(movementController);
        //To-Do nara impedir movimento ate terminar o Load
    }
    private async Awaitable CreateLevelScenario(int levelNumber, CancellationTokenSource cancellationTokenSource) {
        await _levelScenarioController.CreateLevelScenario(levelNumber, cancellationTokenSource);
        _portalController.SetUpPortals(_levelScenarioController.CurrentLevelScenarioView.PortalViews);
        //To-Do adicionar efeitos do cenario
    }

}
