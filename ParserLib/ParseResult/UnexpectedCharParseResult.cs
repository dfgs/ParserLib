﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public class UnexpectedCharParseResult : FailedParseResult, IUnexpectedCharParseResult
	{
		

		private Exception exception;
		public override Exception Exception
		{
			get => exception;
		}

		private char input;
		public char Input
		{
			get => input;
		}

		
		public UnexpectedCharParseResult(long Position,char Input):base(Position)
		{
			this.input = Input;
			this.exception = new UnexpectedCharException(Input,Position);
		}

		



	}
}
