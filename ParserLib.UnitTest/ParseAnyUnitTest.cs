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
			Parser<string> parser;

			parser = Parse.Any();

			Assert.AreEqual("a", parser.Parse("abc"));
		}
		[TestMethod]
		public void ShouldTryParse()
		{
			Parser<string> parser;

			parser = Parse.Any();

			Assert.IsTrue(parser.TryParse("abc"));
			Assert.IsTrue(parser.TryParse("bca"));
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			Parser<string> parser;

			parser = Parse.Any();

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(""));
		}
		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			Parser<string> parser;

			parser = Parse.Any();

			Assert.ThrowsException<EndOfReaderException>(() => parser.TryParse(""));
		}

	}
}
