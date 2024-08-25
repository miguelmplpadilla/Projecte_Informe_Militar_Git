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
    public SpriteRenderer backgroundDiapositive;

    public TextMeshProUGUI descriptionFront;
    public TextMeshProUGUI descriptionBack;

    public Canvas canvas;
    public CanvasGroup canvasGroup;
    
    void Start()
    {
        EventBus<StartDiapositive>.Register(new EventBinding<StartDiapositive>(StartDiapositive));
        EventBus<HideAllScenes>.Register(new EventBinding<HideAllScenes>(HideScene));
    }

    private void OnDestroy()
    {
        EventBus<StartDiapositive>.Deregister(new EventBinding<StartDiapositive>(StartDiapositive));
        EventBus<HideAllScenes>.Deregister(new EventBinding<HideAllScenes>(HideScene));
    }

    private void StartDiapositive(StartDiapositive diapositive)
    {
        EventBus<RestartPositionFrame>.Raise(new RestartPositionFrame());
        
        nextNode = diapositive.nextNode;

        descriptionFront.text = "";
        descriptionBack.text = "";

        canvas.GetComponent<GraphicRaycaster>().enabled = true;
        canvasGroup.alpha = 1;
        parentDiapositive.transform.localScale = Vector3.one;
        
        parentFrame.SetActive(true);
        imageDiapositiveFront.gameObject.SetActive(true);
        imageDiapositiveBack.gameObject.SetActive(false);
        
        if (diapositive.imageDiapositiveFront == null)
        {
            imageDiapositiveFront.gameObject.SetActive(false);
            imageDiapositiveBack.gameObject.SetActive(false);
            
            descriptionFront.enabled = true;
            descriptionFront.text = diapositive.descriptionFront;
            descriptionBack.text = diapositive.descriptionBack;
            return;
        }

        if (diapositive.imageDiapositiveBack != null)
        {
            imageDiapositiveBack.gameObject.SetActive(true);
            imageDiapositiveBack.sprite = diapositive.imageDiapositiveBack;
        }
        
        EventBus<SetReadingText>.Raise(new SetReadingText
        {
            textFront = diapositive.textFront,
            textBack = diapositive.textBack
        });
        
        descriptionFront.enabled = false;
        imageDiapositiveFront.sprite = diapositive.imageDiapositiveFront;
        backgroundDiapositive.sprite = diapositive.backgroundDiapositive;

        backgroundDiapositive.DOFade(diapositive.backgroundDiapositive == null ? 0.7f : 1, 0);
    }

    public void CloseDiapositive()
    {
        EventBus<SetNextScene>.Raise(new SetNextScene
        {
            node = nextNode
        });
    }

    private void HideScene()
    {
        EventBus<RestartPositionFrame>.Raise(new RestartPositionFrame());
        canvas.GetComponent<GraphicRaycaster>().enabled = false;
        canvasGroup.alpha = 0;
        parentDiapositive.transform.localScale = Vector3.zero;
    }
}
