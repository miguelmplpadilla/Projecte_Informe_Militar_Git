using System;
using System.Collections;
using System.Collections.Generic;
using Resources.Scripts;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class LaderController : MonoBehaviour, InterBaseInterface
{
    private GameObject button;
    private GameObject headPlayer;
    private GameObject player;
    private Animator playerAnimator;
    private Rigidbody2D playerRb;
    private PlayerModel playerModel;

    public float currentSpeed = 0;
    public float maxSpeed = 1;

    public bool playerClimbing = false;
    public bool canClimb = false;

    private void Start()
    {
        button = transform.GetChild(0).GetChild(0).gameObject;
        player = GameObject.Find("Player");
        playerAnimator = player.GetComponent<Animator>();
        playerRb = playerAnimator.GetComponent<Rigidbody2D>();
        headPlayer = player.transform.Find("Head").gameObject;
    }

    private void Update()
    {
        button.transform.position = new Vector3(button.transform.position.x, headPlayer.transform.position.y,
            button.transform.position.z);
        
        if (!canClimb) return;
        
        if (Input.GetKeyDown(KeyCode.F) && playerClimbing)
        {
            playerAnimator.SetBool("climbing", false);
            playerRb.bodyType = RigidbodyType2D.Dynamic;
            playerModel.mov = true;
            playerModel.canInter = true;
            playerAnimator.speed = 1;
            canClimb = false;
            playerClimbing = false;
            return;
        }

        float verticalInput = Input.GetAxisRaw("Vertical");
        
        playerAnimator.speed = verticalInput != 0 ? 1 : 0;
        
        Vector2 movement = new Vector2(0, verticalInput);
        
        currentSpeed = verticalInput != 0 ? maxSpeed : 0;

        float verticlaVelocity = movement.normalized.y * Math.Abs(currentSpeed);
        playerRb.velocity = new Vector2(0, verticlaVelocity);

        playerClimbing = true;
    }

    public void interEnter(PlayerModel model)
    {
    }

    public void inter(PlayerModel model)
    {
        playerModel = model;
        player.transform.position = new Vector3(transform.position.x, player.transform.position.y);
        playerAnimator.SetBool("climbing", true);
        playerAnimator.SetTrigger("climb");
        playerRb.bodyType = RigidbodyType2D.Kinematic;
        canClimb = true;
    }

    public void interExit(PlayerModel model)
    {
    }
}
