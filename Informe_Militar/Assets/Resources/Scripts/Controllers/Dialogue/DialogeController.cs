using System;
using System.Collections;
using DG.Tweening;
using Resources.Scripts.Controllers.Player;
using Resources.Scripts.NPCs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogeController : MonoBehaviour
{
    public static DialogeController instance;
    
    private TextMeshProUGUI currentText;

    public TextMeshProUGUI playerText;
    public TextMeshProUGUI npcText;

    public RectTransform panelPlayer;
    public RectTransform panelNPC;
    public RectTransform panelButtons;

    private DialogueBaseNode currentDialogue;

    private GameEventBase currentGameEvent;

    private bool canPlayDialogue = false;
    private bool isShowingText = false;
    private string currentShowingText = "";

    private SpeakerData.TypeSpeaker currentSpeaker;
    
    public SpeakerData playerSpeakerData;
    public SpeakerData npcSpeakerData;

    public Image imagePlayer;
    public Image imageNPC;

    private Coroutine coroutineMostrarTexto;
    
    private UnityEvent primaryEnd;
    private UnityEvent secondaryEnd;

    private int indexEvent = 1;

    public GraphicRaycaster graphicRaycaster;
    public CanvasGroup canvasGroup;

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        EventBus<OnEndGameEvent>.Register(new EventBinding<OnEndGameEvent>(EndGameEvent, gameObject));
    }
    
    private void OnDestroy()
    {
        EventBus<OnEndGameEvent>.Deregister(new EventBinding<OnEndGameEvent>(EndGameEvent, gameObject));
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
                    (currentDialogue as DialogueNode).speakerData.currentSpeaker.Equals(currentSpeaker))
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

    private void SetDialogue()
    {
        EventSystem.current.firstSelectedGameObject = null;
        EventSystem.current.SetSelectedGameObject(null);

        Debug.Log("Current dialogue: "+currentDialogue);
        
        if (currentDialogue is PrimaryFinalDialogueNode)
        {
            Debug.Log("PrimaryFinalDialogueNode");
        }

        if (currentDialogue is SecondaryFinalDialogueNode)
        {
            Debug.Log("SecondaryFinalDialogueNode");
        }

        if (currentDialogue is PrimaryFinalDialogueNode || currentDialogue is SecondaryFinalDialogueNode)
        {
            PlayerModel.instance.canMove = true;
            HideScene();
            return;
        }
        
        if (currentDialogue is EventDialogueNode eventNode)
        {
            HideScene(false);
            
            EventBus<StartTriggerEvent>.Raise(new StartTriggerEvent
            { index = indexEvent, obj = currentGameEvent.gameObject });
            indexEvent++;
            
            currentDialogue = eventNode.followingDialogue;
            
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
        currentSpeaker = SpeakerData.TypeSpeaker.NONE;
        playerText.text = "";
        npcText.text = "";
        
        DialogueNode dialogue = currentDialogue as DialogueNode;
        currentShowingText = dialogue.text.Value;
        currentSpeaker = dialogue.speakerData.currentSpeaker;

        currentText = dialogue.speakerData.currentSpeaker.Equals(SpeakerData.TypeSpeaker.PLAYER)
            ? playerText
            : npcText;
        
        if (dialogue.speakerData.currentSpeaker.Equals(SpeakerData.TypeSpeaker.PLAYER))
            imagePlayer.sprite = playerSpeakerData.expresions.GetSprite(dialogue.speakerData.emotion);
        else 
            imageNPC.sprite = npcSpeakerData.expresions.GetSprite(dialogue.speakerData.emotion);
        
        ShowPanel(dialogue.speakerData.currentSpeaker.Equals(SpeakerData.TypeSpeaker.PLAYER)
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

        Debug.Log("SetDataButton");
        
        button.onClick.AddListener(() =>
            { PressButtonDecision(decisionsNode); });
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = textButton;
    }

    private void PressButtonDecision(DialogueBaseNode decisionPressed)
    {
        Debug.Log("PressButtonDecision");
        EventSystem.current.firstSelectedGameObject = null;
        EventSystem.current.SetSelectedGameObject(null);
        
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();
        button4.onClick.RemoveAllListeners();
        
        currentDialogue = decisionPressed;
        ShowPanelButtons(-450, () =>
        { SetDialogue(); });
    }

    private void EndGameEvent()
    {
        StartCoroutine(ShowDialog());
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

    private void HideScene(bool restartVariables = true)
    {
        if (restartVariables) RestartVariables();
        
        ShowPanel(panelNPC, 1200);
        ShowPanel(panelPlayer, -1200);

        canvasGroup.DOFade(0, 0.2f);
        graphicRaycaster.enabled = false;
    }

    private void RestartVariables()
    {
        currentDialogue = null;
        currentShowingText = "";
        
        canPlayDialogue = false;
        isShowingText = false;

        primaryEnd.RemoveAllListeners();
        secondaryEnd.RemoveAllListeners();

        indexEvent = 1;

        currentText = null;
        currentSpeaker = SpeakerData.TypeSpeaker.NONE;
        
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();
        button4.onClick.RemoveAllListeners();
        
        EventSystem.current.firstSelectedGameObject = null;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SetData(DialogueCreator dialogue, SpeakerData npcData, GameEventBase gameEvent)
    {
        PlayerModel.instance.canMove = false;
        
        currentDialogue = (dialogue.nodes.Find(it => it is StartDialogueNode) as StartDialogueNode).dialogueStart;
        npcSpeakerData = npcData;
        currentGameEvent = gameEvent;
        
        primaryEnd = gameEvent.primaryEnd;
        secondaryEnd = gameEvent.secondaryEnd;
        
        StartCoroutine(ShowDialog());
    }

    private IEnumerator ShowDialog()
    {
        canvasGroup.DOFade(1, 0.2f);
        yield return new WaitForSeconds(0.2f);
        
        graphicRaycaster.enabled = true;
        
        SetDialogue();
    }
}