using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
		public static Parser<string> String(string Value)
		{
			char input;

			if (Value == null) throw new ArgumentNullException(nameof(Value));
			
			ParserDelegate<string> parserDelegate = (reader) =>
			{
				foreach(char value in Value)
				{
					if (reader.EOF) return ParseResult<string>.EndOfReader();
					input = reader.Read();
					if (input == value) continue;
					else return ParseResult<string>.Failed(input);

				}
				
				return ParseResult<string>.Succeeded(Value);
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
		public static Parser<string> AnyInRange(char First,char Last)
		{
			ParserDelegate<string> parserDelegate = (reader) => {
				char input;
				if (reader.EOF) return ParseResult<string>.EndOfReader();
				input = reader.Read();
				if ((input>=First) && (input <= Last)) return ParseResult<string>.Succeeded(input.ToString());
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
		public static Parser<byte> Byte()
		{
			
			Parser<string> a, b, c,d,e;

			a = Parse.Char('2').Then(Parse.Char('5')).Then(Parse.AnyInRange('0', '5'));
			b = Parse.Char('2').Then(Parse.AnyInRange('0', '4')).Then(Parse.AnyInRange('0', '9'));
			c = Parse.Char('1').Then(Parse.AnyInRange('0', '9')).Then(Parse.AnyInRange('0', '9'));
			d = Parse.AnyInRange('1', '9').Then(Parse.AnyInRange('0', '9').ZeroOrOneTime());
			e = Parse.Char('0');
			return from value in a.Or(b).Or(c).Or(d).Or(e)
				   select Convert.ToByte(value);
		}
		public static Parser<int> Int()
		{

			Parser<string> positive,negative;

			negative = Parse.Char('-').Then(Parse.AnyInRange('0', '9').OneOrMoreTimes());
			positive = Parse.AnyInRange('0', '9').OneOrMoreTimes();
			return from value in positive.Or(negative)
				   select Convert.ToInt32(value);
		}
		public static Parser<IPAddress> IPAddress()
		{

			return
				from A in Parse.Byte()
				from _1 in Parse.Char('.')
				from B in Parse.Byte()
				from _2 in Parse.Char('.')
				from C in Parse.Byte()
				from _3 in Parse.Char('.')
				from D in Parse.Byte()

				select new IPAddress(new byte[] {A,B,C,D });
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

		public static Parser<T> OneOrMoreTimes<T>(this Parser<T> Parser,Func<IEnumerable<T>,T> Func)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			if (Func == null) throw new ArgumentNullException(nameof(Func));
			ParserDelegate<T> parserDelegate = (reader) =>
			{
				ParseResult<T> result;
				List<T> items;

				result = Parser.TryParse(reader);
				if (!result.IsSuccess) return result;

				items = new List<T>();
				items.Add(result.Value);
				while (!reader.EOF)
				{
					result = Parser.TryParse(reader);
					if (!result.IsSuccess) break;
					items.Add(result.Value);
				}

				return ParseResult<T>.Succeeded(Func(items));
			};
			return new Parser<T>(parserDelegate);
		}
		public static Parser<string> OneOrMoreTimes(this Parser<string> Parser)
		{
			return Parser.OneOrMoreTimes((items) => string.Join("", items));
		}

		public static Parser<T> ZeroOrMoreTimes<T>(this Parser<T> Parser, Func<IEnumerable<T>, T> Func)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			if (Func == null) throw new ArgumentNullException(nameof(Func));
			ParserDelegate<T> parserDelegate = (reader) =>
			{
				ParseResult<T> result;
				List<T> items;

				result = Parser.TryParse(reader);
				if (!result.IsSuccess) return ParseResult<T>.Succeeded(default(T));

				items = new List<T>();
				items.Add(result.Value);
				while (!reader.EOF)
				{
					result = Parser.TryParse(reader);
					if (!result.IsSuccess) break;
					items.Add(result.Value);
				}

				return ParseResult<T>.Succeeded(Func(items));
			};
			return new Parser<T>(parserDelegate);

		}
		public static Parser<string> ZeroOrMoreTimes(this Parser<string> Parser)
		{
			return Parser.ZeroOrMoreTimes((items) => string.Join("", items));
		}

		public static Parser<T> ZeroOrOneTime<T>(this Parser<T> Parser)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			ParserDelegate<T> parserDelegate = (reader) =>
			{
				ParseResult<T> result;

				result = Parser.TryParse(reader);
				if (result.IsSuccess) return result;

				return ParseResult<T>.Succeeded(default(T));
			};
			return new Parser<T>(parserDelegate);

		}



	}
}
