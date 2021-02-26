using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
    public delegate ParseResult<T> ParserDelegate<T>(IReader Reader);

    public class Parser<T> 
    {

		private ParserDelegate<T> parserDelegate;

		public Parser(ParserDelegate<T> ParserDelegate)
		{
			this.parserDelegate = ParserDelegate;
		}

		public T Parse(string Value)
		{
			if (Value == null) throw new ArgumentNullException(nameof(Value));
			return Parse(new Reader(Value));
		}
		public T Parse(IReader Reader)
		{
			ParseResult<T> result;
			long position;

			if (Reader == null) throw new ArgumentNullException(nameof(Reader));

			position = Reader.Position;
			result = parserDelegate(Reader);
			if (result.IsSuccess) return result.Value;
			Reader.Seek(position);
			throw new UnexpectedCharException(result.Input);
		}
		public ParseResult<T> TryParse(string Value)
		{
			if (Value == null) throw new ArgumentNullException(nameof(Value));
			return TryParse(new Reader(Value));
		}
		public ParseResult<T> TryParse(IReader Reader)
		{
			ParseResult<T> result;
			long position;

			if (Reader == null) throw new ArgumentNullException(nameof(Reader));

			position = Reader.Position;
			result = parserDelegate(Reader);
			if (!result.IsSuccess) Reader.Seek(position);

			return result;
		}


	}


}
