﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public class UnexpectedCharException:ParseException	
	{
		public UnexpectedCharException(char Current) : base($"Unexpected char {Current}")
		{

		}
	}
}
