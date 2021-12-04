using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public interface IParseResult<out T> 
	{
		bool IsSuccess
		{
			get;
		}
	
		T Value
		{
			get;
		}
				
		long Position
		{
			get;
		}
		
		


	}
}
