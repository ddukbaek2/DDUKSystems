namespace DagraacSystems.Framework
{
	/// <summary>
	/// 프레임워크 내에 속한 모든 관리되는 인스턴스의 최상위 객체.
	/// </summary>
	public abstract class Object : DisposableObject, ISubscriber
	{
		public Framework Framework { private set; get; }
		public ulong InstanceID { private set; get; }
		public bool IsActive { private set; get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected Object() : base()
		{
			Framework = null;
			InstanceID = 0ul;
			IsActive = false;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected Object(ulong instanceID) : base()
		{
			Framework = null;
			InstanceID = instanceID;
			IsActive = false;
		}

		[Subscribe(typeof(OnObjectCreate))]
		protected virtual void OnCreate()
		{
			//Logger.Log("[RPGObject] OnCreate()");

			if (InstanceID == 0)
				InstanceID = Framework.UniqueIdentifier.Generate();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			//Logger.Log("[RPGObject] OnDispose()");

			Framework.Messenger.Remove(this);
			Framework.UniqueIdentifier.Free(InstanceID);
			InstanceID = 0;
			Framework = null;

			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 파괴.
		/// </summary>
		public virtual void Dispose()
		{
			if (IsDisposed)
				return;

			Object.Dispose(this);
		}

		/// <summary>
		/// 활성화.
		/// </summary>
		public void SetActive(bool active)
		{
			if (IsActive != active)
			{
				IsActive = active;
			}
		}

		/// <summary>
		/// 생성.
		/// </summary>
		[System.Obsolete("")]
		protected static new T Create<T>() where T : DisposableObject, new()
		{
			throw new System.Exception();
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public static TObject Create<TObject>(Framework framework) where TObject : Object, new()
		{
			var target = DisposableObject.Create<TObject>();
			target.Framework = framework;
			framework.Messenger.Add(target);
			framework.Messenger.Send(target, new OnObjectCreate { });
			return target;
		}


		/// <summary>
		/// 생성.
		/// </summary>
		public static TObject Create<TObject>(Framework framework, ulong instanceID) where TObject : Object, new()
		{
			framework.UniqueIdentifier.Synchronize(instanceID);

			var target = DisposableObject.Create<TObject>();
			target.Framework = framework;
			target.InstanceID = instanceID;
			framework.Messenger.Add(target);
			framework.Messenger.Send(target, new OnObjectCreate { });
			return target;
		}

		/// <summary>
		/// 제거.
		/// </summary>
		public static void Dispose(Object target)
		{
			if (target != null)
			{
				DisposableObject.Dispose(target);
				target = null;
			}
		}
	}
}
