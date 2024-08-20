
using UnityEngine;
using UnityEngine.Video;

public class PlayGame : IEvent
{
    public GameObject gamePrefab;
    public string extraArguments;

    public StoryBaseNode primaryNextNode;
    public StoryBaseNode secondaryNextNode;
}

public class CloseGame : IEvent
{
    public int decision = 0;
}
