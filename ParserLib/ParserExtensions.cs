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
			ParserDelegate<U> parserDelegate= (reader) =>
			{
				IParseResult<T> result;
				result=Parser.TryParse(reader);
				switch(result)
				{
					case ISucceededParseResult<T> succeeded:
						return ParseResult<U>.Succeeded(Selector(result.Value));
					case IUnexpectedCharParseResult<T> failed:
						return ParseResult<U>.Failed(failed.Input);
					case IEndOfReaderParseResult<T> endOfReader:
						return ParseResult<U>.EndOfReader();
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

		public static IParser<U> Then<T, U>(this IParser<T> First, Func<T, IParser<U>> second)
		{

			if (First == null) throw new ArgumentNullException(nameof(First));
			if (second == null) throw new ArgumentNullException(nameof(second));

			ParserDelegate<U> parserDelegate = (reader) => {
				IParseResult<T> result1;
				IParseResult<U> result2;

				result1 = First.TryParse(reader);
				switch (result1)
				{
					case IUnexpectedCharParseResult<T> failed:
						return ParseResult<U>.Failed(failed.Input);
					case IEndOfReaderParseResult<T> endOfReader:
						return ParseResult<U>.EndOfReader();
				}

				result2 = second(result1.Value).TryParse(reader);
				return result2;
			};
			return new Parser<U>(parserDelegate);

		}
		

		
		#endregion

		


	
		
		

	
		








	}
}
