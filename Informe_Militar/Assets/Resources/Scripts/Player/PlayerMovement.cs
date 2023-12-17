using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerModel _model;
    
    public Vector2 movement;
    
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private GameObject spriteObj;

    public float jumpForce = 2f;

    private float horizontalVelocity = 0;
    private float horizontalInput = 0;

    public float maxSpeed = 0;
    public float maxSpeedWalk = 2;
    public float currentSpeed = 2f;

    public float maxSpeedAgachado = 0.5f;


    private void Awake()
    {
        maxSpeed = maxSpeedWalk;
        _model = GetComponent<PlayerModel>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_model.isPaused) return;
        
        if (_model.mov)
        {
            if (_model.sliding) return;
            
            maxSpeed = Input.GetButton("Sprint") && _model.canRun ? maxSpeedWalk*2 : maxSpeedWalk;

            horizontalInput = Input.GetAxisRaw("Horizontal");
            movement = new Vector2(horizontalInput, 0f);

            float velocity = horizontalInput != 0 ? Input.GetButton("Sprint") ? 1 : 0.5f : 0;

            if (_model.agachado)
            {
                maxSpeed = maxSpeedAgachado;
                velocity = horizontalInput != 0 ? 1 : 0;
            }

            _animator.SetFloat("velocity", velocity);

            flip();
            movePlayer();

            if (_model.isGrounded && _model.canJump && 
                Input.GetButtonDown("Jump")) saltar();

            if (Input.GetKeyDown(KeyCode.C))
            {
                _model.agachado = !_model.agachado;
                if (Input.GetButton("Sprint") && _model.agachado) tirarSuelo();
            }

            _animator.SetBool("crouch", _model.agachado);

            return;
        }
        
        _rigidbody.velocity = Vector3.zero;
    }

    private void tirarSuelo()
    {
        _animator.SetTrigger("slide");
        
        _model.sliding = true;

        Vector2 force = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        _rigidbody.AddForce(force, ForceMode2D.Force);
    }
    
    private void movePlayer()
    {
        currentSpeed = Input.GetAxisRaw("Horizontal") == 0 ? movement.x * maxSpeed : maxSpeed;

        horizontalVelocity = movement.normalized.x * Math.Abs(currentSpeed);
        _rigidbody.velocity = new Vector2(horizontalVelocity, _rigidbody.velocity.y);
        
        _animator.SetBool("run", Input.GetButton("Horizontal"));
    }

    private void flip()
    {
        if (horizontalInput != 0) 
            transform.localScale = new Vector3(horizontalInput > 0 ? 1 : -1, 1, 1);
    }

    private void saltar()
    {
        _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
