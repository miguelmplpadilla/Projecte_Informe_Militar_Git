using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundController : MonoBehaviour
{
    private Animator _animator;
    private PlayerModelDeprecated modelDeprecated;

    private void Awake()
    {
        modelDeprecated = GetComponentInParent<PlayerModelDeprecated>();
        _animator = transform.parent.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        _animator.SetBool("IsGrounded", modelDeprecated.isGrounded);

        if (!modelDeprecated.isGrounded) modelDeprecated.isSprinting = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground")) modelDeprecated.isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            modelDeprecated.isGrounded = false;
        }
    }
}
