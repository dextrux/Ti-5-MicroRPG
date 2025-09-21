using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.Logger.Base;
using Logic.Scripts.Utils;
using System.Threading;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Logic.Scripts.GameDomain.GameInputActions {
    public class GameInputActionsController : IGameInputActionsController {
        private readonly global::GameInputActions _gameInputActions;
        private readonly ICommandFactory _commandFactory;

        public GameInputActionsController(global::GameInputActions gameInputActions, ICommandFactory commandFactory) {
            _gameInputActions = gameInputActions;
            _commandFactory = commandFactory;
        }

        public void EnableInputs() {
            LogService.LogTopic("EnableInputs", LogTopicType.Inputs);
            _gameInputActions.Enable();
        }

        public void DisableInputs() {
            LogService.LogTopic("DisableInputs", LogTopicType.Inputs);
            _gameInputActions.Disable();
        }

        public void RegisterAllInputListeners() {
            LogService.LogTopic("Register all input listeners", LogTopicType.Inputs);
            _gameInputActions.Player.ActivateCam.started += OnActivateCamStarted;
            _gameInputActions.Player.CreateCopy1.started += OnCreateCopy1Started;
            _gameInputActions.Player.CreateCopy2.started += OnCreateCopy2Started;
            _gameInputActions.Player.Inspect.started += OnInspectStarted;
            _gameInputActions.Player.Interact.started += OnInteractStarted;
            _gameInputActions.Player.Move.started += OnMoveStarted;
            _gameInputActions.Player.PassTurn.started += OnPassTurnStarted;
            _gameInputActions.Player.Pause.started += OnPauseStarted;
            _gameInputActions.Player.ResetTurn.started += OnResetTurnStarted;
            _gameInputActions.Player.RotateCam.started += OnRotateCamStarted;
            _gameInputActions.Player.UseAbility1.started += OnUseAbility1Started;
            _gameInputActions.Player.UseAbility2.started += OnUseAbility2Started;
            _gameInputActions.Player.UseAbility3.started += OnUseAbility3Started;
            _gameInputActions.Player.UsePotion1.started += OnUsePotion1Started;
            _gameInputActions.Player.UsePotion2.started += OnUsePotion2Started;
        }

        private void OnUsePotion2Started(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<UsePotion2InputCommand>();
        }

        private void OnUsePotion1Started(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<UsePotion1InputCommand>();
        }

        private void OnUseAbility3Started(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<UseAbility3InputCommand>().Execute();
        }

        private void OnUseAbility2Started(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<UseAbility2InputCommand>();
        }

        private void OnUseAbility1Started(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<UseAbility1InputCommand>();
        }

        private void OnRotateCamStarted(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<RotateCamInputCommand>();
        }

        private void OnResetTurnStarted(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<ResetTurnInputCommand>();
        }

        private void OnPauseStarted(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<PauseInputCommand>();
        }

        private void OnPassTurnStarted(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<PassTurnInputCommand>();
        }

        private void OnMoveStarted(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<MoveInputCommand>();
        }

        private void OnInteractStarted(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<InteractInputCommand>();
        }

        private void OnInspectStarted(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<InspectInputCommand>();
        }

        private void OnCreateCopy2Started(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<CreateCopy2InputCommand>();
        }

        private void OnCreateCopy1Started(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<CreateCopy1InputCommand>();
        }

        private void OnActivateCamStarted(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<ActivateCamInputCommand>();
        }

        public void UnregisterAllInputListeners() {
            LogService.LogTopic("Unregister all input listeners", LogTopicType.Inputs);
            _gameInputActions.Player.ActivateCam.started -= OnActivateCamStarted;
            _gameInputActions.Player.CreateCopy1.started -= OnCreateCopy1Started;
            _gameInputActions.Player.CreateCopy2.started -= OnCreateCopy2Started;
            _gameInputActions.Player.Inspect.started -= OnInspectStarted;
            _gameInputActions.Player.Interact.started -= OnInteractStarted;
            _gameInputActions.Player.Move.started -= OnMoveStarted;
            _gameInputActions.Player.PassTurn.started -= OnPassTurnStarted;
            _gameInputActions.Player.Pause.started -= OnPauseStarted;
            _gameInputActions.Player.ResetTurn.started -= OnResetTurnStarted;
            _gameInputActions.Player.RotateCam.started -= OnRotateCamStarted;
            _gameInputActions.Player.UseAbility1.started -= OnUseAbility1Started;
            _gameInputActions.Player.UseAbility2.started -= OnUseAbility2Started;
            _gameInputActions.Player.UseAbility3.started -= OnUseAbility3Started;
            _gameInputActions.Player.UsePotion1.started -= OnUsePotion1Started;
            _gameInputActions.Player.UsePotion2.started -= OnUsePotion2Started;
        }

        public async Awaitable WaitForAnyKeyPressed(CancellationTokenSource cancellationTokenSource, bool canPressOverGui = false) {
            await AwaitableUtils.WaitUntil(() => IsAnyInputPressed(),
                cancellationTokenSource.Token);
        }

        //Substituir por newInput system
        private bool IsAnyInputPressed() {
            return
                (Keyboard.current?.anyKey.wasPressedThisFrame == true) ||
                (Mouse.current?.leftButton.wasPressedThisFrame == true) ||
                (Mouse.current?.rightButton.wasPressedThisFrame == true) ||
                (Touchscreen.current?.primaryTouch.press.wasPressedThisFrame == true);
        }
    }
}
