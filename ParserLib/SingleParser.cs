using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
 
    public class SingleParser<T>:Parser<T>,ISingleParser<T>
    {


		public SingleParser(ParserDelegate<T> ParserDelegate):base(ParserDelegate)
		{
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

		


	}


}
