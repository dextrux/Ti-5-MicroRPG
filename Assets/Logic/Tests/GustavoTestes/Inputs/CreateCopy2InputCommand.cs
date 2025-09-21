using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.Logger.Base;

public class CreateCopy2InputCommand : BaseCommand, ICommandVoid
{
    public override void ResolveDependencies()
    {
        
    }

    public void Execute()
    {
        LogService.Log("Copy2 pressed");
    }
}
