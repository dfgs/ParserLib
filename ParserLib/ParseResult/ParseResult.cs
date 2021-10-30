using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public abstract class ParseResult<T> :IParseResult<T>
	{
		public abstract bool IsSuccess
		{
			get;
		}
	
		public abstract T Value
		{
			get;
		}
				
		public static SucceededParseResult<T> Succeeded(T Value)
		{
			//if (Value == null) throw new ArgumentNullException(nameof(Value));
			return new SucceededParseResult<T>(Value);
		}
		public static UnexpectedCharParseResult<T> Failed(char Input)
		{
			return new UnexpectedCharParseResult<T>(Input);
		}
		public static EndOfReaderParseResult<T> EndOfReader()
		{
			return new EndOfReaderParseResult<T>();
		}
		
		


	}
}
