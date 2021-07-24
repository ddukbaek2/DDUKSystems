using System.Collections.Generic;


namespace DagraacSystems.Node
{
	public class LinkedNode<T>
	{
		public LinkedNode<T> Previous { private set; get; } = null;
		public LinkedNode<T> Next { private set; get; } = null;
		public T Value { private set; get; } = default;

		public bool IsFirst => Previous == null;
		public bool IsLast => Next == null;

		public LinkedNode<T> First
		{
			get
			{
				var node = this;
				while (node.Previous != null)
					node = node.Previous;
				return node;
			}
		}

		public LinkedNode<T> Last
		{
			get
			{
				var node = this;
				while (node.Next != null)
					node = node.Next;
				return node;
			}
		}


		public void SetPrevious(LinkedNode<T> node)
		{
			if (node != null)
			{
				Previous.Next = node;
				node.Previous = Previous;
				node.Next = this;

				Previous = node;
			}
			else
			{
				Previous = null;
			}
		}

		public void SetNext(LinkedNode<T> node)
		{
			if (node != null)
			{
				Next.Previous = node;
				node.Previous = this;
				node.Next = Next;

				Next = node;
			}
			else
			{
				Next = null;
			}
		}

		public static LinkedNode<T> Create(IEnumerator<T> enumerator)
		{
			var current = new LinkedNode<T>();
			if (enumerator != null)
			{
				while (enumerator.MoveNext())
				{
					current.Value = enumerator.Current;
					current.SetNext(new LinkedNode<T>());
					current = current.Next;
				}

				current.SetNext(null);
			}

			return current;
		}
	}
}