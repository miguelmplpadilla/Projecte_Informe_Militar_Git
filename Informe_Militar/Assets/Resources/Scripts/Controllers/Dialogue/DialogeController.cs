using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogeController : MonoBehaviour
{
    private TextMeshProUGUI currentText;

    public Image imageBackground;

    public TextMeshProUGUI playerText;
    public TextMeshProUGUI npcText;

    public RectTransform panelPlayer;
    public RectTransform panelNPC;
    public RectTransform panelButtons;

    private DialogueBaseNode currentDialogue;

    public bool canPlayDialogue = false;
    public bool isShowingText = false;
    public string currentShowingText = "";

    private SpeakerData.TypeSpeaker currentSpeaker;

    private Coroutine coroutineMostrarTexto;

    private StoryBaseNode primaryEnd;
    private StoryBaseNode secondaryEnd;

    public GraphicRaycaster graphicRaycaster;
    public CanvasGroup canvasGroup;

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    private void Start()
    {
        EventBus<SetDialogue>.Register(new EventBinding<SetDialogue>(SetDialogueData));
        EventBus<HideAllScenes>.Register(new EventBinding<HideAllScenes>(HideScene));
    }

    private void OnDestroy()
    {
        EventBus<SetDialogue>.Deregister(new EventBinding<SetDialogue>(SetDialogueData));
        EventBus<HideAllScenes>.Deregister(new EventBinding<HideAllScenes>(HideScene));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && canPlayDialogue)
        {
            if (isShowingText)
            {
                StopCoroutine(coroutineMostrarTexto);
                StartCoroutine(MostrarTexto(true));
                isShowingText = false;
            }
            else
            {
                canPlayDialogue = false;

                if (currentDialogue is DialogueNode &&
                    (currentDialogue as DialogueNode).speakerData.data.typeSpeaker.Equals(currentSpeaker))
                {
                    StartDialogue();
                    return;
                }
                
                ShowPanel(panelNPC, 1200);
                ShowPanel(panelPlayer, -1200, () =>
                {
                    SetDialogue();
                });
            }
        }
    }
    
    private void SetDialogueData(SetDialogue s)
    {
        imageBackground.sprite = s.imageBackground;
        if (s.imageBackground == null) imageBackground.DOFade(0, 0);

        canvasGroup.alpha = 1;
        graphicRaycaster.enabled = true;

        currentDialogue = s.startDialogue;
        primaryEnd = s.primaryEnd;
        secondaryEnd = s.secondaryEnd;
        
        EventBus<FadeInFadeOut>.Raise(new FadeInFadeOut
        {
            fade = false,
            callback = () =>
            {
                SetDialogue();
            }
        });
    }

    private void SetDialogue()
    {
        EventSystem.current.firstSelectedGameObject = null;
        EventSystem.current.SetSelectedGameObject(null);
        
        if (currentDialogue is PrimaryFinalDialogueNode)
        {
            EventBus<SetNextScene>.Raise(new SetNextScene
            {
                node = primaryEnd
            });
            
            return;
        }

        if (currentDialogue is SecondaryFinalDialogueNode)
        {
            EventBus<SetNextScene>.Raise(new SetNextScene
            {
                node = secondaryEnd
            });
            
            return;
        }
        
        if (currentDialogue is DecisionsNode)
        {
            SetButtonsDecisions();
            return;
        }
        
        StartDialogue();
    }

    private void StartDialogue()
    {
        playerText.text = "";
        npcText.text = "";
        
        DialogueNode dialogue = currentDialogue as DialogueNode;
        currentShowingText = dialogue.texts.textEs;
        currentSpeaker = dialogue.speakerData.data.typeSpeaker;

        currentText = dialogue.speakerData.data.typeSpeaker.Equals(SpeakerData.TypeSpeaker.PLAYER)
            ? playerText
            : npcText;
            
        ShowPanel(dialogue.speakerData.data.typeSpeaker.Equals(SpeakerData.TypeSpeaker.PLAYER)
            ? panelPlayer
            : panelNPC, 0, () =>
        {
            canPlayDialogue = true;
            coroutineMostrarTexto = StartCoroutine(MostrarTexto());
        });
            
        currentDialogue = dialogue.followingDialogue;
    }

    private void SetButtonsDecisions()
    {
        DecisionsNode decisionsNode = currentDialogue as DecisionsNode;
        
        SetDataButton(button1, decisionsNode.decisionConnection1, decisionsNode.decision1.decisionEs);
        SetDataButton(button2, decisionsNode.decisionConnection2, decisionsNode.decision2.decisionEs);
        SetDataButton(button3, decisionsNode.decisionConnection3, decisionsNode.decision3.decisionEs);
        SetDataButton(button4, decisionsNode.decisionConnection4, decisionsNode.decision4.decisionEs);
        
        ShowPanelButtons(0);
    }

    private void SetDataButton(Button button, DialogueBaseNode decisionsNode, string textButton)
    {
        if (decisionsNode == null)
        {
            button.gameObject.SetActive(false);
            return;
        }
        
        button.onClick.AddListener(() =>
            { PressButtonDecision(decisionsNode); });
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = textButton;
    }

    private void PressButtonDecision(DialogueBaseNode decisionPressed)
    {
        EventSystem.current.firstSelectedGameObject = null;
        EventSystem.current.SetSelectedGameObject(null);
        
        currentDialogue = decisionPressed;
        ShowPanelButtons(-450, () =>
        { SetDialogue(); });
    }
    
    IEnumerator MostrarTexto(bool showAllCharacters = false)
    {
        isShowingText = true;

        currentText.text = currentShowingText;
        int totalVisibleCharacters = currentShowingText.Length;

        int cont = 0;

        while (true)
        {
            int visibleCount = showAllCharacters ? totalVisibleCharacters : cont % (totalVisibleCharacters + 1);

            currentText.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
                break;

            cont++;

            yield return new WaitForSeconds(0.05f);
        }
        
        isShowingText = false;
    }

    private async void ShowPanel(RectTransform panel, float endPosition, Action callback = null)
    {
        await panel.DOAnchorPosX(endPosition, 0.4f).SetEase(Ease.OutBack)
            .AsyncWaitForCompletion();
        
        callback?.Invoke();
    }
    
    private async void ShowPanelButtons(float endPosition, Action callback = null)
    {
        await panelButtons.DOAnchorPosY(endPosition, 0.4f).SetEase(Ease.OutBack)
            .AsyncWaitForCompletion();
        
        callback?.Invoke();
    }

    private void HideScene()
    {
        ShowPanel(panelNPC, 1200);
        ShowPanel(panelPlayer, -1200);
        
        canvasGroup.alpha = 0;
        graphicRaycaster.enabled = false;
    }
}