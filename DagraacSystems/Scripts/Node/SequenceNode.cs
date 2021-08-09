namespace DagraacSystems.Node
{
	/// <summary>
	/// 시퀀스에 포함되는 데이터.
	/// </summary>
	public interface ISequenceData
	{

	}


	/// <summary>
	/// 시퀀스 처리기.
	/// </summary>
	public class Sequence
	{
		public class SequenceNode : SiblingNode<ISequenceData>
		{
		}


		public SequenceNode Root { private set; get; }

		public Sequence()
		{
			Root = new SequenceNode();
		}

		public SequenceNode AddData<TSequenceData>(int layerIndex) where TSequenceData : ISequenceData, new()
		{
			var action = new TSequenceData();
			var node = new SequenceNode();
			node.SetParent(GetLayer(layerIndex));
			node.Value = action;
			return node;
		}

		public SequenceNode SetData<TSequenceData>(int layerIndex, int actionIndex, TSequenceData data) where TSequenceData : ISequenceData, new()
		{
			var layer = GetLayer(layerIndex);
			if (layer == null)
				return null;

			if (actionIndex < 0)
			{
				var node = AddData<TSequenceData>(layerIndex);
				node.SetAsFirstSibling();
				node.Value = data;
				return node;
			}
			else if (actionIndex >= layer.ChildCount)
			{
				var node = AddData<TSequenceData>(layerIndex);
				node.SetAsLastSibling();
				node.Value = data;
				return node;
			}
			else
			{
				var node = (SequenceNode)layer.GetChild(actionIndex);
				node.Value = data;
				return node;
			}
		}

		public SequenceNode RemoveData(int layerIndex, int dataIndex)
		{
			var layer = GetLayer(layerIndex);
			if (layer == null)
				return null;

			return (SequenceNode)layer.RemoveChild(dataIndex);
		}

		public void RemoveAllActions(int layerIndex)
		{
			var layer = GetLayer(layerIndex);
			if (layer == null)
				return;

			while (layer.ChildCount > 0)
				layer.RemoveChild(0);
		}

		public SequenceNode GetLayer(int layerIndex)
		{
			return (SequenceNode)Root.GetChild(layerIndex);
		}

		public SequenceNode AddLayer<T>()
		{
			var node = new SequenceNode();
			node.SetParent(Root);
			return node;
		}

		public SequenceNode RemoveLayer(int layerIndex)
		{
			return (SequenceNode)Root.RemoveChild(layerIndex);
		}

		public void RemoveAllLayers()
		{
			while (Root.ChildCount > 0)
				Root.RemoveChild(0);
		}
	}
}