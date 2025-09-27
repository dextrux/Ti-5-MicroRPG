using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;

namespace Logic.Scripts.GameDomain.Commands
{
    public class RecenterNaraMovementOnPlayerTurnCommand : BaseCommand, ICommandVoid
    {
        private INaraController _naraController;

        public override void ResolveDependencies()
        {
            _naraController = _diContainer.Resolve<INaraController>();
        }

        public void Execute()
        {
            _naraController.RecenterMovementAreaAtTurnStart();
        }
    }
}


