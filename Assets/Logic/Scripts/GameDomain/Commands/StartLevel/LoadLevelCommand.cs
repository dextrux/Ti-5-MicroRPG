using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.GameDomain.MVC.Ui;
using Logic.Scripts.Services.CommandFactory;
using System.Threading;
using UnityEngine;

public class LoadLevelCommand : BaseCommand, ICommandAsync {

    private IGamePlayUiController _gamePlayUiController;
    private ILevelScenarioController _levelScenarioController;
    private INaraController _naraController;
    private ILevelCancellationTokenService _levelCancellationTokenService;

    //To-Do adicionar efeitos do cenario

    private LoadLevelCommandData _commandData;

    public LoadLevelCommand SetEnterData(LoadLevelCommandData commandData) {
        _commandData = commandData;
        return this;
    }

    public override void ResolveDependencies() {
        _gamePlayUiController = _diContainer.Resolve<IGamePlayUiController>();
        _levelScenarioController = _diContainer.Resolve<ILevelScenarioController>();
        _naraController = _diContainer.Resolve<INaraController>();
        _levelCancellationTokenService = _diContainer.Resolve<ILevelCancellationTokenService>();
    }

    public async Awaitable Execute(CancellationTokenSource cancellationTokenSource) {
        _levelCancellationTokenService.InitCancellationToken();
        //To-Do pregame da Ui
        await CreateLevelScenario(_commandData.LevelNumber, cancellationTokenSource);
        _naraController.CreateNara();
        //To-Do nara impedir movimento ate terminar o Load
    }
    private async Awaitable CreateLevelScenario(int levelNumber, CancellationTokenSource cancellationTokenSource) {
        await _levelScenarioController.CreateLevelScenario(levelNumber, cancellationTokenSource);
        //To-Do adicionar efeitos do cenario
    }

}
