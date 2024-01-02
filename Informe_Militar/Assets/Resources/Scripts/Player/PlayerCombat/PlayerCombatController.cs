using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    private PlayerModel _model;

    public float maxSpeed = 2;

    public float currentSpeed = 2f;
    private float horizontalVelocity = 0;

    public Vector2 movement;

    public bool attaking = false;

    public int numTriggerPunch = 1;
    public int numTriggerKick = 1;

    public BoxColliderInfo[] punchBoxColliderInfo;
    public BoxColliderInfo[] kickBoxColliderInfo;

    public BoxCollider2D hitBox;


    private void Awake()
    {
        _model = GetComponent<PlayerModel>();
    }

    private void Update()
    {
        if (attaking) return;

        if (Input.GetKeyDown(KeyCode.K)) Punch();
        if (Input.GetKeyDown(KeyCode.L)) Kick();

        movePlayer();
    }

    private void movePlayer()
    {
        movement = new Vector2(_model.direction.x, 0f);

        currentSpeed = Input.GetAxisRaw("Horizontal") == 0 ? 0 : maxSpeed;

        horizontalVelocity = movement.normalized.x * Math.Abs(currentSpeed);
        _model.rigidbody.velocity = new Vector2(horizontalVelocity, _model.rigidbody.velocity.y);

        _model.animator.SetFloat("velocity", Input.GetAxisRaw("Horizontal"));

        _model.animator.SetBool("run", Input.GetButton("Horizontal"));

        if (attaking && _model.rigidbody.velocity.x < 0) _model.rigidbody.velocity = Vector2.zero;
    }

    private void Punch()
    {
        hitBox.offset = punchBoxColliderInfo[numTriggerPunch - 1].offset;
        hitBox.size = punchBoxColliderInfo[numTriggerPunch - 1].size;

        _model.animator.SetTrigger("punch" + numTriggerPunch);

        numTriggerPunch++;

        if (numTriggerPunch == 4) numTriggerPunch = 1;

        _model.rigidbody.velocity = Vector2.zero;

        attaking = true;
    }

    private void Kick()
    {
        hitBox.offset = kickBoxColliderInfo[numTriggerKick - 1].offset;
        hitBox.size = kickBoxColliderInfo[numTriggerKick - 1].size;

        _model.animator.SetTrigger("kick" + numTriggerKick);

        numTriggerKick++;

        if (numTriggerKick == 4) numTriggerKick = 1;

        _model.rigidbody.velocity = Vector2.zero;

        attaking = true;
    }

    public void SetAttakingFalse()
    {
        attaking = false;
    }

    public void ActiveHitBox()
    {
        hitBox.enabled = true;
    }

    public void DeactiveHitBox()
    {
        hitBox.enabled = false;
    }

    public void HitEnemy()
    {
        List<Collider2D> results = new List<Collider2D>();
        Physics2D.OverlapCollider(hitBox, new ContactFilter2D().NoFilter(), results);

        foreach (Collider2D coll in results)
        {
            if (coll.name.Equals("HurtBoxEnemy"))
                coll.GetComponent<CombatEnemyHurtController>().Hurt();
        }
    }
}

[Serializable]
public class BoxColliderInfo
{
    public Vector2 offset;
    public Vector2 size;
}
