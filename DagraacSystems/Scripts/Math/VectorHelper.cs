using System; // Math


namespace DagraacSystems
{
	public static class VectorHelper
	{
		public static double DirectionVector2ToDegreeAngle(this Vector2 value)
		{
			var radian = Math.Atan2(value.Y, value.X);
			var degree = radian * MathHelper.RadianToDegree;

			return degree;
		}

		public static Vector2 DegreeAngleToDirectionVector2(this double degree)
		{
			return new Vector2 { X = Math.Sin(degree), Y = -Math.Cos(degree) };
		}

		public static double Length(this Vector2 value)
		{
			return Math.Sqrt((value.X * value.X) + (value.Y * value.Y));
		}

		public static double Length(this Vector3 value)
		{
			return Math.Sqrt((value.X * value.X) + (value.Y * value.Y) + (value.Z * value.Z));
		}

		public static double Length(this Vector4 value)
		{
			return Math.Sqrt((value.X * value.X) + (value.Y * value.Y) + (value.Z * value.Z) + (value.W * value.W));
		}

		public static Vector2 Normalize(this Vector2 value)
		{
			var length = Length(value);
			return new Vector2 { X = value.X / length, Y = value.Y / length };
		}

		public static Vector3 Normalize(this Vector3 value)
		{
			var length = Length(value);
			return new Vector3 { X = value.X / length, Y = value.Y / length, Z = value.Z / length };
		}

		public static Vector4 Normalize(this Vector4 value)
		{
			var length = Length(value);
			return new Vector4 { X = value.X / length, Y = value.Y / length, Z = value.Z / length, W = value.W / length };
		}

		public static Vector2 Add(this Vector2 left, double right)
		{
			return new Vector2 { X = left.X + right, Y = left.Y + right };
		}

		public static Vector2 Subtract(this Vector2 left, double right)
		{
			return new Vector2 { X = left.X - right, Y = left.Y - right };
		}

		public static Vector2 Multiply(this Vector2 left, double right)
		{
			return new Vector2 { X = left.X * right, Y = left.Y * right };
		}

		public static Vector2 Divide(this Vector2 left, double right)
		{
			return new Vector2 { X = left.X / right, Y = left.Y / right };
		}

		public static Vector2 Add(this Vector2 left, Vector2 right)
		{
			return new Vector2 { X = left.X + right.X, Y = left.Y + right.Y };
		}

		public static Vector2 Subtract(this Vector2 left, Vector2 right)
		{
			return new Vector2 { X = left.X - right.X, Y = left.Y - right.Y };
		}

		public static Vector2 Multiply(this Vector2 left, Vector2 right)
		{
			return new Vector2 { X = left.X * right.X, Y = left.Y * right.Y };
		}

		public static Vector2 Divide(this Vector2 left, Vector2 right)
		{
			return new Vector2 { X = left.X / right.X, Y = left.Y / right.Y };
		}
	}
}