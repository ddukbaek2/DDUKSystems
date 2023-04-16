using System;


namespace DagraacSystems
{
	/// <summary>
	/// 구독(전달받을) 내용 속성.
	/// [Subscribe(Message)] 를 수신자에 작성된 함수에 등록한다.
	/// 해당 함수는 다음 중 하나의 형태여야한다.
	///		- void Function(object sender, IMessage message)
	///		- void Function(IMessage message)
	///		- void Function()
	/// </summary>
	public class SubscribeAttribute : Attribute
	{
		public Type Type { set; get; } = null;

		public SubscribeAttribute(Type type)
		{
			Type = type;
		}
	}
}