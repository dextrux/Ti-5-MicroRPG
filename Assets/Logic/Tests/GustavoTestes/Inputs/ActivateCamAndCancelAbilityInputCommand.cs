using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Core.Mvc.WorldCamera;
using Logic.Scripts.GameDomain.MVC.Nara;

public class ActivateCamAndCancelAbilityInputCommand : BaseCommand, ICommandVoid {
    private IWorldCameraController _WorldCameraController;
    private ICastController _castController;
    private INaraController _naraController;

    public override void ResolveDependencies() {
        _WorldCameraController = _diContainer.Resolve<IWorldCameraController>();
        _castController = _diContainer.Resolve<ICastController>();
        _naraController = _diContainer.Resolve<INaraController>();
    }

    public void Execute() {
        _castController.CancelAbilityUse();
        _naraController.Unfreeeze();
        _WorldCameraController.UnlockCameraRotate();
    }
}