using UnityEngine;
using XNode;

[CreateNodeMenu("DialogueCreator/ExecuteEvent")]
[NodeWidth(304)]
public class EventDialogueNode : DialogueBaseNode
{
    [Output] public DialogueBaseNode followingDialogue;
	
    public override void OnCreateConnection(NodePort from, NodePort to) 
    {
        base.OnCreateConnection(from, to);
		
        ConnectFollowingDecision(from, to);
    }

    private void ConnectFollowingDecision(NodePort from, NodePort to)
    {
        DialogueBaseNode fromNode = from.node as DialogueBaseNode;
        DialogueBaseNode toNode = to.node as DialogueBaseNode;
		
        if (fromNode == null || toNode == null) return;

        if (from.GetConnections().Count > 1)
            for (int i = 0; i < from.GetConnections().Count; i++)
                from.Disconnect(i);

        if (to.fieldName == "dialogue" && from.fieldName == "followingDialogue") 
            followingDialogue = toNode;
    }

    public override void OnRemoveConnection(NodePort port)
    {
        base.OnRemoveConnection(port);

        if (port.fieldName.Equals("followingDialogue"))
            followingDialogue = null;
    }
}