using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour
{
    private Rigidbody2D rb;

    private GameObject player;

    public float speed = 2;

    private EnemyCombatModel model;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        model = GetComponent<EnemyCombatModel>();
    }

    private void Start()
    {
        player = GameObject.Find("PlayerCombat");
    }

    void Update()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance < 0.66f || !model.canHit) return;

        transform.localScale = new Vector3(transform.position.x < player.transform.position.x ? 1 : -1, 1, 1);

        transform.position = 
            new Vector3(Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime).x,
            transform.position.y);
    }
}
