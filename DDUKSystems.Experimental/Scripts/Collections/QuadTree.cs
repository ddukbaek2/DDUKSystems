using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace DDUKSystems
{
	public enum QuadDirection
	{
		Invalid = -1,
		TopLeft,
		TopRight,
		BotomRight,
		BottomLeft,
	}

	public interface IQuadTreeNode<TValue>
	{
		TValue Value { get; }
		bool IsLeaf { get; }
		void SetNode(QuadDirection _quadDirection, IQuadTreeNode<TValue> _node);
		IQuadTreeNode<TValue> SetNode(QuadDirection _quadDirection);
	}


	public class QuadTree<TQuadTreeNode, TValue> : DisposableObject where TQuadTreeNode : IQuadTreeNode<TValue>, new()
	{
		private TQuadTreeNode m_Root;
		public Func<IEnumerator<TValue>, TQuadTreeNode> OnBuild { set; get; }
		public Dictionary<TValue, TQuadTreeNode> Datas { private set; get; }

		public QuadTree() : base()
		{
			m_Root = new TQuadTreeNode();
			Datas = new Dictionary<TValue, TQuadTreeNode>();
		}

		protected override void OnDispose(bool _explicitedDispose)
		{
			base.OnDispose(_explicitedDispose);
		}

		public void Build(IEnumerator<TValue> _values)
		{
			Datas.Clear();
			if (OnBuild != null)
				m_Root = OnBuild.Invoke(_values);
		}

		public void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}

		public void Search()
		{
		}
	}
}
