namespace DagraacSystems
{
	/// <summary>
	/// 싱글톤 클래스.
	/// </summary>
	public class SharedClass<T> : ManagedObject where T : SharedClass<T>, new()
	{
		private static T instance = null;
		public static T Instance
		{
			get
			{
				if (instance == null)
					instance = Create();

				return instance;
			}
		}

		/// <summary>
		/// 인스턴스 존재 유무.
		/// </summary>
		public static bool HasInstance()
		{
			return instance != null;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void OnCreate(params object[] _args)
		{
			instance = (T)this;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool _explicitedDispose)
		{
			instance = null;

			base.OnDispose(_explicitedDispose);
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public static T Create(params object[] _args)
		{
			if (instance != null)
				return instance;

			return ManagedObject.Create<T>(_args);
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
	}
}