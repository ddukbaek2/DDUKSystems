﻿using System.Collections;
using System.Collections.Generic;


namespace DagraacSystems.Node
{
	/// <summary>
	/// 형제노드 인터페이스.
	/// </summary>
	public interface ISiblingNode
	{
		ISiblingNode Root { get; }
		ISiblingNode Parent { get; }

		void SetParent(ISiblingNode node);

		void AddChild(ISiblingNode node);
		ISiblingNode RemoveChild(ISiblingNode node);
		ISiblingNode GetChild(int index);

		int IndexOf();
		bool Contains(ISiblingNode node);

		void SetAsFirstSibling();
		void SetAsLastSibling();
		void SetSiblingIndex(int index);
	}

	/// <summary>
	/// 트리형 계층구조에 자료를 보관하는 용도의 형제노드.
	/// </summary>
	public class SiblingNode<TValue> : DisposableObject, ISiblingNode
	{
		/// <summary>
		/// 반복자 구현.
		/// </summary>
		public class Enumerator : IEnumerator<TValue>
		{
			private SiblingNode<TValue> _target;
			private int _index;

			object IEnumerator.Current => _target != null ? ((SiblingNode<TValue>)_target.Children[_index]).Value : default;
			TValue IEnumerator<TValue>.Current => _target != null ? ((SiblingNode<TValue>)_target.Children[_index]).Value : default;

			public Enumerator(SiblingNode<TValue> target)
			{
				_target = target;
				_index = 0;
			}

			~Enumerator()
			{
				_target = null;
				_index = 0;
			}

			public void Dispose()
			{
				_target = null;
				_index = 0;
			}

			public bool MoveNext()
			{
				return ++_index < (_target != null ? _target.Children.Count : 0);
			}

			public void Reset()
			{
				_index = 0;
			}
		}

		ISiblingNode ISiblingNode.Parent => Parent;

		public SiblingNode<TValue> Parent { private set; get; } = null;

		public List<ISiblingNode> Children { private set; get; } = new List<ISiblingNode>();

		public TValue Value { set; get; } = default;

		public int ChildCount => Children.Count;

		public ISiblingNode Root
		{
			get
			{
				var node = this;
				while (node.Parent != null)
					node = node.Parent;
				return node;
			}
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}

		public void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}

		protected virtual void OnChangeParent(ISiblingNode parent)
		{
		}

		public void SetParent(ISiblingNode parent)
		{
			if (Parent == parent)
				return;

			if (Parent != null)
			{
				Parent.Children.Remove(this);
			}

			Parent = (SiblingNode<TValue>)parent;

			if (parent != null)
			{
				Parent.Children.Add(this);
			}
		}

		public void AddChild(ISiblingNode child)
		{
			if (child == null)
				return;

			child.SetParent(this);
		}

		public TSiblingNode RemoveChild<TSiblingNode>(int index) where TSiblingNode : SiblingNode<TValue>
		{
			return (TSiblingNode)RemoveChild(GetChild(index));
		}

		public ISiblingNode RemoveChild(int index)
		{
			return RemoveChild(GetChild(index));
		}

		public ISiblingNode RemoveChild(ISiblingNode child)
		{
			if (child != null)
				child.SetParent(null);
			return child;
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

		public int IndexOf()
		{
			if (Parent == null)
				return -1;
			
			return Parent.Children.IndexOf(this);
		}

		public bool Contains(ISiblingNode node)
		{
			return Children.Contains(node);
		}

		public ISiblingNode GetChild(int index)
		{
			if (Parent == null)
				return null;

			if (index < 0 || index >= Children.Count)
				return null;

			return Children[index];
		}

		public TSiblingNode GetChild<TSiblingNode>(int index) where TSiblingNode : SiblingNode<TValue>
		{
			var child = GetChild(index);
			if (child == null)
				return default;

			return (TSiblingNode)child;
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

		public TValue[] ToSiblingArray()
		{
			if (Parent != null)
			{
				var array = new TValue[Parent.Children.Count];
				for (var i = 0; i < array.Length; ++i)
					array[i] = ((SiblingNode<TValue>)Parent.Children[i]).Value;
				return array;
			}

			return new TValue[0];
		}

		/// <summary>
		/// 자신의 자식들에 대한 반복자 반환.
		/// </summary>
		public IEnumerator<TValue> GetChildEnumerator()
		{
			return new Enumerator(this);
		}

		/// <summary>
		/// 자신의 부모의 자식(자신과 형제들)에 대한 반복자 반환.
		/// </summary>
		public IEnumerator<TValue> GetSiblingEnumerator()
		{
			return new Enumerator(Parent);
		}

		public static SiblingNode<TValue> Convert(IEnumerable array)
		{
			return Convert(array.GetEnumerator());
		}

		public static SiblingNode<TValue> Convert(IEnumerator enumerator)
		{
			var root = new SiblingNode<TValue>();
			while (enumerator.MoveNext())
			{
				var node = new SiblingNode<TValue>();
				node.Value = (TValue)enumerator.Current;
				node.SetParent(root);
			}

			return root;
		}
	}
}