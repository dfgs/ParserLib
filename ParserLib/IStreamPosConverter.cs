using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public interface IStreamPosConverter
	{
		bool TryGetLineAndColumn(Stream Stream,long Position,out int Line,out int Column);
	}
}
