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
				if (result.IsSuccess) return ParseResult<U>.Succeded(Selector(result.Value));
				return ParseResult<U>.Failed(result);
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

			ParserDelegate<U> parserDelegate = (reader) => first.TryParse(reader).IfSuccess(parseResult => second(parseResult.Value).TryParse(reader));
			return new Parser<U>(parserDelegate);
		}
		public static ParseResult<U> IfSuccess<T, U>(this ParseResult<T> result, Func<ParseResult<T>, ParseResult<U>> next)
		{
			if (result == null) throw new ArgumentNullException(nameof(result));

			if (result.IsSuccess)
				return next(result);

			return ParseResult<U>.Failed(result);
		}

		
		#endregion

		


	
		
		

	
		








	}
}
