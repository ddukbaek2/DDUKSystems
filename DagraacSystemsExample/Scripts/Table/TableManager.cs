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

		[FilePath("Tables/ExampleTable.json")]
		ExampleTable,

		[FilePath("Tables/StringTable.json")]
		StringTable,
	}


	/// <summary>
	/// 테이블 매니저.
	/// </summary>
	public class TableManager : TableManagerTemplete<TableManager, eTableID>
	{
		protected override TTableData[] LoadFromFile<TTableData>(string path)
		{
			var json = File.ReadAllText(path);
			var options = new JsonSerializerOptions { IncludeFields = true };

			try
			{
				return JsonSerializer.Deserialize<TTableData[]>(json, options);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return null;
			}
		}

		protected override bool OnCheckIntegrity(eTableID tableID, TableContainer tableContainer)
		{
			return base.OnCheckIntegrity(tableID, tableContainer);
		}

		protected override void OnLoadAll()
		{
			Console.WriteLine(Directory.GetCurrentDirectory());

			Load<ExampleTableData>(eTableID.ExampleTable, FilePathAttributeHelper.GetFilePath(eTableID.ExampleTable), "ID");
			Load<StringTableData>(eTableID.StringTable, FilePathAttributeHelper.GetFilePath(eTableID.StringTable), "ID");
		}

		protected override void OnLoaded(eTableID tableID, TableContainer tableContainer)
		{
			switch (tableID)
			{
				case eTableID.ExampleTable:
					ExampleTable.Instance.SetContainer(tableContainer);
					break;

				case eTableID.StringTable:
					// used to LocalizationManager.
					break;
			}
		}
	}
}