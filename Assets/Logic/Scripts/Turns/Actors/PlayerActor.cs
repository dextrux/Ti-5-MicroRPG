using System.Threading;
using System.Threading.Tasks;
using Logic.Scripts.GameDomain.MVC.Nara;

namespace Logic.Scripts.Turns.Actors {
    public sealed class PlayerActor : TurnActorBase {
        private readonly INaraController _nara;
        private readonly IActionPointsService _ap;
        private readonly IPlayerTurnGate _gate;

        public PlayerActor(INaraController nara, IActionPointsService ap, IPlayerTurnGate gate)
            : base("Player", TurnPhase.PlayerAct) {
            _nara = nara;
            _ap = ap;
            _gate = gate;
        }

        protected override async Task OnExecuteTurnAsync(ITurnContext ctx, CancellationToken ct) {
            _ap?.GainTurnPoints();
            if (_nara?.NaraMove is NaraTurnMovementController naraTurnMovement) {
                naraTurnMovement.ResetMovementArea();
                naraTurnMovement.LineHandlerController.SetVisible(true);
                // Espera UI/Comando sinalizar fim do turno do jogador
                if (_gate != null) await _gate.WaitForPlayerEndAsync(ct).ConfigureAwait(false);
                naraTurnMovement.LineHandlerController.SetVisible(false);
            }

        }
    }
}


