using System;
using System.Collections.Generic; // Queue


namespace DagraacSystems
{
	/// <summary>
	/// 풀.
	/// 미리 사용량만큼 할당하여 확보된채로 하나씩 가져다가 사용하고, 사용이 끝나면 다시 풀에 반환시켜 메모리 할당을 최소화 한다.
	/// 다형성 구조를 가진 객체는 사용하면 문제가 발생할 수 있으니 사용불가 (최하위클래스를 기준으로 여러개의 풀을 사용하길 권장).
	/// IPooledObject 를 상속받으면 풀에서 빼내거나 다시 풀에 넣을떄 이벤트를 받을 수 있다.
	/// 사용 후 반환하지 않으면 현재 잔여량이 남지 않으므로 Dequeue(true) 를 통해 할당량을 증가시키면서 신규생성.
	/// </summary>
	public class Pool<T> : DisposableObject, IPool<T> where T : class, new()
	{
		/// <summary>
		/// 기본 생성자.
		/// </summary>
		public class DefaultCreatable : IPooledCreatable<T>
		{
			T IPooledCreatable<T>.CreateInstance(IPool _pool)
			{
				return new T();
			}
		}


		/// <summary>
		/// 기본 소멸자.
		/// </summary>
		public class DefaultDisposable : IPooledDisposable<T>
		{
			void IPooledDisposable<T>.DisposeInstance(IPool _pool, T _value)
			{
				if (_value is DisposableObject)
				{
					var disposableObject = _value as DisposableObject;
					DisposableObject.SafeDispose(ref disposableObject);
				}
			}
		}


		/// <summary>
		/// 내부에서 사용될 자료구조.
		/// </summary>
		private Queue<T> queue;

		/// <summary>
		/// 생성.
		/// </summary>
		private IPooledCreatable<T> creatable;

		/// <summary>
		/// 파괴.
		/// </summary>
		private IPooledDisposable<T> disposable;

		/// <summary>
		/// 현재 잔여량.
		/// </summary>
		public int Count => queue.Count;

		/// <summary>
		/// 풀에 넣음.
		/// </summary>
		public Action<T> OnEnqueued { set; get; }

		/// <summary>
		/// 풀에서 빼냄.
		/// </summary>
		public Action<T> OnDequeued { set; get; }

		/// <summary>
		/// 생성.
		/// </summary>
		public Pool(IPooledCreatable<T> _creatable = default, IPooledDisposable<T> _disposable = default)
		{
			queue = new Queue<T>();
			creatable = _creatable;
			disposable = _disposable;

			if (creatable == default)
				creatable = new DefaultCreatable();
			if (disposable == default)
				disposable = new DefaultDisposable();
		}

		/// <summary>
		/// 인터페이스 구현.
		/// </summary>
		void IPool.Enqueue(object _obj) => Enqueue((T)_obj);

		/// <summary>
		/// 인터페이스 구현.
		/// </summary>
		object IPool.Dequeue(bool _autoIncrease, bool _silentEnqueue) => Dequeue(_autoIncrease, _silentEnqueue);

		/// <summary>
		/// 인터페이스 구현.
		/// </summary>
		bool IPool.Contains(object _obj) => Contains((T)_obj);

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool _explicitedDispose)
		{
			Clear();

			base.OnDispose(_explicitedDispose);
		}

		/// <summary>
		/// 해제.
		/// </summary>
		public void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}

		/// <summary>
		/// 모두 비운다.
		/// </summary>
		public void Clear()
		{
			while (Count > 0)
			{
				var obj = queue.Dequeue();
				disposable.DisposeInstance(this, obj);
			}

			queue.Clear();
		}

		/// <summary>
		/// 포함 여부.
		/// </summary>
		public bool Contains(T _obj)
		{
			return queue.Contains(_obj);
		}

		/// <summary>
		/// 풀에 넣음.
		/// </summary>
		public void Enqueue(T _obj)
		{
			if (_obj == default)
				return;

			if (queue.Contains(_obj))
				return;

			if (_obj is IPooledObject<T>)
			{
				var pooledObject = _obj as IPooledObject<T>;
				pooledObject.OnEnqueued(this);
			}
			else if (_obj is IPooledObject)
			{
				var pooledObject = _obj as IPooledObject;
				pooledObject.OnEnqueued(this);
			}

			queue.Enqueue(_obj);
			OnEnqueued?.Invoke(_obj);
		}

		/// <summary>
		/// 풀에서 빼냄.
		/// 빼낸 오브젝트는 수동삭제 필요.
		/// </summary>
		public T Dequeue(bool _autoIncrease = true, bool _silentEnqueue = true)
		{
			var obj = default(T);
			if (queue.Count > 0)
			{
				obj = queue.Dequeue();
				OnDequeued?.Invoke(obj);
			}
			else
			{
				if (!_autoIncrease)
					return default;

				obj = creatable.CreateInstance(this);
				if (obj == default)
					return default;

				if (_silentEnqueue)
				{
					queue.Enqueue(obj);
				}
				else
				{
					Enqueue(obj);
				}

				obj = queue.Dequeue();
				OnDequeued?.Invoke(obj);
			}

			if (obj is IPooledObject<T>)
			{
				var pooledObject = obj as IPooledObject<T>;
				pooledObject.OnDequeued(this);
			}
			else if (obj is IPooledObject)
			{
				var pooledObject = obj as IPooledObject;
				pooledObject.OnDequeued(this);
			}

			return obj;
		}
	}
}