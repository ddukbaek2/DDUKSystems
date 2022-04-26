using DagraacSystems;


public class ExampleTableData : DagraacSystems.ITableData
{
	/// <summary>고유식별자</summary>
	public int ID;

	/// <summary>정수</summary>
	public int Num;

	/// <summary>실수</summary>
	public float Mob_Speed;

	/// <summary>논리</summary>
	public bool IsVisible;

	/// <summary>문자열</summary>
	public string Desc;

	public System.Collections.Generic.List<System.Tuple<string, System.Type, object>> ToFields()
	{
		return new System.Collections.Generic.List<System.Tuple<string, System.Type, object>>()
		{
			new System.Tuple<string, System.Type, object>("ID", ID.GetType(), ID),
			new System.Tuple<string, System.Type, object>("Num", Num.GetType(), Num),
			new System.Tuple<string, System.Type, object>("Mob_Speed", Mob_Speed.GetType(), Mob_Speed),
			new System.Tuple<string, System.Type, object>("IsVisible", IsVisible.GetType(), IsVisible),
			new System.Tuple<string, System.Type, object>("Desc", Desc.GetType(), Desc),
		};
	}

	public int GetFieldIndex(string name)
	{
		switch (name)
		{
			case "ID": return 0;
			case "Num": return 1;
			case "Mob_Speed": return 2;
			case "IsVisible": return 3;
			case "Desc": return 4;
		}
		return -1;
	}

	public string GetFieldName(int index)
	{
		switch (index)
		{
			case 0: return "ID";
			case 1: return "Num";
			case 2: return "Mob_Speed";
			case 3: return "IsVisible";
			case 4: return "Desc";
		}
		return string.Empty;
	}

	public System.Type GetFieldType(int index)
	{
		switch (index)
		{
			case 0: return ID.GetType();
			case 1: return Num.GetType();
			case 2: return Mob_Speed.GetType();
			case 3: return IsVisible.GetType();
			case 4: return Desc.GetType();
		}
		return null;
	}

	public object GetFieldValue(int index)
	{
		switch (index)
		{
			case 0: return (object)ID;
			case 1: return (object)Num;
			case 2: return (object)Mob_Speed;
			case 3: return (object)IsVisible;
			case 4: return (object)Desc;
		}
		return null;
	}

	public int GetFieldCount()
	{
		return 5;
	}
}