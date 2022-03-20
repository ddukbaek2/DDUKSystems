using DagraacSystems;
using DagraacSystems.Table;
using DagraacSystems.Log;
using System;
using System.IO;
using System.Text.Json;


namespace DagraacSystemsExample
{
	/// <summary>
	/// 테이블 식별자 (직접추가).
	/// </summary>
	public enum eTableID
	{
		Invalid,

		[Value("Tables/ExampleTable.json")]
		ExampleTable,

		[Value("Tables/StringTable.json")]
		StringTable,
	}


	/// <summary>
	/// 테이블 매니저.
	/// </summary>
	public class TableManager : TableManager<TableManager, eTableID>
	{
		/// <summary>
		/// 실제 어플리케이션에서의 파일 로드 구현.
		/// 바이너리로 부를지 텍스트로 부를지는 구현에 따라 다름.
		/// </summary>
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

		/// <summary>
		/// 비동기 로드 구현.
		/// 일단 임시로 함수만 만들어놓고 실제 구현하지는 않음.
		/// </summary>
		protected override void LoadFromFileAsync<TTableData>(string path, Action<TTableData[]> onLoadComplete)
		{
			onLoadComplete?.Invoke(LoadFromFile<TTableData>(path));
		}

		/// <summary>
		/// 전체 로드하라는 함수가 호출되었을 때.
		/// 로드할 테이블들 옵션 맞춰서 직접 추가.
		/// </summary>
		protected override void OnLoadAll()
		{
			Console.WriteLine(Directory.GetCurrentDirectory());

			Load<ExampleTableData>(eTableID.ExampleTable, ValueAttributeHelper.ExtractValueFromEnum(eTableID.ExampleTable), "ID");
			Load<StringTableData>(eTableID.StringTable, ValueAttributeHelper.ExtractValueFromEnum(eTableID.StringTable), "ID");
		}

		/// <summary>
		/// 로드가 성공한 뒤 (싱글톤 접근 및 별도 공간이 필요한 테이블만 직접 추가).
		/// </summary>
		protected override void OnLoaded(eTableID tableID, TableContainer tableContainer)
		{
			switch (tableID)
			{
				case eTableID.ExampleTable:
					ExampleTable.Instance.SetTableContainer(tableContainer);
					break;

				case eTableID.StringTable:
					// used to LocalizationManager.
					return;
			}

			CheckIntegrity(tableID);
		}

		/// <summary>
		/// 정합성 검사 (검사가 필요한 테이블만 직접 추가).
		/// </summary>
		protected override bool OnCheckIntegrity(eTableID tableID, TableContainer tableContainer)
		{
			switch (tableID)
			{
				case eTableID.ExampleTable:
				{
					var exampleTableData = tableContainer.Get<int, ExampleTableData>(1);
					if (exampleTableData.Mob_Speed < 0)
					{
						LogManager.Instance.Print($"[ERR][{tableID}][{exampleTableData.ID}] Mob_Speed: {exampleTableData.Mob_Speed}");
						return false;
					}

					return true;
				}
			}

			return base.OnCheckIntegrity(tableID, tableContainer);
		}
	}
}