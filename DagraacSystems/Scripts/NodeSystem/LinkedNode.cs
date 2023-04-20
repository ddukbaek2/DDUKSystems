using System.Collections.Generic;


namespace DagraacSystems.Node
{
	public interface ILinkedNodeTarget
	{

	}

	/// <summary>
	/// 연결 노드.
	/// </summary>
	public class LinkedNode<TLinkedNodeTarget> where TLinkedNodeTarget : ILinkedNodeTarget
	{
		public LinkedNode<TLinkedNodeTarget> Previous { private set; get; } = null;
		public LinkedNode<TLinkedNodeTarget> Next { private set; get; } = null;
		public TLinkedNodeTarget Value { private set; get; } = default;

		public bool IsFirst => Previous == null;
		public bool IsLast => Next == null;

		public LinkedNode<TLinkedNodeTarget> First
		{
			get
			{
				var node = this;
				while (node.Previous != null)
					node = node.Previous;
				return node;
			}
		}

		public LinkedNode<TLinkedNodeTarget> Last
		{
			get
			{
				var node = this;
				while (node.Next != null)
					node = node.Next;
				return node;
			}
		}


		public void SetPrevious(LinkedNode<TLinkedNodeTarget> node)
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

		public void SetNext(LinkedNode<TLinkedNodeTarget> node)
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

		public static LinkedNode<TLinkedNodeTarget> Create(IEnumerator<TLinkedNodeTarget> enumerator)
		{
			var current = new LinkedNode<TLinkedNodeTarget>();
			if (enumerator != null)
			{
				while (enumerator.MoveNext())
				{
					current.Value = enumerator.Current;
					current.SetNext(new LinkedNode<TLinkedNodeTarget>());
					current = current.Next;
				}

				current.SetNext(null);
			}

			return current;
		}
	}
}