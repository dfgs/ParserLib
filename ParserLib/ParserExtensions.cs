using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public static class ParserExtensions
	{
		#region Linq extensions
		public static Parser<U> Select<T,U>(this Parser<T> Parser, Func<T,U> Selector)
		{
			return (reader) =>
			{
				IParseResult<T> result;
				result=Parser(reader);
				if (result.Success) return ParseResult<U>.Succeded(result.Input, Selector(result.Value));
				return ParseResult<U>.Failed(result.Input, default(U));
			};
		}
		public static Parser<V> SelectMany<T, U, V>(
		   this Parser<T> parser,
		   Func<T, Parser<U>> selector,
		   Func<T, U, V> projector)
		{
			if (parser == null) throw new ArgumentNullException(nameof(parser));
			if (selector == null) throw new ArgumentNullException(nameof(selector));
			if (projector == null) throw new ArgumentNullException(nameof(projector));

			return parser.Then(t => selector(t).Select(u => projector(t, u)));
		}

		public static Parser<U> Then<T, U>(this Parser<T> first, Func<T, Parser<U>> second)
		{
			if (first == null) throw new ArgumentNullException(nameof(first));
			if (second == null) throw new ArgumentNullException(nameof(second));

			return reader => first(reader).IfSuccess(s => second(s.Value)(reader));
		}
		public static IParseResult<U> IfSuccess<T, U>(this IParseResult<T> result, Func<IParseResult<T>, IParseResult<U>> next)
		{
			if (result == null) throw new ArgumentNullException(nameof(result));

			if (result.Success)
				return next(result);

			return ParseResult<U>.Failed(result.Input, default(U));
		}

		public static IParseResult<T> IfFailure<T>(this IParseResult<T> result, Func<IParseResult<T>, IParseResult<T>> next)
		{
			if (result == null) throw new ArgumentNullException(nameof(result));

			return result.Success
				? result
				: next(result);
		}
		#endregion

		public static T Parse<T>(this Parser<T> Parser,IEnumerable<char> Value)
		{
			IParseResult<T> result;

			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			if (Value == null) throw new ArgumentNullException(nameof(Value));

			result = Parser(new Reader(Value));
			if (result.Success) return result.Value;
			throw new UnexpectedCharException(result.Input);
		}
		public static T Parse<T>(this Parser<T> Parser, IReader Reader)
		{
			IParseResult<T> result;

			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			if (Reader == null) throw new ArgumentNullException(nameof(Reader));

			result = Parser(Reader);
			if (result.Success) return result.Value;
			throw new UnexpectedCharException(result.Input);
		}
		public static bool TryParse<T>(this Parser<T> Parser, IEnumerable<char> Value)
		{
			IParseResult<T> result;

			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			if (Value == null) throw new ArgumentNullException(nameof(Value));

			result = Parser(new Reader(Value));
			return result.Success;
		}
		public static bool TryParse<T>(this Parser<T> Parser, IReader Reader)
		{
			IParseResult<T> result;

			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			if (Reader == null) throw new ArgumentNullException(nameof(Reader));

			result = Parser(Reader);
			return result.Success;
		}


		public static Parser<T> Or<T>(this Parser<T> A, Parser<T> B)
		{
			if (A == null) throw new ArgumentNullException(nameof(A));
			if (B == null) throw new ArgumentNullException(nameof(B));
			return (reader) =>
			{
				IParseResult<T> result;

				result = A(reader);
				if (result.Success) return result;
				result = B(reader);
				if (result.Success) return result;

				return result;
			};
		}
		
		
		
		public static Parser<string> Then(this Parser<string> A, Parser<string> B)
		{
			if (A == null) throw new ArgumentNullException(nameof(A));
			if (B == null) throw new ArgumentNullException(nameof(B));
			return (reader) =>
			{
				IParseResult<string> result1;
				IParseResult<string> result2;

				result1 = A(reader);
				if (!result1.Success) return ParseResult<string>.Failed(result1.Input, null);
				result2 = B(reader);
				if (!result2.Success) return ParseResult<string>.Failed(result2.Input, null);

				return ParseResult<string>.Succeded(result1.Input, result1.Value + result2.Value);
			};
		}

	
		public static Parser<string> Many(this Parser<string> Parser)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			return (reader) =>
			{
				IParseResult<string> result;
				List<string> items;

				result = Parser(reader);
				if (!result.Success) return ParseResult<string>.Failed(result.Input, null);

				items = new List<string>();
				items.Add(result.Value);
				do
				{
					result = Parser(reader);
					if (!result.Success) break;
					items.Add(result.Value);
				} while (!reader.EOF);

				return ParseResult<string>.Succeded(result.Input,string.Join("",items) );
			};

		}








	}
}
