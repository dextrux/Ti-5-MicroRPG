using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;

public class UseAbility5InputCommand : BaseCommand, ICommandVoid {
    private const int FOUR_INT_CONST = 4;

    private INaraController _naraController;
    private ICastController _castController;
    public override void ResolveDependencies() {
        _naraController = _diContainer.Resolve<INaraController>();
        _castController = _diContainer.Resolve<ICastController>();
    }

    public void Execute() {
        _castController.CancelAbilityUse();
        if (_castController.TryUseAbility(FOUR_INT_CONST, (IEffectable)_naraController)) {

        }

    }
}