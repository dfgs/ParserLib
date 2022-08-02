﻿using System;
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
						return ParseResult<U>.Succeeded(result.Position, Selector(result.Value));
					case IUnexpectedCharParseResult<T> failed:
						return ParseResult<U>.Failed(failed.Position,failed.Input);
					case IEndOfReaderParseResult<T> endOfReader:
						return ParseResult<U>.EndOfReader(endOfReader.Position);
					default:throw new NotSupportedException($"Parse result of type {result.GetType().Name} is not supported");
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
					case IUnexpectedCharParseResult<T> failed:
						return ParseResult<U>.Failed(failed.Position,failed.Input);
					case IEndOfReaderParseResult<T> endOfReader:
						return ParseResult<U>.EndOfReader(endOfReader.Position);
				}

				result2 = Second(result1.Value).TryParse(reader, includedChars);
				return result2;
			};
			return new Parser<U>(parserDelegate);

		}
		public static IParser<string> ToStringParser<T>(this IParser<T> A)
		{

			if (A == null) throw new ArgumentNullException(nameof(A));

			ParserDelegate<string> parserDelegate = (reader, includedChars) =>
			{
				IParseResult<T> resultA;

				resultA = A.TryParse(reader, includedChars);
				switch (resultA)
				{
					case IUnexpectedCharParseResult<T> failed:
						return ParseResult<string>.Failed(failed.Position, failed.Input);
					case IEndOfReaderParseResult<T> endOfReader:
						return ParseResult<string>.EndOfReader(endOfReader.Position);
					case ISucceededParseResult<T> success:
						return ParseResult<string>.Succeeded(resultA.Position, success.Value?.ToString()??null);
					default:throw new NotSupportedException("Result type not supported");
				}
				
			};
			return new Parser<string>(parserDelegate);

		}


		#endregion


















	}
}
