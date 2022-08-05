using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
	public class StreamPosConverter:IStreamPosConverter
	{
		private int bufferSize;

		public StreamPosConverter(int BufferSize)
		{
			if (BufferSize <= 0) throw new ArgumentOutOfRangeException(nameof(BufferSize));
			this.bufferSize = BufferSize;
		}

		public bool TryGetLineAndColumn(Stream Stream,long Position, out int Line, out int Column)
		{
			long currentPos;
			int currentLine;
			int currentColumn;
			byte[] buffer;
			int count;

			if (Stream == null) throw new ArgumentNullException(nameof(Stream));

			buffer = new byte[bufferSize];
			Line = 0;Column = 0;
			currentLine = 1; currentColumn = 1;
			 currentPos = 0;
			try
			{
				Stream.Seek(0, SeekOrigin.Begin);
				
				while (currentPos < Stream.Length)
				{

					count=Stream.Read(buffer, 0, bufferSize);
					for(int t=0;t<count;t++)
					{
						if (currentPos == Position)
						{
							Column = currentColumn;
							Line = currentLine;
							return true;
						}

						if (buffer[currentPos] == 10)
						{
							currentLine++;
							currentColumn = 1;
						}
						else
						{
							currentColumn++;
						}
						currentPos++;
					}
					
				}

				return false;

			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
