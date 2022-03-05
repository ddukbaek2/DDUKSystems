namespace DagraacSystems.Framework
{
	/// <summary>
	/// RPGENGINE 내에 속한 모든 관리되는 인스턴스의 최상위 객체.
	/// </summary>
	public abstract class Object : DisposableObject, ISubscriber
	{
		public Framework Engine { private set; get; }
		public ulong InstanceID { private set; get; }
		public bool IsActive { private set; get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected Object() : base()
		{
			Engine = null;
			InstanceID = 0ul;
			IsActive = false;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected Object(ulong instanceID) : base()
		{
			Engine = null;
			InstanceID = instanceID;
			IsActive = false;
		}

		[Subscribe(typeof(OnObjectCreate))]
		protected virtual void OnCreate()
		{
			//Logger.Log("[RPGObject] OnCreate()");

			if (InstanceID == 0)
				InstanceID = Engine.UniqueIdentifier.Generate();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			//Logger.Log("[RPGObject] OnDispose()");

			Engine.Messenger.Remove(this);
			Engine.UniqueIdentifier.Free(InstanceID);
			InstanceID = 0;
			Engine = null;

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
		public static TRPGObject Create<TRPGObject>(Framework engine) where TRPGObject : Object, new()
		{
			var target = DisposableObject.Create<TRPGObject>();
			target.Engine = engine;
			engine.Messenger.Add(target);
			engine.Messenger.Send(target, new OnObjectCreate { });
			return target;
		}


		/// <summary>
		/// 생성.
		/// </summary>
		public static TRPGObject Create<TRPGObject>(Framework engine, ulong instanceID) where TRPGObject : Object, new()
		{
			engine.UniqueIdentifier.Synchronize(instanceID);

			var target = DisposableObject.Create<TRPGObject>();
			target.Engine = engine;
			target.InstanceID = instanceID;
			engine.Messenger.Add(target);
			engine.Messenger.Send(target, new OnObjectCreate { });
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
