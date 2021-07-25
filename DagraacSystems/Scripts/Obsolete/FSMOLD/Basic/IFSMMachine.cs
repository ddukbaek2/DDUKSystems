using System;
using System.Collections.Generic;
using System.Text;

namespace DagraacSystems.Obsolete
{
	/// <summary>
	/// 상태기계 인터페이스.
	/// </summary>
	public interface IFSMMachine
	{
		/// <summary>
		/// 상태의 프레임 처리.
		/// </summary>
		void FrameMove(float deltaTime);

		/// <summary>
		/// 다음 상태로 전이.
		/// </summary>
		void To(IFSMState next, int layer = 0);

		/// <summary>
		/// 다음 상태로 전이.
		/// </summary>
		void To(int key, int layer = 0);

		/// <summary>
		/// 현재 동작중인 레이어와 각 상태 컨테이너.
		/// </summary>
		Dictionary<int, IFSMState> GetCurrent();

		/// <summary>
		/// 현재 들고 있는 상태 목록.
		/// </summary>
		Dictionary<int, IFSMState> GetStates();

		/// <summary>
		/// 상태의 소유권자 설정.
		/// </summary>
		void SetOwnerTarget(IFSMTarget target);

		/// <summary>
		/// 상태의 소유권자 반환.
		/// </summary>
		IFSMTarget GetOwnerTarget();
	}
}