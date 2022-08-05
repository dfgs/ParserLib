using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public interface ISucceededParseResult<out T> : IParseResult<T>
	{
		T Value
		{
			get;
		}

		IEnumerable<T> EnumerateValue();


	}
	

	
	

}
