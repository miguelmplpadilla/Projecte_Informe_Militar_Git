using UnityEngine;
using UnityEngine.Video;
using XNode;

[CreateNodeMenu("StoryCreator/Animation")]
[NodeWidth(304)]
[CreateAssetMenu(fileName = "AnimationData", menuName = "StoryCreator/AnimationData", order = 1)]
public class AnimationNode : StoryBaseNode
{
    [Output] public StoryBaseNode next;
    [Space(20)] 
    [TextArea(4,150)] public string description;
    [Space(20)]
    public VideoClip videoAnimationES;
    public VideoClip videoAnimationEN;
    
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