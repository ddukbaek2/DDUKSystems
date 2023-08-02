namespace DagraacSystems.UI
{
	/// <summary>
	/// 사각 영역.
	/// </summary>
	public class Rectangle : Node
	{
		public Vector2 Anchor { set; get; }
		public Vector2 Pivot { set; get; }
		public Vector2 Position { set; get; }
		public Vector2 Size { set; get; }
		public float Angle { set; get; }

		public Rectangle() : base()
		{
		}
	}
}