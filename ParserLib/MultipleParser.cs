using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
 
    public class MultipleParser<T>:Parser<T>,IMultipleParser<T>
    {


		public MultipleParser(ParserDelegate<T> ParserDelegate):base(ParserDelegate)
		{
		}

	

		public IEnumerable<T> Parse(string Value, params char[] IgnoredChars)
		{
			if (Value == null) throw new ArgumentNullException(nameof(Value));
			return Parse(new StringReader(Value, IgnoredChars));
		}
		public IEnumerable<T> Parse(IReader Reader, params char[] IncludedChars)
		{
			IParseResult<T> result;
			long position;

			if (Reader == null) throw new ArgumentNullException(nameof(Reader));

			position = Reader.Position;
			result = parserDelegate(Reader, IncludedChars);

			if (result is ISucceededParseResult<T> success) return success.EnumerateValue();

			Reader.Seek(position);
			throw ((IFailedParseResult<T>)result).Exception;
		}

		


	}


}
