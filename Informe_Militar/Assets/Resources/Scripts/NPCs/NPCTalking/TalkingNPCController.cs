using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TalkingNPCController : MonoBehaviour
{
    public CustomNPCController customNPCController1;
    public CustomNPCController customNPCController2;

    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;

    public string jsonName;

    public Talk talk = new Talk();

    private AudioManagerController audioManagerController;

    private void Start()
    {
        audioManagerController = GameObject.Find("AudioManager").GetComponent<AudioManagerController>();

        text1.text = "";
        text2.text = "";

        TextAsset json = UnityEngine.Resources.Load<TextAsset>("JSON/" + IdiomaController.getLanguage() + "/JSONTalking/" + jsonName);

        JsonUtility.FromJsonOverwrite(json.text, talk);

        Invoke("StartTalking", 3);
    }

    private async void StartTalking()
    {
        for (int i = 0; i < talk.dialoge.Count; i++)
        {
            SetAnimation(talk.dialoge[i].npc == 1 ? customNPCController1 : customNPCController2, 
                talk.dialoge[i].animation);

            await ShowText(talk.dialoge[i].npc == 1 ? text1 : text2, talk.dialoge[i].text);
            await Task.Delay(1000);

            text1.text = "";
            text2.text = "";
        }

        Invoke("StartTalking", 10);
    }

    private async Task ShowText(TextMeshProUGUI textDialoge, string textShow)
    {
        audioManagerController.PlayAudio("NPCTalk-0" + Random.Range(1, 8 + 1), gameObject);

        int cantLetter = 0;

        string text = "";
        foreach (var character in textShow)
        {
            text += character;
            textDialoge.text = text;

            cantLetter++;

            if (character == ' ' || cantLetter >= 4)
            {
                audioManagerController.PlayAudio("NPCTalk-0" + Random.Range(1, 8 + 1), gameObject);
                cantLetter = 0;
            }

            await Task.Delay(100);
        }
    }

    private void SetAnimation(CustomNPCController animator, string animation)
    {
        animator.SetAllAnimations(animation);
    }
}
