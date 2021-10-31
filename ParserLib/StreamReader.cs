using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ParserLib
{
	public class StreamReader : IReader
	{
		private Stream stream;
		private char[] ignoredChars;
		
		public long Position
		{
			get => stream.Position;
		}

		public bool EOF => stream.Position >= stream.Length;

		public StreamReader(Stream Stream,params char[] IgnoredChars)
		{
			if (Stream == null) throw new ArgumentNullException(nameof(Stream));
			this.stream = Stream;
			ignoredChars = IgnoredChars;
		}
			

		public bool Read(out char Value)
		{
			Value=(char)0;

			while (true)
			{
				if (EOF) return false;
				Value = (char)stream.ReadByte();
				if (ignoredChars.Contains(Value)) continue;
				return true;
			}
		}

		public void Seek(long Position)
		{
			if (Position >= stream.Position) throw new IOException();
			stream.Seek(Position, SeekOrigin.Begin);
		}

	}
}
