using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public class EndOfReaderException:ParseException
	{
		public EndOfReaderException() : base($"End of reader is reached")
		{

		}
	}
}
