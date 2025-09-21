using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Core.Mvc.WorldCamera;
using Logic.Scripts.Services.Logger.Base;

public class DeactivateCamInputCommand : BaseCommand, ICommandVoid
{
    private IWorldCameraController _iWorldCameraController;

    public override void ResolveDependencies()
    {
        _iWorldCameraController = _diContainer.Resolve<IWorldCameraController>();
    }

    public void Execute()
    {
        LogService.Log("Cam released");
        _iWorldCameraController.LockCameraRotate();
    }
}
