using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public class SucceededParseResult<T>:ParseResult, ISucceededParseResult<T>
	{
		/*public override bool IsSuccess
		{
			get => true;
		}//*/
		
		private T value;
		public T Value
		{
			get => value;
		}

		
		public SucceededParseResult(long Position,T Value):base(Position)
		{
			this.value = Value;
		}


		



	}
}
