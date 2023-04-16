using System.Collections.Generic;


namespace DagraacSystems
{
	/// <summary>
	/// 관리 대상.
	/// </summary>
	public interface IPooledObject
	{
		void OnPush(FPool pool);
		void OnPop(FPool pool);
	}


	/// <summary>
	/// 프레임워크 오브젝트들을 관리하는 객체.
	/// </summary>
	public class FPool : FObject
	{
		protected Queue<IPooledObject> m_PooledObjects;

		public int Count => m_PooledObjects.Count;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public FPool() : base()
		{
			m_PooledObjects = new Queue<IPooledObject>();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			if (m_PooledObjects != null)
			{
				m_PooledObjects.Clear();
				m_PooledObjects = null;
			}

			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 집어넣음.
		/// </summary>
		public void Push(IPooledObject pooledObject)
		{
			pooledObject.OnPush(this);
			m_PooledObjects.Enqueue(pooledObject);
		}

		/// <summary>
		/// 꺼냄.
		/// </summary>
		public T Pop<T>() where T : IPooledObject
		{
			if (m_PooledObjects.Count == 0)
				return default;

			var pooledObject = (T)m_PooledObjects.Dequeue();
			pooledObject.OnPop(this);
			return pooledObject;
		}
	}
}