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

		private long position;
		public long Position
		{
			get => position;
		}
		public UnexpectedCharParseResult(char Input,long Position)
		{
			this.input = Input;this.position = Position;
			this.exception = new UnexpectedCharException(Input,Position);
		}

					
		


	}
}
