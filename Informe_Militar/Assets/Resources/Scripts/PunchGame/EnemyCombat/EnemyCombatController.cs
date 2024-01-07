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

        if (distance < 0.66f || !model.canHit || !model.canMove) return;

        model.animator.SetBool("run", true);

        transform.localScale = new Vector3(transform.position.x < model.player.transform.position.x ? 1 : -1, 1, 1);

        transform.position = 
            new Vector3(Vector2.MoveTowards(transform.position, model.player.transform.position, speed * Time.deltaTime).x,
            transform.position.y);
    }
}
