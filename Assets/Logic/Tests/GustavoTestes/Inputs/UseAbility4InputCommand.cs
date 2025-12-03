using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;

public class UseAbility4InputCommand : BaseCommand, ICommandVoid
{
    private const int THREE_INT_CONST = 3;

    private INaraController _naraController;
    private ICastController _castController;
    public override void ResolveDependencies() {
        _naraController = _diContainer.Resolve<INaraController>();
        _castController = _diContainer.Resolve<ICastController>();
    }

    public void Execute() {
        _castController.CancelAbilityUse();
        if (_castController.TryUseAbility(THREE_INT_CONST, (IEffectable)_naraController)) {
            _naraController.Freeeze();
        }
    }
}