﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public abstract class ParseException:Exception
	{
		public ParseException(string Message):base(Message)
		{

		}
	}
}
