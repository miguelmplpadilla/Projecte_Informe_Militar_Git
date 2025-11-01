using DG.Tweening;
using Resources.Scripts.Inventory;
using UnityEngine;

public class ViewObjectController : MonoBehaviour
{
    public static ViewObjectController instance;

    public CanvasGroup canvasGroup;

    public GameObject frameObject;

    private void Awake()
    {
        instance = this;
    }

    public void ShowObject(ItemData itemData)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void HideObject()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        RestartRotationItem();
    }

    private void RestartRotationItem()
    {
        frameObject.transform.DORotate(Vector3.zero, 0);
    }
}
