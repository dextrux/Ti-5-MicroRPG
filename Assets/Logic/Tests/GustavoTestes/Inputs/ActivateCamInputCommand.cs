using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Core.Mvc.WorldCamera;

public class ActivateCamInputCommand : BaseCommand, ICommandVoid
{
    private IWorldCameraController _WorldCameraController;
    private ICastController _castController;

    public override void ResolveDependencies()
    {
        _WorldCameraController = _diContainer.Resolve<IWorldCameraController>();
        //_castController = _diContainer.Resolve<ICastController>();
    }

    public void Execute()
    {
        //_castController.CancelAbilityUse();
        _WorldCameraController.UnlockCameraRotate();
    }
}