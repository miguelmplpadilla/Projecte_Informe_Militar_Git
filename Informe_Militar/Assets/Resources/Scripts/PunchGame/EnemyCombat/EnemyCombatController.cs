using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour
{
    public float speed = 2;

    private EnemyCombatModel model;

    private void Awake()
    {
        model = GetComponent<EnemyCombatModel>();
    }

    void Update()
    {
        float distance = Vector2.Distance(model.player.transform.position, transform.position);

        model.animator.SetBool("run", false);

        if (model.atack && !model.atacking)
        {
            model.currentTimeAtack += Time.deltaTime;

            if (model.currentTimeAtack >= model.timeWaitAtack) Attack();
        }

        if (!model.canHit || !model.canMove || model.dashing) return;

        if (distance < 0.66f)
        {
            model.atack = true;
            return;
        }

        model.animator.SetBool("run", true);

        transform.localScale = new Vector3(transform.position.x < model.player.transform.position.x ? 1 : -1, 1, 1);

        transform.position = 
            new Vector3(Vector2.MoveTowards(transform.position, model.player.transform.position, speed * Time.deltaTime).x,
            transform.position.y);
    }

    public void Attack()
    {
        model.atacking = true;
        model.animator.SetTrigger("atack"+Random.Range(1, 6+1));
        model.currentTimeAtack = 0;
    }

    public void SetAtackingFalse()
    {
        model.animator.SetTrigger("idle");
        model.atack = false;
        model.atacking = false;
        model.currentTimeAtack = 0;
    }
}
