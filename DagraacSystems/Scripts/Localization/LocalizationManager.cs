﻿using DagraacSystems.Table;


namespace DagraacSystems.Localization
{
	public class LocalizationManager : Manager<LocalizationManager>
	{
		public const string ErrorFormat = "TEXTERR({0})";

		private string m_Language;
		private TableContainer m_StringTable;

		public LocalizationManager() : base()
		{
			m_Language = string.Empty;
			m_StringTable = null;
		}

		protected override void OnDispose(bool disposing)
		{
			if (disposing)
			{
				m_Language = string.Empty;
				m_StringTable = null;
			}
		}

		public void SetCurrentLanguage(string language)
		{
			m_Language = language;
		}

		public void SetStringTable(TableContainer stringTable)
		{
			m_StringTable = stringTable;
		}

		private static string GetValue(ITableData tableData, string language)
		{
			if (tableData == null)
				return string.Empty;

			var fieldIndex = tableData.GetFieldIndex(language);
			if (fieldIndex == -1)
				return string.Empty;

			var fieldValue = tableData.GetFieldValue(fieldIndex);
			if (fieldValue == null)
				return string.Empty;

			return fieldValue.ToString();
		}

		public string Get(int id)
		{
			var stringTableData = m_StringTable.Get<ITableData>(id.ToString());
			if (stringTableData == null)
				return string.Format(ErrorFormat, id);
			return GetValue(stringTableData, m_Language);
		}

		public string Get(string key)
		{
			var stringTableData = m_StringTable.Find<ITableData>(it =>
			{
				var fieldIndex = it.GetFieldIndex("Key");
				if (fieldIndex == -1)
					return false;
				var fieldValue = it.GetFieldValue(fieldIndex);
				if (fieldValue == null)
					return false;

				return fieldValue.ToString() == key;
			});

			if (stringTableData == null)
				return string.Format(ErrorFormat, key);
			return GetValue(stringTableData, m_Language);
		}

		/// <summary>
		/// 찾을 데이터가 포맷 형태의 텍스트일 경우, 추가로 넣는 인자를 출력.
		/// </summary>
		public string Get(int id, params object[] args)
		{
			var format = Get(id);
			return string.Format(format, args);
		}

		/// <summary>
		/// 찾을 데이터가 포맷 형태의 텍스트일 경우, 추가로 넣는 인자를 출력.
		/// </summary>
		public string Get(string key, params object[] args)
		{
			var format = Get(key);
			return string.Format(format, args);
		}

		/// <summary>
		/// 해당하는 여러건의 데이터를 찾아서 하나의 포맷 문자열로 변환하여 반환.
		/// </summary>
		public string Format(string format, int[] keys)
		{
			var values = new string[keys.Length];
			for (var i = 0; i < values.Length; ++i)
				values[i] = Get(keys[i]);

			return string.Format(format, values);
		}

		/// <summary>
		/// 해당하는 여러건의 데이터를 찾아서 하나의 포맷 문자열로 변환하여 반환.
		/// </summary>
		public string Format(string format, string[] keys)
		{
			var values = new string[keys.Length];
			for (var i = 0; i < values.Length; ++i)
				values[i] = Get(keys[i]);

			return string.Format(format, values);
		}

		/// <summary>
		/// 현재 선택 언어를 반환.
		/// </summary>
		public string GetCurrentLanguage()
		{
			return m_Language;
		}

		/// <summary>
		/// 현재 스트링 테이블을 반환.
		/// </summary>
		public TableContainer GetStringTable()
		{
			return m_StringTable;
		}
	}
}