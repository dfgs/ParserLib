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
		public static ISingleParser<char> Char(char Value)
		{
			ParserDelegate<char> parserDelegate = (reader, includedChars) =>
			{
				char input;
				long position;

				position = reader.Position;
				if (!reader.Read(out input, includedChars)) return ParseResult<char>.EndOfReader(position);
				if (input == Value) return ParseResult<char>.Succeeded(position, input);
				else return ParseResult<char>.Failed(position, input);
			};
			return new SingleParser<char>(parserDelegate);
		}
		public static ISingleParser<string> String(string Value)
		{
			char input;

			if (Value == null) throw new ArgumentNullException(nameof(Value));
			
			ParserDelegate<string> parserDelegate = (reader, includedChars) =>
			{
				long position;

				foreach (char value in Value)
				{
					position= reader.Position; 
					if (!reader.Read(out input,includedChars)) return ParseResult<string>.EndOfReader(position);
					if (input == value) continue;
					else return ParseResult<string>.Failed(position,input);

				}
				
				return ParseResult<string>.Succeeded(reader.Position - 1,Value);
			};
			return new SingleParser<string>(parserDelegate);
		}
		public static ISingleParser<char> Any()
		{
			ParserDelegate<char> parserDelegate = (reader, includedChars) => {
				char input;
				long position;

				position = reader.Position;
				if (!reader.Read(out input,includedChars)) return ParseResult<char>.EndOfReader(position);
				return ParseResult<char>.Succeeded(position, input);
			};
			return new SingleParser<char>(parserDelegate);
		}

		public static ISingleParser<char> AnyOf(params char[] Values)
		{
			ParserDelegate<char> parserDelegate = (reader, includedChars) => {
				char input;
				long position;

				position = reader.Position;
				if (!reader.Read(out input,includedChars)) return ParseResult<char>.EndOfReader(position);
				if (Values.Contains(input)) return ParseResult<char>.Succeeded(position, input);
				else return ParseResult<char>.Failed(position,input);
			};
			return new SingleParser<char>(parserDelegate);
		}
		public static ISingleParser<char> AnyInRange(char First,char Last)
		{
			ParserDelegate<char> parserDelegate = (reader, includedChars) => {
				char input;
				long position;

				position = reader.Position;
				if (!reader.Read(out input,includedChars)) return ParseResult<char>.EndOfReader(position);
				if ((input>=First) && (input <= Last)) return ParseResult<char>.Succeeded(position, input);
				else return ParseResult<char>.Failed(position,input);
			};
			return new SingleParser<char>(parserDelegate);
		}
		public static ISingleParser<char> Except(params char[] Values)
		{
			ParserDelegate<char> parserDelegate = (reader, includedChars) => {
				char input;
				long position;

				position = reader.Position;
				if (!reader.Read(out input,includedChars)) return ParseResult<char>.EndOfReader(position);
				if (!Values.Contains(input)) return ParseResult<char>.Succeeded(position, input);
				else return ParseResult<char>.Failed(position,input);
			};
			return new SingleParser<char>(parserDelegate);
		}

		public static ISingleParser<byte> Digit()
		{
			ParserDelegate<byte> parserDelegate = (reader, includedChars) =>
			{
				char input;
				long position;

				position = reader.Position;
				if (!reader.Read(out input,includedChars)) return ParseResult<byte>.EndOfReader(position);
				switch (input)
				{
					case '0': return ParseResult<byte>.Succeeded(position, (byte)0);
					case '1': return ParseResult<byte>.Succeeded(position, (byte)1);
					case '2': return ParseResult<byte>.Succeeded(position, (byte)2);
					case '3': return ParseResult<byte>.Succeeded(position, (byte)3);
					case '4': return ParseResult<byte>.Succeeded(position, (byte)4);
					case '5': return ParseResult<byte>.Succeeded(position, (byte)5);
					case '6': return ParseResult<byte>.Succeeded(position, (byte)6);
					case '7': return ParseResult<byte>.Succeeded(position, (byte)7);
					case '8': return ParseResult<byte>.Succeeded(position, (byte)8);
					case '9': return ParseResult<byte>.Succeeded(position, (byte)9);
				}

				return ParseResult<byte>.Failed(position, input);
			};
			return new SingleParser<byte>(parserDelegate);
		}
		public static ISingleParser<byte> Byte()
		{
			
			ISingleParser<string> a, b, c,d,e;
			
			a = (Parse.Char('2').Then(Parse.Char('5')).Then(Parse.AnyInRange('0', '5'))).ToStringParser() ;
			b = (Parse.Char('2').Then(Parse.AnyInRange('0', '4')).Then(Parse.AnyInRange('0', '9'))).ToStringParser();
			c = (Parse.Char('1').Then(Parse.AnyInRange('0', '9')).Then(Parse.AnyInRange('0', '9'))).ToStringParser();
			d = (Parse.AnyInRange('1', '9').Then(Parse.AnyInRange('0', '9').ZeroOrOneTime())).ToStringParser();
			e = Parse.Char('0').ToStringParser();
			
			return from value in a.Or(b).Or(c).Or(d).Or(e)
				   select Convert.ToByte(value) ;
		}
		public static ISingleParser<int> Int()
		{

			ISingleParser<string> positive,negative;

			negative = Parse.Char('-').Then(Parse.AnyInRange('0', '9').OneOrMoreTimes()).ToStringParser();
			positive = Parse.AnyInRange('0', '9').OneOrMoreTimes().ToStringParser();
			return from value in positive.Or(negative)
				   select  Convert.ToInt32(value) ;
		}
		public static ISingleParser<IPAddress> IPAddress()
		{

			return
				from A in Parse.Byte()
				from _1 in Parse.Char('.')
				from B in Parse.Byte()
				from _2 in Parse.Char('.')
				from C in Parse.Byte()
				from _3 in Parse.Char('.')
				from D in Parse.Byte()

				select new IPAddress(new byte[] {A,B, C, D });
		}

		public static ISingleParser<T> Or<T>(this ISingleParser<T> A, ISingleParser<T> B)
		{
			if (A == null) throw new ArgumentNullException(nameof(A));
			if (B == null) throw new ArgumentNullException(nameof(B));
			ParserDelegate<T> parseDelegate = (reader, includedChars) =>
			{
				IParseResult<T> resultA,resultB;

				resultA = A.TryParse(reader, includedChars);
				if (resultA is ISucceededParseResult<T>) return resultA;


				resultB = B.TryParse(reader, includedChars);
				if (resultB is ISucceededParseResult<T>) return resultB;

				if (resultA.Position > resultB.Position) return resultA;
				else return resultB;

			};
			return new SingleParser<T>(parseDelegate);
		}
		public static IMultipleParser<T> Or<T>(this IMultipleParser<T> A, IMultipleParser<T> B)
		{
			if (A == null) throw new ArgumentNullException(nameof(A));
			if (B == null) throw new ArgumentNullException(nameof(B));
			ParserDelegate<T> parseDelegate = (reader, includedChars) =>
			{
				IParseResult<T> resultA, resultB;

				resultA = A.TryParse(reader, includedChars);
				if (resultA is ISucceededParseResult<T>) return resultA;


				resultB = B.TryParse(reader, includedChars);
				if (resultB is ISucceededParseResult<T>) return resultB;

				if (resultA.Position > resultB.Position) return resultA;
				else return resultB;

			};
			return new MultipleParser<T>(parseDelegate);
		}
		public static ISingleParser<T> ReaderIncludes<T>(this IParser<T> A, params char[] IncludedChars)
		{
			if (A == null) throw new ArgumentNullException(nameof(A));
			ParserDelegate<T> parseDelegate = (reader, includedChars) =>
			{
				IParseResult<T> result;

				result = A.TryParse(reader, includedChars.Union(IncludedChars).ToArray());
				return result;
			};
			return new SingleParser<T>(parseDelegate);
		}


		public static IMultipleParser<T> OneOrMoreTimes<T>(this IParser<T> Parser)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			ParserDelegate<T> parserDelegate = (reader, includedChars) =>
			{
				IParseResult<T> result;
				List<T> items;
				long position;

				items = new List<T>();

				position = reader.Position;
				result = Parser.TryParse(reader, includedChars);
				switch (result)
				{
					case ISucceededParseResult<T> success:
						items.AddRange(success.EnumerateValue());
						break;
					default: return result;
				}

				while (!reader.EOF)
				{
					result = Parser.TryParse(reader, includedChars);
					switch (result)
					{
						case ISucceededParseResult<T> success:
							items.AddRange(success.EnumerateValue());
							break;
						default:
							return ParseResult<T>.Succeeded(position, items);
					}
				}

				return ParseResult<T>.Succeeded(position,items);
			};
			return new MultipleParser<T>(parserDelegate);
		}
		public static IMultipleParser<T> ZeroOrMoreTimes<T>(this IParser<T> Parser)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			ParserDelegate<T> parserDelegate = (reader, includedChars) =>
			{
				IParseResult<T> result;
				List<T> items;
				long position;

				items = new List<T>();

				position = reader.Position;	
				result = Parser.TryParse(reader, includedChars);
				switch (result)
				{
					case ISucceededParseResult<T> success:
						items.AddRange(success.EnumerateValue());
						break;
					default:
						return ParseResult<T>.Succeeded(position, Enumerable.Empty<T>());
				}

				while (!reader.EOF)
				{
					result = Parser.TryParse(reader, includedChars);
					switch (result)
					{
						case ISucceededParseResult<T> success:
							items.AddRange(success.EnumerateValue());
							break;
						default:
							return ParseResult<T>.Succeeded(position, items);
					}
				}

				return ParseResult<T>.Succeeded(position,items);
			};
			return new MultipleParser<T>(parserDelegate);

		}

		public static IMultipleParser<T> ZeroOrOneTime<T>(this IParser<T> Parser)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			ParserDelegate<T> parserDelegate = (reader, includedChars) =>
			{
				IParseResult<T> result;
				long position;

				position = reader.Position;
				result = Parser.TryParse(reader, includedChars);
				
				switch (result)
				{
					case ISucceededParseResult<T> success: return ParseResult<T>.Succeeded(success.Position, success.EnumerateValue()); ;
					default:
						return ParseResult<T>.Succeeded(position, Enumerable.Empty<T>()); 
				}
				
			};
			return new MultipleParser<T>(parserDelegate);

		}



	}
}
