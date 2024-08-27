using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FrameController : MonoBehaviour
{
    private Vector3 mPrevPos = Vector3.zero;
    private Vector3 mPosDelta = Vector3.zero;

    public float rotationSpeed = 2;
    
    private bool canRotate;

    private void Start()
    {
        EventBus<HideAllScenes>.Register(new EventBinding<HideAllScenes>(RestartVariables));
    }

    private void OnDestroy()
    {
        EventBus<HideAllScenes>.Deregister(new EventBinding<HideAllScenes>(RestartVariables));
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

    private void OnMouseDown()
    {
        if (ReadingTextController.instance.isReading) return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        List<RaycastHit> hitsDrag = Physics.RaycastAll(ray).ToList();
        foreach (var hit in hitsDrag)
        {
            if (hit.collider.name.Equals("PanelFrame"))
            {
                canRotate = true;
                break;
            } 
        }
    }

    private void OnMouseDrag()
    {
        if (canRotate && !ReadingTextController.instance.isReading)
            RotateObject();
    }

    private void OnMouseUp()
    {
        if (ReadingTextController.instance.isReading) return;
        
        canRotate = false;
    }

    private void RotateObject()
    {
        mPosDelta = Input.mousePosition - mPrevPos;
        transform.Rotate(transform.up,
            -Vector3.Dot(mPosDelta, Camera.main.transform.right) * rotationSpeed, Space.World);
        transform.Rotate(Camera.main.transform.right,
            Vector3.Dot(mPosDelta, Camera.main.transform.up) * rotationSpeed, Space.World);
    }
}
