using System;
using System.Collections.Generic;


namespace DagraacSystems.Table
{
	/// <summary>
	/// 테이블 컨테이너.
	/// 실제 테이블 데이터가 저장되는 객체이며 내부적으로 string을 key로 사용한다.
	/// </summary>
	public class TableContainer
	{
		/// <summary>
		/// 실제적인 자료구조.
		/// </summary>
		protected SortedDictionary<string, ITableData> _data = new SortedDictionary<string, ITableData>();

		/// <summary>
		/// 키를 생성할 콜백.
		/// </summary>
		protected Func<int, ITableData, string> _makeKeyCallback;

		/// <summary>
		/// 갯수.
		/// </summary>
		public int Count => _data.Count;

		/// <summary>
		/// 초기화.
		/// </summary>
		public void Clear()
		{
			_data.Clear();
			_makeKeyCallback = null;
		}

		/// <summary>
		/// 컨테이너 셋팅.
		/// </summary>
		public void SetTableData(ITableData[] tableDataList, Func<int, ITableData, string> makeKeyCallback)
		{
			Clear();
			_makeKeyCallback = makeKeyCallback;
			AddTableData(tableDataList);
		}

		/// <summary>
		/// 기존 것에 추가.
		/// </summary>
		public void AddTableData(ITableData[] tableDataList)
		{
			if (_makeKeyCallback == null)
				return;

			for (var index = 0; index < tableDataList.Length; ++index)
			{
				ITableData tableData = tableDataList[index];
				_data.Add(_makeKeyCallback(index, tableData), tableData);
			}
		}

		public TTableData Get<TTableData>(string key) where TTableData : ITableData
		{
			return (TTableData)_data[key];
		}

		public TTableData Get<TKey, TTableData>(TKey key) where TTableData : ITableData
		{
			return Get<TTableData>(key.ToString());
		}

		/// <summary>
		/// 반복문 처리.
		/// 조건에 일치되는 대상을 반환.
		/// </summary>
		public TTableData ForEach<TTableData>(Predicate<TTableData> predicate) where TTableData : ITableData
		{
			if (predicate == null)
				return default(TTableData);
			foreach (KeyValuePair<string, ITableData> keyValuePair in _data)
			{
				if (predicate((TTableData)keyValuePair.Value))
					return (TTableData)keyValuePair.Value;
			}
			return default(TTableData);
		}

		/// <summary>
		/// 검색하여 요소 1건 반환.
		/// </summary>
		public TTableData Find<TTableData>(Predicate<TTableData> predicate) where TTableData : ITableData
		{
			return ForEach<TTableData>(predicate);
		}

		/// <summary>
		/// 검색된 모든 요소 반환.
		/// </summary>
		public List<TTableData> FindAll<TTableData>(Predicate<TTableData> predicate) where TTableData : ITableData
		{
			var list = new List<TTableData>();
			if (predicate == null)
				return list;

			ForEach<TTableData>(tableData =>
			{
				if (predicate(tableData))
					list.Add(tableData);
				return false;
			});

			return list;
		}

		/// <summary>
		/// 전체 요소 반환.
		/// </summary>
		public List<TTableData> All<TTableData>() where TTableData : ITableData
		{
			var list = new List<TTableData>();
			ForEach<TTableData>(tableData =>
			{
				list.Add(tableData);
				return false;
			});
			return list;
		}
	}
}