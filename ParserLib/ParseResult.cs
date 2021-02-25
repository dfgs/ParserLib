using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public sealed class ParseResult<T> : IParseResult<T>
	{
		
		public string Message
		{
			get;
			private set;
		}

		public T Value
		{
			get;
			private set;
		}

		private ParseResult(T Value,string Message)
		{
			this.Value = Value;this.Message = Message;
		}

		public static ParseResult<T> Success(T Value)
		{
			return new ParseResult<T>(Value, "Success");
		}
		public static ParseResult<T> Failure(string Message)
		{
			return new ParseResult<T>(default(T), Message);
		}


	}
}
