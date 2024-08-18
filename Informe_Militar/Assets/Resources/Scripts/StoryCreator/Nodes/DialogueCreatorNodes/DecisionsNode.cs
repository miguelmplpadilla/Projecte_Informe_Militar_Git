using System;
using UnityEngine;
using XNode;

[CreateNodeMenu("DialogueCreator/Decisions")]
[NodeWidth(304)]
public class DecisionsNode : DialogueBaseNode
{
	[Output] public DialogueBaseNode decisionConnection1;
	public Decision decision1;
	[Output] public DialogueBaseNode decisionConnection2;
	public Decision decision2;
	[Output] public DialogueBaseNode decisionConnection3;
	public Decision decision3;
	[Output] public DialogueBaseNode decisionConnection4;
	public Decision decision4;
	
	[Space(20)]
	
	public DialogueNode.Texts texts;
	
	public override void OnCreateConnection(NodePort from, NodePort to) 
	{
		base.OnCreateConnection(from, to);
		
		ConnectDecision(from, to);
	}

	private void ConnectDecision(NodePort from, NodePort to)
	{
		DecisionsNode fromNode = from.node as DecisionsNode;
		DialogueNode toNode = to.node as DialogueNode;
		
		if (fromNode == null || toNode == null) return;

		if (from.GetConnections().Count > 1)
			for (int i = 0; i < from.GetConnections().Count; i++)
				from.Disconnect(i);

		if (to.fieldName == "dialogue" && from.fieldName == "decisionConnection1")
			fromNode.decisionConnection1 = toNode;
		if (to.fieldName == "dialogue" && from.fieldName == "decisionConnection2")
			fromNode.decisionConnection2 = toNode;
		if (to.fieldName == "dialogue" && from.fieldName == "decisionConnection3")
			fromNode.decisionConnection3 = toNode;
		if (to.fieldName == "dialogue" && from.fieldName == "decisionConnection4")
			fromNode.decisionConnection4 = toNode;
	}

	public override void OnRemoveConnection(NodePort port)
	{
		base.OnRemoveConnection(port);

		if (port.fieldName.Equals("decisionConnection1"))
			decisionConnection1 = null;
		if (port.fieldName.Equals("decisionConnection2"))
			decisionConnection2 = null;
		if (port.fieldName.Equals("decisionConnection3"))
			decisionConnection3 = null;
		if (port.fieldName.Equals("decisionConnection4"))
			decisionConnection4 = null;
	}

	[Serializable]
	public class Decision
	{
		[TextArea(10,300)]
		public string decisionEs;
		[TextArea(10,300)]
		public string decisionEn;
	}
}