using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.Logger.Base;
using Logic.Scripts.Utils;
using System.Threading;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

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

        public void RegisterAllInputListeners()
        {
            LogService.LogTopic("Register all input listeners", LogTopicType.Inputs);
            _gameInputActions.Player.ActivateCam.started += OnActivateCamStarted;
            _gameInputActions.Player.ActivateCam.canceled += OnActivateCamCanceled;

            _gameInputActions.Player.CreateCopy1.started += OnCreateCopy1Started;
            _gameInputActions.Player.CreateCopy1.canceled += OnCreateCopy1Canceled;

            _gameInputActions.Player.CreateCopy2.started += OnCreateCopy2Started;
            _gameInputActions.Player.CreateCopy2.canceled += OnCreateCopy2Canceled;

            _gameInputActions.Player.Inspect.started += OnInspectStarted;
            _gameInputActions.Player.Inspect.canceled += OnInspectCanceled;

            _gameInputActions.Player.Interact.started += OnInteractStarted;
            _gameInputActions.Player.Interact.canceled += OnInteractCanceled;

            //_gameInputActions.Player.Move.started += OnMoveStarted;
            _gameInputActions.Player.Move.performed += OnMovePerformed;
            _gameInputActions.Player.Move.canceled += OnMoveCanceled;

            _gameInputActions.Player.PassTurn.started += OnPassTurnStarted;
            _gameInputActions.Player.PassTurn.canceled += OnPassTurnCanceled;

            _gameInputActions.Player.Pause.started += OnPauseStarted;
            _gameInputActions.Player.Pause.canceled += OnPauseCanceled;

            _gameInputActions.Player.ResetTurn.started += OnResetTurnStarted;
            _gameInputActions.Player.ResetTurn.canceled += OnResetTurnCanceled;

            _gameInputActions.Player.RotateCam.started += OnRotateCamStarted;
            _gameInputActions.Player.RotateCam.canceled += OnRotateCamCanceled;

            _gameInputActions.Player.UseAbility1.started += OnUseAbility1Started;
            _gameInputActions.Player.UseAbility1.canceled += OnUseAbility1Canceled;

            _gameInputActions.Player.UseAbility2.started += OnUseAbility2Started;
            _gameInputActions.Player.UseAbility2.canceled += OnUseAbility2Canceled;

            _gameInputActions.Player.UseAbility3.started += OnUseAbility3Started;
            _gameInputActions.Player.UseAbility3.canceled += OnUseAbility3Canceled;

            _gameInputActions.Player.UsePotion1.started += OnUsePotion1Started;
            _gameInputActions.Player.UsePotion1.canceled += OnUsePotion1Canceled;

            _gameInputActions.Player.UsePotion2.started += OnUsePotion2Started;
            _gameInputActions.Player.UsePotion2.canceled += OnUsePotion2Canceled;
            
            _gameInputActions.Player.MouseClick.started -= OnMouseClickStarted;
        }

        private void OnMouseClickStarted(InputAction.CallbackContext context)
        {
            _commandFactory.CreateCommandVoid<MouseClickInputCommand>().Execute();
        }

        private void OnUsePotion2Started(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<UsePotion2InputCommand>().Execute();
        }
        private void OnUsePotion1Started(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<UsePotion1InputCommand>().Execute();
        }
        private void OnUseAbility3Started(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<UseAbility3InputCommand>().Execute();
        }
        private void OnUseAbility2Started(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<UseAbility2InputCommand>().Execute();
        }
        private void OnUseAbility1Started(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<UseAbility1InputCommand>().Execute();
        }
        private void OnRotateCamStarted(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<RotateCamInputCommand>().Execute();
        }
        private void OnResetTurnStarted(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<ResetTurnInputCommand>().Execute();
        }
        private void OnPauseStarted(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<PauseInputCommand>().Execute();
        }
        private void OnPassTurnStarted(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<PassTurnInputCommand>().Execute();
        }
        private void OnMoveStarted(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<MoveInputCommand>().Execute();
        }
        private void OnMovePerformed(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<MoveInputCommand>().Execute();
        }
        private void OnInteractStarted(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<InteractInputCommand>().Execute();
        }
        private void OnInspectStarted(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<InspectInputCommand>().Execute();
        }
        private void OnCreateCopy2Started(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<CreateCopy2InputCommand>().Execute();
        }
        private void OnCreateCopy1Started(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<CreateCopy1InputCommand>().Execute();
        }
        private void OnActivateCamStarted(InputAction.CallbackContext obj)
        {
            _commandFactory.CreateCommandVoid<ActivateCamInputCommand>().Execute();
        }
        private void OnUsePotion2Canceled(InputAction.CallbackContext context) { }
        private void OnUsePotion1Canceled(InputAction.CallbackContext context) { }
        private void OnUseAbility3Canceled(InputAction.CallbackContext context) { }
        private void OnUseAbility2Canceled(InputAction.CallbackContext context) { }
        private void OnUseAbility1Canceled(InputAction.CallbackContext context) { }
        private void OnRotateCamCanceled(InputAction.CallbackContext context) { }
        private void OnResetTurnCanceled(InputAction.CallbackContext context) { }
        private void OnPauseCanceled(InputAction.CallbackContext context) { }
        private void OnPassTurnCanceled(InputAction.CallbackContext context) { }
        private void OnMoveCanceled(InputAction.CallbackContext context) { _commandFactory.CreateCommandVoid<StopMoveInputCommand>().Execute(); }
        private void OnInteractCanceled(InputAction.CallbackContext context) { }
        private void OnInspectCanceled(InputAction.CallbackContext context) { }
        private void OnCreateCopy2Canceled(InputAction.CallbackContext context) { }
        private void OnCreateCopy1Canceled(InputAction.CallbackContext context) { }
        private void OnActivateCamCanceled(InputAction.CallbackContext context) { _commandFactory.CreateCommandVoid<DeactivateCamInputCommand>().Execute(); }

        public void UnregisterAllInputListeners()
        {
            LogService.LogTopic("Unregister all input listeners", LogTopicType.Inputs);
            _gameInputActions.Player.ActivateCam.started -= OnActivateCamStarted;
            _gameInputActions.Player.ActivateCam.canceled -= OnActivateCamCanceled;

            _gameInputActions.Player.CreateCopy1.started -= OnCreateCopy1Started;
            _gameInputActions.Player.CreateCopy1.canceled -= OnCreateCopy1Canceled;

            _gameInputActions.Player.CreateCopy2.started -= OnCreateCopy2Started;
            _gameInputActions.Player.CreateCopy2.canceled -= OnCreateCopy2Canceled;

            _gameInputActions.Player.Inspect.started -= OnInspectStarted;
            _gameInputActions.Player.Inspect.canceled -= OnInspectCanceled;

            _gameInputActions.Player.Interact.started -= OnInteractStarted;
            _gameInputActions.Player.Interact.canceled -= OnInteractCanceled;

            //_gameInputActions.Player.Move.started -= OnMoveStarted;
            _gameInputActions.Player.Move.performed -= OnMovePerformed;
            _gameInputActions.Player.Move.canceled -= OnMoveCanceled;

            _gameInputActions.Player.PassTurn.started -= OnPassTurnStarted;
            _gameInputActions.Player.PassTurn.canceled -= OnPassTurnCanceled;

            _gameInputActions.Player.Pause.started -= OnPauseStarted;
            _gameInputActions.Player.Pause.canceled -= OnPauseCanceled;

            _gameInputActions.Player.ResetTurn.started -= OnResetTurnStarted;
            _gameInputActions.Player.ResetTurn.canceled -= OnResetTurnCanceled;

            _gameInputActions.Player.RotateCam.started -= OnRotateCamStarted;
            _gameInputActions.Player.RotateCam.canceled -= OnRotateCamCanceled;

            _gameInputActions.Player.UseAbility1.started -= OnUseAbility1Started;
            _gameInputActions.Player.UseAbility1.canceled -= OnUseAbility1Canceled;

            _gameInputActions.Player.UseAbility2.started -= OnUseAbility2Started;
            _gameInputActions.Player.UseAbility2.canceled -= OnUseAbility2Canceled;

            _gameInputActions.Player.UseAbility3.started -= OnUseAbility3Started;
            _gameInputActions.Player.UseAbility3.canceled -= OnUseAbility3Canceled;

            _gameInputActions.Player.UsePotion1.started -= OnUsePotion1Started;
            _gameInputActions.Player.UsePotion1.canceled -= OnUsePotion1Canceled;

            _gameInputActions.Player.UsePotion2.started -= OnUsePotion2Started;
            _gameInputActions.Player.UsePotion2.canceled -= OnUsePotion2Canceled;

            _gameInputActions.Player.MouseClick.started -= OnMouseClickStarted;
        }

        public async Awaitable WaitForAnyKeyPressed(CancellationTokenSource cancellationTokenSource, bool canPressOverGui = false) {
            await AwaitableUtils.WaitUntil(() => IsAnyInputPressed(), cancellationTokenSource.Token);
        }

        private bool IsAnyInputPressed() {
            return
                (Keyboard.current?.anyKey.wasPressedThisFrame == true) ||
                (Mouse.current?.leftButton.wasPressedThisFrame == true) ||
                (Mouse.current?.rightButton.wasPressedThisFrame == true) ||
                (Touchscreen.current?.primaryTouch.press.wasPressedThisFrame == true);
        }
    }
}