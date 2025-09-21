using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Core.Mvc.WorldCamera;

public class DeactivateCamInputCommand : BaseCommand, ICommandVoid
{
    private IWorldCameraController _iWorldCameraController;

    public override void ResolveDependencies()
    {
        _iWorldCameraController = _diContainer.Resolve<IWorldCameraController>();
    }

    public void Execute()
    {
        _iWorldCameraController.LockCameraRotate();
    }
}
