using System.Collections.Generic;


namespace DagraacSystems
{
	/// <summary>
	/// 관리 대상.
	/// </summary>
	public interface IPooledObject
	{
		void OnPush(Pool pool);
		void OnPop(Pool pool);
	}


	/// <summary>
	/// 프레임워크 오브젝트들을 관리하는 객체.
	/// </summary>
	public class Pool : FObject
	{
		protected Queue<IPooledObject> _pooledObjects;

		public int Count => _pooledObjects.Count;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Pool() : base()
		{
			_pooledObjects = new Queue<IPooledObject>();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			if (_pooledObjects != null)
			{
				_pooledObjects.Clear();
				_pooledObjects = null;
			}

			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 집어넣음.
		/// </summary>
		public void Push(IPooledObject pooledObject)
		{
			pooledObject.OnPush(this);
			_pooledObjects.Enqueue(pooledObject);
		}

		/// <summary>
		/// 꺼냄.
		/// </summary>
		public T Pop<T>() where T : IPooledObject
		{
			if (_pooledObjects.Count == 0)
				return default;

			var pooledObject = (T)_pooledObjects.Dequeue();
			pooledObject.OnPop(this);
			return pooledObject;
		}
	}
}