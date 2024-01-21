using System;


namespace DDUKSystems
{
	/// <summary>
	/// 전달받을 내용 속성.
	/// [EventSystem(Message)] 를 수신자에 작성된 함수에 등록한다.
	/// 해당 함수는 다음 중 하나의 형태여야한다.
	///		- void Function(object sender, IEventParameter eventParameter)
	///		- void Function(IEventParameter eventParameter)
	///		- void Function()
	/// </summary>
	public class EventAttribute : Attribute
	{
		public Type Type { set; get; } = null;

		public EventAttribute(Type type)
		{
			Type = type;
		}
	}
}