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
		public void ShouldPeek()
		{
			Reader reader;

			reader = new Reader("abc");
			Assert.AreEqual('a', reader.Peek());
			Assert.AreEqual('a', reader.Peek());
			Assert.AreEqual('a', reader.Peek());
			Assert.IsFalse(reader.EOF);
		}
		[TestMethod]
		public void ShouldPop()
		{
			Reader reader;

			reader = new Reader("abc");
			Assert.AreEqual('a', reader.Pop());
			Assert.AreEqual('b', reader.Pop());
			Assert.AreEqual('c', reader.Pop());
			Assert.IsTrue(reader.EOF);
		}

		[TestMethod]
		public void ShouldNotPeekWhenEOF()
		{
			Reader reader;

			reader = new Reader("abc");
			Assert.AreEqual('a', reader.Pop());
			Assert.AreEqual('b', reader.Pop());
			Assert.AreEqual('c', reader.Pop());
			Assert.IsTrue(reader.EOF);
			Assert.ThrowsException<EndOfReaderException>(() => reader.Peek());
		}
		[TestMethod]
		public void ShouldNotPopWhenEOF()
		{
			Reader reader;

			reader = new Reader("abc");
			Assert.AreEqual('a', reader.Pop());
			Assert.AreEqual('b', reader.Pop());
			Assert.AreEqual('c', reader.Pop());
			Assert.IsTrue(reader.EOF);
			Assert.ThrowsException<EndOfReaderException>(() => reader.Pop());
		}

	}
}
