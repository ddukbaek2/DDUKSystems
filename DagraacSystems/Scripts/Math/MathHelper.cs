namespace DagraacSystems
{
	public static class MathHelper
	{
		public const double PI = 3.1415926535897931d;

		public static double RadianToDegree(double radian)
		{
			return radian * (MathHelper.PI / 180d);
		}
	}
}