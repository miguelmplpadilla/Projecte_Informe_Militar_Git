using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewDialogosController : MonoBehaviour
{
    public GameObject scrollBar;
    public GameObject content;

    public ScrollRect scrollRect;

    private void LateUpdate()
    {
        scrollRect.vertical = scrollBar.activeSelf;
        scrollRect.gameObject.SetActive(content.GetComponentsInChildren<RectTransform>().Length > 1);
    }
}