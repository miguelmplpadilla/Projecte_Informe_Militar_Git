using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MotoController : MonoBehaviour
{
    public Vector2 movement;

    public float speed = 1;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector3 rotation = new Vector3();

        if (movement.y > 0 || movement.y == 0)
        {
            rotation = new Vector3(0, 0, 20 * -movement.x);
        }
        else
        {
            rotation = new Vector3(0, 0, 20 * movement.x);
        }

        transform.GetChild(0).DORotate(rotation, 1);

        rb.velocity = movement.normalized * speed;
    }
}