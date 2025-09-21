using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;

public class UseAbility2InputCommand : BaseCommand, ICommandVoid
{
    private IAbilityController _abilityController;
    private INaraController _naraController;
    private ICastController _castController;
    private const int TWO_INT_CONST = 2;
    public override void ResolveDependencies()
    {
        _abilityController = _diContainer.Resolve<IAbilityController>();
        _naraController = _diContainer.Resolve<INaraController>();
        _castController = _diContainer.Resolve<ICastController>();
    }

    public void Execute()
    {
        if (_castController.TryUseAbility(_abilityController.ActiveAbilities[TWO_INT_CONST], _naraController.NaraSkillSpotTransform)) {

        }
        return;
    }
}
