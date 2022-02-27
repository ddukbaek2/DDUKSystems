using System.Collections;


namespace DagraacSystems
{
	/// <summary>
	/// 코루틴.
	/// </summary>
	public class Coroutine : DisposableObject
	{
		private enum Condition { Continue, Wait, Finished, }

		private IEnumerator _enumerator;
		private IYield _yield;
		private Condition _condition;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Coroutine() : base()
		{
			_enumerator = null;
			_yield = null;
			_condition = Condition.Finished;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			Stop();

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
		/// 시작.
		/// </summary>
		public void Start(IEnumerator enumerator)
		{
			_enumerator = enumerator;
			_yield = null;
			_condition = Condition.Continue;
		}

		/// <summary>
		/// 정지.
		/// </summary>
		public void Stop()
		{
			_enumerator = null;
			_yield = null;
			_condition = Condition.Finished;
		}

		/// <summary>
		/// 매 프레임마다 갱신.
		/// </summary>
		public void Tick(float tick)
		{
			switch (_condition)
			{
				case Condition.Continue:
					{
						_condition = Continue();
						if (_condition == Condition.Wait)
						{
							_yield.Begin();
						}

						break;
					}

				case Condition.Wait:
					{
						if (_yield.Stay(tick))
						{
							_yield.End();
							_condition = Condition.Continue;
						}

						break;
					}

				case Condition.Finished:
					{
						Stop();
						break;
					}
			}
		}

		/// <summary>
		/// 다음 코드블록을 수행한다.
		/// </summary>
		private Condition Continue()
		{
			if (_enumerator.MoveNext())
			{
				_yield = (IYield)_enumerator.Current;
				return Condition.Wait;
			}

			return Condition.Finished;
		}
	}
}