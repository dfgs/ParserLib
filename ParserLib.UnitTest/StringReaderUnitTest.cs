using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class StringReaderUnitTest
	{
		[TestMethod]
		public void ShouldCheckConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new StringReader(null));
		}
		[TestMethod]
		public void ShouldReturnEOFIsTrueWhenValueIsEmpty()
		{
			StringReader reader;

			reader=  new StringReader("");
			Assert.IsTrue(reader.EOF);
		}
		[TestMethod]
		public void ShouldReturnEOFIsFalseWhenValueIsNotEmpty()
		{
			StringReader reader;

			reader = new StringReader("a");
			Assert.IsFalse(reader.EOF);
		}

		
		[TestMethod]
		public void ShouldRead()
		{
			StringReader reader;
			char value;
			bool result;

			reader = new StringReader("abc");

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
			StringReader reader;
			char value;
			bool result;

			reader = new StringReader("a b c ",' ');

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
			StringReader reader;
			char value;
			bool result;

			reader = new StringReader("a b c ", ' ');

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
			StringReader reader;
			char value;
				
			reader = new StringReader("abc");
			Assert.IsTrue(reader.Read(out value));
			Assert.IsTrue(reader.Read(out value));
			Assert.IsTrue(reader.Read(out value));
			Assert.IsTrue(reader.EOF);
			Assert.IsFalse(reader.Read(out value));
		}

		[TestMethod]
		public void ShouldSeek()
		{
			StringReader reader;

			reader = new StringReader("abc");
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
			StringReader reader;

			reader = new StringReader("abc");
			Assert.ThrowsException<IndexOutOfRangeException>(() => reader.Seek(-1));
			Assert.ThrowsException<IndexOutOfRangeException>(() => reader.Seek(10));
		}


	}
}
