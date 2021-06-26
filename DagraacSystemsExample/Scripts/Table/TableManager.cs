using DagraacSystems;
using DagraacSystems.Table;
using System;
using System.IO;
using System.Text.Json;


namespace DagraacSystemsExample
{
	/// <summary>
	/// 테이블 식별자.
	/// </summary>
	public enum eTableID
	{
		Invalid,

		[FilePath("CommonTable.json")]
		CommonTable,
	}


	/// <summary>
	/// 테이블 매니저.
	/// </summary>
	public class TableManager : TableManagerTemplete<TableManager, eTableID>
	{
		public const string PrefixJsonPath = "Data/Tables/";

		protected override TTableData[] LoadFromFile<TTableData>(string path)
		{
			var json = File.ReadAllText(path);
			return JsonSerializer.Deserialize<TTableData[]>(json);
		}

		protected override bool OnCheckIntegrity(eTableID tableID, TableContainer tableContainer)
		{
			return base.OnCheckIntegrity(tableID, tableContainer);
		}

		protected override void OnLoadAll()
		{
			Load<CommonTableData>(eTableID.CommonTable, PrefixJsonPath + FilePathAttributeHelper.GetFilePath(eTableID.CommonTable), "ID");
		}

		protected override void OnLoaded(eTableID tableID, TableContainer tableContainer)
		{
			switch (tableID)
			{
				case eTableID.CommonTable:
					CommonTable.Instance.SetContainer(tableContainer);
					break;
			}
		}
	}
}