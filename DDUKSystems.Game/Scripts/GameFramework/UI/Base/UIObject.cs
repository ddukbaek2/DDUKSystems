using DDUKSystems;


namespace DDUKSystems.Game.UI
{
	public class UIObject : FObject
	{
		public IController Controller;

		protected override void OnCreate(params object[] _args)
		{
			base.OnCreate(_args);
		}

		protected override void OnDispose(bool _explicitedDispose)
		{
			base.OnDispose(_explicitedDispose);
		}
	}
}