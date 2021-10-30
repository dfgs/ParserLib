using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
 
    public class Parser<T>:IParser<T>
    {

		private ParserDelegate<T> parserDelegate;

		public Parser(ParserDelegate<T> ParserDelegate)
		{
			this.parserDelegate = ParserDelegate;
		}

		public T Parse(string Value,params char[] IgnoredChars)
		{
			if (Value == null) throw new ArgumentNullException(nameof(Value));
			return Parse(new Reader(Value,IgnoredChars));
		}
		public T Parse(IReader Reader)
		{
			IParseResult<T> result;
			long position;

			if (Reader == null) throw new ArgumentNullException(nameof(Reader));

			position = Reader.Position;
			result = parserDelegate(Reader);
			if (result.IsSuccess) return result.Value;
			Reader.Seek(position);
			throw ((FailedParseResult<T>)result).Exception;
		}
		public IParseResult<T> TryParse(string Value, params char[] IgnoredChars)
		{
			if (Value == null) throw new ArgumentNullException(nameof(Value));
			return TryParse(new Reader(Value,IgnoredChars));
		}
		public IParseResult<T> TryParse(IReader Reader)
		{
			IParseResult<T> result;
			long position;

			if (Reader == null) throw new ArgumentNullException(nameof(Reader));

			position = Reader.Position;
			result = parserDelegate(Reader);
			if (!result.IsSuccess) Reader.Seek(position);

			return result;
		}


	}


}
