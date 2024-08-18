using System;
using UnityEngine;
using XNode;

[CreateNodeMenu("DialogueCreator/Dialogue")]
[NodeWidth(304)]
public class DialogueNode : DialogueBaseNode
{
	[Output] public DialogueBaseNode followingDialogue;
	
	[Space(20)]
	
	public Speaker speakerData;
	public Texts texts;
	
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

	[Serializable]
	public class Decision
	{
		[TextArea(10,300)]
		public string decisionEs;
		[TextArea(10,300)]
		public string decisionEn;
	}

	[Serializable]
	public class Speaker
	{
		public SpeakerData data;
		public Emotion emocion;
	}
	
	[Serializable]
	public class Texts
	{
		[TextArea(10,300)] public string textEs;
		[TextArea(10,300)] public string textEn;
	}
	
	public enum Emotion
	{
		IDLE, ENFADADO, TRISTE, CONTENTO, PENSATIVO
	}
}