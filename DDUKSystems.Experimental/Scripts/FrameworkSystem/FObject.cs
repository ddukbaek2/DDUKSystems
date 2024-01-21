namespace DDUKSystems
{
	/// <summary>
	/// 프레임워크 내에 속한 모든 관리되는 인스턴스의 최상위 객체.
	/// </summary>
	public abstract class FObject : ManagedObject, ISubscriber
	{
		private bool m_IsActive;

		/// <summary>
		/// 활성화 여부 (기본값: 꺼짐).
		/// </summary>
		public bool IsActive { set => SetActive(value); get => m_IsActive; }

		/// <summary>
		/// 참조된 프레임워크.
		/// </summary>
		public FrameworkSystem FrameworkSystem { internal set; get; }

		/// <summary>
		/// 고유식별자.
		/// </summary>
		public ulong InstanceID { internal set; get; }

		///// <summary>
		///// 생성됨.
		///// </summary>
		//protected FrameworkObject(FrameworkSystem _franeworkSystem = null, ulong instanceID = 0ul) : base()
		//{
		//	m_IsActive = false;
		//	FrameworkSystem = _franeworkSystem;
		//	InstanceID = instanceID;
		//}

		protected override void OnCreate(params object[] _args)
		{
			base.OnCreate(_args);

			if (InstanceID == 0)
				InstanceID = FrameworkSystem.UniqueIdentifier.New();

			IsActive = true;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			FrameworkSystem.MessageSystem.Remove(this);
			FrameworkSystem.UniqueIdentifier.Delete(InstanceID);
			FrameworkSystem = null;
			InstanceID = 0;

			base.OnDispose(explicitedDispose);
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
			if (m_IsActive != isActive || isForced)
			{
				m_IsActive = isActive;
				if (m_IsActive)
					OnActive();
				else
					OnDeactive();
			}
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public static TObject Create<TObject>(FrameworkSystem framework) where TObject : FObject, new()
		{
			return Create<TObject>(framework, 0);
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public static TObject Create<TObject>(FrameworkSystem framework, ulong instanceID) where TObject : FObject, new()
		{
			if (framework == null)
			{
				return default;
			}

			framework.UniqueIdentifier.Synchronize(instanceID);

			var target = ManagedObject.Create<TObject>();
			target.FrameworkSystem = framework;
			target.InstanceID = instanceID;
			framework.MessageSystem.Add(target);

			return target;
		}

		/// <summary>
		/// 제거.
		/// </summary>
		public static void Dispose(FObject target)
		{
			if (target != null)
			{
				DisposableObject.Dispose(target);
				target = null;
			}
		}
	}
}
