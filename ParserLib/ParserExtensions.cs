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
		public static ISingleParser<TResult> Select<T,TResult>(this ISingleParser<T> Parser, Func<T,TResult> Selector)
		{
			ParserDelegate<TResult> parserDelegate= (reader, includedChars) =>
			{
				IParseResult result;
				result=Parser.TryParse(reader, includedChars);
				switch(result)
				{
					case ISucceededParseResult<T> succeeded:
						return ParseResult.Succeeded(result.Position, Selector(succeeded.Value)  );
					default: return result;
				}

			};
			return new SingleParser<TResult>($"{Parser.Description}%", parserDelegate);
		}
		public static ISingleParser<TResult> Select<T, TResult>(this IMultipleParser<T> Parser, Func<IEnumerable<T>, TResult> Selector)
		{
			ParserDelegate<TResult> parserDelegate = (reader, includedChars) =>
			{
				IParseResult result;
				result = Parser.TryParse(reader, includedChars);
				switch (result)
				{
					case ISucceededParseResult<T> succeeded:
						return ParseResult.Succeeded(result.Position, Selector(succeeded.EnumerateValue()));
					default: return result;
				}

			};
			return new SingleParser<TResult>($"{Parser.Description}%",parserDelegate);
		}
		
		
		// Single_Single
		public static ISingleParser<TResult> SelectMany<T, U, TResult>(
		   this ISingleParser<T> Parser,
		   Func<T, ISingleParser<U>> Selector,
		   Func<T, U, TResult> Projector)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			if (Selector == null) throw new ArgumentNullException(nameof(Selector));
			if (Projector == null) throw new ArgumentNullException(nameof(Projector));

			return Parser.Then(t => Selector(t).Select(u => Projector(t,  u  )));
		}

		// Single_Multiple
		public static ISingleParser<TResult> SelectMany<T, U, TResult>(
		   this ISingleParser<T> Parser,
		   Func<T, IMultipleParser<U>> Selector,
		   Func<T, IEnumerable<U>, TResult> Projector)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			if (Selector == null) throw new ArgumentNullException(nameof(Selector));
			if (Projector == null) throw new ArgumentNullException(nameof(Projector));

			return Parser.Then(t => Selector(t).Select(u => Projector(t, u)));
		}

		// Multiple_Multiple
		public static ISingleParser<TResult> SelectMany<T, U, TResult>(
		   this IMultipleParser<T> Parser,
		   Func<IEnumerable<T>, IMultipleParser<U>> Selector,
		   Func<IEnumerable<T>, IEnumerable<U>, TResult> Projector)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			if (Selector == null) throw new ArgumentNullException(nameof(Selector));
			if (Projector == null) throw new ArgumentNullException(nameof(Projector));

			return Parser.Then(t => Selector(t).Select(u => Projector(t, u)));
		}



		// Multiple_Single
		public static ISingleParser<TResult> SelectMany<T, U, TResult>(
		   this IMultipleParser<T> Parser,
		   Func<IEnumerable<T>, ISingleParser<U>> Selector,
		   Func<IEnumerable<T>, U, TResult> Projector)
		{
			if (Parser == null) throw new ArgumentNullException(nameof(Parser));
			if (Selector == null) throw new ArgumentNullException(nameof(Selector));
			if (Projector == null) throw new ArgumentNullException(nameof(Projector));

			return Parser.Then(t => Selector(t).Select(u => Projector(t, u)));
		}




		public static IMultipleParser<T> Then<T>(this IParser<T> A, IParser<T> B)
		{
			if (A == null) throw new ArgumentNullException(nameof(A));
			if (B == null) throw new ArgumentNullException(nameof(B));
			ParserDelegate<T> parserDelegate = (reader, includedChars) =>
			{
				IParseResult result1;
				IParseResult result2;
				ISucceededParseResult<T>? success1, success2;


				result1 = A.TryParse(reader, includedChars);
				success1 = result1 as ISucceededParseResult<T>;
				if (success1==null) return result1;
				
				result2 = B.TryParse(reader, includedChars);
				success2 = result2 as ISucceededParseResult<T>;
				if (success2 == null) return result2;

				return ParseResult.Succeeded(result1.Position, success1.EnumerateValue().Concat(success2.EnumerateValue()) );
			};
			return new MultipleParser<T>($"{A.Description}{B.Description}",parserDelegate);
		}
		
		// Single_Single
		public static ISingleParser<TResult> Then<T, TResult>(this ISingleParser<T> First, Func<T, ISingleParser<TResult>> Second)
		{

			if (First == null) throw new ArgumentNullException(nameof(First));
			if (Second == null) throw new ArgumentNullException(nameof(Second));

			ParserDelegate<TResult> parserDelegate = (reader, includedChars) => {
				IParseResult result1;
				IParseResult result2;
	
				result1 = First.TryParse(reader, includedChars);
				switch (result1)
				{
					case ISucceededParseResult<T> success:
						result2 = Second(success.Value).TryParse(reader, includedChars);
						return result2;// ParseResult.Succeeded(result1.Position, success.EnumerateValue().Concat(success.EnumerateValue()));
					default: return result1;
				}


			};
			return new SingleParser<TResult>($"{First.Description}%",parserDelegate);

		}

		// Multiple_Multiple
		public static ISingleParser<TResult> Then<T, TResult>(this IMultipleParser<T> First, Func<IEnumerable<T>, ISingleParser<TResult>> Second)
		{

			if (First == null) throw new ArgumentNullException(nameof(First));
			if (Second == null) throw new ArgumentNullException(nameof(Second));

			ParserDelegate<TResult> parserDelegate = (reader, includedChars) => {
				IParseResult result1;
				IParseResult result2;

				result1 = First.TryParse(reader, includedChars);
				switch (result1)
				{
					case ISucceededParseResult<T> success:
						result2 = Second(success.EnumerateValue()).TryParse(reader, includedChars);
						return result2;// ParseResult.Succeeded(result1.Position, success.EnumerateValue().Concat(success.EnumerateValue()));
					default: return result1;
				}


			};
			return new SingleParser<TResult>($"{First.Description}%",parserDelegate);

		}
		

		public static ISingleParser<string> ToStringParser<T>(this IParser<T> A)
		{
			string value;

			if (A == null) throw new ArgumentNullException(nameof(A));

			ParserDelegate<string> parserDelegate = (reader, includedChars) =>
			{
				IParseResult resultA;

				resultA = A.TryParse(reader, includedChars);
				switch (resultA)
				{
					case ISucceededParseResult<T> success:
						value = string.Concat(success.EnumerateValue());
						return ParseResult.Succeeded(resultA.Position,value);
					default: return resultA;
				}

			};
			return new SingleParser<string>(A.Description,parserDelegate);
		}
		
		#endregion


















	}
}
