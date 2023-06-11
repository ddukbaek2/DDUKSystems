namespace DagraacSystems
{
	/// <summary>
	/// 공유 클래스.
	/// </summary>
	public class SharedClass<T> : ManagedObject where T : SharedClass<T>, new()
	{
		/// <summary>
		/// 실제 인스턴스.
		/// </summary>
		private static T s_Instance = null;

		/// <summary>
		/// 인스턴스 반환.
		/// </summary>
		public static T Instance
		{
			get
			{
				if (s_Instance == null)
					s_Instance = Create();

				return s_Instance;
			}
		}

		/// <summary>
		/// 인스턴스 존재 유무.
		/// </summary>
		public static bool HasInstance
		{
			get
			{
				return s_Instance != null;
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void OnCreate(params object[] args)
		{
			if (s_Instance == null)
				s_Instance = (T)this;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			if (s_Instance != null && s_Instance == this)
				s_Instance = null;

			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public static T Create(params object[] args)
		{
			if (s_Instance != null)
				return s_Instance;

			return ManagedObject.Create<T>(args);
		}

		/// <summary>
		/// 해제.
		/// </summary>
		public virtual void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}
	}
}