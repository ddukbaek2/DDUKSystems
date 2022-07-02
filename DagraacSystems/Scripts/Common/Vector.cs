using System; // Math


namespace DagraacSystems
{
	public struct Vector2
	{
		public double X;
		public double Y;

		public static implicit operator Vector3(Vector2 value)
		{
			return new Vector3 { X = value.X, Y = value.Y, Z = default };
		}

		public static implicit operator Vector4(Vector2 value)
		{
			return new Vector4 { X = value.X, Y = value.Y, Z = default, W = default };
		}
	}


	public struct Vector3
	{
		public double X;
		public double Y;
		public double Z;

		public static implicit operator Vector2(Vector3 value)
		{
			return new Vector2 { X = value.X, Y = value.Y };
		}

		public static implicit operator Vector4(Vector3 value)
		{
			return new Vector4 { X = value.X, Y = value.Y, Z = value.Z, W = default };
		}
	}


	public struct Vector4
	{
		public double X;
		public double Y;
		public double Z;
		public double W;

		public static implicit operator Vector2(Vector4 value)
		{
			return new Vector2 { X = value.X, Y = value.Y };
		}

		public static implicit operator Vector3(Vector4 value)
		{
			return new Vector4 { X = value.X, Y = value.Y, Z = value.Z };
		}
	}


	public static class VectorHelper
	{
		public static double Length(Vector2 value)
		{
			return Math.Sqrt((value.X * value.X) + (value.Y * value.Y));
		}

		public static double Length(Vector3 value)
		{
			return Math.Sqrt((value.X * value.X) + (value.Y * value.Y) + (value.Z * value.Z));
		}

		public static double Length(Vector4 value)
		{
			return Math.Sqrt((value.X * value.X) + (value.Y * value.Y) + (value.Z * value.Z) + (value.W * value.W));
		}

		public static Vector2 Normalize(Vector2 value)
		{
			var length = Length(value);
			return new Vector2 { X = value.X / length, Y = value.Y / length };
		}

		public static Vector3 Normalize(Vector3 value)
		{
			var length = Length(value);
			return new Vector3 { X = value.X / length, Y = value.Y / length, Z = value.Z / length };
		}

		public static Vector4 Normalize(Vector4 value)
		{
			var length = Length(value);
			return new Vector4 { X = value.X / length, Y = value.Y / length, Z = value.Z / length, W = value.W / length };
		}

		public static Vector2 Add(Vector2 left, float right)
		{
			return new Vector2 { X = left.X + right, Y = left.Y + right };
		}

		public static Vector2 Subtract(Vector2 left, float right)
		{
			return new Vector2 { X = left.X - right, Y = left.Y - right };
		}

		public static Vector2 Multiply(Vector2 left, float right)
		{
			return new Vector2 { X = left.X * right, Y = left.Y * right };
		}

		public static Vector2 Divide(Vector2 left, float right)
		{
			return new Vector2 { X = left.X / right, Y = left.Y / right };
		}

		public static Vector2 Add(Vector2 left, Vector2 right)
		{
			return new Vector2 { X = left.X + right.X, Y = left.Y + right.Y };
		}

		public static Vector2 Subtract(Vector2 left, Vector2 right)
		{
			return new Vector2 { X = left.X - right.X, Y = left.Y - right.Y };
		}

		public static Vector2 Multiply(Vector2 left, Vector2 right)
		{
			return new Vector2 { X = left.X * right.X, Y = left.Y * right.Y };
		}

		public static Vector2 Divide(Vector2 left, Vector2 right)
		{
			return new Vector2 { X = left.X / right.X, Y = left.Y / right.Y };
		}
	}
}