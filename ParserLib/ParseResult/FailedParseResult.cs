using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public abstract class FailedParseResult<T> : ParseResult<T>, IFailedParseResult<T>
	{
		public override bool IsSuccess
		{
			get => false;
		}

		public override T Value => default(T);

		public abstract Exception Exception
		{
			get;
		}

		public FailedParseResult(long Position):base(Position)
		{
		}

					
		


	}
}
