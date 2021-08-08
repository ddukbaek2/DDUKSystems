using System.Collections;
using System.Collections.Generic;


namespace DagraacSystems.Node
{
	/// <summary>
	/// 트리형 계층구조에 자료를 보관하는 용도.
	/// </summary>
	public class SiblingNode<T>
	{
		/// <summary>
		/// 반복자 구현.
		/// </summary>
		internal class Enumerator : IEnumerator<T>
		{
			private SiblingNode<T> m_Target;
			private int m_Index;

			object IEnumerator.Current => m_Target != null ? m_Target.Children[m_Index].Value : default;
			T IEnumerator<T>.Current => m_Target != null ? m_Target.Children[m_Index].Value : default;

			public Enumerator(SiblingNode<T> target)
			{
				m_Target = target;
				m_Index = 0;
			}

			~Enumerator()
			{
				m_Target = null;
				m_Index = 0;
			}

			public void Dispose()
			{
				m_Target = null;
				m_Index = 0;
			}

			public bool MoveNext()
			{
				return ++m_Index < (m_Target != null ? m_Target.Children.Count : 0);
			}

			public void Reset()
			{
				m_Index = 0;
			}
		}


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

		public T[] ToSiblingArray()
		{
			if (Parent != null)
			{
				var array = new T[Parent.Children.Count];
				for (var i = 0; i < array.Length; ++i)
					array[i] = Parent.Children[i].Value;
				return array;
			}

			return new T[0];
		}

		/// <summary>
		/// 자신의 자식들에 대한 반복자 반환.
		/// </summary>
		public IEnumerator<T> GetChildEnumerator()
		{
			return new Enumerator(this);
		}

		/// <summary>
		/// 자신의 부모의 자식(자신과 형제들)에 대한 반복자 반환.
		/// </summary>
		public IEnumerator<T> GetSiblingEnumerator()
		{
			return new Enumerator(Parent);
		}

		public static SiblingNode<T> Convert(IEnumerable array)
		{
			return Convert(array.GetEnumerator());
		}

		public static SiblingNode<T> Convert(IEnumerator enumerator)
		{
			var root = new SiblingNode<T>();
			while (enumerator.MoveNext())
			{
				var node = new SiblingNode<T>();
				node.Value = (T)enumerator.Current;
				node.SetParent(root);
			}

			return root;
		}
	}
}