using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Turns;
using UnityEngine;

public class PassTurnInputCommand : BaseCommand, ICommandVoid {
    private TurnFlowController _turnFlowController;
    private INaraController _naraController;
    public override void ResolveDependencies() {
        _turnFlowController = _diContainer.Resolve<TurnFlowController>();
        _naraController = _diContainer.Resolve<INaraController>();
    }

    public void Execute() {
        if (_naraController?.NaraMove is NaraTurnMovementController naraTurnMovement) naraTurnMovement.RemoveMovementRadius();
        _turnFlowController.CompletePlayerAction();
        _naraController.Freeeze();
    }
}
