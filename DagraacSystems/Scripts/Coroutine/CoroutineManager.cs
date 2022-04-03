using System.Collections;


namespace DagraacSystems
{
	/// <summary>
	/// 코루틴.
	/// </summary>
	public class CoroutineManager : Singleton<CoroutineManager>
	{
		protected override void OnCreate()
		{
			base.OnCreate();
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}

		public void FrameMove(float deltaTime)
		{
		}

		public Coroutine CreateCoroutine()
		{
			var coroutine = new Coroutine();
			return coroutine;
		}

		public void DisposeCoroutine()
 		{
		}
	}
}