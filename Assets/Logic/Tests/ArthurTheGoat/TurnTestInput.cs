using UnityEngine;
using Zenject;

namespace Logic.Scripts.Turns.Ui
{
    public class TurnTestInput : MonoBehaviour
    {
        private TurnFlowController _turnFlowController;

        [Inject]
        public void Construct(TurnFlowController turnFlowController)
        {
            _turnFlowController = turnFlowController;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                _turnFlowController.StartTurns();

            if (Input.GetKeyDown(KeyCode.X))
                _turnFlowController.StopTurns();

            if (Input.GetKeyDown(KeyCode.Space))
                _turnFlowController.CompletePlayerAction();

            if (Input.GetKeyDown(KeyCode.S))
                _turnFlowController.SkipTurn();
        }
    }
}
