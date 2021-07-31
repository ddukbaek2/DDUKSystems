using System;


namespace DagraacSystems.FSM
{
	/// <summary>
	/// 상태 전이.
	/// </summary>
	public class FSMTransition : FSMInstance
	{
		private FSMState m_Source;
		private FSMState m_Destination;
		private Func<bool> m_Predicate;

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void OnCreate(params object[] args)
		{
			base.OnCreate(args);

			m_Source = args[0] as FSMState;
			m_Destination = args[1] as FSMState;
			m_Predicate = args[2] as Func<bool>;
		}

		protected override void OnExecute(params object[] args)
		{
			base.OnExecute(args);

			m_Source.Target.RunState(m_Destination);
			Finish();
		}

		/// <summary>
		/// 조건 체크.
		/// </summary>
		public virtual bool IsContidition()
		{
			return m_Predicate?.Invoke() ?? false;
		}

		/// <summary>
		/// 실행된 상태.
		/// </summary>
		public FSMState GetSourceState()
		{
			return m_Source;
		}

		/// <summary>
		/// 전이할 상태.
		/// </summary>
		public FSMState GetDestinationState()
		{
			return m_Destination;
		}
	}
}