using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Turns;
using Logic.Scripts.Turns.Actors;

namespace Logic.Scripts.GameDomain.Commands
{
    public class CompletePlayerActionCommand : BaseCommand, ICommandVoid
    {
        private TurnFlowController _turnFlowController;
		private IPlayerTurnGate _playerTurnGate;

        public override void ResolveDependencies()
        {
            _turnFlowController = _diContainer.Resolve<TurnFlowController>();
			_playerTurnGate = _diContainer.Resolve<IPlayerTurnGate>();
        }

        public void Execute()
        {
			// Sinaliza o novo gate para o fluxo baseado em atores
			_playerTurnGate?.SignalPlayerEndedTurn();

			// Mantém compatibilidade com o fluxo atual enquanto migramos
            _turnFlowController.CompletePlayerAction();
        }
    }
}
