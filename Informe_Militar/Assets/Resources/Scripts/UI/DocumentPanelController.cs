using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DocumentPanelController : MonoBehaviour
{
    public void closePanel()
    {
        gameObject.transform.parent.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
    }
}
