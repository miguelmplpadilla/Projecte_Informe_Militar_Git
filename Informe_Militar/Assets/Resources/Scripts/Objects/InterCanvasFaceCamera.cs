using UnityEngine;

public class InterCanvasFaceCamera : MonoBehaviour
{
    private GameObject camera;

    private void Start()
    {
        camera = GameObject.Find("Camera");
    }

    void LateUpdate()
    {
        Vector3 dir = transform.position - camera.transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
