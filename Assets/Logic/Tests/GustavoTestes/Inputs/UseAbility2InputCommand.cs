using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.Logger.Base;

public class UseAbility2InputCommand : BaseCommand, ICommandVoid
{
    public override void ResolveDependencies()
    {
        
    }

    public void Execute()
    {
        LogService.Log("Ability 2 pressed");
    }
}
