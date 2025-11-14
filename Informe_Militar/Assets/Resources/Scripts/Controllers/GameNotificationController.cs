using System.Collections;
using DG.Tweening;
using Resources.Scripts.Tools;
using UnityEngine;

public class GameNotificationController : MonoBehaviour
{
    public static GameNotificationController instance;

    public LocalizableController textLocalizable;

    public CanvasGroup canvasGroupNotification;

    public Coroutine coroutineNotification;

    private void Awake()
    {
        instance = this;
    }

    public void ShowNotification(LocalizableString localizable)
    {
        if (coroutineNotification != null)
            StopCoroutine(coroutineNotification);
        coroutineNotification = StartCoroutine(ShowNotificationIE(localizable));
    }

    public IEnumerator ShowNotificationIE(LocalizableString localizable)
    {
        canvasGroupNotification.DOKill();
        yield return null;
        canvasGroupNotification.alpha = 1;
        textLocalizable.SetText(localizable);
        yield return new WaitForSeconds(3);
        canvasGroupNotification.DOFade(0, 1.5f);
        
    }
}
