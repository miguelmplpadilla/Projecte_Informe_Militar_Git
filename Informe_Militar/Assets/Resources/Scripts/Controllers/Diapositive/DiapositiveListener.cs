
using UnityEngine;

public class StartDiapositive : IEvent
{
    public Sprite imageDiapositiveFront;
    public Sprite imageDiapositiveBack;
    public Sprite backgroundDiapositive;

    public string textFront;
    public string textBack;

    public string descriptionFront;
    public string descriptionBack;
    
    public StoryBaseNode nextNode;
}

public class RestartPositionFrame : IEvent { }

public class SetReadingText : IEvent
{
    public string textFront;
    public string textBack;
}
