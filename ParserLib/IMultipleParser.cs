using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{

	public interface IMultipleParser<out T>:IParser<T>
	{

		IEnumerable<T> Parse(string Value, params char[] IgnoredChars);

		IEnumerable<T> Parse(IReader Reader, params char[] IncludedChars);

		

	}


}
