using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public abstract class ParseResult :IParseResult
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
		public static SucceededParseResult<T> Succeeded<T>(long Position,T Value)
		{
			//if (Value == null) throw new ArgumentNullException(nameof(Value));
			return new SucceededParseResult<T>(Position,Value);
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
