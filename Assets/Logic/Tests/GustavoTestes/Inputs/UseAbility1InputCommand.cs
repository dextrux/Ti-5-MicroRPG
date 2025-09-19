using Logic.Scripts.Services.CommandFactory;

public class UseAbility1InputCommand : BaseCommand, ICommandVoid
{
    GameInputActions _GameInputAction;

    public override void ResolveDependencies()
    {
        _GameInputAction = _diContainer.Resolve<GameInputActions>();
    }

    public void Execute()
    {
        //Executa a ação do input
    }
}
