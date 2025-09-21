using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.Logger.Base;

public class MoveInputCommand : BaseCommand, ICommandVoid
{
    public override void ResolveDependencies()
    {

    }

    public void Execute()
    {
        LogService.Log("Move Pressed");
    }
}
