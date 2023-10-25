using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{

    [SerializeField] private float speed = 0.1f;
    
    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
    }
}
