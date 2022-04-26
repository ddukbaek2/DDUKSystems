namespace DagraacSystems
{
	/// <summary>
	/// 코루틴 지연 객체.
	/// </summary>
	public class YieldInstruction : DisposableObject
	{
		/// <summary>
		/// 생성.
		/// </summary>
		public YieldInstruction() : base()
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
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
		/// 지연객체 시작.
		/// </summary>
		public void Start()
		{
			OnStarted();
		}

		/// <summary>
		/// 지연객체 갱신.
		/// </summary>
		public bool Update(float tick)
		{
			return OnUpdated(tick);
		}

		/// <summary>
		/// 지연객체 종료.
		/// </summary>
		public void Finish()
		{
			OnFinished();
		}

		/// <summary>
		/// 갱신.
		/// </summary>
		public void Reset()
		{
		}

		/// <summary>
		/// 복제.
		/// </summary>
		public YieldInstruction Clone()
		{
			return (YieldInstruction)MemberwiseClone();
		}

		protected virtual void OnStarted()
		{
		}

		protected virtual bool OnUpdated(float tick)
		{
			// 종료 후 다음프레임에 재개.
			return true;
		}

		protected virtual void OnFinished()
		{
		}
	}
}