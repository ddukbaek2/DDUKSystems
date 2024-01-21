using System;
using System.Collections.Generic; // Queue


namespace DDUKSystems
{
    /// <summary>
    /// 비동기 풀.
    /// 미리 사용량만큼 할당하여 확보된채로 하나씩 가져다가 사용하고, 사용이 끝나면 다시 풀에 반환시켜 메모리 할당을 최소화 한다.
    /// 다형성 구조를 가진 객체는 사용하면 문제가 발생할 수 있으니 사용불가 (최하위클래스를 기준으로 여러개의 풀을 사용하길 권장).
    /// IPooledObject 를 상속받으면 풀에서 빼내거나 다시 풀에 넣을떄 이벤트를 받을 수 있다.
    /// 사용 후 반환하지 않으면 현재 잔여량이 남지 않으므로 Dequeue(true) 를 통해 할당량을 증가시키면서 신규생성.
    /// </summary>
    public class PoolAsync<T> : DisposableObject, IPoolAsync<T> where T : class, new()
	{
		/// <summary>
		/// 기본 생성자.
		/// </summary>
		public class DefaultCreatableAsync : IPooledCreatableAsync<T>
		{
			void IPooledCreatableAsync<T>.CreateInstanceAsync(IPoolAsync _poolAsync, Action<T> _onComplete)
			{
				_onComplete?.Invoke(new T());
			}
		}


		/// <summary>
		/// 기본 소멸자.
		/// </summary>
		public class DefaultDisposableAsync : IPooledDisposableAsync<T>
		{
			void IPooledDisposableAsync<T>.DisposeInstanceAsync(IPoolAsync _poolAsync, T _value)
			{
			}
		}


		/// <summary>
		/// 내부에서 사용될 자료구조.
		/// </summary>
		private Queue<T> queue;

		/// <summary>
		/// 생성.
		/// </summary>
		private IPooledCreatableAsync<T> creatableAsync;

		/// <summary>
		/// 파괴.
		/// </summary>
		private IPooledDisposableAsync<T> disposableAsync;

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
		public PoolAsync(IPooledCreatableAsync<T> _creatableAsync = null, IPooledDisposableAsync<T> _disposableAsync = null)
		{
			queue = new Queue<T>();
			creatableAsync = _creatableAsync;
			disposableAsync = _disposableAsync;

			if (creatableAsync == null)
				creatableAsync = new DefaultCreatableAsync();
			if (disposableAsync == null)
				disposableAsync = new DefaultDisposableAsync();
		}

		/// <summary>
		/// 인터페이스 구현.
		/// </summary>
		void IPoolAsync.Enqueue(object _obj) => Enqueue((T)_obj);

		/// <summary>
		/// 인터페이스 구현.
		/// </summary>
		void IPoolAsync.DequeueAsync(Action<object> _onComplete, bool _autoIncrease, bool _silentEnqueue) => DequeueAsync((_obj) => _onComplete?.Invoke(_obj), _autoIncrease, _silentEnqueue);

		/// <summary>
		/// 인터페이스 구현.
		/// </summary>
		bool IPoolAsync.Contains(object _obj) => Contains((T)_obj);

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
				disposableAsync.DisposeInstanceAsync(this, obj);
			}

			queue.Clear();
		}

		/// <summary>
		/// 포함 여부.
		/// </summary>
		public bool Contains(T _obj)
		{
			if (_obj == default)
				return false;

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

			if (_obj is IPooledObjectAsync<T>)
			{
				var pooledObjectAsync = _obj as IPooledObjectAsync<T>;
				pooledObjectAsync.OnEnqueued(this);
			}
			else if (_obj is IPooledObjectAsync)
			{
				var pooledObjectAsync = _obj as IPooledObjectAsync;
				pooledObjectAsync.OnEnqueued(this);
			}

			queue.Enqueue(_obj);
			OnEnqueued?.Invoke(_obj);
		}

		/// <summary>
		/// 풀에서 빼냄 (없을 경우 추가 생성을 위한 비동기).
		/// 빼낸 오브젝트는 수동삭제 필요.
		/// </summary>
		public void DequeueAsync(Action<T> _onComplete, bool _autoIncrease = false, bool _silentEnqueue = true)
		{
			var obj = default(T);
			if (queue.Count > 0)
			{
				obj = queue.Dequeue();

				if (obj is IPooledObjectAsync<T>)
				{
					var pooledObjectAsync = obj as IPooledObjectAsync<T>;
					pooledObjectAsync.OnDequeued(this);
				}
				else if (obj is IPooledObjectAsync)
				{
					var pooledObjectAsync = obj as IPooledObjectAsync;
					pooledObjectAsync.OnDequeued(this);
				}

				_onComplete?.Invoke(obj);
				OnDequeued?.Invoke(obj);
			}
			else
			{
				if (!_autoIncrease)
				{
					_onComplete?.Invoke(default);
					return;
				}

				creatableAsync.CreateInstanceAsync(this, (_obj) =>
				{
					if (_obj == default)
					{
						_onComplete?.Invoke(default);
						return;
					}

					if (_silentEnqueue)
					{
						queue.Enqueue(_obj);
					}
					else
					{
						Enqueue(_obj);
					}

					obj = queue.Dequeue();
					OnDequeued?.Invoke(obj);

					if (obj is IPooledObjectAsync<T>)
					{
						var pooledObjectAsync = obj as IPooledObjectAsync<T>;
						pooledObjectAsync.OnDequeued(this);
					}
					else if (obj is IPooledObjectAsync)
					{
						var pooledObjectAsync = obj as IPooledObjectAsync;
						pooledObjectAsync.OnDequeued(this);
					}

					_onComplete?.Invoke(obj);
				});
			}
		}
	}
}