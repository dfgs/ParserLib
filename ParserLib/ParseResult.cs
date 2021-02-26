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
		private char input;
		public char Input
		{
			get => input;
		}
		private T value;
		public T Value
		{
			get => value;
		}

		private ParseResult(bool IsSuccess,char Input,T Value)
		{
			this.isSuccess = IsSuccess;
			this.input = Input;
			this.value = Value;
		}
		
		public static ParseResult<T> Succeded(char Input, T Value)
		{
			if (Value == null) throw new ArgumentNullException(nameof(Value));
			return new ParseResult<T>(true, Input,Value);
		}
		public static ParseResult<T> Failed(char Input)
		{
			//if (Value == null) throw new ArgumentNullException(nameof(Value));
			return new ParseResult<T>(false, Input, default(T));
		}
		

	}
}
