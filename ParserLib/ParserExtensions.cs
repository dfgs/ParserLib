using System;
using System.Collections;
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
				IParseResult<T> result;
				result=Parser.TryParse(reader, includedChars);
				switch(result)
				{
					case ISucceededParseResult<T> succeeded:
						return ParseResult<U>.Succeeded(result.Position, Selector(succeeded.Value)  );
					case IFailedParseResult<T> failed: return failed.Cast<U>();
					default: throw new NotSupportedException("Invalid result type");
				}

			};
			return new Parser<U>(parserDelegate);
		}
		public static IParser<V> SelectMany<T, U, V>(
		   this IParser<T> Parser,
		   Func<T, IParser<U>> Selector,
		   Func<T, U, V> Projector)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			if (Selector == null) throw new ArgumentNullException(nameof(Selector));
			if (Projector == null) throw new ArgumentNullException(nameof(Projector));

			return Parser.Then(t => Selector(t).Select(u => Projector(t,  u  )));
		}//*/
		public static IParser<T> Then<T>(this IParser<T> A, IParser<T> B)
		{
			if (A == null) throw new ArgumentNullException(nameof(A));
			if (B == null) throw new ArgumentNullException(nameof(B));
			ParserDelegate<T> parserDelegate = (reader, includedChars) =>
			{
				IParseResult<T> result1;
				IParseResult<T> result2;
				ISucceededParseResult<T>? success1, success2;


				result1 = A.TryParse(reader, includedChars);
				success1 = result1 as ISucceededParseResult<T>;
				if (success1==null) return result1;
				
				result2 = B.TryParse(reader, includedChars);
				success2 = result2 as ISucceededParseResult<T>;
				if (success2 == null) return result2;

				return ParseResult<T>.Succeeded(result1.Position, success1.EnumerateValue().Concat(success2.EnumerateValue()) );
			};
			return new Parser<T>(parserDelegate);
		}
		/*public static IParser<IEnumerable<T>> Then<T>(this IParser<T> A, IParser<IEnumerable<T>> B)
		{
			if (A == null) throw new ArgumentNullException(nameof(A));
			if (B == null) throw new ArgumentNullException(nameof(B));
			ParserDelegate<IEnumerable<T>> parserDelegate = (reader, includedChars) =>
			{
				IParseResult<T> result1;
				IParseResult<IEnumerable<T>> result2;
				ISucceededParseResult<T>? success1;
				ISucceededParseResult<IEnumerable<T>>? success2;


				result1 = A.TryParse(reader, includedChars);
				success1 = result1 as ISucceededParseResult<T>;
				if (success1 == null) return ((IFailedParseResult<T>)result1).Cast<IEnumerable<T>>(); 

				result2 = B.TryParse(reader, includedChars);
				success2 = result2 as ISucceededParseResult<IEnumerable<T>>;
				if (success2 == null) return result2; 

				return ParseResult<T>.Succeeded(result1.Position, success2.Value.Prepend( success1.Value).ToArray());
			};
			return new Parser<IEnumerable<T>>(parserDelegate);
		}
		public static IParser<IEnumerable<T>> Then<T>(this IParser<IEnumerable<T>> A, IParser<T> B)
		{
			if (A == null) throw new ArgumentNullException(nameof(A));
			if (B == null) throw new ArgumentNullException(nameof(B));
			ParserDelegate<IEnumerable<T>> parserDelegate = (reader, includedChars) =>
			{
				IParseResult<IEnumerable<T>> result1;
				IParseResult<T> result2;
				ISucceededParseResult<IEnumerable<T>>? success1;
				ISucceededParseResult<T>? success2;


				result1 = A.TryParse(reader, includedChars);
				success1 = result1 as ISucceededParseResult<IEnumerable<T>>;
				if (success1 == null) return result1;

				result2 = B.TryParse(reader, includedChars);
				success2 = result2 as ISucceededParseResult<T>;
				if (success2 == null) return ((IFailedParseResult<T>)result2).Cast<IEnumerable<T>>();

				return ParseResult<T>.Succeeded(result1.Position, success1.Value.Append(success2.Value).ToArray());
			};
			return new Parser<IEnumerable<T>>(parserDelegate);
		}//*/
		public static IParser<U> Then<T, U>(this IParser<T> First, Func<T, IParser<U>> Second)
		{

			if (First == null) throw new ArgumentNullException(nameof(First));
			if (Second == null) throw new ArgumentNullException(nameof(Second));

			ParserDelegate<U> parserDelegate = (reader, includedChars) => {
				IParseResult<T> result1;
				IParseResult<U> result2;

				result1 = First.TryParse(reader, includedChars);
				switch (result1)
				{
					case ISucceededParseResult<T> success:
						result2 = Second(success.Value).TryParse(reader, includedChars);
						return result2;
					case IFailedParseResult<T> failed: return failed.Cast<U>();
					default: throw new NotSupportedException("Invalid result type");
				}


			};
			return new Parser<U>(parserDelegate);

		}
		
		public static IParser<string> ToStringParser<T>(this IParser<T> A)
		{
			string value;

			if (A == null) throw new ArgumentNullException(nameof(A));

			ParserDelegate<string> parserDelegate = (reader, includedChars) =>
			{
				IParseResult<T> resultA;

				resultA = A.TryParse(reader, includedChars);
				switch (resultA)
				{
					case ISucceededParseResult<T> success:
						value = string.Concat(success.EnumerateValue());
						return ParseResult<string>.Succeeded(resultA.Position,value);
					case IFailedParseResult<T> failed: return failed.Cast<string>();
					default: throw new NotSupportedException("Invalid result type");
				}

			};
			return new Parser<string>(parserDelegate);
		}

		#endregion


















	}
}
