using System;


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


	/// <summary>
	/// 참이 될때까지 머무름.
	/// </summary>
	public class WaitUntil : YieldInstruction
	{
		private Func<bool> _condition;

		public WaitUntil(Func<bool> condition) : base()
		{
			_condition = condition;
		}

		protected override bool OnUpdated(float tick)
		{
			if (_condition == null)
				return false; // 무한 대기.

			return _condition.Invoke();
		}
	}


	/// <summary>
	/// 일정 시간(초)만큼 머무름.
	/// </summary>
	public class WaitForSeconds : YieldInstruction
	{
		private float _time;
		private float _duration;

		public WaitForSeconds(float duration) : base()
		{
			_duration = duration;
		}

		protected override void OnStarted()
		{
			_time = 0f;
		}

		protected override bool OnUpdated(float tick)
		{
			_time += tick;
			if (_time < _duration)
				return false; // 무한 대기.

			return true;
		}
	}
}