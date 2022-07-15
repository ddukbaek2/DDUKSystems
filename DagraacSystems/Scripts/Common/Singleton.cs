namespace DagraacSystems
{
	/// <summary>
	/// 싱글톤.
	/// </summary>
	public class Singleton<T> : DisposableObject where T : Singleton<T>, new()
	{
		private static T _instance = default;
		public static T Instance
		{
			get
			{
				if (_instance == null)
					 Create();

				return _instance;
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected Singleton() : base()
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected virtual void OnCreate()
		{
			_instance = (T)this;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			_instance = null;

			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public static T Create()
		{
			if (_instance != null)
				return _instance;

			var instance = DisposableObject.Create<T>();
			instance.OnCreate();
			return instance;
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