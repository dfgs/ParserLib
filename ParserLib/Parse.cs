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
		public static IParser<string> Char(char Value)
		{
			ParserDelegate<string> parserDelegate = (reader, includedChars) =>
			{
				char input;
				long position;

				position=reader.Position; 
				if (!reader.Read(out input,includedChars)) return ParseResult.EndOfReader(position);
				if (input == Value) return ParseResult.Succeeded(position,input.ToString());
				else return ParseResult.Failed(position,input);
			};
			return new Parser<string>(parserDelegate);
		}
		public static IParser<string> String(string Value)
		{
			char input;

			if (Value == null) throw new ArgumentNullException(nameof(Value));
			
			ParserDelegate<string> parserDelegate = (reader, includedChars) =>
			{
				long position;

				foreach (char value in Value)
				{
					position= reader.Position; 
					if (!reader.Read(out input,includedChars)) return ParseResult.EndOfReader(position);
					if (input == value) continue;
					else return ParseResult.Failed(position,input);

				}
				
				return ParseResult.Succeeded(reader.Position - 1,Value);
			};
			return new Parser<string>(parserDelegate);
		}
		public static IParser<string> Any()
		{
			ParserDelegate<string> parserDelegate = (reader, includedChars) => {
				char input;
				long position;

				position = reader.Position;
				if (!reader.Read(out input,includedChars)) return ParseResult.EndOfReader(position);
				return ParseResult.Succeeded(position, input.ToString());
			};
			return new Parser<string>(parserDelegate);
		}

		public static IParser<string> AnyOf(params char[] Values)
		{
			ParserDelegate<string> parserDelegate = (reader, includedChars) => {
				char input;
				long position;

				position = reader.Position;
				if (!reader.Read(out input,includedChars)) return ParseResult.EndOfReader(position);
				if (Values.Contains(input)) return ParseResult.Succeeded(position,input.ToString());
				else return ParseResult.Failed(position,input);
			};
			return new Parser<string>(parserDelegate);
		}
		public static IParser<string> AnyInRange(char First,char Last)
		{
			ParserDelegate<string> parserDelegate = (reader, includedChars) => {
				char input;
				long position;

				position = reader.Position;
				if (!reader.Read(out input,includedChars)) return ParseResult.EndOfReader(position);
				if ((input>=First) && (input <= Last)) return ParseResult.Succeeded(position,input.ToString());
				else return ParseResult.Failed(position,input);
			};
			return new Parser<string>(parserDelegate);
		}
		public static IParser<string> Except(params char[] Values)
		{
			ParserDelegate<string> parserDelegate = (reader, includedChars) => {
				char input;
				long position;

				position = reader.Position;
				if (!reader.Read(out input,includedChars)) return ParseResult.EndOfReader(position);
				if (!Values.Contains(input)) return ParseResult.Succeeded(position,input.ToString());
				else return ParseResult.Failed(position,input);
			};
			return new Parser<string>(parserDelegate);
		}

		public static IParser<byte> Digit()
		{
			ParserDelegate<byte> parserDelegate = (reader, includedChars) =>
			{
				char input;
				long position;

				position = reader.Position;
				if (!reader.Read(out input,includedChars)) return ParseResult.EndOfReader(position);
				switch (input)
				{
					case '0': return ParseResult.Succeeded(position,(byte)0);
					case '1': return ParseResult.Succeeded(position,(byte)1);
					case '2': return ParseResult.Succeeded(position, (byte)2);
					case '3': return ParseResult.Succeeded(position, (byte)3);
					case '4': return ParseResult.Succeeded(position, (byte)4);
					case '5': return ParseResult.Succeeded(position, (byte)5);
					case '6': return ParseResult.Succeeded(position, (byte)6);
					case '7': return ParseResult.Succeeded(position, (byte)7);
					case '8': return ParseResult.Succeeded(position, (byte)8);
					case '9': return ParseResult.Succeeded(position, (byte)9);
				}

				return ParseResult.Failed(position, input);
			};
			return new Parser<byte>(parserDelegate);
		}
		public static IParser<byte> Byte()
		{
			
			IParser<string> a, b, c,d,e;

			a = Parse.Char('2').Then(Parse.Char('5')).Then(Parse.AnyInRange('0', '5'));
			b = Parse.Char('2').Then(Parse.AnyInRange('0', '4')).Then(Parse.AnyInRange('0', '9'));
			c = Parse.Char('1').Then(Parse.AnyInRange('0', '9')).Then(Parse.AnyInRange('0', '9'));
			d = Parse.AnyInRange('1', '9').Then(Parse.AnyInRange('0', '9').ZeroOrOneTime());
			e = Parse.Char('0');
			return from value in a.Or(b).Or(c).Or(d).Or(e)
				   select Convert.ToByte(value);
		}
		public static IParser<int> Int()
		{

			IParser<string> positive,negative;

			negative = Parse.Char('-').Then(Parse.AnyInRange('0', '9').OneOrMoreTimes());
			positive = Parse.AnyInRange('0', '9').OneOrMoreTimes();
			return from value in positive.Or(negative)
				   select Convert.ToInt32(value);
		}
		public static IParser<IPAddress> IPAddress()
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

		public static IParser<T> Or<T>(this IParser<T> A, IParser<T> B)
		{
			if (A == null) throw new ArgumentNullException(nameof(A));
			if (B == null) throw new ArgumentNullException(nameof(B));
			ParserDelegate<T> parseDelegate = (reader, includedChars) =>
			{
				IParseResult resultA,resultB;

				resultA = A.TryParse(reader, includedChars);
				if (resultA is ISucceededParseResult<T>) return resultA;


				resultB = B.TryParse(reader, includedChars);
				if (resultB is ISucceededParseResult<T>) return resultB;

				if (resultA.Position > resultB.Position) return resultA;
				else return resultB;

			};
			return new Parser<T>(parseDelegate);
		}

		public static IParser<T> ReaderIncludes<T>(this IParser<T> A, params char[] IncludedChars)
		{
			if (A == null) throw new ArgumentNullException(nameof(A));
			ParserDelegate<T> parseDelegate = (reader, includedChars) =>
			{
				IParseResult result;

				result = A.TryParse(reader, includedChars.Union(IncludedChars).ToArray());
				return result;
			};
			return new Parser<T>(parseDelegate);
		}

		public static IParser<string> Then(this IParser<string> A, IParser<string> B)
		{
			if (A == null) throw new ArgumentNullException(nameof(A));
			if (B == null) throw new ArgumentNullException(nameof(B));
			ParserDelegate<string> parserDelegate = (reader, includedChars) =>
			{
				IParseResult result1;
				IParseResult result2;
				ISucceededParseResult<string> success1,success2;

				result1 = A.TryParse(reader, includedChars);
				success1 = result1 as ISucceededParseResult<string>;
				if (success1==null) return result1;
				
				result2 = B.TryParse(reader, includedChars);
				success2 = result2 as ISucceededParseResult<string>;
				if (success2 == null) return result2;

				return ParseResult.Succeeded(result1.Position,success1.Value + success2.Value) ;
			};
			return new Parser<string>(parserDelegate);
		}
		

		public static IParser<IEnumerable<T>> OneOrMoreTimes<T>(this IParser<T> Parser)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			ParserDelegate<IEnumerable<T>> parserDelegate = (reader, includedChars) =>
			{
				IParseResult result;
				List<T> items;
				long position;

				items = new List<T>();

				position = reader.Position;
				result = Parser.TryParse(reader, includedChars);
				switch (result)
				{
					case ISucceededParseResult<T> success:
						items.Add(success.Value);
						break;
					default:return result;
				}

				while (!reader.EOF)
				{
					result = Parser.TryParse(reader, includedChars);
					switch (result)
					{
						case ISucceededParseResult<T> success:
							items.Add(success.Value);
							break;
						default:
							return ParseResult.Succeeded<IEnumerable<T>>(position, items);
					}
				}

				return ParseResult.Succeeded<IEnumerable<T>>(position,items);
			};
			return new Parser<IEnumerable<T>>(parserDelegate);
		}
		public static IParser<T> OneOrMoreTimes<T>(this IParser<T> Parser,Func<IEnumerable<T>,T> Func)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			if (Func == null) throw new ArgumentNullException(nameof(Func));
			ParserDelegate<T> parserDelegate = (reader, includedChars) =>
			{
				IParseResult result;
				List<T> items;
				long position;

				items = new List<T>();

				position = reader.Position;
				result = Parser.TryParse(reader, includedChars);
				switch (result)
				{
					
					case ISucceededParseResult<T> success:
						items.Add(success.Value);
						break;
					default:return result;
				}

				while (!reader.EOF)
				{
					result = Parser.TryParse(reader, includedChars);
					switch (result)
					{
						case ISucceededParseResult<T> success:
							items.Add(success.Value);
							break;
						default:
							return ParseResult.Succeeded<T>(position, Func(items));
					}
				}

				return ParseResult.Succeeded<T>(position,Func(items));
			};
			return new Parser<T>(parserDelegate);
		}
		public static IParser<string> OneOrMoreTimes(this IParser<string> Parser)
		{
			return Parser.OneOrMoreTimes((items) => string.Join("", items));
		}
		public static IParser<IEnumerable<T>> ZeroOrMoreTimes<T>(this IParser<T> Parser)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			ParserDelegate<IEnumerable<T>> parserDelegate = (reader, includedChars) =>
			{
				IParseResult result;
				List<T> items;
				long position;

				items = new List<T>();

				position = reader.Position;	
				result = Parser.TryParse(reader, includedChars);
				switch (result)
				{
					case ISucceededParseResult<T> success:
						items.Add(success.Value);
						break;
					default:
						return ParseResult.Succeeded(position, Enumerable.Empty<T>());
				}

				while (!reader.EOF)
				{
					result = Parser.TryParse(reader, includedChars);
					switch (result)
					{
						case ISucceededParseResult<T> success:
							items.Add(success.Value);
							break;
						default:
							return ParseResult.Succeeded(position, items);
					}
				}

				return ParseResult.Succeeded(position,items);
			};
			return new Parser<IEnumerable<T>>(parserDelegate);

		}
		public static IParser<T> ZeroOrMoreTimes<T>(this IParser<T> Parser, Func<IEnumerable<T>, T> Func)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			if (Func == null) throw new ArgumentNullException(nameof(Func));
			ParserDelegate<T> parserDelegate = (reader, includedChars) =>
			{
				IParseResult result;
				List<T> items;
				long position;


				items = new List<T>();

				position = reader.Position;
				result = Parser.TryParse(reader, includedChars);
				switch (result)
				{
					case ISucceededParseResult<T> success:
						items.Add(success.Value);
						break;
					default:
						return ParseResult.Succeeded(position, default(T)); ;
				}

				while (!reader.EOF)
				{
					result = Parser.TryParse(reader, includedChars);
					switch (result)
					{
						case ISucceededParseResult<T> success:
							items.Add(success.Value);
							break;
						default:
							return ParseResult.Succeeded(position, Func(items));
					}
				}

				return ParseResult.Succeeded(position, Func(items));
			};
			return new Parser<T>(parserDelegate);
		}
		public static IParser<string> ZeroOrMoreTimes(this IParser<string> Parser)
		{
			return Parser.ZeroOrMoreTimes((items) => string.Join("", items));
		}

		public static IParser<T> ZeroOrOneTime<T>(this IParser<T> Parser)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			ParserDelegate<T> parserDelegate = (reader, includedChars) =>
			{
				IParseResult result;
				long position;

				position = reader.Position;
				result = Parser.TryParse(reader, includedChars);

				switch (result)
				{
					case ISucceededParseResult<T> success: return result;
					default:
						return ParseResult.Succeeded(position, default(T)); ;
				}
				
			};
			return new Parser<T>(parserDelegate);

		}



	}
}
