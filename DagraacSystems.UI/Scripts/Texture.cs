namespace DagraacSystems
{
	public class Texture : DisposableObject
	{
		public uint Width { private set; get; }
		public uint Height { private set; get; }

		public Texture(uint width, uint height) : base()
		{
			Width = width;
			Height = height;
		}

		protected override void OnDispose(bool explicitedDispose)
		{
			base.OnDispose(explicitedDispose);
		}

		public void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}
	}
}