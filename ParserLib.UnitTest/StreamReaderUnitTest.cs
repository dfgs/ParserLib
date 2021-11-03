using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class StreamReaderUnitTest
	{
		[TestMethod]
		public void ShouldCheckConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new StreamReader(null));
		}
		[TestMethod]
		public void ShouldReturnEOFIsTrueWhenValueIsEmpty()
		{
			StreamReader reader;

			reader=  new StreamReader(new System.IO.MemoryStream(Encoding.Default.GetBytes("")));
			Assert.IsTrue(reader.EOF);
		}
		[TestMethod]
		public void ShouldReturnEOFIsFalseWhenValueIsNotEmpty()
		{
			StreamReader reader;

			reader = new StreamReader(new System.IO.MemoryStream(Encoding.Default.GetBytes("a")));
			Assert.IsFalse(reader.EOF);
		}

		
		[TestMethod]
		public void ShouldRead()
		{
			StreamReader reader;
			char value;
			bool result;

			reader = new StreamReader(new System.IO.MemoryStream(Encoding.Default.GetBytes("abc")));

			result = reader.Read(out value);
			Assert.IsTrue(result);
			Assert.AreEqual('a', value);
			result = reader.Read(out value);
			Assert.IsTrue(result);
			Assert.AreEqual('b', value);
			result = reader.Read(out value);
			Assert.IsTrue(result);
			Assert.AreEqual('c', value);

			Assert.IsTrue(reader.EOF);
			result = reader.Read(out value);
			Assert.IsFalse(result);
		}
		[TestMethod]
		public void ShouldIgnoreChars()
		{
			StreamReader reader;
			char value;
			bool result;

			reader = new StreamReader(new System.IO.MemoryStream(Encoding.Default.GetBytes("a b c ")),' ');

			result = reader.Read(out value);
			Assert.IsTrue(result);
			Assert.AreEqual('a', value);
			result = reader.Read(out value);
			Assert.IsTrue(result);
			Assert.AreEqual('b', value);
			result = reader.Read(out value);
			Assert.IsTrue(result);
			Assert.AreEqual('c', value);

			Assert.IsFalse(reader.EOF);
			result = reader.Read(out value);
			Assert.IsFalse(result);
			Assert.IsTrue(reader.EOF);
		}
		[TestMethod]
		public void ShouldNotIgnoreChars()
		{
			StreamReader reader;
			char value;
			bool result;

			reader = new StreamReader(new System.IO.MemoryStream(Encoding.Default.GetBytes("a b c ")), ' ');

			result = reader.Read(out value);
			Assert.IsTrue(result);
			Assert.AreEqual('a', value);
			result = reader.Read(out value, ' ');
			Assert.IsTrue(result);
			Assert.AreEqual(' ', value);
			result = reader.Read(out value);
			Assert.IsTrue(result);
			Assert.AreEqual('b', value);
			result = reader.Read(out value, ' ');
			Assert.IsTrue(result);
			Assert.AreEqual(' ', value);
			result = reader.Read(out value);
			Assert.IsTrue(result);
			Assert.AreEqual('c', value);
			result = reader.Read(out value, ' ');
			Assert.IsTrue(result);
			Assert.AreEqual(' ', value);

			Assert.IsTrue(reader.EOF);
		}
		[TestMethod]
		public void ShouldNotReadWhenEOF()
		{
			StreamReader reader;
			char value;

			reader = new StreamReader(new System.IO.MemoryStream(Encoding.Default.GetBytes("abc")));
			Assert.IsTrue(reader.Read(out value));
			Assert.IsTrue(reader.Read(out value));
			Assert.IsTrue(reader.Read(out value));
			Assert.IsTrue(reader.EOF);
			Assert.IsFalse(reader.Read(out value));
		}

		[TestMethod]
		public void ShouldSeek()
		{
			StreamReader reader;

			reader = new StreamReader(new System.IO.MemoryStream(Encoding.Default.GetBytes("abc")) );
			reader.Seek(1);
			Assert.AreEqual(1, reader.Position);
			Assert.IsFalse(reader.EOF);
			reader.Seek(2);
			Assert.AreEqual(2, reader.Position);
			Assert.IsFalse(reader.EOF);
			reader.Seek(0);
			Assert.AreEqual(0, reader.Position);
			Assert.IsFalse(reader.EOF);
			reader.Seek(3);
			Assert.AreEqual(3, reader.Position);
			Assert.IsTrue(reader.EOF);
		}
		[TestMethod]
		public void ShouldNotSeek()
		{
			StreamReader reader;

			reader = new StreamReader(new System.IO.MemoryStream(Encoding.Default.GetBytes("abc")));
			Assert.ThrowsException< System.IO.IOException> (() => reader.Seek(-1));
			Assert.ThrowsException<System.IO.IOException>(() => reader.Seek(10));
		}


	}
}
