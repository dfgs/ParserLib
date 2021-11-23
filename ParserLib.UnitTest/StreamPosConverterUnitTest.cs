using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class StreamPosConverterUnitTest
	{
		[TestMethod]
		public void ShouldCheckConstructor()
		{
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => new StreamPosConverter(0));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => new StreamPosConverter(-1));
		}

		[TestMethod]
		public void ShouldCheckParameters()
		{
			StreamPosConverter streamPosConverter;
			int line, column;


			streamPosConverter = new StreamPosConverter(1024);
			Assert.ThrowsException<ArgumentNullException>(() => streamPosConverter.TryGetLineAndColumn(null,0,out line,out column));
		}

	
		
		[TestMethod]
		public void ShouldReturnLineAndColumnAtPosition0()
		{
			MemoryStream stream;
			StreamPosConverter streamPosConverter;
			int line, column;
			bool result;

			stream = new MemoryStream(Encoding.Default.GetBytes("012\r\n345\r\n678"));
			streamPosConverter = new StreamPosConverter(1024);
			result = streamPosConverter.TryGetLineAndColumn(stream, 0, out line, out column);
			Assert.IsTrue(result);
			Assert.AreEqual(1, line);
			Assert.AreEqual(1, column);
		}
		[TestMethod]
		public void ShouldReturnLineAndColumnAtPosition1()
		{
			MemoryStream stream;
			StreamPosConverter streamPosConverter;
			int line, column;
			bool result;

			stream = new MemoryStream(Encoding.Default.GetBytes("012\r\n345\r\n678"));
			streamPosConverter = new StreamPosConverter(1024);
			result = streamPosConverter.TryGetLineAndColumn(stream, 1, out line, out column);
			Assert.IsTrue(result);
			Assert.AreEqual(1, line);
			Assert.AreEqual(2, column);
		}
		[TestMethod]
		public void ShouldReturnLineAndColumnAtPosition6()
		{
			MemoryStream stream;
			StreamPosConverter streamPosConverter;
			int line, column;
			bool result;

			stream = new MemoryStream(Encoding.Default.GetBytes("012\r\n567\r\nABC"));
			streamPosConverter = new StreamPosConverter(1024);
			result = streamPosConverter.TryGetLineAndColumn(stream, 6, out line, out column);
			Assert.IsTrue(result);
			Assert.AreEqual(2, line);
			Assert.AreEqual(2, column);
		}
		[TestMethod]
		public void ShouldReturnLineAndColumnAtPosition12()
		{
			MemoryStream stream;
			StreamPosConverter streamPosConverter;
			int line, column;
			bool result;

			stream = new MemoryStream(Encoding.Default.GetBytes("012\r\n567\r\nABC"));
			streamPosConverter = new StreamPosConverter(1024);
			result = streamPosConverter.TryGetLineAndColumn(stream, 12, out line, out column);
			Assert.IsTrue(result);
			Assert.AreEqual(3, line);
			Assert.AreEqual(3, column);
		}
		[TestMethod]
		public void ShouldNotReturnLineAndColumnAtPosition13()
		{
			MemoryStream stream;
			StreamPosConverter streamPosConverter;
			int line, column;
			bool result;

			stream = new MemoryStream(Encoding.Default.GetBytes("012\r\n567\r\nABC"));
			streamPosConverter = new StreamPosConverter(1024);
			result = streamPosConverter.TryGetLineAndColumn(stream, 13, out line, out column);
			Assert.IsFalse(result);
			Assert.AreEqual(0, line);
			Assert.AreEqual(0, column);
		}
	}
}
