using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ShootingCameraController : MonoBehaviour
{
    public float mouseSensitivity = 2;
    public float cameraVerticalRotation = 0;

    public float maxCameraX = 65;
    public float maxCameraY = 20;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraVerticalRotation -= inputY;
        if (cameraVerticalRotation > maxCameraY || cameraVerticalRotation < -maxCameraY)
            cameraVerticalRotation = cameraVerticalRotation < 0 ? -maxCameraY : maxCameraY;

        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90, 90);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        transform.parent.Rotate(Vector3.up * inputX);

        float yRotation = transform.parent.eulerAngles.y <= 180 ?
            transform.parent.eulerAngles.y : transform.parent.eulerAngles.y - 360;

        if (yRotation < -maxCameraX || yRotation > maxCameraX)
            transform.parent.localRotation = 
                Quaternion.Euler(0, transform.parent.localRotation.y < 0 ? -maxCameraX : maxCameraX, 0);
    }
}
