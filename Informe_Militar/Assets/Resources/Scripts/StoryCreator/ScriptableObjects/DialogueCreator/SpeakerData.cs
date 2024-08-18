using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeakerData", menuName = "StoryCreator/SpeakerData", order = 1)]
public class SpeakerData : ScriptableObject
{
    public TypeSpeaker typeSpeaker;
    public string idSpeaker;
    public Expresions expresions;
    
    [Serializable]
    public class Expresions
    {
        public Sprite idle;
        public Sprite enfadado;
        public Sprite triste;
        public Sprite contento;
        public Sprite pensativo;
    }
    
    public enum TypeSpeaker
    {
        PLAYER, NPC
    }
}
