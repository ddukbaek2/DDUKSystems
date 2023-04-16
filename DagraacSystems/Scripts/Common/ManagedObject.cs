using System; // Activator


namespace DagraacSystems
{
	/// <summary>
	/// 생성, 해제 가능한 오브젝트 (일반 클래스 전용).
	/// 생성시 인자를 넣어줄 수 있음 (다만 박싱으로 인해 과도한 밸류타입 사용 주의).
	/// ManagedObject.Create<TClass>() 로 사용한다.
	/// </summary>
	public class ManagedObject : DisposableObject
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		protected virtual void OnCreate(params object[] _args)
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool _explicitedDispose)
		{
			base.OnDispose(_explicitedDispose);
		}

		/// <summary>
		/// 타입을 기준으로 생성.
		/// </summary>
		public static T Create<T>(params object[] _args) where T : ManagedObject, new()
		{
			return Create(typeof(T), _args) as T;// new T();
		}

		/// <summary>
		/// 타입 인스턴스를 기준으로 생성.
		/// 별도로 타입 인스턴스를 체크 하지 않고 단순 생성 후 형변환하여 반환 하므로 사용상 주의.
		/// </summary>
		public static ManagedObject Create(Type _disposableObjectType, params object[] _args)
		{
			var managedObject = Activator.CreateInstance(_disposableObjectType) as ManagedObject;
			if (managedObject == null)
				return null;

			managedObject.OnCreate(_args);
			return managedObject;
		}

	}
}