using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClimbController : MonoBehaviour
{
    public Transform rayPosition1;
    public Transform rayPosition2;

    public float distanceCheckRay = 1;

    private PlayerModel playerModel;
    private Animator animatorPlayer;
    private Rigidbody2D rb;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerModel = GetComponent<PlayerModel>();
        animatorPlayer = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        rayPosition1 = transform.GetChild(2);
        rayPosition2 = transform;
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private async void Update()
    {
        if (!playerModel.mov || playerModel.isPaused) return;

        Vector3 positionRay2 = rayPosition2.position + new Vector3(0, 0.05f, 0);

        Debug.DrawRay(rayPosition1.position,
            Vector2.right * transform.localScale.x * distanceCheckRay,
            Color.red);
        Debug.DrawRay(positionRay2,
            Vector2.right * transform.localScale.x * distanceCheckRay,
            Color.red);

        if (!playerControls.Gameplay.Jump.WasPressedThisFrame()) return;

        RaycastHit2D hit1 = Physics2D.Raycast(rayPosition1.position, 
            Vector2.right * transform.localScale.x, 
            distanceCheckRay, LayerMask.GetMask("Ground"));

        RaycastHit2D hit2 = Physics2D.Raycast(positionRay2,
            Vector2.right * transform.localScale.x,
            distanceCheckRay, LayerMask.GetMask("Ground"));

        if (hit2.collider != null && hit1.collider == null)
        {
            rb.bodyType = RigidbodyType2D.Static;

            playerModel.canInter = false;
            playerModel.mov = false;

            animatorPlayer.SetTrigger("ClimbLedge");

            Vector3 vectorSum = new Vector3(transform.localScale.x == 1 ? 0.5f : -0.5f, 0, 0);

            RaycastHit2D hitGround = Physics2D.Raycast(rayPosition1.position + vectorSum,
            Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

            float distanceDiference = 0.1751041f - Vector2.Distance(rayPosition1.position + vectorSum, hitGround.point);

            transform.position += new Vector3(0, distanceDiference, 0);
        }
    }

    public void EndClimb()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;

        Vector3 vectorSum = new Vector3(transform.localScale.x == 1 ? 0.5f : -0.5f, 0, 0);

        RaycastHit2D hitGround = Physics2D.Raycast(rayPosition1.position + vectorSum,
        Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

        transform.position = hitGround.point;

        playerModel.canInter = true;
        playerModel.mov = true;
    }
}
