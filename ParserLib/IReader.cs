using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public interface IReader
	{
		bool EOF
		{
			get;
		}

		long Position
		{
			get;
		}

		bool Read(out char Result);

		void Seek(long Position);
	}
}
