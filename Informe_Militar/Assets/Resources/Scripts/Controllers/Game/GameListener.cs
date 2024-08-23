
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

public class ActiveDesactiveCurrentGame : IEvent
{
    public bool active = true;
}

public class ReanudeGame : IEvent
{
    public int numFinal = 0;
}

public class SendDecision : IEvent
{
    public int final = 0;
}
