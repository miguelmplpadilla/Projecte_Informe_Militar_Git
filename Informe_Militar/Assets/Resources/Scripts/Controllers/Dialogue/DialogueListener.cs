using UnityEngine;

public class SetDialogue : IEvent
{
    public DialogueBaseNode startDialogue;

    public StoryBaseNode primaryEnd;
    public StoryBaseNode secondaryEnd;
}

public class OnEndGameEvent : IEvent {}