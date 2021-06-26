using System;


namespace DagraacSystems.Notification
{
	public class NotificationAttribute : Attribute
	{
		public NotificationAttribute(int eventID, string receiverID = "")
		{

		}
	}
}