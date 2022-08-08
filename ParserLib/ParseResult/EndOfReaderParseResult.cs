using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public class EndOfReaderParseResult : FailedParseResult, IEndOfReaderParseResult
	{
	

		private Exception exception;
		public override Exception Exception
		{
			get => exception;
		}

		public EndOfReaderParseResult(long Position):base(Position)
		{
			this.exception = new EndOfReaderException();
		}

		


	}
}
