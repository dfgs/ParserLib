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

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d')) ;
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			parser = a.Or(b);

			Assert.AreEqual("abc", parser.Parse("abc"));
			Assert.AreEqual("abd", parser.Parse("abd"));
		}
		[TestMethod]
		public void ShouldTryParse()
		{
			Parser<string> parser, a, b;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d'));
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			parser = a.Or(b);

			Assert.IsTrue(parser.TryParse("abc"));
			Assert.IsTrue(parser.TryParse("abd"));
			Assert.IsFalse(parser.TryParse("cbc"));
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

		[TestMethod]
		public void ShouldSeekToPreviousPositionWhenTryParse()
		{
			Parser<string> parser,a,b;
			Reader reader;


			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d'));
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			reader = new Reader("abcabe");
			parser = a.Or(b);

			Assert.IsTrue(parser.TryParse(reader));
			Assert.AreEqual(3, reader.Position);
			Assert.IsFalse(parser.TryParse(reader));
			Assert.AreEqual(3, reader.Position);
		}

	}
}
