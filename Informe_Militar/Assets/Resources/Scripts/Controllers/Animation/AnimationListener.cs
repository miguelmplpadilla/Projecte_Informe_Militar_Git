
using UnityEngine;
using UnityEngine.Video;

public class SetAnimation : IEvent
{
    public VideoClip video;
    public string descriptionAnimation;
    
    public StoryBaseNode nextNode;
}

public class PlayAnimation : IEvent
{
    
}
