using System.Collections;
using System.Collections.Generic;


namespace DagraacSystems
{
	/// <summary>
	/// 코루틴 처리기.
	/// </summary>
	internal class CoroutineManager : SharedClass<CoroutineManager>
	{
		private List<Coroutine> _coroutines;

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
				if (coroutine.IsRunning)
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
			var coroutine = DisposableObject.Create<Coroutine>();
			_coroutines.Add(coroutine);
			return coroutine;
		}

		public void DisposeCoroutine(Coroutine coroutine)
 		{
			_coroutines.Remove(coroutine);
			coroutine.Dispose();
		}

		public void Start(IEnumerator process)
		{
			IEnumerator Process(Coroutine self)
			{
				while (process.MoveNext())
					yield return process;

				self.Dispose();
			}

			var coroutine = CreateCoroutine();
			coroutine.Start(Process(coroutine));
		}
	}
}