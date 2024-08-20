using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("StoryCreator/Game")]
[NodeWidth(304)]
public class GameNode : StoryBaseNode
{
    [Output] public StoryBaseNode primaryNext;
    [Output] public StoryBaseNode secondaryNext;
    [Space(20)]
    public GameObject prefabGame;
    [TextArea(2,50)] public string extraArguments;
    [Space(20)] 
    [TextArea(4,150)] public string description;
    
    public override void OnCreateConnection(NodePort from, NodePort to) 
    {
        base.OnCreateConnection(from, to);
		
        StoryBaseNode fromNode = from.node as StoryBaseNode;
        StoryBaseNode toNode = to.node as StoryBaseNode;
		
        if (fromNode == null || toNode == null) return;

        if (from.GetConnections().Count > 1)
            for (int i = 0; i < from.GetConnections().Count; i++)
                from.Disconnect(i);

        if (to.fieldName == "input" && from.fieldName == "primaryNext") 
            primaryNext = toNode;
        
        if (to.fieldName == "input" && from.fieldName == "secondaryNext") 
            secondaryNext = toNode;
    }
    
    public override void OnRemoveConnection(NodePort port)
    {
        base.OnRemoveConnection(port);

        if (port.fieldName.Equals("primaryNext"))
            primaryNext = null;
        
        if (port.fieldName.Equals("secondaryNext"))
            secondaryNext = null;
    }
    
}