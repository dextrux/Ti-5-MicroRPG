using UnityEngine;
using Zenject;

namespace Logic.Scripts.Turns.Ui
{
    public class TurnDebugInput : MonoBehaviour
    {
        [SerializeField] private KeyCode _enterTurnKey = KeyCode.E;
        [SerializeField] private KeyCode _exitTurnKey = KeyCode.X;
        [SerializeField] private KeyCode _playerActionDoneKey = KeyCode.Space;

        private Logic.Scripts.Services.CommandFactory.ICommandFactory _commandFactory;

        [Inject]
        public void Construct(Logic.Scripts.Services.CommandFactory.ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        private void Update()
        {
            if (Input.GetKeyDown(_enterTurnKey))
            {
                _commandFactory.CreateCommandVoid<Logic.Scripts.GameDomain.Commands.EnterTurnModeCommand>().Execute();
            }

            if (Input.GetKeyDown(_exitTurnKey))
            {
                _commandFactory.CreateCommandVoid<Logic.Scripts.GameDomain.Commands.ExitTurnModeCommand>().Execute();
            }

            if (Input.GetKeyDown(_playerActionDoneKey))
            {
                _commandFactory.CreateCommandVoid<Logic.Scripts.GameDomain.Commands.CompletePlayerActionCommand>().Execute();
            }
        }
    }
}


