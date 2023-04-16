using System;


namespace DagraacSystems
{
	/// <summary>
	/// 전달받을 내용 속성.
	/// [Message(Message)] 를 수신자에 작성된 함수에 등록한다.
	/// 해당 함수는 다음 중 하나의 형태여야한다.
	///		- void Function(object sender, IMessage message)
	///		- void Function(IMessage message)
	///		- void Function()
	/// </summary>
	public class MessageAttribute : Attribute
	{
		public Type Type { set; get; } = null;

		public MessageAttribute(Type type)
		{
			Type = type;
		}
	}
}