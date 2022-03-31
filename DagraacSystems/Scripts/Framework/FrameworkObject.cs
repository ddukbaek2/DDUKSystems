namespace DagraacSystems.Framework
{
	/// <summary>
	/// 프레임워크 내에 속한 모든 관리되는 인스턴스의 최상위 객체.
	/// </summary>
	public abstract class FrameworkObject : DisposableObject, ISubscriber
	{
		private bool _isActive;
		private Framework _framework;
		private ulong _instanceID;

		public bool IsActive { set => SetActive(value); get => _isActive; }
		public Framework Framework => _framework;
		public ulong InstanceID => _instanceID;

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected FrameworkObject() : base()
		{
			_isActive = false;
			_framework = null;
			_instanceID = 0ul;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected FrameworkObject(ulong instanceID) : base()
		{
			_isActive = false;
			_framework = null;
			_instanceID = instanceID;
		}

		[Subscribe(typeof(OnObjectCreate))]
		protected virtual void OnCreate()
		{
			//Logger.Log("[RPGObject] OnCreate()");

			if (_instanceID == 0)
				_instanceID = Framework.UniqueIdentifier.Generate();

			IsActive = true;
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			//Logger.Log("[RPGObject] OnDispose()");

			Framework.Messenger.Remove(this);
			Framework.UniqueIdentifier.Free(_instanceID);
			_framework = null;
			_instanceID = 0;

			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 파괴.
		/// </summary>
		public virtual void Dispose()
		{
			if (IsDisposed)
				return;

			FrameworkObject.Dispose(this);
		}

		/// <summary>
		/// 활성화됨.
		/// </summary>
		protected virtual void OnActive()
		{
		}

		/// <summary>
		/// 비활성화됨.
		/// </summary>
		protected virtual void OnDeactive()
		{
		}

		/// <summary>
		/// 활성화.
		/// </summary>
		public void SetActive(bool isActive, bool isForced = false)
		{
			if (_isActive != isActive || isForced)
			{
				_isActive = isActive;
				if (_isActive)
					OnActive();
				else
					OnDeactive();
			}
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public static TObject Create<TObject>(Framework framework) where TObject : FrameworkObject, new()
		{
			return Create<TObject>(framework, 0);
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public static TObject Create<TObject>(Framework framework, ulong instanceID) where TObject : FrameworkObject, new()
		{
			if (framework == null)
			{
				return default;
			}

			framework.UniqueIdentifier.Synchronize(instanceID);

			var target = DisposableObject.Create<TObject>();
			target._framework = framework;
			target._instanceID = instanceID;

			framework.Messenger.Add(target);
			framework.Messenger.Send(target, new OnObjectCreate { });
			return target;
		}

		/// <summary>
		/// 제거.
		/// </summary>
		public static void Dispose(FrameworkObject target)
		{
			if (target != null)
			{
				DisposableObject.Dispose(target);
				target = null;
			}
		}
	}
}
