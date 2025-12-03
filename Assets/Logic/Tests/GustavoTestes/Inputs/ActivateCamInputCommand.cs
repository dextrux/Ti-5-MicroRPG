using Logic.Scripts.Core.Mvc.WorldCamera;
using Logic.Scripts.Services.CommandFactory;

public class ActivateCamInputCommand : BaseCommand, ICommandVoid {
    private IWorldCameraController _WorldCameraController;

    public override void ResolveDependencies() {
        _WorldCameraController = _diContainer.Resolve<IWorldCameraController>();
    }

    public void Execute() {
        _WorldCameraController.UnlockCameraRotate();
    }
}
