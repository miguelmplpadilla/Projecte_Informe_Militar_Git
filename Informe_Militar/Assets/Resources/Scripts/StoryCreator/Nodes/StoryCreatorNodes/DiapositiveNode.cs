using UnityEngine;
using UnityEngine.Serialization;
using XNode;

[CreateNodeMenu("StoryCreator/Diapositive")]
[NodeWidth(304)]
[CreateAssetMenu(fileName = "DiapositiveData", menuName = "StoryCreator/DiapositiveData", order = 1)]
public class DiapositiveNode : StoryBaseNode
{
    [Output] public StoryBaseNode next;
    [Space(20)] 
    [TextArea(4,150)] public string description;
    [Space(20)]
    public Sprite imageDialositiveFrontES;
    public Sprite imageDialositiveFrontEN;
    
    public Sprite imageDialositiveBackES;
    public Sprite imageDialositiveBackEN;
    [Space(20)]
    public Sprite backgroundDiapositive;
    
    public override void OnCreateConnection(NodePort from, NodePort to) 
    {
        base.OnCreateConnection(from, to);
		
        StoryBaseNode fromNode = from.node as StoryBaseNode;
        StoryBaseNode toNode = to.node as StoryBaseNode;
		
        if (fromNode == null || toNode == null) return;

        if (from.GetConnections().Count > 1)
            for (int i = 0; i < from.GetConnections().Count; i++)
                from.Disconnect(i);

        if (to.fieldName == "input" && from.fieldName == "next") 
            next = toNode;
    }
    
    public override void OnRemoveConnection(NodePort port)
    {
        base.OnRemoveConnection(port);

        if (port.fieldName.Equals("next"))
            next = null;
    }
    
}