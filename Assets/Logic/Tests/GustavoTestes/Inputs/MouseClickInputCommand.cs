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
        if (_castController.GetCanUseAbility() == true) {
            _castController.UseAbility((IEffectable)_naraController);
            if (_naraController?.NaraMove is NaraTurnMovementController naraTurnMovement) {
                naraTurnMovement.RecalculateRadiusAfterAbility();
                naraTurnMovement.SetMovementRadiusCenter();
                naraTurnMovement.Refresh();
            }
            _castController.SetCanUseAbility(false);
        }
    }
}