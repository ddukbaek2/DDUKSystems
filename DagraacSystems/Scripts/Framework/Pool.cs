using System.Collections.Generic;


namespace DagraacSystems.Framework
{
	/// <summary>
	/// 모델들을 관리하는 객체.
	/// </summary>
	public class Pool : Object
	{
		protected Module _module;
		protected List<Model> _models;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Pool() : base()
		{
			_module = null;
			_models = new List<Model>();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			_module = null;

			_models.Clear();
			_models = null;

			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 추가.
		/// </summary>
		public void AddModel(Model model)
		{
		}

		/// <summary>
		/// 제거.
		/// </summary>
		public void RemoveModel(Model model)
		{
		}

		public void FrameMove(float deltaTime)
		{
			//foreach (var model in _models)
			//model.
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public static TPool CreatePool<TPool>(Module module) where TPool : Pool, new()
		{
			var pool = Object.Create<TPool>(module.Framework);
			pool._module = module;
			return pool;
		}
	}
}