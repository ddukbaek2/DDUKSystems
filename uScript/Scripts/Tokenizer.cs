using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace uScript
{
	public class Tokenizer
    {
        public List<Token> Tokenize(string input)
        {
            var tokens = new List<Token>();
            var tokenDefinitions = new List<TokenDefinition>
            {
                new TokenDefinition(TokenType.Number, @"\b\d+\b"),
                new TokenDefinition(TokenType.Plus, @"\+"),
                new TokenDefinition(TokenType.Minus, @"-")
            };

            foreach (var tokenDefinition in tokenDefinitions)
            {
                var matches = Regex.Matches(input, tokenDefinition.RegexPattern);
                foreach (Match match in matches)
                {
                    tokens.Add(new Token(tokenDefinition.Type, match.Value));
                }
            }

            // This example does not sort tokens by their position in the input string.
            // For a real tokenizer, you might need to sort tokens based on their starting index.

            return tokens;
        }
    }
}