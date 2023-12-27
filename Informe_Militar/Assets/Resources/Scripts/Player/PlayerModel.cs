using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerModel : MonoBehaviour
{
    public bool canInter = true;
    public bool mov = true;
    public bool isGrounded = true;
    public bool canJump = false;
    public bool canRun = true;

    public bool isSprinting = false;

    public bool isPaused = false;
    public bool canPause = true;

    public bool agachado = false;
    public bool sliding = false;

    public Vector2 direction;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerControls.Gameplay.Move.performed += ReadInput;
        playerControls.Gameplay.Move.canceled += ReadInput;

        playerControls.Gameplay.Sprint.performed += Sprint;
        playerControls.Gameplay.Sprint.canceled += Sprint;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void ReadInput(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        direction.x = input.x;
        direction.y = input.y;
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValue<float>() == 1;
    }
}
