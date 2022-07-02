namespace DagraacSystems
{
	public struct Dimension<TX>
	{
		public TX X;
	}


	public struct Dimension<TX, TY>
	{
		public TX X;
		public TY Y;
	}


	public struct Dimension<TX, TY, TZ>
	{
		public TX X;
		public TY Y;
		public TZ Z;
	}


	public struct Dimension<TX, TY, TZ, TW>
	{
		public TX X;
		public TY Y;
		public TZ Z;
		public TW W;
	}


	public static class DimensionHelper
	{

	}
}