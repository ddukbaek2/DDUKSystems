namespace uScript
{
    public enum TokenType
    {
        Number,
		Plus,
		Minus,
		Multiply,
		Divide,
		LeftParenthesis,
		RightParenthesis,
	}

    public class Token
    {
        public TokenType Type { private set; get; }
        public string Value { private set; get; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}