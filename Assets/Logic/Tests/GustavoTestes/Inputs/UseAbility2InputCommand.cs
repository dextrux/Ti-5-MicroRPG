using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;

public class UseAbility2InputCommand : BaseCommand, ICommandVoid
{
    private IAbilityController _abilityController;
    private INaraController _naraController;
    private const int TWO_INT_CONST = 3;
    public override void ResolveDependencies()
    {
        _abilityController = _diContainer.Resolve<IAbilityController>();
        _naraController = _diContainer.Resolve<INaraController>();
    }

    public void Execute()
    {
        _abilityController.CreateAbility(_naraController.NaraSkillSpotTransform, TWO_INT_CONST);
    }
}
