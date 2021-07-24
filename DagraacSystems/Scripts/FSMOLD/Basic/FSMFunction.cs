using System.Collections.Generic;


namespace DagraacSystems.FSM
{
	/// <summary>
	/// 상태처리기의 실제 기능 구현.
	/// </summary>
	public static class FSMFunction
	{
		/// <summary>
		/// 임시 상태 배열.
		/// </summary>
		private static Dictionary<int, IFSMState> s_TemporaryChangeList = new Dictionary<int, IFSMState>();

		/// <summary>
		/// 매 프레임 마다 처리.
		/// </summary>
		public static void FrameMove(IFSMMachine machine, float deltaTime)
		{
			s_TemporaryChangeList.Clear();
			foreach (var pair in machine.GetCurrent())
			{
				if (pair.Value == null)
					continue;

				pair.Value.SetOwnerMachine(machine);
				pair.Value.FrameMove(deltaTime);

				var next = pair.Value.Decide();
				if (pair.Value != next)
				{
					s_TemporaryChangeList.Add(pair.Key, next);
				}
			}

			foreach (var pair in s_TemporaryChangeList)
			{
				FSMFunction.To(machine, pair.Value, pair.Key);
			}
		}

		/// <summary>
		/// 상태 트랜지션.
		/// </summary>
		public static void To(IFSMMachine machine, IFSMState next, int layer)
		{
			var current = machine.GetCurrent();
			var contains = current.ContainsKey(layer);

			// exit or new.
			if (contains)
			{
				current[layer].SetOwnerMachine(machine);
				current[layer].Exit();
				current[layer] = next;
			}
			else
			{
				current.Add(layer, next);
			}

			// enter or delete.
			if (current[layer] != null)
			{
				current[layer].SetOwnerMachine(machine);
				current[layer].Enter();
			}
			else
			{
				current.Remove(layer);
			}
		}

		/// <summary>
		/// 상태 트랜지션.
		/// </summary>
		public static void To(IFSMMachine machine, int next, int layer = 0)
		{
			var state = GetStateFromKey(machine, next);
			To(machine, state, layer);
		}

		public static IFSMState GetCurrentState(IFSMMachine machine, int layer = 0)
		{
			var current = machine.GetCurrent();
			if (current.ContainsKey(layer))
			{
				return current[layer];
			}
			return null;
		}

		/// <summary>
		/// 머신에 상태 추가.
		/// </summary>
		public static void AddState(IFSMMachine machine, int key, IFSMState state)
		{
			var states = machine.GetStates();
			if (states.ContainsKey(key))
			{
				states[key] = state;
			}
			else
			{
				states.Add(key, state);
			}
		}

		/// <summary>
		/// 머신에 상태 제거.
		/// </summary>
		public static void RemoveState(IFSMMachine machine, int key)
		{
			var states = machine.GetStates();
			if (states.ContainsKey(key))
			{
				states.Remove(key);
			}
		}

		/// <summary>
		/// 머신에 모든 상태 제거.
		/// </summary>
		public static void ClearAllStates(IFSMMachine machine)
		{
			var states = machine.GetStates();
			states.Clear();
		}

		/// <summary>
		/// 머신에서 상태 얻어옴.
		/// </summary>
		public static IFSMState GetStateFromKey(IFSMMachine machine, int key)
		{
			var states = machine.GetStates();
			if (states.ContainsKey(key))
			{
				return states[key];
			}
			return null;
		}

		/// <summary>
		/// 머신에서 상태키 얻어옴.
		/// </summary>
		public static bool GetKeyFromState(IFSMMachine machine, IFSMState state, out int key)
		{
			var states = machine.GetStates();
			foreach (var pair in states)
			{
				if (pair.Value == state)
				{
					key = pair.Key;
					return true;
				}
			}

			// 실패.
			key = 0;
			return false;
		}

		public static T GetOwnerMachine<T>(IFSMState state) where T : IFSMMachine
		{
			return (T)state.GetOwnerMachine();
		}

		public static T GetOwnerTarget<T>(IFSMMachine machine) where T : IFSMTarget
		{
			return (T)machine.GetOwnerTarget();
		}

		public static T GetOwnerTarget<T>(IFSMState state) where T : IFSMTarget
		{
			return GetOwnerTarget<T>(state.GetOwnerMachine());
		}
	}
}