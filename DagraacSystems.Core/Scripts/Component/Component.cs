namespace DagraacSystems
{
    /// <summary>
    /// 컴포넌트.
    /// </summary>
    public class Component : ManagedObject, IComponent
    {
        /// <summary>
        /// 활성화 여부.
        /// </summary>
        private bool isEnable;

        /// <summary>
        /// 활성화 여부.
        /// </summary>
        public bool Enable
        {
            set
            {
                if (isEnable != value)
                {
                    isEnable = value;
                    if (isEnable)
                        OnEnable();
                    else
                        OnDisable();
				}
			}
            get
            {
                return isEnable;
			}
        }

        /// <summary>
        /// 생성됨.
        /// </summary>
        protected override void OnCreate(params object[] args)
        {
            base.OnCreate(args);

			Enable = true;
		}

        /// <summary>
        /// 해제됨.
        /// </summary>
        protected override void OnDispose(bool explicitedDispose)
        {
            Enable = false;

			base.OnDispose(explicitedDispose);
        }

        /// <summary>
        /// 활성화됨.
        /// </summary>
        protected virtual void OnEnable()
        {
        }

        /// <summary>
        /// 비활성화됨.
        /// </summary>
        protected virtual void OnDisable()
        {
        }

        /// <summary>
        /// 매프레임호출됨.
        /// </summary>
		protected virtual void OnFrameMove(float deltaTime)
        {
        }

        /// <summary>
        /// 매프레임호출.
        /// </summary>
		void IComponent.FrameMove(float deltaTime)
		{
			if (!Enable)
				return;

			OnFrameMove(deltaTime);
		}
	}
}