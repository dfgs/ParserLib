using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
    public delegate IParseResult ParserDelegate<T>(IReader Reader,char[] IncludedChars);

	public interface IParser<out T>
	{
		string Description
		{
			get;
		}
		IParseResult TryParse(string Value, params char[] IgnoredChars);

		IParseResult TryParse(IReader Reader, params char[] IncludedChars);

	}


}
