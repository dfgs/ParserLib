using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public abstract class SucceededParseResult<T>:ParseResult<T>, ISucceededParseResult<T>
	{
		public abstract T Value { get; }


		public SucceededParseResult(long Position):base(Position)
		{
			
		}


		public abstract IEnumerable<T> EnumerateValue();

	}
}
