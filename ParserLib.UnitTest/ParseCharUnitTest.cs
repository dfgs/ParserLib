using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseCharUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			Parser<string> parser;

			parser = Parse.Char('a');

			Assert.AreEqual("a", parser.Parse("abc"));
		}
		[TestMethod]
		public void ShouldTryParse()
		{
			Parser<string> parser;

			parser = Parse.Char('a');

			Assert.IsTrue(parser.TryParse("abc"));
			Assert.IsFalse(parser.TryParse("bca"));
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			Parser<string> parser;

			parser = Parse.Char('b');

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(""));
		}

		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			Parser<string> parser;

			parser = Parse.Char('b');

			Assert.ThrowsException<EndOfReaderException>(() => parser.TryParse(""));
		}
		[TestMethod]
		public void ShouldNotParse()
		{
			Parser<string> parser;

			parser = Parse.Char('b');

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse("abc"));
		}

		[TestMethod]
		public void ShouldSeekToPreviousPositionWhenTryParse()
		{
			Parser<string> parser;
			Reader reader;


			parser = Parse.Char('a');
			reader = new Reader("abcd");
			Assert.IsTrue(parser.TryParse(reader));
			Assert.AreEqual(1, reader.Position);
			Assert.IsFalse(parser.TryParse(reader));
			Assert.AreEqual(1, reader.Position);
		}
	}
}
