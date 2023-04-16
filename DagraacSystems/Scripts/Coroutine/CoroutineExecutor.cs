using System.Collections;
using System.Collections.Generic;


namespace DagraacSystems
{
	/// <summary>
	/// 코루틴 실행기.
	/// </summary>
	internal class CoroutineExecutor : ManagedObject
	{
		private List<Coroutine> _coroutines;

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void OnCreate(params object[] args)
		{
			base.OnCreate(args);

			_coroutines = new List<Coroutine>();
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			foreach (var coroutine in _coroutines)
				coroutine.Dispose();
			_coroutines.Clear();

			base.OnDispose(explicitedDispose);
		}

		public void Update(float deltaTime)
		{
			foreach (var coroutine in _coroutines)
			{
				if (coroutine == null || !coroutine.IsRunning)
					continue;

				coroutine.Update(deltaTime);
			}

			for (var i = 0; i < _coroutines.Count; ++i)
			{
				var coroutine = _coroutines[i];
				if (coroutine == null || coroutine.IsDisposed)
				{
					_coroutines.RemoveAt(i);
					--i;
				}
			}
		}

		public Coroutine CreateCoroutine()
		{
			var coroutine = ManagedObject.Create<Coroutine>();
			_coroutines.Add(coroutine);
			return coroutine;
		}

		public void DisposeCoroutine(Coroutine coroutine)
 		{
			_coroutines.Remove(coroutine);
			coroutine.Dispose();
		}

		public void Start(IEnumerator _process)
		{
			IEnumerator Process(Coroutine _coroutine)
			{
				while (_process.MoveNext())
					yield return _process;

				_coroutine.Dispose();
			}

			var coroutine = CreateCoroutine();
			coroutine.Start(Process(coroutine));
		}
	}
}