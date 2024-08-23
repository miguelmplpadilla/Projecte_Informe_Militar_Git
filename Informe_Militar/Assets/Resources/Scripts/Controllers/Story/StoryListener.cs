using System;

public class SetNextScene : IEvent
{
    public StoryBaseNode node;
}

public class HideAllScenes : IEvent
{
    
}

public class FadeInFadeOut : IEvent
{
    public bool fade = true;
    public Action callback = null;
}

public class PlayNodeInGame : IEvent
{
    public StoryBaseNode node;
}