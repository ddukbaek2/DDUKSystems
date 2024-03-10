using DDUKSystems;


namespace uScript
{
    /// <summary>
    /// 기본 오브젝트.
    /// </summary>
    public class UObject : DisposableObject
    {
        /// <summary>
        /// 생성됨.
        /// </summary>
        public UObject() : base()
        {
        }

        /// <summary>
        /// 해제됨.
        /// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}

        /// <summary>
        /// 해제.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            DisposableObject.Dispose(this);
        }
	}
}