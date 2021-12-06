namespace DagraacSystems.Node
{
	public class TimeAction : Sequence.ISequenceAction
	{
		void Sequence.ISequenceAction.Execute()
		{
			throw new System.NotImplementedException();
		}

		void Sequence.ISequenceAction.Finish()
		{
			throw new System.NotImplementedException();
		}

		void Sequence.ISequenceAction.Update()
		{
			throw new System.NotImplementedException();
		}
	}


	/// <summary>
	/// 시퀀스 처리기.
	/// </summary>
	public class Sequence
	{
		/// <summary>
		/// 시퀀스에 포함되는 데이터.
		/// </summary>
		public interface ISequenceAction
		{
			void Execute();
			void Finish();
			void Update();
		}

		/// <summary>
		/// 구조.
		/// </summary>
		public class SequenceNode : SiblingNode<ISequenceAction>
		{
		}


		public SequenceNode Root { private set; get; }

		public Sequence()
		{
			Root = new SequenceNode();
		}

		public SequenceNode AddData<TSequenceAction>(int layerIndex) where TSequenceAction : ISequenceAction, new()
		{
			var action = new TSequenceAction();
			var node = new SequenceNode();
			node.SetParent(GetLayer(layerIndex));
			node.Value = action;
			return node;
		}

		public SequenceNode SetData<TSequenceAction>(int layerIndex, int actionIndex, TSequenceAction data) where TSequenceAction : ISequenceAction, new()
		{
			var layer = GetLayer(layerIndex);
			if (layer == null)
				return null;

			if (actionIndex < 0)
			{
				var node = AddData<TSequenceAction>(layerIndex);
				node.SetAsFirstSibling();
				node.Value = data;
				return node;
			}
			else if (actionIndex >= layer.ChildCount)
			{
				var node = AddData<TSequenceAction>(layerIndex);
				node.SetAsLastSibling();
				node.Value = data;
				return node;
			}
			else
			{
				var node = layer.GetChild<SequenceNode>(actionIndex);
				node.Value = data;
				return node;
			}
		}

		public SequenceNode RemoveData(int layerIndex, int dataIndex)
		{
			var layer = GetLayer(layerIndex);
			if (layer == null)
				return null;

			return layer.RemoveChild<SequenceNode>(dataIndex);
		}

		public void RemoveAllActions(int layerIndex)
		{
			var layer = GetLayer(layerIndex);
			if (layer == null)
				return;

			while (layer.ChildCount > 0)
				layer.RemoveChild<SequenceNode>(0);
		}

		public SequenceNode GetLayer(int layerIndex)
		{
			return Root.GetChild<SequenceNode>(layerIndex);
		}

		public SequenceNode AddLayer<T>()
		{
			var node = new SequenceNode();
			node.SetParent(Root);
			return node;
		}

		public SequenceNode RemoveLayer(int layerIndex)
		{
			return Root.RemoveChild<SequenceNode>(layerIndex);
		}

		public void RemoveAllLayers()
		{
			while (Root.ChildCount > 0)
				Root.RemoveChild<SequenceNode>(0);
		}
	}
}