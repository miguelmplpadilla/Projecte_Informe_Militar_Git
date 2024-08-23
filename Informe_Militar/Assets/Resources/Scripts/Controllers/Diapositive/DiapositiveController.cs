using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiapositiveController : BaseControllerStory
{
    public GameObject parentMarco;
    public GameObject parentDiapositive;
    
    public Image imageDiapositiveFront;
    public Image imageDiapositiveBack;
    public SpriteRenderer backgroundDiapositive;

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
        parentDiapositive.transform.localScale = Vector3.one;
        
        parentMarco.SetActive(true);
        
        if (diapositive.imageDiapositiveFront == null)
        {
            parentMarco.SetActive(false);
            
            descriptionText.enabled = true;
            descriptionText.text = diapositive.description;
            return;
        }
        
        imageDiapositiveBack.gameObject.SetActive(false);

        if (diapositive.imageDiapositiveBack != null)
        {
            imageDiapositiveBack.gameObject.SetActive(true);
            imageDiapositiveBack.sprite = diapositive.imageDiapositiveBack;
        } 
        
        descriptionText.enabled = false;
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
        canvas.GetComponent<GraphicRaycaster>().enabled = false;
        canvasGroup.alpha = 0;
        parentDiapositive.transform.localScale = Vector3.zero;
    }
}
