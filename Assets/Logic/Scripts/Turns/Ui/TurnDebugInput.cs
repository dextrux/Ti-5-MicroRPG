using UnityEngine;
using Zenject;

namespace Logic.Scripts.Turns.Ui
{
    public class TurnDebugInput : MonoBehaviour
    {
        [SerializeField] private KeyCode _enterTurnKey = KeyCode.E;
        [SerializeField] private KeyCode _exitTurnKey = KeyCode.X;
        [SerializeField] private KeyCode _playerActionDoneKey = KeyCode.Space;
        [SerializeField] private KeyCode _skipTurnKey = KeyCode.S;

        private ITurnEventBus _eventBus;

        [Inject]
        public void Construct(ITurnEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void Update()
        {
            if (Input.GetKeyDown(_enterTurnKey))
            {
                _eventBus.Publish(new RequestEnterTurnModeSignal());
            }

            if (Input.GetKeyDown(_exitTurnKey))
            {
                _eventBus.Publish(new RequestExitTurnModeSignal());
            }

            if (Input.GetKeyDown(_skipTurnKey))
            {
                _eventBus.Publish(new TurnSkippedSignal());
            }
        }
    }
}


