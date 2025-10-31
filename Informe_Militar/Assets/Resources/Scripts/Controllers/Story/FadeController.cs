using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public GraphicRaycaster graphicRaycaster;
    public Image fadeImage;

    private void Start()
    {
        EventBus<FadeInFadeOut>.Register(new EventBinding<FadeInFadeOut>(FadeInFadeOut, gameObject));
    }

    private void OnDestroy()
    {
        EventBus<FadeInFadeOut>.Deregister(new EventBinding<FadeInFadeOut>(FadeInFadeOut, gameObject));
    }

    private void FadeInFadeOut(FadeInFadeOut f)
    {
        if (f.fade) FadeIn(f.callback);
        else FadeOut(f.callback);
    }

    private async void FadeIn(Action callback = null)
    {
        await fadeImage.DOFade(1, 2f).AsyncWaitForCompletion();
        graphicRaycaster.enabled = false;
        
        callback?.Invoke();
    }
    
    private async void FadeOut(Action callback = null)
    {
        await fadeImage.DOFade(0, 2f).AsyncWaitForCompletion();
        graphicRaycaster.enabled = true;
        
        callback?.Invoke();
    }
}
