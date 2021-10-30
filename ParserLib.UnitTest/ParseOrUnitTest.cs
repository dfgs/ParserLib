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
			Reader reader;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d'));
			parser = a.Or(b);

			reader = new Reader("abc");
			Assert.AreEqual("abc", parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);

			reader = new Reader("abd");
			Assert.AreEqual("abd", parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParse()
		{
			IParser<string> a, b, parser;
			Reader reader;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d'));
			parser = a.Or(b);

			reader = new Reader("abe");
			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			IParser<string> a, b, parser;
			Reader reader;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d'));
			parser = a.Or(b);

			reader = new Reader("aab"); reader.Seek(1);
			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<string> a, b, parser;
			Reader reader;
			IParseResult<string> result;



			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d'));
			parser = a.Or(b);

			reader = new Reader("abc");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("abc", result.Value);
			Assert.AreEqual(3, reader.Position);

			reader = new Reader("abd");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("abd", result.Value);
			Assert.AreEqual(3, reader.Position);

		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			IParser<string> a, b, parser;
			Reader reader;
			IParseResult<string> result;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d'));
			parser = a.Or(b);

			reader = new Reader("abe");
			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<string> a, b, parser;
			Reader reader;
			IParseResult<string> result;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d'));
			parser = a.Or(b);

			reader = new Reader("ab"); reader.Seek(1);
			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(1, reader.Position);
		}

	}
}
