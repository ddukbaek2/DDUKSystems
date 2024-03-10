using System;
using System.Collections.Generic;


namespace US
{
	/*
	 * 8 + 2 - 3 + 0
	 *	=> push(0, 8)
	 *	=> push(1, +)
	 *	=> push(2, 2)
	 *	=> check ( left, right, operator )
	 *	=> push(3, calculate(stack[0], stack[1], stack[2]))
	 *	=> push(4, -)
	 *	=> push(5, 3)
	 *	=> check ( left, right, operator )
	 *	=> push(6, calculate(stack[3], stack[4], stack[5]))
	 *	=> push(7, +)
	 *	=> push(8, 0)
	 *	=> check ( left, right, operator )
	 *	=> push(9, calculate(stack[6], stack[7], stack[8]))
	 */
	public static class Program
	{
		public static void Main(string[] args)
		{
			var tokenizer = new Tokenizer();

			var input = "8 + 2 - 3 + 0";
			Console.WriteLine(input);

			var tokens = tokenizer.Tokenize(input);

			foreach (var token in tokens)
			{
				Console.WriteLine($"Type: {token.Type}, Value: {token.Value}");
			}

			Console.ReadLine();
		}

		public static AbstractSyntaxNode Parse()
		{
			return new AbstractSyntaxNode
			{
				
			};
		}

		public static int Calculate(List<Token> tokens)
		{
			int result = 0;
			string operation = "+";

			foreach (var token in tokens)
			{
				if (token.Type == TokenType.Number)
				{
					int value = int.Parse(token.Value);
					if (operation == "+")
					{
						result += value;
					}
					else if (operation == "-")
					{
						result -= value;
					}
				}
				else if (token.Type == TokenType.Plus)
				{
					operation = "+";
				}
				else if (token.Type == TokenType.Minus)
				{
					operation = "-";
				}
			}

			return result;
		}
	}
}