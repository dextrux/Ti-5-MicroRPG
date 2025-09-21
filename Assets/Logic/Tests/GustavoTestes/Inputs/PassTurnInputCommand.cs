using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.Logger.Base;

public class PassTurnInputCommand : BaseCommand, ICommandVoid
{
    public override void ResolveDependencies()
    {
        
    }

    public void Execute()
    {
        LogService.Log("Pass turn pressed");
    }
}
