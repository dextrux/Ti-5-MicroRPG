using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.Logger.Base;

public class MouseClickInputCommand : BaseCommand, ICommandVoid {
    private INaraController _naraController;
    private ICastController _castController;
    public override void ResolveDependencies() {
        _naraController = _diContainer.Resolve<INaraController>();
        _castController = _diContainer.Resolve<ICastController>();
    }

    public void Execute() {
        _castController.UseAbility((IEffectable)_naraController);
        if (_castController.GetCanUseAbility() == true)
        {
            _naraController.SetNewMovementArea();
            _castController.SetCanUseAbility(false);
        }
    }
}