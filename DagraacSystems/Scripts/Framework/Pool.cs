using System.Collections.Generic;


namespace DagraacSystems
{
	/// <summary>
	/// 프레임워크 오브젝트들을 관리하는 객체.
	/// </summary>
	public class Pool : FrameworkObject
	{
		protected Queue<FrameworkObject> _objects;

		public int Count => _objects.Count;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Pool() : base()
		{
			_objects = new Queue<FrameworkObject>();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			_objects.Clear();
			_objects = null;

			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 집어넣음.
		/// </summary>
		public void Push(FrameworkObject obj)
		{
			_objects.Enqueue(obj);
		}

		/// <summary>
		/// 꺼냄.
		/// </summary>
		public T Pop<T>() where T : FrameworkObject
		{
			if (_objects.Count > 0)
			{
				return _objects.Dequeue() as T;
			}

			return null;
		}
	}
}