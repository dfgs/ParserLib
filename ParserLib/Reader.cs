using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserLib
{
	public class Reader : IReader
	{
		private char[] value;
		private int index;

		public bool EOF => index >= value.Length;

		public Reader(IEnumerable<char> Value)
		{
			if (Value == null) throw new ArgumentNullException(nameof(Value));
			this.value = Value.ToArray();
			index = 0;
		}

		public char Peek()
		{
			if (EOF) throw new EndOfReaderException();
			return value[index];
		}

		public char Pop()
		{
			if (EOF) throw new EndOfReaderException();
			return value[index++];
		}
	}
}
