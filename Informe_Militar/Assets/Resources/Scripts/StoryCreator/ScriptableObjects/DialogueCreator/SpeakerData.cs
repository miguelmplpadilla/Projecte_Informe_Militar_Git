using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeakerData", menuName = "StoryCreator/SpeakerData", order = 1)]
public class SpeakerData : ScriptableObject
{
    public TypeSpeaker typeSpeaker;
    public string idSpeaker;
    public Expressions expresions;
    
    [Serializable]
    public class Expressions
{
    public Sprite idle;
    public Sprite angry;
    public Sprite sad;
    public Sprite happy;
    public Sprite thoughtful;

    public Sprite GetSprite(DialogueNode.Emotion emotion)
    {
        switch (emotion)
        {
            case DialogueNode.Emotion.IDLE:
                return idle;
            case DialogueNode.Emotion.ANGRY:
                return angry;
            case DialogueNode.Emotion.SAD:
                return sad;
            case DialogueNode.Emotion.HAPPY:
                return happy;
            case DialogueNode.Emotion.THOUGHTFUL:
                return thoughtful;
            default:
                return null;
        }
    }
}
    
    public enum TypeSpeaker
    {
        NONE, PLAYER, NPC
    }
}
