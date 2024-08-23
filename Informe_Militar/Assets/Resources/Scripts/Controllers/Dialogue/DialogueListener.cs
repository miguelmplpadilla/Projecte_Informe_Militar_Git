using UnityEngine;

public class SetDialogue : IEvent
{
    public Sprite imageBackground;
    public DialogueBaseNode startDialogue;

    public StoryBaseNode primaryEnd;
    public StoryBaseNode secondaryEnd;
}