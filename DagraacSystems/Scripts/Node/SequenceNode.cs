namespace DagraacSystems.Node
{
	public class Sequence
	{
		public class Action
		{
		}


		public SiblingNode<Action> Root { private set; get; }

		public Sequence()
		{
			Root = new SiblingNode<Action>();
		}

		public SiblingNode<Action> AddAction<TAction>(int layerIndex) where TAction : Action, new()
		{
			var action = new TAction();
			var node = new SiblingNode<Action>();
			node.SetParent(GetLayer(layerIndex));
			node.Value = action;
			return node;
		}

		public void RemoveAction(int layerIndex, int actionIndex)
		{
			GetLayer(layerIndex)?.Children[actionIndex].SetParent(null);
		}

		public void RemoveAllActions(int layerIndex)
		{

		}

		public SiblingNode<Action> GetLayer(int layerIndex)
		{
			return Root.Children[layerIndex];
		}

		public SiblingNode<Action> AddLayer()
		{
			var node = new SiblingNode<Action>();
			node.SetParent(Root);
			return node;
		}

		public SiblingNode<Action> RemoveLayer(int layerIndex)
		{
			var node = Root.Children[layerIndex];
			node.SetParent(null);
			return node;
		}

		public void RemoveAllLayers()
		{
			while (Root.Children.Count > 0)
				Root.Children[0].SetParent(null);
		}
	}

	//public class TimeSequence<T> : Sequence
	//{
		
	//}
}