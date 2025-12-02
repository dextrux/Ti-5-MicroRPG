using Logic.Scripts.GameDomain.GameInputActions;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Utils;
using System.Threading;
using UnityEngine;

public class ExitGamePlayStateCommand : BaseCommand, ICommandVoid {

    private ICommandFactory _commandFactory;
    private IGameInputActionsController _gameInputActionsController;

    public override void ResolveDependencies() {
        _commandFactory = _diContainer.Resolve<ICommandFactory>();
        _gameInputActionsController = _diContainer.Resolve<GameInputActionsController>();
    }

    public void Execute() {
        _commandFactory.CreateCommandVoid<DisposeLevelCommand>().SetShouldReleaseAssetsFromMemory(true).Execute();
        _gameInputActionsController.DisableInputs();
        return;
    }
}
