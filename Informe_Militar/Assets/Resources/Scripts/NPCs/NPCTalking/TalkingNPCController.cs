using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TalkingNPCController : MonoBehaviour
{
    public CustomNPCController customNPCController1;
    public CustomNPCController customNPCController2;

    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;

    public TextAsset json;

    public Talk talk = new Talk();

    private void Start()
    {
        text1.text = "";
        text2.text = "";

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
        string text = "";
        foreach (var character in textShow)
        {
            text += character;
            textDialoge.text = text;

            await Task.Delay(100);
        }
    }

    private void SetAnimation(CustomNPCController animator, string animation)
    {
        animator.SetAllAnimations(animation);
    }
}
