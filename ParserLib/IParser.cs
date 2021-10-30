using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
    public delegate IParseResult<T> ParserDelegate<T>(IReader Reader);

    public interface IParser<out T> 
    {

		T Parse(string Value,params char[] IgnoredChars);

		T Parse(IReader Reader);

		IParseResult<T> TryParse(string Value, params char[] IgnoredChars);

		IParseResult<T> TryParse(IReader Reader);

	}


}
