using System;


namespace DagraacSystems
{
	/// <summary>
	/// 해제 가능한 오브젝트 (일반 클래스 전용).
	/// 참조가 전부 날아가면 당연히 자동으로 제거되겠지만 명시적으로 해제해줌으로서 좀더 상황을 명확히 통제할 수 있음.
	/// </summary>
	public class DisposableObject : IDisposable
	{
		private bool _isDisposed;
		public bool IsDisposed => _isDisposed;

		/// <summary>
		/// 생성.
		/// </summary>
		public DisposableObject()
		{
			_isDisposed = false;
		}

		/// <summary>
		/// 소멸.
		/// </summary>
		~DisposableObject()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				OnDispose(false);
			}
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected virtual void OnDispose(bool explicitedDispose)
		{
		}

		/// <summary>
		/// IDisposable 상속.
		/// </summary>
		void IDisposable.Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				OnDispose(true);
			}

			// 소멸자 호출 금지.
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 박싱 회피용 비교 함수.
		/// </summary>
		public bool Equals(DisposableObject target)
		{
			return this == target;
		}

		/// <summary>
		/// 타입을 기준으로 생성.
		/// </summary>
		public static T Create<T>() where T : DisposableObject, new()
		{
			return new T();
		}

		/// <summary>
		/// 타입 인스턴스를 기준으로 생성.
		/// 별도로 타입 인스턴스를 체크 하지 않고 단순 생성 후 형변환하여 반환 하므로 사용상 주의.
		/// </summary>
		public static DisposableObject Create(Type type)
		{
			return Activator.CreateInstance(type) as DisposableObject;
		}

		/// <summary>
		/// 해제.
		/// 해당 오브젝트의 모든 상속자는 명시적 해제 기능을 직접 구현해야함 (자세한건 상속된 오브젝트 참고).
		/// </summary>
		protected static void Dispose(DisposableObject target)
		{
			if (target != null)
				((IDisposable)target)?.Dispose();
		}
	}
}