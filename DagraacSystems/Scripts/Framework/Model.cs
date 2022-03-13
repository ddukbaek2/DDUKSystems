namespace DagraacSystems.Framework
{
	/// <summary>
	/// 다수의 데이터로 존재할 수 있는 특정 개체의 기본 틀.
	/// </summary>
	public class Model : FrameworkObject
	{
		protected Pool _pool;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Model() : base()
		{
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}

		protected virtual void OnUpdate(float deltaTime)
		{
		}


		/// <summary>
		/// 생성.
		/// </summary>
		public static TModel CreateModel<TModel>(Pool pool) where TModel : Model, new()
		{
			var model = FrameworkObject.Create<TModel>(pool.Framework);
			pool.AddModel(model);
			return model;
		}
	}
}