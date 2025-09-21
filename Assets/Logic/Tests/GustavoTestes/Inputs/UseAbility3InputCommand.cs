using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;

public class UseAbility3InputCommand : BaseCommand, ICommandVoid
{
    private IAbilityController _abilityController;
    private INaraController _naraController;
    private const int THREE_INT_CONST = 3;
    public override void ResolveDependencies()
    {
        _abilityController = _diContainer.Resolve<IAbilityController>();
        _naraController = _diContainer.Resolve<INaraController>();
    }

    public void Execute()
    {
        return;
    }
}
