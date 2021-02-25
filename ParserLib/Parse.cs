using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public static class Parse
	{
		public static Parser<char> Char(char Value)
		{
			return (reader) =>{
				return ParseResult<char>.Success(Value);
			};
		}

	}
}
