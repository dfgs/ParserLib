using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public class SingleSucceededParseResult<T>:SucceededParseResult<T>
	{
		private T value;
		override public T Value => value;

		public SingleSucceededParseResult(long Position,T Value):base(Position)
		{
			this.value = Value;
		}
		public override IEnumerable<T> EnumerateValue()
		{
			yield return Value;
		}


	}
}
