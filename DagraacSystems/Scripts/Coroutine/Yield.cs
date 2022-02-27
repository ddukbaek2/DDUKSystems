using System;


namespace DagraacSystems
{
	/// <summary>
	/// 코루틴의 지연 객체.
	/// </summary>
	public interface IYield
	{
		void Begin();
		bool Stay(float tick); // 참이 되면 End() 후 다음 프레임에 재개.
		void End();
	}

	/// <summary>
	/// 실제 지연객체.
	/// </summary>
	public class Yield : DisposableObject, IYield
	{
		/// <summary>
		/// 생성.
		/// </summary>
		public Yield() : base()
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

		void IYield.Begin()
		{
			OnBegin();
		}

		bool IYield.Stay(float tick)
		{
			return OnStay(tick);
		}

		void IYield.End()
		{
			OnEnd();
		}

		protected virtual void OnBegin()
		{
		}

		protected virtual bool OnStay(float tick)
		{
			// 종료 후 다음프레임에 재개.
			return true;
		}

		protected virtual void OnEnd()
		{
		}
	}

	/// <summary>
	/// 참이 될때까지 머무름.
	/// </summary>
	public class WaitUntil : Yield
	{
		private Func<bool> _condition;

		public WaitUntil(Func<bool> condition) : base()
		{
			_condition = condition;
		}

		protected override bool OnStay(float tick)
		{
			if (_condition == null)
				return false; // 무한 대기.

			return _condition.Invoke();
		}
	}

	/// <summary>
	/// 일정 초만큼 머무름.
	/// </summary>
	public class WaitForSeconds : Yield
	{
		private float _time;
		private float _duration;

		public WaitForSeconds(float seconds) : base()
		{
			_duration = seconds;
		}

		protected override void OnBegin()
		{
			_time = 0f;
		}

		protected override bool OnStay(float tick)
		{
			_time += tick;
			if (_time < _duration)
				return false; // 무한 대기.

			return true;
		}
	}
}