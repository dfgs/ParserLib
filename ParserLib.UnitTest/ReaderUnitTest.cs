using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ReaderUnitTest
	{
		[TestMethod]
		public void ShouldCheckConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new Reader(null));
		}
		[TestMethod]
		public void ShouldReturnEOFIsTrueWhenValueIsEmpty()
		{
			Reader reader;

			reader=  new Reader("");
			Assert.IsTrue(reader.EOF);
		}
		[TestMethod]
		public void ShouldReturnEOFIsFalseWhenValueIsNotEmpty()
		{
			Reader reader;

			reader = new Reader("a");
			Assert.IsFalse(reader.EOF);
		}

		
		[TestMethod]
		public void ShouldRead()
		{
			Reader reader;
			char value;
			bool result;

			reader = new Reader("abc");

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
			Reader reader;
			char value;
			bool result;

			reader = new Reader("a b c ",' ');

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
		public void ShouldNotReadWhenEOF()
		{
			Reader reader;
			char value;
				
			reader = new Reader("abc");
			Assert.IsTrue(reader.Read(out value));
			Assert.IsTrue(reader.Read(out value));
			Assert.IsTrue(reader.Read(out value));
			Assert.IsTrue(reader.EOF);
			Assert.IsFalse(reader.Read(out value));
		}

		[TestMethod]
		public void ShouldSeek()
		{
			Reader reader;

			reader = new Reader("abc");
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
			Reader reader;

			reader = new Reader("abc");
			Assert.ThrowsException<IndexOutOfRangeException>(() => reader.Seek(-1));
			Assert.ThrowsException<IndexOutOfRangeException>(() => reader.Seek(10));
		}


	}
}
