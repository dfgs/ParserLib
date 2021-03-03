using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseResultUnitTest
	{
		[TestMethod]
		public void ShouldCheckConstructorParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => ParseResult<string>.Succeeded( null));
		}
		/*[TestMethod]
		public void ShouldPopReaderWhenSuccess()
		{
			Reader reader;
			ParseResult<char> result;

			reader = new Reader("a");
			Assert.IsFalse(reader.EOF);
			result=ParseResult<char>.Succeeded(reader);
			Assert.IsTrue(result.Success);
			Assert.AreEqual('a', result.Value);
			Assert.IsTrue(reader.EOF);
		}
		[TestMethod]
		public void ShouldPeekReaderWhenFails()
		{
			Reader reader;
			ParseResult<char> result;

			reader = new Reader("a");
			Assert.IsFalse(reader.EOF);
			result = ParseResult<char>.Failed(reader);
			Assert.IsFalse(result.Success);
			Assert.AreEqual('a', result.Value);
			Assert.IsFalse(reader.EOF);
		}*/

	}
}
