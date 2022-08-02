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
		public static IParser<U> Select<T,U>(this IParser<T> Parser, Func<T,U> Selector)
		{
			ParserDelegate<U> parserDelegate= (reader, includedChars) =>
			{
				IParseResult result;
				result=Parser.TryParse(reader, includedChars);
				switch(result)
				{
					case ISucceededParseResult<T> succeeded:
						return ParseResult.Succeeded<U>(result.Position, Selector(succeeded.Value));
					default :
						return result;
				}
				
			};
			return new Parser<U>(parserDelegate);
		}//*/
		public static IParser<V> SelectMany<T, U, V>(
		   this IParser<T> Parser,
		   Func<T, IParser<U>> Selector,
		   Func<T, U, V> Projector)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			if (Selector == null) throw new ArgumentNullException(nameof(Selector));
			if (Projector == null) throw new ArgumentNullException(nameof(Projector));

			return Parser.Then(t => Selector(t).Select(u => Projector(t, u)));
		}
		public static IParser<IEnumerable<T>> Then<T>(this IParser<T> A, IParser<T> B)
		{
			if (A == null) throw new ArgumentNullException(nameof(A));
			if (B == null) throw new ArgumentNullException(nameof(B));
			ParserDelegate<IEnumerable<T>> parserDelegate = (reader, includedChars) =>
			{
				IParseResult result1;
				IParseResult result2;
				ISucceededParseResult<T> success1, success2;


				result1 = A.TryParse(reader, includedChars);
				success1 = result1 as ISucceededParseResult<T>;
				if (success1==null) return result1;
				
				result2 = B.TryParse(reader, includedChars);
				success2 = result2 as ISucceededParseResult<T>;
				if (success2 == null) return result2;

				return ParseResult.Succeeded<IEnumerable<T>>(result1.Position, new T[] { success1.Value, success2.Value });
			};
			return new Parser<IEnumerable<T>>(parserDelegate);
		}
		public static IParser<U> Then<T, U>(this IParser<T> First, Func<T, IParser<U>> Second)
		{

			if (First == null) throw new ArgumentNullException(nameof(First));
			if (Second == null) throw new ArgumentNullException(nameof(Second));

			ParserDelegate<U> parserDelegate = (reader, includedChars) => {
				IParseResult result1;
				IParseResult result2;

				result1 = First.TryParse(reader, includedChars);
				switch (result1)
				{
					case ISucceededParseResult<T> success:
						result2 = Second(success.Value).TryParse(reader, includedChars);
						return result2;
					default:return result1;
				}


			};
			return new Parser<U>(parserDelegate);

		}
		public static IParser<string> ToStringParser<T>(this IParser<T> A)
		{

			if (A == null) throw new ArgumentNullException(nameof(A));

			ParserDelegate<string> parserDelegate = (reader, includedChars) =>
			{
				IParseResult resultA;

				resultA = A.TryParse(reader, includedChars);
				switch (resultA)
				{
					case ISucceededParseResult<T> success:
						return ParseResult.Succeeded<string>(resultA.Position, success.Value?.ToString() ?? null);
					default:return resultA;
				}
				
			};
			return new Parser<string>(parserDelegate);

		}


		#endregion


















	}
}
