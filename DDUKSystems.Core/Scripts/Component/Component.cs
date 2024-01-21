namespace DDUKSystems
{
    /// <summary>
    /// 컴포넌트.
    /// </summary>
    public class Component : ManagedObject
    {
        /// <summary>
        /// 활성화 여부.
        /// </summary>
        private bool m_IsEnable;

        /// <summary>
        /// 활성화 여부.
        /// </summary>
        public bool Enable
        {
            set
            {
                if (m_IsEnable != value)
                {
                    m_IsEnable = value;
                    if (m_IsEnable)
                        OnEnable();
                    else
                        OnDisable();
				}
			}
            get
            {
                return m_IsEnable;
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
		protected virtual void OnUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// 매프레임호출.
        /// </summary>
		public void Update(float deltaTime)
		{
			if (!Enable)
				return;

			OnUpdate(deltaTime);
		}
	}
}