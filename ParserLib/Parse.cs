using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public static class Parse
	{
		public static Parser<string> Char(char Value)
		{
			ParserDelegate<string> parserDelegate = (reader) =>
			{
				char input;
				if (reader.EOF) return ParseResult<string>.EndOfReader();
				input = reader.Read();
				if (input == Value) return ParseResult<string>.Succeeded(input.ToString());
				else return ParseResult<string>.Failed(input);
			};
			return new Parser<string>(parserDelegate);
		}
		public static Parser<string> Any()
		{
			ParserDelegate<string> parserDelegate = (reader) => {
				char input;
				if (reader.EOF) return ParseResult<string>.EndOfReader();
				input = reader.Read();
				return ParseResult<string>.Succeeded( input.ToString());
			};
			return new Parser<string>(parserDelegate);
		}

		public static Parser<string> AnyOf(params char[] Values)
		{
			ParserDelegate<string> parserDelegate = (reader) => {
				char input;
				if (reader.EOF) return ParseResult<string>.EndOfReader();
				input = reader.Read();
				if (Values.Contains(input)) return ParseResult<string>.Succeeded(input.ToString());
				else return ParseResult<string>.Failed(input);
			};
			return new Parser<string>(parserDelegate);
		}

		public static Parser<string> Except(params char[] Values)
		{
			ParserDelegate<string> parserDelegate = (reader) => {
				char input;
				if (reader.EOF) return ParseResult<string>.EndOfReader();
				input = reader.Read();
				if (!Values.Contains(input)) return ParseResult<string>.Succeeded(input.ToString());
				else return ParseResult<string>.Failed(input);
			};
			return new Parser<string>(parserDelegate);
		}

		public static Parser<byte> Digit()
		{
			ParserDelegate<byte> parserDelegate = (reader) =>
			{
				char input;
				if (reader.EOF) return ParseResult<byte>.EndOfReader();
				input = reader.Read();
				switch(input)
				{
					case '0': return ParseResult<byte>.Succeeded((byte)0);
					case '1': return ParseResult<byte>.Succeeded((byte)1);
					case '2': return ParseResult<byte>.Succeeded((byte)2);
					case '3': return ParseResult<byte>.Succeeded((byte)3);
					case '4': return ParseResult<byte>.Succeeded((byte)4);
					case '5': return ParseResult<byte>.Succeeded((byte)5);
					case '6': return ParseResult<byte>.Succeeded((byte)6);
					case '7': return ParseResult<byte>.Succeeded((byte)7);
					case '8': return ParseResult<byte>.Succeeded((byte)8);
					case '9': return ParseResult<byte>.Succeeded((byte)9);
				}

				return ParseResult<byte>.Failed(input);
			};
			return new Parser<byte>(parserDelegate);
		}

		public static Parser<T> Or<T>(this Parser<T> A, Parser<T> B)
		{
			if (A == null) throw new ArgumentNullException(nameof(A));
			if (B == null) throw new ArgumentNullException(nameof(B));
			ParserDelegate<T> parseDelegate = (reader) =>
			{
				ParseResult<T> result;

				result = A.TryParse(reader);
				if (result.IsSuccess) return result;
				result = B.TryParse(reader);

				return result;
			};
			return new Parser<T>(parseDelegate);
		}

		public static Parser<string> Then(this Parser<string> A, Parser<string> B)
		{
			if (A == null) throw new ArgumentNullException(nameof(A));
			if (B == null) throw new ArgumentNullException(nameof(B));
			ParserDelegate<string> parserDelegate = (reader) =>
			{
				ParseResult<string> result1;
				ParseResult<string> result2;

				result1 = A.TryParse(reader);
				if (!result1.IsSuccess) return result1;
				result2 = B.TryParse(reader);
				if (!result2.IsSuccess) return result2;

				return ParseResult<string>.Succeeded(result1.Value + result2.Value);
			};
			return new Parser<string>(parserDelegate);
		}

		public static Parser<string> OneOrMoreTimes(this Parser<string> Parser)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			ParserDelegate<string> parserDelegate = (reader) =>
			{
				ParseResult<string> result;
				List<string> items;

				result = Parser.TryParse(reader);
				if (!result.IsSuccess) return result;

				items = new List<string>();
				items.Add(result.Value);
				while (!reader.EOF)
				{
					result = Parser.TryParse(reader);
					if (!result.IsSuccess) break;
					items.Add(result.Value);
				}

				return ParseResult<string>.Succeeded(string.Join("", items));
			};
			return new Parser<string>(parserDelegate);

		}
		public static Parser<string> ZeroOrMoreTimes(this Parser<string> Parser)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			ParserDelegate<string> parserDelegate = (reader) =>
			{
				ParseResult<string> result;
				List<string> items;

				result = Parser.TryParse(reader);
				if (!result.IsSuccess) return ParseResult<string>.Succeeded("");

				items = new List<string>();
				items.Add(result.Value);
				while (!reader.EOF)
				{
					result = Parser.TryParse(reader);
					if (!result.IsSuccess) break;
					items.Add(result.Value);
				}

				return ParseResult<string>.Succeeded(string.Join("", items));
			};
			return new Parser<string>(parserDelegate);

		}
		public static Parser<string> ZeroOrOneTime(this Parser<string> Parser)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			ParserDelegate<string> parserDelegate = (reader) =>
			{
				ParseResult<string> result;

				result = Parser.TryParse(reader);
				if (result.IsSuccess) return result;

				return ParseResult<string>.Succeeded("");
			};
			return new Parser<string>(parserDelegate);

		}



	}
}
