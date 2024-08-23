using UnityEngine;
using XNode;

[CreateNodeMenu("DialogueCreator/StartDialogue")]
[NodeWidth(304)]
public class StartDialogueNode : Node {

	[Output] public DialogueNode dialogueStart;
    
	public override void OnCreateConnection(NodePort from, NodePort to) {
		
		base.OnCreateConnection(from, to);
		
		ConnectDialogue(from, to);
	}

	private void ConnectDialogue(NodePort from, NodePort to)
	{
		StartDialogueNode fromNode = from.node as StartDialogueNode;
		DialogueNode toNode = to.node as DialogueNode;
		
		if (fromNode == null || toNode == null) return;

		if (from.GetConnections().Count > 1)
			for (int i = 0; i < from.GetConnections().Count; i++)
				from.Disconnect(i);

		if (to.fieldName == "dialogue" && from.fieldName == "dialogueStart")
			fromNode.dialogueStart = toNode;
	}
}