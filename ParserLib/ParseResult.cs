using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public sealed class ParseResult<T> 
	{
		private bool isSuccess;
		public bool IsSuccess
		{
			get => isSuccess;
		}
		
		private T value;
		public T Value
		{
			get => value;
		}

		private Exception exception;
		public Exception Exception
		{
			get => exception;
		}
		private ParseResult(bool IsSuccess,T Value, Exception Exception)
		{
			this.isSuccess = IsSuccess;
			this.value = Value;
			this.exception = Exception;
		}

		

				
		public static ParseResult<T> Succeded( T Value)
		{
			if (Value == null) throw new ArgumentNullException(nameof(Value));
			return new ParseResult<T>(true, Value,null);
		}
		public static ParseResult<T> Failed(char Input)
		{
			return new ParseResult<T>(false, default(T), new UnexpectedCharException(Input));
		}
		public static ParseResult<T> Failed<U>(ParseResult<U> Model)
		{
			return new ParseResult<T>(Model.IsSuccess, default(T), Model.Exception);
		}
		public static ParseResult<T> EndOfReader()
		{
			return new ParseResult<T>(false, default(T), new EndOfReaderException());
		}


	}
}
