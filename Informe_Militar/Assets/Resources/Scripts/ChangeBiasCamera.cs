using UnityEngine;

public class ChangeBiasCamera : MonoBehaviour
{
    public Vector3 targetRotation;
    public float duration = 1;

    public bool hasEntered = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (hasEntered) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Enter");
            CameraManager.instance.RotateBias(targetRotation, duration);
            hasEntered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exit");
            CameraManager.instance.RotateOriginal(duration);
            hasEntered = false;
        }
    }
}
