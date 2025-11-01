using DG.Tweening;
using Resources.Scripts.Inventory;
using Resources.Scripts.Objects;
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

    public DocumentsData documentsData;
    
    void Start()
    {
        EventBus<StartDiapositiveEvent>.Register(new EventBinding<StartDiapositiveEvent>(StartDiapositive, gameObject));
        //EventBus<HideAllScenes>.Register(new EventBinding<HideAllScenes>(HideScene, gameObject));
    }

    private void OnDestroy()
    {
        EventBus<StartDiapositiveEvent>.Deregister(new EventBinding<StartDiapositiveEvent>(StartDiapositive, gameObject));
        //EventBus<HideAllScenes>.Deregister(new EventBinding<HideAllScenes>(HideScene, gameObject));
    }

    private void StartDiapositive(StartDiapositiveEvent diapositive)
    {
        DocumentData documentData = GetDocumentData(diapositive.keyDiapositive);
        
        EventBus<RestartPositionFrame>.Raise(new RestartPositionFrame());

        descriptionFront.text = "";
        descriptionBack.text = "";

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        parentDiapositive.transform.localScale = Vector3.one;
        
        parentFrame.SetActive(true);
        imageDiapositiveFront.enabled = true;
        imageDiapositiveBack.enabled = false;
        
        if (documentData.ValueFront == null)
        {
            imageDiapositiveFront.enabled = false;
            imageDiapositiveBack.enabled = false;
            
            descriptionFront.enabled = true;
            descriptionFront.text = documentData.descriptionFront.Value;
            descriptionBack.text = documentData.descriptionBack.Value;
            return;
        }

        if (documentData.ValueBack != null)
        {
            imageDiapositiveBack.enabled = true;
            imageDiapositiveBack.sprite = documentData.ValueBack;
        }
        
        EventBus<SetReadingText>.Raise(new SetReadingText
        {
            textFront = documentData.textFront.Value,
            textBack = documentData.textBack.Value
        });
        
        descriptionFront.enabled = false;
        imageDiapositiveFront.sprite = documentData.ValueFront;

        background.DOFade(0.6f, 0);
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

    private DocumentData GetDocumentData(string keyDocument)
    {
        return documentsData.documents.Find(it => it.keyDocument == keyDocument);
    }
}

public class RestartDiapositiveEvent : IEvent {}
