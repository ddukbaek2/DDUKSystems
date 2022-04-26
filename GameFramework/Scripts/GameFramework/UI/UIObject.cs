using DagraacSystems;


namespace GameFramework.UI
{
	public class UIObject : FrameworkObject
	{
		public IController Controller;

		protected override void OnCreate()
		{
			base.OnCreate();
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}
	}
}