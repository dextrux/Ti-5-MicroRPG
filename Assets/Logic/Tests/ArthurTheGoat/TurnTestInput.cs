using UnityEngine;
using Zenject;
using Logic.Scripts.Turns;

namespace Logic.Scripts.Turns.Ui
{
    public class TurnTestInput : MonoBehaviour
    {
        private ITurnEventBus _bus;

        [Inject]
        public void Construct(ITurnEventBus bus)
        {
            _bus = bus;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                _bus.Publish(new RequestEnterTurnModeSignal());

            if (Input.GetKeyDown(KeyCode.X))
                _bus.Publish(new RequestExitTurnModeSignal());

            if (Input.GetKeyDown(KeyCode.Space))
                _bus.Publish(new PlayerActionCompletedSignal());

            if (Input.GetKeyDown(KeyCode.S))
                _bus.Publish(new TurnSkippedSignal());
        }
    }
}
