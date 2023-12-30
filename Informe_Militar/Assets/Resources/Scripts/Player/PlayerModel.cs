using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerModel : MonoBehaviour
{
    public bool canInter = true;
    public bool mov = true;
    public bool isGrounded = true;
    public bool canRun = true;

    public bool isSprinting = false;

    public bool isPaused = false;
    public bool canPause = true;

    public bool agachado = false;
    public bool sliding = false;

    public bool positionedInRamp = false;

    public Vector2 direction;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerControls.Gameplay.Move.performed += ReadInput;
        playerControls.Gameplay.Move.canceled += ReadInput;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        isSprinting = playerControls.Gameplay.Sprint.IsPressed();
    }

    private void ReadInput(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        direction.x = input.x;
        direction.y = input.y;

        positionedInRamp = false;
    }
}
