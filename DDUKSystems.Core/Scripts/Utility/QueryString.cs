using System.Collections.Generic;
using System.Text;


namespace DDUKSystems
{
	/// <summary>
	/// 쿼리스트링 처리기.
	/// </summary>
	public class QueryString : DisposableObject
	{
		/// <summary>
		/// 데이터.
		/// </summary>
		private Dictionary<string, string> m_Query;

		/// <summary>
		/// 데이터.
		/// </summary>
		public Dictionary<string, string> Query => m_Query;

		/// <summary>
		/// 인덱서.
		/// </summary>
		public string this[string key]
		{
			set => AddQuery(key, value);
			get => GetQuery(key);
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public QueryString() : base()
		{
			m_Query = new Dictionary<string, string>();
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public QueryString(string queryString) : base()
		{
			TryParse(queryString, out m_Query);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitedDispose)
		{
			Clear();

			base.OnDispose(explicitedDispose);
		}

		/// <summary>
		/// 초기화.
		/// </summary>
		public void Clear()
		{
			m_Query.Clear();
		}

		/// <summary>
		/// 해제.
		/// </summary>
		public void Dispose()
		{
			if (IsDisposed)
				return;

			DisposableObject.Dispose(this);
		}

		/// <summary>
		/// 쿼리 추가 (중복되면 그냥 값을 덮어씀).
		/// </summary>
		public void AddQuery(string key, string value)
		{
			if (m_Query.ContainsKey(key))
			{
				m_Query[key] = value;
			}
			else
			{
				m_Query.Add(key, value);
			}
		}

		/// <summary>
		/// 쿼리 반환.
		/// </summary>
		public string GetQuery(string key)
		{
			if (!m_Query.TryGetValue(key, out string value))
				return string.Empty;

			return value;
		}

		/// <summary>
		/// 쿼리스트링 초기화 및 추가.
		/// </summary>
		public void SetQueryString(string queryString)
		{
			Clear();
			AddQueryString(queryString);
		}

		/// <summary>
		/// 쿼리스트링 추가.
		/// </summary>
		public void AddQueryString(string queryString)
		{
			if (!TryParse(queryString, out var query))
				return;

			foreach (var q in query)
			{
				AddQuery(q.Key, q.Value);
			}
		}

		/// <summary>
		/// 해당 키가 존재하는지 유무.
		/// </summary>
		public bool TryGetValue(string key, out string value)
		{
			return m_Query.TryGetValue(key, out value);
		}

		/// <summary>
		/// 해당 키가 존재하는지 유무.
		/// </summary>
		public bool ContainsKey(string value)
		{
			return m_Query.ContainsKey(value);
		}

		/// <summary>
		/// 해당 값이 존재하는지 유무.
		/// </summary>
		public bool ContainsValue(string value)
		{
			return m_Query.ContainsValue(value);
		}

		/// <summary>
		/// 쿼리스트링 반환.
		/// </summary>
		public override string ToString()
		{
			return ToString(m_Query);
		}

		/// <summary>
		/// 쿼리스트링 파싱.
		/// </summary>
		public static bool TryParse(string queryString, out Dictionary<string, string> query)
		{
			query = new Dictionary<string, string>();

			if (string.IsNullOrEmpty(queryString))
				return false;

			var index = 0;
			foreach (var queryParam in queryString.Split('&'))
			{
				var values = queryParam.Split('=');
				
				if (values.Length == 2)
				{
					query.Add(values[0], values[1]);
				}
				else
				{
					query.Add($"noname_{index}", queryParam);
				}

				++index;
			}

			return query.Count > 0;
		}

		/// <summary>
		/// 쿼리스트링 반환.
		/// </summary>
		public static string ToString(Dictionary<string, string> query)
		{
			if (query == null || query.Count == 0)
				return string.Empty;

			var stringBuilder = new StringBuilder();

			var index = 0;
			foreach (var q in query)
			{
				if (q.Key.StartsWith("noname_"))
				{
					stringBuilder.Append(q.Value);
				}
				else
				{
					stringBuilder.Append($"{q.Key}={q.Value}");
				}

				if (index + 1 < query.Count)
				{
					stringBuilder.Append("&");
				}

				++index;
			}

			return stringBuilder.ToString();
		}
	}
}