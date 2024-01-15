using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    private PlayerModel _model;

    public float maxSpeed = 2;

    public float dashTime = 0.25f;
    public float dashSpeed = 3;

    public float currentSpeed = 2f;
    private float horizontalVelocity = 0;

    public Vector2 movement;

    public bool attaking = false;
    public bool dashing = false;

    public int numTriggerPunch = 1;
    public int numTriggerKick = 1;

    public BoxColliderInfo[] punchBoxColliderInfo;
    public BoxColliderInfo[] kickBoxColliderInfo;

    public BoxCollider2D hitBox;

    private GameObject enemy;


    private void Awake()
    {
        _model = GetComponent<PlayerModel>();
    }

    private void Start()
    {
        enemy = GameObject.Find("EnemyCombat");
    }

    private void Update()
    {
        if (attaking || dashing)
        {
            _model.animator.SetBool("run", false);
            return;
        }

        transform.localScale = new Vector3(transform.position.x < enemy.transform.position.x ? 1 : -1, 1, 1);

        _model.animator.SetBool("run", true);

        if (Input.GetKeyDown(KeyCode.K)) Punch();
        if (Input.GetKeyDown(KeyCode.L)) Kick();

        movePlayer();

        if (Input.GetKeyDown(KeyCode.Space)) Dash();
    }

    private void movePlayer()
    {
        movement = new Vector2(_model.direction.x, 0f);

        currentSpeed = Input.GetAxisRaw("Horizontal") == 0 ? 0 : maxSpeed;

        horizontalVelocity = movement.normalized.x * Math.Abs(currentSpeed);
        _model.rigidbody.velocity = new Vector2(horizontalVelocity, _model.rigidbody.velocity.y);

        _model.animator.SetFloat("velocity", Input.GetAxisRaw("Horizontal"));

        bool reduceVelocity = transform.localScale.x > 0 ? _model.rigidbody.velocity.x < 0 : _model.rigidbody.velocity.x > 0;

        if (attaking && reduceVelocity) _model.rigidbody.velocity = Vector2.zero;
    }

    private async void Dash()
    {
        dashing = true;

        _model.rigidbody.velocity = Vector2.zero;

        _model.animator.SetTrigger("dash");

        Vector2 direction = Input.GetAxisRaw("Horizontal") != 0 ?
            Input.GetAxisRaw("Horizontal") * new Vector2(1, 0) : new Vector2(transform.localScale.x > 0 ? -1 : 1, 0);

        _model.rigidbody.AddForce(direction * dashSpeed, ForceMode2D.Impulse);

        await Task.Delay((int)(dashTime*1000));

        dashing = false;
    }

    private void Punch()
    {
        StopCoroutine("RestartComboPunch");

        hitBox.offset = punchBoxColliderInfo[numTriggerPunch - 1].offset;
        hitBox.size = punchBoxColliderInfo[numTriggerPunch - 1].size;

        _model.animator.SetTrigger("punch" + numTriggerPunch);

        numTriggerPunch++;
        numTriggerKick = 1;

        if (numTriggerPunch == 4) numTriggerPunch = 1;

        _model.rigidbody.velocity = Vector2.zero;

        attaking = true;

        StartCoroutine("RestartComboPunch");
    }

    private void Kick()
    {
        StopCoroutine("RestartComboKick");

        hitBox.offset = kickBoxColliderInfo[numTriggerKick - 1].offset;
        hitBox.size = kickBoxColliderInfo[numTriggerKick - 1].size;

        _model.animator.SetTrigger("kick" + numTriggerKick);

        numTriggerKick++;
        numTriggerPunch = 1;

        if (numTriggerKick == 4) numTriggerKick = 1;

        _model.rigidbody.velocity = Vector2.zero;

        attaking = true;

        StartCoroutine("RestartComboKick");
    }

    private IEnumerator RestartComboPunch()
    {
        yield return new WaitForSeconds(2);

        numTriggerPunch = 1;
    }

    private IEnumerator RestartComboKick()
    {
        yield return new WaitForSeconds(2);

        numTriggerKick= 1;
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
                coll.GetComponent<EnemyCombatHurtController>().Hurt();
        }
    }
}

[Serializable]
public class BoxColliderInfo
{
    public Vector2 offset;
    public Vector2 size;
}
