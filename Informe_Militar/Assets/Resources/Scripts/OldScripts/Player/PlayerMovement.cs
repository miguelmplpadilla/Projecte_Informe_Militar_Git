using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerModelDeprecated modelDeprecated;
    
    public Vector2 movement;
    
    private CapsuleCollider2D _capsuleCollider;

    public float jumpForce = 2f;

    private float horizontalVelocity = 0;

    public float maxSpeed = 0;
    public float maxSpeedWalk = 2;
    public float currentSpeed = 2f;

    public float maxSpeedAgachado = 0.5f;

    public PhysicsMaterial2D fullFriction;
    public PhysicsMaterial2D noFriction;

    private void Awake()
    {
        maxSpeed = maxSpeedWalk;
        modelDeprecated = GetComponent<PlayerModelDeprecated>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if (modelDeprecated.isPaused) return;
        
        if (modelDeprecated.mov)
        {
            if (modelDeprecated.sliding) return;
            
            maxSpeed = modelDeprecated.isSprinting && modelDeprecated.canRun ? maxSpeedWalk*2 : maxSpeedWalk;

            movement = new Vector2(modelDeprecated.direction.x, 0f);

            float velocity = modelDeprecated.direction.x != 0 ? modelDeprecated.isSprinting ? 1 : 0.5f : 0;

            if (modelDeprecated.agachado)
            {
                maxSpeed = maxSpeedAgachado;
                velocity = modelDeprecated.direction.x != 0 ? 1 : 0;
            }

            modelDeprecated.animator.SetFloat("velocity", velocity);

            flip();
            movePlayer();

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

            _capsuleCollider.sharedMaterial = modelDeprecated.direction.x != 0 ? null : fullFriction;

            if (modelDeprecated.isGrounded && hit.collider != null && modelDeprecated.direction.x == 0 && !modelDeprecated.positionedInRamp)
            {
                modelDeprecated.rigidbody.linearVelocity = Vector2.zero;
                transform.position = new Vector3(transform.position.x, hit.point.y);
                modelDeprecated.positionedInRamp = true;
            }

            if (modelDeprecated.playerControls.Gameplay.Slide.WasPressedThisFrame())
            {
                modelDeprecated.agachado = !modelDeprecated.agachado;
                if (modelDeprecated.isSprinting && modelDeprecated.agachado) tirarSuelo();
            }

            modelDeprecated.animator.SetBool("crouch", modelDeprecated.agachado);

            return;
        }

        modelDeprecated.animator.SetFloat("velocity", 0);
        modelDeprecated.rigidbody.linearVelocity = Vector3.zero;
    }

    private void tirarSuelo()
    {
        modelDeprecated.animator.SetTrigger("slide");
        
        modelDeprecated.sliding = true;

        Vector2 force = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        modelDeprecated.rigidbody.AddForce(force, ForceMode2D.Force);
    }
    
    private void movePlayer()
    {
        currentSpeed = Input.GetAxisRaw("Horizontal") == 0 ? movement.x * maxSpeed : maxSpeed;

        horizontalVelocity = movement.normalized.x * Math.Abs(currentSpeed);
        modelDeprecated.rigidbody.linearVelocity = new Vector2(horizontalVelocity, modelDeprecated.rigidbody.linearVelocity.y);

        modelDeprecated.animator.SetBool("run", Input.GetButton("Horizontal"));
    }

    private void flip()
    {
        if (modelDeprecated.direction.x != 0) 
            transform.localScale = new Vector3(modelDeprecated.direction.x > 0 ? 1 : -1, 1, 1);
    }
}
