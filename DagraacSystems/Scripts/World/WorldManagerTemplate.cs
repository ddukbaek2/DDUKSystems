using System;
using System.Linq;
using System.Collections.Generic;


namespace DagraacSystems.World
{
	public class WorldManagerTemplate<TWorldManager> : Singleton<TWorldManager>
		where TWorldManager : WorldManagerTemplate<TWorldManager>, new()
	{
		//public Dictionary<Guid, Object> Objects { private set; get; } = new Dictionary<Guid, Object>();
		private long prevTick = 0;

		public float DeltaTime = 0f;

		protected override void OnCreate()
		{
			base.OnCreate();
		}

		protected override void OnDispose(bool disposing)
		{
		}

		public void FrameMove()
		{
			//TimeSpan.FromTicks();
			var currentTick = System.DateTime.Now.Ticks;
			if (prevTick > 0)
			{
				var time = TimeSpan.FromTicks(currentTick - prevTick);
				DeltaTime = (float)time.TotalMilliseconds * 0.001f;
				OnUpdate(DeltaTime);
			}
			prevTick = currentTick;
		}

		private void OnUpdate(float deltaTime)
		{
			//foreach (var targetObject in Object.Objects.Values)
			//	targetObject.OnUpdate(deltaTime);
		}
	}
}