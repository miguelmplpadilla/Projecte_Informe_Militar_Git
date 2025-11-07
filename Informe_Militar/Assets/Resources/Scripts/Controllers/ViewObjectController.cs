using DG.Tweening;
using UnityEngine;

public class ViewObjectController : MonoBehaviour
{
    public static ViewObjectController instance;

    public CanvasGroup canvasGroup;

    public GameObject frameObject;

    public Camera cameraFaceObject;

    private void Awake()
    {
        instance = this;
    }

    public void ShowObject()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        cameraFaceObject.enabled = false;
    }

    public void HideObject()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        cameraFaceObject.enabled = true;
        RestartRotationItem();
    }

    private void RestartRotationItem()
    {
        frameObject.transform.DORotate(Vector3.zero, 0);
    }
}
