namespace US
{
	public class TokenDefinition
	{
		public TokenType Type { get; private set; }
		public string RegexPattern { get; private set; }

		public TokenDefinition(TokenType type, string regexPattern)
		{
			Type = type;
			RegexPattern = regexPattern;
		}
	}
}