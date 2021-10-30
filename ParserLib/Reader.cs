﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserLib
{
	public class Reader : IReader
	{
		private char[] value;

		private char[] ignoredChars;
		private long position;
		public long Position
		{
			get => position;
		}

		public bool EOF => position >= value.Length;

		public Reader(IEnumerable<char> Value,params char[] IgnoredChars)
		{
			if (Value == null) throw new ArgumentNullException(nameof(Value));
			ignoredChars = IgnoredChars;
			this.value = Value.ToArray();
			position = 0;
		}
			

		public bool Read(out char Value)
		{
			Value=(char)0;

			while (true)
			{
				if (EOF) return false;
				Value= value[position++];
				if (ignoredChars.Contains(Value)) continue;
				return true;
			}
		}

		public void Seek(long Position)
		{
			if ((Position<0) || (Position>value.Length)) throw new IndexOutOfRangeException();
			this.position = Position;
		}

	}
}
