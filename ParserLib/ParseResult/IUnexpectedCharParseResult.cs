﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public interface IUnexpectedCharParseResult<out T>:IFailedParseResult<T>
	{
		char Input
		{
			get;
		}






	}
}