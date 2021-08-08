namespace DagraacSystems
{
	public class Sequence<TTarget, TAction> : Node.SiblingNode<TTarget>
	{
		public TTarget Target;
		public TAction Action;
	}
}