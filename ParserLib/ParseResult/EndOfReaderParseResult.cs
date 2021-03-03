using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public class EndOfReaderParseResult<T> : FailedParseResult<T>
	{
	

		private Exception exception;
		public override Exception Exception
		{
			get => exception;
		}

		public EndOfReaderParseResult()
		{
			this.exception = new EndOfReaderException();
		}

					
		


	}
}
