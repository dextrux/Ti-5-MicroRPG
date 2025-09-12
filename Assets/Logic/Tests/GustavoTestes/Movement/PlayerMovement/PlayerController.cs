using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    private IMovement movement;
    private PlayerInput input;
    private PlayerData data;

    [Inject]
    public void Construct(IMovement movement, PlayerInput input, PlayerData data)
    {
        this.movement = movement;
        this.input = input;
        this.data = data;
    }

    private void Update()
    {
        if (movement == null || input == null || data == null) return;
        movement.Move(input.Direction, data.velocity, data.rotation);

        if (input.Jump)
        {
            movement.Jump(data.jumpForce, data.gravity);
            input.UseJump();
        }
    }
}
