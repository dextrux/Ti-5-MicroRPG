using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.Logger.Base;
using Logic.Scripts.Utils;
using System.Threading;
using UnityEngine.InputSystem;
using UnityEngine;
using Logic.Scripts.GameDomain.Commands;
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

        public void RegisterAllInputListeners() {
            LogService.LogTopic("Register all input listeners", LogTopicType.Inputs);
            _gameInputActions.Player.Aim.started += OnAimStarted;
        }

        public void UnregisterAllInputListeners() {
            LogService.LogTopic("Unregister all input listeners", LogTopicType.Inputs);
            _gameInputActions.Player.Aim.started -= OnAimStarted;
        }

        private void OnAimStarted(InputAction.CallbackContext context) {
            _commandFactory.CreateCommandVoid<UnlockCameraInvokedCommand>().Execute();
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
