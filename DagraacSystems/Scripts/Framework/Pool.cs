using System.Collections.Generic;


namespace DagraacSystems
{
	/// <summary>
	/// 모델들을 관리하는 객체.
	/// </summary>
	public class Pool : FrameworkObject
	{
		protected Module _module;
		protected Queue<Model> _models;

		public int Count => _models.Count;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Pool() : base()
		{
			_module = null;
			_models = new Queue<Model>();
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

		public void PushModel(Model model)
		{
			_models.Enqueue(model);
		}

		public Model PopModel()
		{
			if (_models.Count > 0)
			{
				return _models.Dequeue();
			}

			return null;
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public static TPool CreatePool<TPool>(Module module) where TPool : Pool, new()
		{
			var pool = FrameworkObject.Create<TPool>(module.Framework);
			pool._module = module;
			return pool;
		}
	}
}