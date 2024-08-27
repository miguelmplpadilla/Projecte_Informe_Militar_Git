using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class StoryCreator : NodeGraph 
{ 
	public List<Node> nodesCreated = new List<Node>();

	private List<Type> nodesAllowed = new List<Type>
	{
		typeof(DialogueStoryNode), typeof(StartStoryNode), typeof(AnimationNode), typeof(GameNode),
		typeof(EndStoryPrimaryNode), typeof(EndStorySecondaryNode), typeof(DiapositiveNode)
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
		
		nodesCreated.Add(node);
        
		return node;
	}

	public override void RemoveNode(Node node)
	{
		base.RemoveNode(node);

		nodesCreated.Remove(node);
	}
}