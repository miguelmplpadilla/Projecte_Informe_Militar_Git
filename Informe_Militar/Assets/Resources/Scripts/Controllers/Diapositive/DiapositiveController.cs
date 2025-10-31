using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiapositiveController : BaseControllerStory
{
    public GameObject parentFrame;
    public GameObject parentDiapositive;
    
    public Image imageDiapositiveFront;
    public Image imageDiapositiveBack;

    public TextMeshProUGUI descriptionFront;
    public TextMeshProUGUI descriptionBack;

    public Image background;

    public Canvas canvas;
    public CanvasGroup canvasGroup;
    
    void Start()
    {
        EventBus<StartDiapositive>.Register(new EventBinding<StartDiapositive>(StartDiapositive, gameObject));
        //EventBus<HideAllScenes>.Register(new EventBinding<HideAllScenes>(HideScene, gameObject));
    }

    private void OnDestroy()
    {
        EventBus<StartDiapositive>.Deregister(new EventBinding<StartDiapositive>(StartDiapositive, gameObject));
        //EventBus<HideAllScenes>.Deregister(new EventBinding<HideAllScenes>(HideScene, gameObject));
    }

    private void StartDiapositive(StartDiapositive diapositive)
    {
        EventBus<RestartPositionFrame>.Raise(new RestartPositionFrame());
        
        nextNode = diapositive.nextNode;

        descriptionFront.text = "";
        descriptionBack.text = "";

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        parentDiapositive.transform.localScale = Vector3.one;
        
        parentFrame.SetActive(true);
        imageDiapositiveFront.enabled = true;
        imageDiapositiveBack.enabled = false;
        
        if (diapositive.imageDiapositiveFront == null)
        {
            imageDiapositiveFront.enabled = false;
            imageDiapositiveBack.enabled = false;
            
            descriptionFront.enabled = true;
            descriptionFront.text = diapositive.descriptionFront;
            descriptionBack.text = diapositive.descriptionBack;
            return;
        }

        if (diapositive.imageDiapositiveBack != null)
        {
            imageDiapositiveBack.enabled = true;
            imageDiapositiveBack.sprite = diapositive.imageDiapositiveBack;
        }
        
        EventBus<SetReadingText>.Raise(new SetReadingText
        {
            textFront = diapositive.textFront,
            textBack = diapositive.textBack
        });
        
        descriptionFront.enabled = false;
        imageDiapositiveFront.sprite = diapositive.imageDiapositiveFront;

        background.DOFade(diapositive.backgroundDiapositive == null ? 0.7f : 1, 0);
    }

    public void CloseDiapositive()
    {
        EventBus<RestartDiapositiveEvent>.Raise(new RestartDiapositiveEvent());
        
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
        parentDiapositive.transform.localScale = Vector3.zero;
        Time.timeScale = 1;
    }

    private void HideScene()
    {
        imageDiapositiveFront.enabled = false;
        imageDiapositiveBack.enabled = false;
        
        EventBus<RestartPositionFrame>.Raise(new RestartPositionFrame());
        canvas.GetComponent<GraphicRaycaster>().enabled = false;
        canvasGroup.alpha = 0;
        parentDiapositive.transform.localScale = Vector3.zero;
    }
}

public class RestartDiapositiveEvent : IEvent {}
