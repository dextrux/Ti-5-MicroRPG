using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;
using UnityEngine;

public class MouseClickInputCommand : BaseCommand, ICommandVoid {
    private INaraController _naraController;
    private ICastController _castController;
    public override void ResolveDependencies() {
        _naraController = _diContainer.Resolve<INaraController>();
        _castController = _diContainer.Resolve<ICastController>();
    }

    public void Execute() {
        _castController.UseAbility((IEffectable)_naraController);
        if (_castController?.GetCanUseAbility() == true) {
            if (_naraController?.NaraMove is NaraTurnMovementController naraTurnMovement) {
                naraTurnMovement.RecalculateRadiusAfterAbility();
                naraTurnMovement.SetMovementRadiusCenter();
                naraTurnMovement.Refresh();
                _castController.SetCanUseAbility(false);
                _naraController.Unfreeeze();
            }
        }
    }
}