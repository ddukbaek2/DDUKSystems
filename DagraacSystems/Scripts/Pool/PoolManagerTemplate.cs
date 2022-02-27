using System;
using System.Collections.Generic;
using System.Text;

namespace DagraacSystems.Pool
{
	public interface IPooledObject
	{
		void AwakeFromPool();
		void SleepToPool();
	}

	public class PoolManagerTemplate<TPoolManager, TKey> : Singleton<TPoolManager>
		where TPoolManager : PoolManagerTemplate<TPoolManager, TKey>, new()
	{
		public class PooledObjectMetaData
		{

		}


		private Dictionary<TKey, List<IPooledObject>> m_Objects;

		public PoolManagerTemplate() : base()
		{
			m_Objects = new Dictionary<TKey, List<IPooledObject>>();
		}

		protected override void OnDispose(bool disposing)
		{
		}

		protected virtual IPooledObject CreateObject(TKey key)
		{
			return null;
		}

		public void CreatePool<T>()
		{

		}

		public void DestroyPool<T>()
		{

		}

		public void Push(IPooledObject pooledObject)
		{
			//pooledObject
		}

		public IPooledObject Pop()
		{
			return null;
		}
	}
}
