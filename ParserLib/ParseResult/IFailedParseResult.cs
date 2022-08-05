using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public interface IFailedParseResult<T> :IParseResult<T>
	{
		Exception Exception
		{
			get;
		}

		public IFailedParseResult<U> Cast<U>();



	}
}
