using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundController : MonoBehaviour
{
    private Animator _animator;
    private PlayerModel _model;

    private void Awake()
    {
        _model = GetComponentInParent<PlayerModel>();
        _animator = transform.parent.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        _animator.SetBool("IsGrounded", _model.isGrounded);

        if (!_model.isGrounded) _model.isSprinting = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground")) _model.isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            _model.isGrounded = false;
        }
    }
}
