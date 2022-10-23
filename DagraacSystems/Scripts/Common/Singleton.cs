namespace DagraacSystems
{
	/// <summary>
	/// 싱글톤.
	/// </summary>
	public class Singleton<T> : DisposableObject where T : Singleton<T>, new()
	{
		private static T _instance = null;
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
		protected override void OnCreate(params object[] args)
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
		public static T Create(params object[] args)
		{
			if (_instance != null)
				return _instance;

			return DisposableObject.Create<T>(args);
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