using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Turns;

public class PassTurnInputCommand : BaseCommand, ICommandVoid
{
    private ITurnEventBus _eventBus;
    public override void ResolveDependencies()
    {
        _eventBus = _diContainer.Resolve<ITurnEventBus>();
    }

    public void Execute()
    {
        _eventBus.Publish(new PlayerActionCompletedSignal());
    }
}
