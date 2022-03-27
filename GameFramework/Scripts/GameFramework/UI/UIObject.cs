using DagraacSystems;


namespace GameFramework.UI
{
	public interface IUITarget
	{
	}

	public class UIObject : DisposableObject
	{
		public IUITarget Target;

		public UIObject()
		{
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}
	}
}