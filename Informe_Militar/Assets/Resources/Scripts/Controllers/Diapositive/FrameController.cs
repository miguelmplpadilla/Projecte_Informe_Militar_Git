using UnityEngine;
using UnityEngine.EventSystems;

public class FrameController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 mPrevPos = Vector3.zero;
    private Vector3 mPosDelta = Vector3.zero;

    public float rotationSpeed = 2;

    public GameObject frame;
    
    private bool canRotate;

    private void Start()
    {
        EventBus<RestartDiapositiveEvent>.Register(new EventBinding<RestartDiapositiveEvent>(RestartVariables, gameObject));
    }

    private void OnDestroy()
    {
        EventBus<RestartDiapositiveEvent>.Deregister(new EventBinding<RestartDiapositiveEvent>(RestartVariables, gameObject));
    }

    private void Update()
    {
        mPrevPos = Input.mousePosition;
    }

    private void RestartVariables()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
        mPosDelta = Vector3.zero;
    }

    private void RotateObject()
    {
        mPosDelta = Input.mousePosition - mPrevPos;
        frame.transform.Rotate(
            Vector3.up,
            -Vector3.Dot(mPosDelta, Camera.main.transform.right) * rotationSpeed,
            Space.World
        );

    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (ReadingTextController.instance.isReading) return;
        
        canRotate = true;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (canRotate && !ReadingTextController.instance.isReading)
            RotateObject();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (ReadingTextController.instance.isReading) return;
        
        canRotate = false;
    }
}
