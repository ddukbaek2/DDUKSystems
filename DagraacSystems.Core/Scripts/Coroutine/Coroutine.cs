using System.Collections;


namespace DagraacSystems
{
	/// <summary>
	/// 코루틴.
	/// </summary>
	public class Coroutine : ManagedObject
	{
		/// <summary>
		/// 내부 상태.
		/// </summary>
		private enum Condition { Continue, Wait, Finished }

		private IEnumerator m_Enumerator;
		private YieldInstruction m_Yield;
		private Condition m_Condition;
		private bool m_IsRunning;

		/// <summary>
		/// 작동 중 여부.
		/// </summary>
		public bool IsRunning => m_IsRunning;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Coroutine() : base()
		{
			m_Enumerator = null;
			m_Yield = null;
			m_Condition = Condition.Finished;
			m_IsRunning = false;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void OnCreate(params object[] _args)
		{
			base.OnCreate(_args);
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
		/// 시작.
		/// </summary>
		public void Start(IEnumerator enumerator)
		{
			m_Enumerator = enumerator;
			m_Yield = null;
			m_Condition = Condition.Continue;
			m_IsRunning = true;
		}

		/// <summary>
		/// 정지.
		/// </summary>
		public void Stop()
		{
			m_Enumerator = null;
			m_Yield = null;
			m_Condition = Condition.Finished;
			m_IsRunning = false;
		}

		/// <summary>
		/// 매 프레임마다 갱신.
		/// </summary>
		public void Tick(float _tick)
		{
			if (!m_IsRunning)
				return;

			switch (m_Condition)
			{
				case Condition.Continue:
					{
						m_Condition = Coroutine.Continue(this);
						if (m_Condition == Condition.Wait)
						{
							if (m_Yield != null)
								m_Yield.Start();
						}

						break;
					}

				case Condition.Wait:
					{
						if (m_Yield != null)
						{
							if (m_Yield.Tick(_tick))
							{
								m_Yield.Finish();
								m_Yield = null;
								m_Condition = Condition.Continue;
							}
						}
						else
						{
							m_Condition = Condition.Continue;
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
		/// 다음 코드블록을 처리한다.
		/// </summary>
		private static Condition Continue(Coroutine _coroutine)
		{
			if (_coroutine == null || _coroutine.m_Enumerator == null)
				return Condition.Finished;

			if (_coroutine.m_Enumerator.MoveNext())
			{
				_coroutine.m_Yield = _coroutine.m_Enumerator.Current as YieldInstruction;
				return Condition.Wait;
			}

			return Condition.Finished;
		}
	}
}