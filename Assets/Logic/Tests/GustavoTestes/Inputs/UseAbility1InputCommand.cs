using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;

public class UseAbility1InputCommand : BaseCommand, ICommandVoid
{
    private IAbilityController _abilityController;
    private INaraController _naraController;
    private const int ONE_INT_CONST = 1;
    public override void ResolveDependencies() {
        _abilityController = _diContainer.Resolve<IAbilityController>();
        _naraController = _diContainer.Resolve<INaraController>();
    }

    public void Execute() {
        _abilityController.CreateAbility(_naraController.NaraSkillSpotTransform, ONE_INT_CONST);
        return; 
    }
}
