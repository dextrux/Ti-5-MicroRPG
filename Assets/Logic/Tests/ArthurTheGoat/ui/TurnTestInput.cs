using UnityEngine;
using Zenject;

namespace Logic.Tests.ArthurTheGoat.Turns
{
    public class TurnTestInput : MonoBehaviour
    {
        ITurnEventBus _bus;

        [Inject]
        public void Construct(ITurnEventBus bus)
        {
            _bus = bus;
        }

        void Update()
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


