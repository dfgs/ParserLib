using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public interface IParseResult<T>
	{
		string Message
		{
			get;
		}
		T Value
		{
			get;
		}
	}
}
