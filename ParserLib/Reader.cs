﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserLib
{
	public class Reader : IReader
	{
		private char[] value;
		private long position;
		public long Position
		{
			get => position;
		}

		public bool EOF => position >= value.Length;

		public Reader(IEnumerable<char> Value)
		{
			if (Value == null) throw new ArgumentNullException(nameof(Value));
			this.value = Value.ToArray();
			position = 0;
		}

	

		public char Read()
		{
			if (EOF) throw new IndexOutOfRangeException();
			return value[position++];
		}

		public void Seek(long Position)
		{
			if ((Position<0) || (Position>value.Length)) throw new IndexOutOfRangeException();
			this.position = Position;
		}

	}
}
