using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject camera;

    private void Start()
    {
        camera = GameObject.Find("Player");
    }

    void Update()
    {
        transform.LookAt(camera.transform);
    }
}
