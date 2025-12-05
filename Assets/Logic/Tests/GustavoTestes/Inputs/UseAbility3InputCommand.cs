using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;

public class UseAbility3InputCommand : BaseCommand, ICommandVoid
{
    private const int TWO_INT_CONST = 2;

    private INaraController _naraController;
    private ICastController _castController;
    public override void ResolveDependencies() {
        _naraController = _diContainer.Resolve<INaraController>();
        _castController = _diContainer.Resolve<ICastController>();
    }

    public void Execute() {
        _castController.CancelAbilityUse();
        if (_castController.TryUseAbility(TWO_INT_CONST, (IEffectable)_naraController)) {
            _naraController.Freeeze();
        }
    }
}
