using Logic.Scripts.Core.Mvc.WorldCamera;
using Logic.Scripts.GameDomain.GameInputActions;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;

public class DisposeLevelCommand : BaseCommand, ICommandVoid {
    private ILevelScenarioController _levelScenarioController;
    private INaraController _naraController;
    private IWorldCameraController _iWorldCameraController;
    private ILevelCancellationTokenService _levelCancellationTokenService;

    private bool _shouldReleaseFromMemory;

    public DisposeLevelCommand SetShouldReleaseAssetsFromMemory(bool shouldReleaseFromMemory) {
        _shouldReleaseFromMemory = shouldReleaseFromMemory;
        return this;
    }

    public override void ResolveDependencies() {
        _levelScenarioController = _diContainer.Resolve<ILevelScenarioController>();
        _naraController = _diContainer.Resolve<INaraController>();
        _iWorldCameraController = _diContainer.Resolve<IWorldCameraController>();
        _levelCancellationTokenService = _diContainer.Resolve<ILevelCancellationTokenService>();
    }

    public void Execute() {
        _levelCancellationTokenService.CancelCancellationToken();
        _iWorldCameraController.StopFollowTarget();
        _levelScenarioController.DestroyScenario(_shouldReleaseFromMemory);
        _naraController.ResetController();
    }
}