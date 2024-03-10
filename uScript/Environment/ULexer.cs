namespace uScript
{
	/*
		Expression -> Term {( "+" | "-" ) Term}
		Term -> Factor {( "*" | "/" ) Factor}
		Factor -> "(" Expression ")" | Number
		Number -> [0-9]+
	 */

	/// <summary>
	/// 나누어질 단위.
	/// </summary>
	public class UToken
	{
		public string Type { set; get; }
		public string Value { set; get; }
	}


	/// <summary>
	/// 문법 규칙.
	/// </summary>
	public class ULexer
	{
		public ULexer(string text)
		{

		}
	}


	/// <summary>
	/// 파서.
	/// </summary>
	public class UParser
	{
		public UParser(ULexer lexer)
		{
		}

		public void Expression()
		{
		}
	}
}