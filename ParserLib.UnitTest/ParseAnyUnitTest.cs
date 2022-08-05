using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseAnyUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			IParser<char> parser;
			StringReader reader;

			reader = new StringReader("abc");
			parser = Parse.Any();

			Assert.AreEqual('a', parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}
		[TestMethod]
		public void ShouldParseWithIgnoredChars()
		{
			IParser<char> parser;
			StringReader reader;

			reader = new StringReader("   abc", ' ');
			parser = Parse.Any();

			Assert.AreEqual('a', parser.Parse(reader));
			Assert.AreEqual(4, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			IParser<char> parser;
			StringReader reader;

			reader = new StringReader("a"); reader.Seek(1);
			parser = Parse.Any();

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<char> parser;
			StringReader reader;
			IParseResult<char> result;

			reader = new StringReader("abc");
			parser = Parse.Any();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<char>);
			Assert.AreEqual('a', ((ISucceededParseResult<char>)result).Value);
			Assert.AreEqual(1, reader.Position);
		}




		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<char> parser;
			StringReader reader;
			IParseResult<char> result;

			reader = new StringReader("a"); reader.Seek(1);
			parser = Parse.Any();

			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<char>);
			Assert.AreEqual(1, reader.Position);
		}



	}
}
