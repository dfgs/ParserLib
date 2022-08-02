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
			return Parse(new StringReader(Value,IgnoredChars));
		}
		public T Parse(IReader Reader, params char[] IncludedChars)
		{
			IParseResult result;
			long position;

			if (Reader == null) throw new ArgumentNullException(nameof(Reader));

			position = Reader.Position;
			result = parserDelegate(Reader,IncludedChars);
			
			if (result is ISucceededParseResult<T> success) return success.Value;

			Reader.Seek(position);
			throw ((IFailedParseResult)result).Exception;
		}
		public IParseResult TryParse(string Value, params char[] IgnoredChars)
		{
			if (Value == null) throw new ArgumentNullException(nameof(Value));
			return TryParse(new StringReader(Value,IgnoredChars));
		}
		public IParseResult TryParse(IReader Reader, params char[] IncludedChars)
		{
			IParseResult result;
			long position;

			if (Reader == null) throw new ArgumentNullException(nameof(Reader));

			position = Reader.Position;
			result = parserDelegate(Reader, IncludedChars);
			if (!(result is ISucceededParseResult<T>)) Reader.Seek(position);

			return result;
		}


	}


}
