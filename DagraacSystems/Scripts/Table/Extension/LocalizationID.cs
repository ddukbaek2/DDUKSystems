namespace DagraacSystems.Table.Extension
{
	public class LocalizationID
	{
		public int ID;

		public LocalizationID(int id)
		{
			ID = id;
		}

		public LocalizationID(string id)
		{
			ID = int.Parse(id);
		}

		public static implicit operator LocalizationID(int id)
		{
			return new LocalizationID(id);
		}

		public static implicit operator LocalizationID(string id)
		{
			return new LocalizationID(id);
		}

		public static implicit operator string(LocalizationID localizationID)
		{
			return localizationID.ToString();
		}

		public override string ToString()
		{
			return LocalizationManager.Instance.Get(ID);
		}
	}
}