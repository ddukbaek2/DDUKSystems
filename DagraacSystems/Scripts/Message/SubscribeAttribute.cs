using System;


namespace DagraacSystems
{
	/// <summary>
	/// 구독(전달받을) 내용 속성.
	/// [Listen(Message)] 를 수신자에 작성된 함수에 등록한다.
	/// 해당 함수는 void Function(Message message) 혹은 void Function(void) 형태여야 한다.
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