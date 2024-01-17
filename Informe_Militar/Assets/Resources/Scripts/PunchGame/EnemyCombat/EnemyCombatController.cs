using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyCombatController : MonoBehaviour
{
    public float speed = 2;

    private EnemyCombatModel model;
    private PlayerHurtController playerHurtController;
    
    public BoxCollider2D hitBox;

    public BoxColliderInfo[] boxColliderInfo;

    private void Awake()
    {
        model = GetComponent<EnemyCombatModel>();
    }

    private void Start()
    {
        playerHurtController = GameObject.Find("HurtBoxPlayer").GetComponent<PlayerHurtController>();
    }

    void Update()
    {
        if (playerHurtController.life <= 0) return;
        
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
            model.timeWaitAtack = Random.Range(model.minTimeWaitAtack, model.maxTimeWaitAtack+1);
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
        int numTrigger = Random.Range(1, 6 + 1);
        model.animator.SetTrigger("atack"+numTrigger);
        
        hitBox.offset = boxColliderInfo[numTrigger - 1].offset;
        hitBox.size = boxColliderInfo[numTrigger - 1].size;
        
        model.currentTimeAtack = 0;
    }
    
    public void HitPlayer()
    {
        List<Collider2D> results = new List<Collider2D>();
        Physics2D.OverlapCollider(hitBox, new ContactFilter2D().NoFilter(), results);

        foreach (Collider2D coll in results)
        {
            if (coll.name.Equals("HurtBoxPlayer"))
                coll.GetComponent<PlayerHurtController>().Hurt();
        }
    }

    public void SetAtackingFalse()
    {
        model.animator.SetTrigger("idle");
        model.atack = false;
        model.atacking = false;
        model.currentTimeAtack = 0;
    }
}