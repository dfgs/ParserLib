using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public static class Parse
	{
		public static Parser<string> Char(char Value)
		{
			return (reader) => {
				char input;
				input = reader.Peek();
				if ( input == Value) return ParseResult<string>.Succeded( input, reader.Pop().ToString());
				else return ParseResult<string>.Failed(input, null);
			};
		}
		public static Parser<string> Any()
		{
			return (reader) => {
				char input;
				input = reader.Peek();
				return ParseResult<string>.Succeded(input, reader.Pop().ToString());
			};
		}
	}
}
