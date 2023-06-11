//namespace DagraacSystems
//{
//	/// <summary>
//	/// 해제가능한 오브젝트의 해제 대행자.
//	/// </summary>
//	public class Disposer : DisposableObject
//	{
//		private DisposableObject m_Target;

//		/// <summary>
//		/// 생성.
//		/// </summary>
//		public Disposer(DisposableObject target) : base()
//		{
//			m_Target = target;
//		}

//		/// <summary>
//		/// 해제됨.
//		/// </summary>
//		protected override void OnDispose(bool explicitedDispose)
//		{
//			if (m_Target != null && !m_Target.IsDisposed)
//			{
//				DisposableObject.Dispose(m_Target);
//				m_Target = null;
//			}

//			base.OnDispose(explicitedDispose);
//		}

//		/// <summary>
//		/// 해제.
//		/// </summary>
//		public void Dispose()
//		{
//			if (IsDisposed)
//				return;

//			DisposableObject.Dispose(this);
//		}

//		/// <summary>
//		/// 반환.
//		/// </summary>
//		public TDisposableObject GetTarget<TDisposableObject>() where TDisposableObject : DisposableObject
//		{
//			return m_Target as TDisposableObject;
//		}

//		/// <summary>
//		/// 생성.
//		/// </summary>
//		public static Disposer Create(DisposableObject target)
//		{
//			return new Disposer(target);
//		}
//	}


//	/// <summary>
//	/// 해제가능한 오브젝트의 해제 대행자.
//	/// </summary>
//	public class Disposer<TDisposableObject> : Disposer where TDisposableObject : DisposableObject
//	{
//		public TDisposableObject Target => base.GetTarget<TDisposableObject>();

//		/// <summary>
//		/// 생성.
//		/// </summary>
//		public Disposer(TDisposableObject target) : base(target)
//		{
//		}

//		/// <summary>
//		/// 생성.
//		/// </summary>
//		public static Disposer<TDisposableObject> Create(TDisposableObject target)
//		{
//			return new Disposer<TDisposableObject>(target);
//		}

//		/// <summary>
//		/// 암시적 변환 : DisposableObject to Disposer.
//		/// </summary>
//		public static implicit operator Disposer<TDisposableObject>(TDisposableObject target)
//		{
//			return Disposer<TDisposableObject>.Create(target);
//		}

//		/// <summary>
//		/// 암시적 변환 : Disposer to DisposableObject.
//		/// </summary>
//		public static implicit operator TDisposableObject(Disposer<TDisposableObject> target)
//		{
//			return target != null ? target.Target : null;
//		}
//	}
//}