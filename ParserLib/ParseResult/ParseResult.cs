using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public abstract class ParseResult : IParseResult
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
		public static SingleSucceededParseResult<T> Succeeded<T>(long Position, T Value)
		{
			return new SingleSucceededParseResult<T>(Position, Value);
		}
		public static MultipleSucceededParseResult<T> Succeeded<T>(long Position, IEnumerable<T> Value)
		{
			return new MultipleSucceededParseResult<T>(Position, Value);
		}
		public static UnexpectedCharParseResult Failed(long Position, char Input)
		{
			return new UnexpectedCharParseResult(Position,Input);
		}
		public static EndOfReaderParseResult EndOfReader(long Position)
		{
			return new EndOfReaderParseResult(Position);
		}

	}
}
