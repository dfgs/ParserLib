using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseOrUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			Parser<string> parser,a,b;

			a = Parse.Char('a');
			b = Parse.Char('b');
			parser = a.Or(b);

			Assert.AreEqual("b", parser.Parse("b"));
			Assert.AreEqual("a", parser.Parse("a"));
		}
		[TestMethod]
		public void ShouldTryParse()
		{
			Parser<string> parser, a, b;

			a = Parse.Char('a');
			b = Parse.Char('b');
			parser = a.Or(b);

			Assert.IsTrue(parser.TryParse("b"));
			Assert.IsTrue(parser.TryParse("a"));
			Assert.IsFalse(parser.TryParse("c"));
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			Parser<string> parser, a, b;

			a = Parse.Char('a');
			b = Parse.Char('b');
			parser = a.Or(b);

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(""));
		}

		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			Parser<string> parser, a, b;

			a = Parse.Char('a');
			b = Parse.Char('b');
			parser = a.Or(b);

			Assert.ThrowsException<EndOfReaderException>(() => parser.TryParse(""));
		}

		[TestMethod]
		public void ShouldNotParse()
		{
			Parser<string> parser, a, b;

			a = Parse.Char('a');
			b = Parse.Char('b');
			parser = a.Or(b);

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse("c"));

		}

	}
}
