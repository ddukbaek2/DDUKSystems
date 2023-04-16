using System;


namespace DagraacSystems
{
	/// <summary>
	/// 상태 전이.
	/// </summary>
	public class FSMTransition : FSMObject
	{
		private FSMState _source;
		private FSMState _destination;
		private Func<bool> _predicate;

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void OnCreate(params object[] args)
		{
			base.OnCreate(args);

			_source = args[0] as FSMState;
			_destination = args[1] as FSMState;
			_predicate = args[2] as Func<bool>;
		}

		protected override void OnExecute(params object[] args)
		{
			base.OnExecute(args);

			_source.Target.RunState(_destination);
			Finish();
		}

		/// <summary>
		/// 조건 체크.
		/// </summary>
		public virtual bool IsContidition()
		{
			return _predicate?.Invoke() ?? false;
		}

		/// <summary>
		/// 실행된 상태.
		/// </summary>
		public FSMState GetSourceState()
		{
			return _source;
		}

		/// <summary>
		/// 전이할 상태.
		/// </summary>
		public FSMState GetDestinationState()
		{
			return _destination;
		}
	}
}