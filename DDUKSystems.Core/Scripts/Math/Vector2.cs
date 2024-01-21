namespace DDUKSystems
{
	/// <summary>
	/// 2차원 벡터.
	/// </summary>
	public struct Vector2
	{
		public static Vector2 Zero { private set; get; } = new Vector2(0f, 0f);
		public static Vector2 One { private set; get; } = new Vector2(1f, 1f);
		public static Vector2 Right { private set; get; } = new Vector2(1f, 0f);
		public static Vector2 Up { private set; get; } = new Vector2(0f, 1f);
		public static Vector2 Left { private set; get; } = new Vector2(-1f, 0f);
		public static Vector2 Down { private set; get; } = new Vector2(0f, -1f);

		public float X { set; get; }
		public float Y { set; get; }

		public Vector2()
		{
			X = 0f;
			Y = 0f;
		}

		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}
	}
}