using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public GameObject target;
    
    public float speedFollowTarget = 5;

    private Tween tweenRotation = null;

    private Vector3 originalRotation;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        originalRotation = transform.eulerAngles;
        
        Vector3 finalTarget = target.transform.position;
        finalTarget.y = transform.position.y;
        transform.position = finalTarget;
    }

    private void Update()
    {
        Vector3 finalTarget = target.transform.position;
        finalTarget.y = transform.position.y;
        
        transform.position = Vector3.MoveTowards(transform.position, finalTarget,
            speedFollowTarget * Time.deltaTime);
    }

    public void RotateBias(Vector3 targetRotation, float duration)
    {
        if (tweenRotation != null)
            tweenRotation.Kill();

        tweenRotation = transform.DORotate(targetRotation, duration);
    }
    
    public void RotateOriginal(float duration)
    {
        tweenRotation.Kill();
        tweenRotation = transform.DORotate(originalRotation, duration);
    }
}
