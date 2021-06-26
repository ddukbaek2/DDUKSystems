using DagraacSystems.Table.Extension;


public class StringTableData : DagraacSystems.Table.ITableData
{
	/// <summary>고유식별자</summary>
	public int ID;

	/// <summary>한국어</summary>
	public string Korean;

	/// <summary>영어</summary>
	public string English;

	public System.Collections.Generic.List<System.Tuple<string, System.Type, object>> ToFields()
	{
		return new System.Collections.Generic.List<System.Tuple<string, System.Type, object>>()
		{
			new System.Tuple<string, System.Type, object>("ID", ID.GetType(), ID),
			new System.Tuple<string, System.Type, object>("Korean", Korean.GetType(), Korean),
			new System.Tuple<string, System.Type, object>("English", English.GetType(), English),
		};
	}

	public int GetFieldIndex(string name)
	{
		switch (name)
		{
			case "ID": return 0;
			case "Korean": return 1;
			case "English": return 2;
		}
		return -1;
	}

	public string GetFieldName(int index)
	{
		switch (index)
		{
			case 0: return "ID";
			case 1: return "Korean";
			case 2: return "English";
		}
		return string.Empty;
	}

	public System.Type GetFieldType(int index)
	{
		switch (index)
		{
			case 0: return ID.GetType();
			case 1: return Korean.GetType();
			case 2: return English.GetType();
		}
		return null;
	}

	public object GetFieldValue(int index)
	{
		switch (index)
		{
			case 0: return (object)ID;
			case 1: return (object)Korean;
			case 2: return (object)English;
		}
		return null;
	}

	public int GetFieldCount()
	{
		return 3;
	}
}