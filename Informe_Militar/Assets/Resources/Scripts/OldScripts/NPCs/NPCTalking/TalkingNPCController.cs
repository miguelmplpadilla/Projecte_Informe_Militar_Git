using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TalkingNPCController : MonoBehaviour
{
    public CustomNPCController customNPCController1;
    public CustomNPCController customNPCController2;

    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;

    public AudioManagerController.VoiceTone voice1;
    public AudioManagerController.VoiceTone voice2;

    public string jsonName;

    public Talk talk = new Talk();

    private GameObject player;

    [Range(0.1f, 1)]
    public float volume = 1;

    public bool talkStarted = false;
    public bool isTalking = false;

    public int dialogueIndex = 0;

    private PlayerModel playerModel;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerModel = player.GetComponent<PlayerModel>();

        SetTexts();
    }

    public void SetTexts()
    {
        text1.text = "";
        text2.text = "";

        dialogueIndex = 0;
        
        talkStarted = false;
        
        TextAsset json = UnityEngine.Resources.Load<TextAsset>("JSON/" + IdiomaController.getLanguage() + "/JSONTalking/" + jsonName);

        JsonUtility.FromJsonOverwrite(json.text, talk);
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= 7 && !talkStarted && !playerModel.isPaused)
        {
            talkStarted = true;
            Invoke("StartTalking", 1);
        } else if (distanceToPlayer > 11)
        {
            talkStarted = false;
        }
    }

    private async void StartTalking()
    {
        if (isTalking) return;

        isTalking = true;

        for (int i = dialogueIndex; i < talk.dialoge.Count; i++)
        {
            dialogueIndex = i;

            if (!talkStarted || playerModel.isPaused)
            {
                isTalking = false;
                text1.text = "";
                text2.text = "";
                if (playerModel.isPaused) talkStarted = false;
                return;
            }

            SetAnimation(talk.dialoge[i].npc == 1 ? customNPCController1 : customNPCController2, 
                talk.dialoge[i].animation);

            await ShowText(talk.dialoge[i].npc == 1 ? text1 : text2, talk.dialoge[i].text, talk.dialoge[i].npc);
            await Task.Delay(1000);

            text1.text = "";
            text2.text = "";
        }

        dialogueIndex = 0;

        isTalking = false;

        Invoke("StartTalking", 10);
    }

    private async Task ShowText(TextMeshProUGUI textDialoge, string textShow, int npc)
    {
        PlayAudioTalk(npc == 1 ? voice1.ToString() : voice2.ToString());

        int cantLetter = 0;

        string text = "";
        foreach (var character in textShow)
        {
            text += character;
            textDialoge.text = text;

            cantLetter++;

            if (character == ' ' || cantLetter >= 4)
            {
                PlayAudioTalk(npc == 1 ? voice1.ToString() : voice2.ToString());
                cantLetter = 0;
            }

            await Task.Delay(100);
        }
    }

    private void PlayAudioTalk(string npc)
    {
        if (npc.Equals("") || npc.Equals("None")) return;
        
        AudioManagerController.PlaySfx("NPCTalk-"+npc+"-0" + Random.Range(1, 8 + 1), gameObject, 
            pitch: Random.Range(1.0f, 1.15f), volume: volume);
    }

    private void SetAnimation(CustomNPCController animator, string animation)
    {
        animator.SetAllAnimations(animation);
    }
}
