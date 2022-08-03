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

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d')).ToStringParser();
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

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d')).ToStringParser();
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

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d')).ToStringParser();
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
			IParseResult result;



			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d')).ToStringParser();
			parser = a.Or(b);

			reader = new StringReader("abc");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult);
			Assert.AreEqual("abc", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("abd");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult);
			Assert.AreEqual("abd", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);

		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			IParser<string> a, b, parser;
			StringReader reader;
			IParseResult result;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d')).ToStringParser();
			parser = a.Or(b);

			reader = new StringReader("abe");
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult);
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<string> a, b, parser;
			StringReader reader;
			IParseResult result;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d')).ToStringParser();
			parser = a.Or(b);

			reader = new StringReader("ab"); reader.Seek(1);
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult);
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldReturnHigherErrorPos()
		{
			IParser<string> a, b, parser;
			StringReader reader;
			IParseResult result;

			a = Parse.Char('a').Then(Parse.Char('a')).Then(Parse.Char('a')).ToStringParser();
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();

			parser = a.Or(b);
			reader = new StringReader("aae");
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult);
			Assert.AreEqual(2, ((UnexpectedCharParseResult)result).Position);


			parser = b.Or(a);
			reader = new StringReader("aae");
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult);
			Assert.AreEqual(2, ((UnexpectedCharParseResult)result).Position);
		}




	}
}
