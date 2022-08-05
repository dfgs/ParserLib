using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public abstract class ParseResult<T> : IParseResult<T>
	{
		/*public abstract bool IsSuccess
		{
			get;
		}*/
		private long position;
		public long Position
		{
			get => position;
		}
		
			
		public ParseResult(long Position)
		{
			this.position = Position;
		}
		public static SingleSucceededParseResult<T> Succeeded(long Position, T Value)
		{
			return new SingleSucceededParseResult<T>(Position, Value);
		}
		public static MultipleSucceededParseResult<T> Succeeded(long Position, IEnumerable<T> Value)
		{
			return new MultipleSucceededParseResult<T>(Position, Value);
		}
		public static UnexpectedCharParseResult<T> Failed(long Position, char Input)
		{
			return new UnexpectedCharParseResult<T>(Position,Input);
		}
		public static EndOfReaderParseResult<T> EndOfReader(long Position)
		{
			return new EndOfReaderParseResult<T>(Position);
		}

	}
}
