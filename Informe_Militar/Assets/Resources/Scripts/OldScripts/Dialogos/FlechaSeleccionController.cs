using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlechaSeleccionController : MonoBehaviour
{

    private void LateUpdate()
    {
        Vector3 dir = Input.mousePosition - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle-45, Vector3.forward);
    }
}
