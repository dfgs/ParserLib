using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public class SucceededParseResult<T>:ParseResult<T>
	{
		public override bool IsSuccess
		{
			get => true;
		}
		
		private T value;
		public override T Value
		{
			get => value;
		}

		
		public SucceededParseResult(T Value)
		{
			this.value = Value;
		}

		

				
		

	}
}
