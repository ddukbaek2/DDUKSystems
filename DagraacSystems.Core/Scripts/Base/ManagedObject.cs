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
        /// 고유식별자 생성기.
        /// </summary>
        private static UniqueIdentifier s_UniqueIdentifier = new UniqueIdentifier();

        /// <summary>
        /// 고유 객체.
        /// </summary>
        //private static Dictionary<ulong, ManagedObject> s_ManagedObjects = new Dictionary<ulong, ManagedObject>();

        /// <summary>
        /// 고유식별자.
        /// </summary>
        public ulong UniqueID { private set; get; } = 0ul;

        /// <summary>
        /// 생성자.
        /// </summary>
        protected ManagedObject() : base()
        {
        }

        /// <summary>
        /// 생성됨.
        /// </summary>
        protected virtual void OnCreate(params object[] args)
        {
            UniqueID = s_UniqueIdentifier.New();
            //s_ManagedObjects.AddEventTarget(UniqueID, this);
		}

        /// <summary>
        /// 해제됨.
        /// </summary>
        protected override void OnDispose(bool explicitedDispose)
        {
            s_UniqueIdentifier.Delete(UniqueID);
            //s_ManagedObjects.Remove(UniqueID);

			base.OnDispose(explicitedDispose);
        }

        /// <summary>
        /// 타입을 기준으로 생성.
        /// </summary>
        public static T Create<T>(params object[] args) where T : ManagedObject, new()
        {
            return Create(typeof(T), args) as T;// new TValue();
        }

		/// <summary>
		/// 타입 인스턴스를 기준으로 생성.
		/// 별도로 타입 인스턴스를 체크 하지 않고 단순 생성 후 형변환하여 반환 하므로 사용상 주의.
		/// </summary>
		public static ManagedObject Create<TParameter>(Type managedObjecType, params TParameter[] args)
		{
			if (managedObjecType == null)
				return null;

			if (managedObjecType.IsAbstract)
				return null;

			if (!managedObjecType.IsSubclassOf(typeof(ManagedObject)))
				return null;

			var managedObject = Activator.CreateInstance(managedObjecType) as ManagedObject;
			if (managedObject == null)
				return null;

			managedObject.OnCreate(args);
			return managedObject;
		}

		/// <summary>
		/// 타입 인스턴스를 기준으로 생성.
		/// 별도로 타입 인스턴스를 체크 하지 않고 단순 생성 후 형변환하여 반환 하므로 사용상 주의.
		/// </summary>
		public static ManagedObject Create(Type managedObjecType, params object[] args)
        {
            if (managedObjecType == null)
                return null;

            if (managedObjecType.IsAbstract)
                return null;

            if (!managedObjecType.IsSubclassOf(typeof(ManagedObject)))
                return null;

			var managedObject = Activator.CreateInstance(managedObjecType) as ManagedObject;
            if (managedObject == null)
                return null;

            managedObject.OnCreate(args);
            return managedObject;
        }

        /// <summary>
        /// 해제.
        /// </summary>
        public virtual void Dispose()
        {
            if (IsDisposed)
                return;

            Dispose(this);
        }
    }
}