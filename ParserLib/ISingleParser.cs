using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{

	public interface ISingleParser<out T>:IParser<T>
	{

		T Parse(string Value, params char[] IgnoredChars);

		T Parse(IReader Reader, params char[] IncludedChars);
	

		

	}


}
