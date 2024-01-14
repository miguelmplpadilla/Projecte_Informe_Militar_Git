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
    public bool pauseShowed = false;

    public bool agachado = false;
    public bool sliding = false;

    public bool positionedInRamp = false;

    public Vector2 direction;

    public PlayerControls playerControls;

    public Animator animator;
    public Rigidbody2D rigidbody;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();

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
        isSprinting = playerControls.Gameplay.Sprint.IsPressed() && canRun;
    }

    private void ReadInput(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        direction.x = input.x;
        direction.y = input.y;

        positionedInRamp = false;
    }
}
