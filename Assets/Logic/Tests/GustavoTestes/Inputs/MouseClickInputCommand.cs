using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.Logger.Base;

public class MouseClickInputCommand : BaseCommand, ICommandVoid
{
    public override void ResolveDependencies()
    {
        
    }

    public void Execute()
    {
        LogService.Log("Left Mouse button pressed");
    }
}