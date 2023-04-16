using System.Collections;
using System.Collections.Generic; // List


namespace DagraacSystems
{
	/// <summary>
	/// 코루틴 실행기.
	/// </summary>
	internal class CoroutineSystem : ManagedObject
	{
		private List<Coroutine> m_Coroutines;

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void OnCreate(params object[] args)
		{
			base.OnCreate(args);

			m_Coroutines = new List<Coroutine>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			foreach (var coroutine in m_Coroutines)
				coroutine.Dispose();
			m_Coroutines.Clear();

			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 갱신됨.
		/// </summary>
		public void Update(float deltaTime)
		{
			foreach (var coroutine in m_Coroutines)
			{
				if (coroutine == null || !coroutine.IsRunning)
					continue;

				coroutine.Update(deltaTime);
			}

			for (var i = 0; i < m_Coroutines.Count; ++i)
			{
				var coroutine = m_Coroutines[i];
				if (coroutine == null || coroutine.IsDisposed)
				{
					m_Coroutines.RemoveAt(i);
					--i;
				}
			}
		}

		public Coroutine CreateCoroutine()
		{
			var coroutine = ManagedObject.Create<Coroutine>();
			m_Coroutines.Add(coroutine);
			return coroutine;
		}

		public void DisposeCoroutine(Coroutine coroutine)
 		{
			m_Coroutines.Remove(coroutine);
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