using XNode;
using Node = XNode.Node;

[CreateNodeMenu("StoryCreator/StartStory")]
public class StartStoryNode : Node
{
	[Output] public StoryBaseNode startStoryOutput;

	public StoryBaseNode nodeTest;
	
	public override void OnCreateConnection(NodePort from, NodePort to) 
	{
		base.OnCreateConnection(from, to);
		
		StartStoryNode fromNode = from.node as StartStoryNode;
		StoryBaseNode toNode = to.node as StoryBaseNode;
		
		if (fromNode == null || toNode == null) return;

		if (from.GetConnections().Count > 1)
			for (int i = 0; i < from.GetConnections().Count; i++)
				from.Disconnect(i);

		if (to.fieldName == "input" && from.fieldName == "startStoryOutput") 
			startStoryOutput = toNode;
	}
	
	public override void OnRemoveConnection(NodePort port)
	{
		base.OnRemoveConnection(port);

		if (port.fieldName.Equals("startStoryOutput"))
			startStoryOutput = null;
	}
}