using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class DialogueCreator : NodeGraph
{
	private List<Type> nodesAllowed = new List<Type>
	{
		typeof(DialogueNode), typeof(StartDialogueNode), typeof(DecisionsNode), typeof(PrimaryFinalDialogueNode),
		typeof(SecondaryFinalDialogueNode), typeof(EventDialogueNode)
	};

	public override Node AddNode(Type type)
	{
		Node node = base.AddNode(type);
 
		bool canCreate = false;

		for (int i = 0; i < nodesAllowed.Count; i++)
			if (nodesAllowed[i] == type) canCreate = true;

		if (!canCreate)
		{
			RemoveNode(node);
			return null;
		}
        
		return node;
	}
}