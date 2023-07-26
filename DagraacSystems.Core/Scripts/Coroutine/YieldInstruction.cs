namespace DagraacSystems
{
    /// <summary>
    /// 코루틴 지연 객체.
    /// </summary>
    public abstract class YieldInstruction : ManagedObject
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void OnCreate(params object[] args)
		{
			base.OnCreate(args);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 지연객체 시작.
		/// </summary>
		public void Start()
		{
			OnStart();
		}

		/// <summary>
		/// 지연객체 갱신.
		/// </summary>
		public bool Tick(float tick)
		{
			return OnTick(tick);
		}

		/// <summary>
		/// 지연객체 종료.
		/// </summary>
		public void Finish()
		{
			OnFinish();
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

		/// <summary>
		/// 시작됨.
		/// </summary>
		protected virtual void OnStart()
		{
		}

		/// <summary>
		/// 갱신됨.
		/// 참을 반환하면 현재 객체는 다음프레임에 파괴되고, 코루틴은 재개된다.
		/// </summary>
		protected virtual bool OnTick(float _tick)
		{
			return true;
		}

		/// <summary>
		/// 종료됨.
		/// </summary>
		protected virtual void OnFinish()
		{
		}
	}
}