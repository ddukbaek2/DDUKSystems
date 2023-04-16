namespace DagraacSystems
{
	/// <summary>
	/// 다수의 데이터로 존재할 수 있는 특정 개체의 기본 틀.
	/// </summary>
	public class Model : FObject, IPooledObject
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public Model() : base()
		{
		}

		void IPooledObject.OnPush(Pool pool)
		{
			OnPush();
		}

		void IPooledObject.OnPop(Pool pool)
		{
			OnPop();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 업데이트 됨.
		/// </summary>
		protected virtual void OnUpdate(float deltaTime)
		{
		}

		/// <summary>
		/// 풀에 집어넣음.
		/// </summary>
		protected virtual void OnPush()
		{
		}

		/// <summary>
		/// 풀에서 빠져나옴.
		/// </summary>
		protected virtual void OnPop()
		{
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public static TModel CreateModel<TModel>(Pool pool = null) where TModel : Model, new()
		{
			var model = FObject.Create<TModel>(pool.FrameworkSystem);
			if (pool != null)
				pool.Push(model);
			return model;
		}
	}
}