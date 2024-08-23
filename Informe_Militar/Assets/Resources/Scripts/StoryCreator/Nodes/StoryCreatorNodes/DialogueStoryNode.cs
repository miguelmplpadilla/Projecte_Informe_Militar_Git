using UnityEngine;
using XNode;

[CreateNodeMenu("StoryCreator/Dialogue")]
[NodeWidth(304)]
[CreateAssetMenu(fileName = "DialogueStoryNode", menuName = "StoryCreator/DialogueStoryNode", order = 1)]
public class DialogueStoryNode : StoryBaseNode
{
	[Space(20)]
	[Output] public StoryBaseNode primaryNext;
	[Output] public StoryBaseNode secondaryNext;
	[Space(20)]
	public DialogueCreator dialogueGraph;
	[Space(20)] 
	public Sprite background;
	[Space(20)] 
	[TextArea(4,150)] public string backgroundDescription;
	
	public override void OnCreateConnection(NodePort from, NodePort to) 
	{
		base.OnCreateConnection(from, to);
		
		ConnectNext(from, to);
	}

	private void ConnectNext(NodePort from, NodePort to)
	{
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