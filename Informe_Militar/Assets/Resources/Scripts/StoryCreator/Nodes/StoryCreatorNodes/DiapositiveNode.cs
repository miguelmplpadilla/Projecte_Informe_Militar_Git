using System;
using UnityEngine;
using UnityEngine.Serialization;
using XNode;

[CreateNodeMenu("StoryCreator/Diapositive")]
[NodeWidth(304)]
[CreateAssetMenu(fileName = "DiapositiveData", menuName = "StoryCreator/DiapositiveData", order = 1)]
public class DiapositiveNode : StoryBaseNode
{
    [Output] public StoryBaseNode next;
    public DataDiapositive dataDiapositive;
    
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

    [Serializable]
    public class DataDiapositive
    {
        [Space(20)] 
        [Space(20)]
        public Sprite frontES;
        public Sprite frontEN;
        [TextArea(4,150)] public string textFront;
        [TextArea(4,150)] public string descriptionFront;
    
        public Sprite backES;
        public Sprite backEN;
        [TextArea(4,150)] public string textBack;
        [TextArea(4,150)] public string descriptionBack;
        [Space(20)]
        public Sprite backgroundDiapositive;
    }
}