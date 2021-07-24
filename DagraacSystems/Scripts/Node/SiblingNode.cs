using System.Collections.Generic;


namespace DagraacSystems.Node
{
	public class SiblingNode<T>
	{
		public SiblingNode<T> Parent { private set; get; } = null;
		public List<SiblingNode<T>> Children { private set; get; } = new List<SiblingNode<T>>();
		public T Value { private set; get; } = default;

		public SiblingNode<T> Root
		{
			get
			{
				var node = this;
				while (node.Parent != null)
					node = node.Parent;
				return node;
			}
		}

		public void SetParent(SiblingNode<T> parent)
		{
			if (Parent == parent)
				return;

			if (Parent != null)
			{
				Parent.Children.Remove(this);
			}

			Parent = parent;

			if (parent != null)
			{
				Parent.Children.Add(this);
			}
		}

		public void SetAsFirstSibling()
		{
			if (Parent == null)
				return;

			SetSiblingIndex(0);
		}

		public void SetAsLastSibling()
		{
			if (Parent == null)
				return;

			SetSiblingIndex(Parent.Children.Count);
		}

		public void SetSiblingIndex(int index)
		{
			if (Parent == null)
				return;

			Parent.Children.Remove(this);
			Parent.Children.Insert(index, this);
		}

		public int GetSiblingIndex()
		{
			if (Parent == null)
				return -1;

			return Parent.Children.IndexOf(this);
		}

		public int GetChildCount()
		{
			if (Parent == null)
				return 0;

			return Parent.Children.Count;
		}
	}
}