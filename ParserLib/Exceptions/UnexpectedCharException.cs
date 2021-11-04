using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public class UnexpectedCharException:ParseException	
	{
		public char Current
		{
			get;
			
		}
		public long Position
		{
			get;
		}
		public UnexpectedCharException(char Current,long Position) : base($"Unexpected char {Current} at position {Position}")
		{
			this.Current = Current;this.Position = Position;
		}
	}
}
