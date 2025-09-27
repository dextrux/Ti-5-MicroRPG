using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Turns;

public class PassTurnInputCommand : BaseCommand, ICommandVoid
{
    private TurnFlowController _turnFlowController;
    public override void ResolveDependencies()
    {
        _turnFlowController = _diContainer.Resolve<TurnFlowController>();
    }

    public void Execute()
    {
        _turnFlowController.CompletePlayerAction();
    }
}
