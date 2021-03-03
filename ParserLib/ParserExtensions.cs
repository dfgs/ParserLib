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
			ParserDelegate<U> parserDelegate= (reader) =>
			{
				ParseResult<T> result;
				result=Parser.TryParse(reader);
				switch(result)
				{
					case SucceededParseResult<T> succeeded:
						return ParseResult<U>.Succeeded(Selector(result.Value));
					case UnexpectedCharParseResult<T> failed:
						return ParseResult<U>.Failed(failed.Input);
					case EndOfReaderParseResult<T> endOfReader:
						return ParseResult<U>.EndOfReader();
					default:throw new NotSupportedException($"Parse result of type {result.GetType().Name} is not supported");
				}
				
			};
			return new Parser<U>(parserDelegate);
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

			ParserDelegate<U> parserDelegate = (reader) => first.TryParse(reader).IfSuccess<T,U>(parseResult => second((parseResult).Value).TryParse(reader));
			return new Parser<U>(parserDelegate);
		}
		public static ParseResult<U> IfSuccess<T, U>(this ParseResult<T> result, Func<ParseResult<T>, ParseResult<U>> next)
		{
			if (result == null) throw new ArgumentNullException(nameof(result));


			switch (result)
			{
				case SucceededParseResult<T> succeeded:
					return next(result);
				case UnexpectedCharParseResult<T> failed:
					return ParseResult<U>.Failed(failed.Input);
				case EndOfReaderParseResult<T> endOfReader:
					return ParseResult<U>.EndOfReader();
				default: throw new NotSupportedException($"Parse result of type {result.GetType().Name} is not supported");
			}

		}

		
		#endregion

		


	
		
		

	
		








	}
}
