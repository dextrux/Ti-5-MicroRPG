using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerInput : MonoBehaviour
{
    private GameInputActions gameInputsActions;
    private bool moveActive;
    private bool jumpActive = true;

    public Vector2 Direction => moveActive ? gameInputsActions.Player.Move.ReadValue<Vector2>() : Vector2.zero;
    public bool Jump => jumpActive;

    [Inject]
    public void Construct()
    {
        gameInputsActions = new GameInputActions();

        gameInputsActions.Player.Move.performed += OnMovePerformed;
        gameInputsActions.Player.Move.canceled += OnMoveCanceled;
        gameInputsActions.Player.Jump.performed += OnJumpPerformed;
        gameInputsActions.Player.Jump.canceled += OnJumpCanceled;

        gameInputsActions.Player.Enable();
    }

    private void OnMovePerformed(InputAction.CallbackContext _) => moveActive = true;
    private void OnMoveCanceled(InputAction.CallbackContext _) => moveActive = false;
    private void OnJumpPerformed(InputAction.CallbackContext _) => jumpActive = true;
    private void OnJumpCanceled(InputAction.CallbackContext _) => jumpActive = false;

    public void UseJump()
    {
        jumpActive = false;
    }

    private void OnDisable()
    {
        gameInputsActions?.Player.Disable();
    }

    private void OnDestroy()
    {
        gameInputsActions?.Dispose();
    }
}
