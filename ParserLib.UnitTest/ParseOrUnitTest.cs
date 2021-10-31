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
			IParser<string> a,b,parser;
			StringReader reader;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d'));
			parser = a.Or(b);

			reader = new StringReader("abc");
			Assert.AreEqual("abc", parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("abd");
			Assert.AreEqual("abd", parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParse()
		{
			IParser<string> a, b, parser;
			StringReader reader;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d'));
			parser = a.Or(b);

			reader = new StringReader("abe");
			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			IParser<string> a, b, parser;
			StringReader reader;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d'));
			parser = a.Or(b);

			reader = new StringReader("aab"); reader.Seek(1);
			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<string> a, b, parser;
			StringReader reader;
			IParseResult<string> result;



			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d'));
			parser = a.Or(b);

			reader = new StringReader("abc");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("abc", result.Value);
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("abd");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("abd", result.Value);
			Assert.AreEqual(3, reader.Position);

		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			IParser<string> a, b, parser;
			StringReader reader;
			IParseResult<string> result;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d'));
			parser = a.Or(b);

			reader = new StringReader("abe");
			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<string> a, b, parser;
			StringReader reader;
			IParseResult<string> result;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d'));
			parser = a.Or(b);

			reader = new StringReader("ab"); reader.Seek(1);
			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(1, reader.Position);
		}

	}
}
