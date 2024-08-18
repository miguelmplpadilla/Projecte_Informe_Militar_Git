using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerModel _model;
    
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
        _model = GetComponent<PlayerModel>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if (_model.isPaused) return;
        
        if (_model.mov)
        {
            if (_model.sliding) return;
            
            maxSpeed = _model.isSprinting && _model.canRun ? maxSpeedWalk*2 : maxSpeedWalk;

            movement = new Vector2(_model.direction.x, 0f);

            float velocity = _model.direction.x != 0 ? _model.isSprinting ? 1 : 0.5f : 0;

            if (_model.agachado)
            {
                maxSpeed = maxSpeedAgachado;
                velocity = _model.direction.x != 0 ? 1 : 0;
            }

            _model.animator.SetFloat("velocity", velocity);

            flip();
            movePlayer();

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

            _capsuleCollider.sharedMaterial = _model.direction.x != 0 ? null : fullFriction;

            if (_model.isGrounded && hit.collider != null && _model.direction.x == 0 && !_model.positionedInRamp)
            {
                _model.rigidbody.velocity = Vector2.zero;
                transform.position = new Vector3(transform.position.x, hit.point.y);
                _model.positionedInRamp = true;
            }

            if (_model.playerControls.Gameplay.Slide.WasPressedThisFrame())
            {
                _model.agachado = !_model.agachado;
                if (_model.isSprinting && _model.agachado) tirarSuelo();
            }

            _model.animator.SetBool("crouch", _model.agachado);

            return;
        }

        _model.animator.SetFloat("velocity", 0);
        _model.rigidbody.velocity = Vector3.zero;
    }

    private void tirarSuelo()
    {
        _model.animator.SetTrigger("slide");
        
        _model.sliding = true;

        Vector2 force = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        _model.rigidbody.AddForce(force, ForceMode2D.Force);
    }
    
    private void movePlayer()
    {
        currentSpeed = Input.GetAxisRaw("Horizontal") == 0 ? movement.x * maxSpeed : maxSpeed;

        horizontalVelocity = movement.normalized.x * Math.Abs(currentSpeed);
        _model.rigidbody.velocity = new Vector2(horizontalVelocity, _model.rigidbody.velocity.y);

        _model.animator.SetBool("run", Input.GetButton("Horizontal"));
    }

    private void flip()
    {
        if (_model.direction.x != 0) 
            transform.localScale = new Vector3(_model.direction.x > 0 ? 1 : -1, 1, 1);
    }
}
