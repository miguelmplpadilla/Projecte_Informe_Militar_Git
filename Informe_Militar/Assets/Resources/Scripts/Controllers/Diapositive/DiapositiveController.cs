using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiapositiveController : BaseControllerStory
{
    public Image imageDiapositive;
    public Image backgroundDiapositive;

    public TextMeshProUGUI descriptionText;

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
        nextNode = diapositive.nextNode;

        canvas.GetComponent<GraphicRaycaster>().enabled = true;
        canvasGroup.alpha = 1;

        Debug.Log(diapositive.description);
        
        if (diapositive.imageDiapositive == null)
        {
            imageDiapositive.enabled = false;
            backgroundDiapositive.enabled = false;
            
            descriptionText.enabled = true;
            descriptionText.text = diapositive.description;
            return;
        }
        
        descriptionText.enabled = false;
        imageDiapositive.sprite = diapositive.imageDiapositive;
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
        canvas.GetComponent<GraphicRaycaster>().enabled = false;
        canvasGroup.alpha = 0;
    }
}
