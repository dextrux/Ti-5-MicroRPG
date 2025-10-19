using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;

public class UseAbility1InputCommand : BaseCommand, ICommandVoid {
    private IAbilityController _abilityController;
    private INaraController _naraController;
    private ICastController _castController;
    private const int ZERO_INT_CONST = 0;
    public override void ResolveDependencies() {
        _abilityController = _diContainer.Resolve<IAbilityController>();
        _naraController = _diContainer.Resolve<INaraController>();
        _castController = _diContainer.Resolve<ICastController>();
    }

    public void Execute() {
        _castController.CancelAbilityUse();
        /*if (_castController.TryUseAbility(_abilityController.ActiveAbilities[ZERO_INT_CONST], _naraController)) {

        }*/
        return;
    }
}
