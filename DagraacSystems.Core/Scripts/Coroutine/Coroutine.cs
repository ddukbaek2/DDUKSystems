using System.Collections;


namespace DagraacSystems
{
	/// <summary>
	/// 코루틴.
	/// </summary>
	public class Coroutine : ManagedObject, IUpdateTarget
	{
		/// <summary>
		/// 내부 상태.
		/// </summary>
		private enum Condition { Continue, Wait, Finished }

		private IEnumerator m_Enumerator;
		private IYieldInstruction m_YieldInstruction;
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
			m_YieldInstruction = null;
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
			m_YieldInstruction = null;
			m_Condition = Condition.Continue;
			m_IsRunning = true;
		}

		/// <summary>
		/// 정지.
		/// </summary>
		public void Stop()
		{
			m_Enumerator = null;
			m_YieldInstruction = null;
			m_Condition = Condition.Finished;
			m_IsRunning = false;
		}

		/// <summary>
		/// 완료될때까지 대기.
		/// </summary>
		public WaitWhile WaitForCompletion()
		{
			return new WaitWhile(() => !IsRunning);
		}

		/// <summary>
		/// 매 프레임마다 갱신.
		/// 시스템쪽에서 콜해주는 것으로, Check 자체가 지정타이밍에 이루어진다.
		/// </summary>
		void IUpdateTarget.Update(float deltaTime)
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
							if (m_YieldInstruction != null)
								m_YieldInstruction.Start();
						}

						break;
					}

				case Condition.Wait:
					{
						if (m_YieldInstruction != null)
						{
							if (m_YieldInstruction.Update(deltaTime))
							{
								m_YieldInstruction.Complete();
								m_YieldInstruction = null;
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
		private static Condition Continue(Coroutine coroutine)
		{
			if (coroutine == null || coroutine.m_Enumerator == null)
				return Condition.Finished;

			if (!coroutine.m_Enumerator.MoveNext())
				return Condition.Finished;

			coroutine.m_YieldInstruction = coroutine.m_Enumerator.Current as IYieldInstruction;
			return Condition.Wait;
		}
	}
}