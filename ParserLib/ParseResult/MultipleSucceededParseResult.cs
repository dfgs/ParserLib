using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public class MultipleSucceededParseResult<T>:SucceededParseResult<T>
	{
		private IEnumerable<T> values;
		public override T Value => values.First();

		public MultipleSucceededParseResult(long Position, IEnumerable<T> Values):base(Position)
		{
			this.values = Values;
		}
		public override IEnumerable<T> EnumerateValue()
		{
			return values;
		}


	}
}
