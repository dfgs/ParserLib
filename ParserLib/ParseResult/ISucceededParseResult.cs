using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public interface ISucceededParseResult : IParseResult
	{
	}

	public interface ISucceededParseResult<out T> : ISucceededParseResult
	{
		T Value
		{
			get;
		}






	}
}
