using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Turns;

namespace Logic.Scripts.GameDomain.Commands
{
    public class CompletePlayerActionCommand : BaseCommand, ICommandVoid
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
}
