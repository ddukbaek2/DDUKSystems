namespace DDUKSystems.UI
{
	/// <summary>
	/// 색상.
	/// </summary>
	public struct Color
	{
		public static readonly Color White = new Color(1f, 1f, 1f, 1f);
		public static readonly Color Black = new Color(0f, 0f, 0f, 1f);
		public static readonly Color Red = new Color(1f, 0f, 0f, 1f);
		public static readonly Color Green = new Color(0f, 1f, 0f, 1f);
		public static readonly Color Blue = new Color(0f, 0f, 1f, 1f);
		public static readonly Color Clear = new Color(0f, 0f, 0f, 0f);

		public float R { set; get; }
		public float G { set; get; }
		public float B { set; get; }
		public float A { set; get; }

		public Color(float r, float g, float b, float a = 1f)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}

		public Color(byte r, byte g, byte b, byte a = byte.MaxValue)
		{
			R = (float)r / 255f;
			G = (float)g / 255f;
			B = (float)b / 255f;
			A = (float)a / 255f;
		}
	}
}