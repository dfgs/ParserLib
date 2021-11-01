using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public interface IFailedParseResult<out T> :IParseResult<T>
	{
		Exception Exception
		{
			get;
		}





	}
}
