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

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerModel = GetComponent<PlayerModel>();
        animatorPlayer = GetComponent<Animator>();

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
            playerModel.canInter = false;
            playerModel.mov = false;

            Vector3 vectorSum = new Vector3(transform.localScale.x == 1 ? 0.5f : -0.5f, 0, 0);

            RaycastHit2D hitGround = Physics2D.Raycast(rayPosition1.position + vectorSum,
            Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

            transform.position = hitGround.point;

            animatorPlayer.SetTrigger("levantar");

            await Task.Delay((int)(((animatorPlayer.GetCurrentAnimatorClipInfo(0)[0].clip.length-0.4f) / animatorPlayer.speed) * 1000));

            playerModel.canInter = true;
            playerModel.mov = true;
        }
    }
}
