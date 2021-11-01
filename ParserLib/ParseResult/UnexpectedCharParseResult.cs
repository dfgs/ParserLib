using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public class UnexpectedCharParseResult<T> : FailedParseResult<T>, IUnexpectedCharParseResult<T>
	{
		


		private Exception exception;
		public override Exception Exception
		{
			get => exception;
		}

		private char input;
		public char Input
		{
			get => input;
		}
		public UnexpectedCharParseResult(char Input)
		{
			this.input = Input;
			this.exception = new UnexpectedCharException(Input);
		}

					
		


	}
}
