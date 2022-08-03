using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseThenUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();

			Assert.AreEqual("abc", parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);
		}
		[TestMethod]
		public void ShouldNotParse()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("abd");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("aab"); reader.Seek(1);
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult);
			Assert.AreEqual("abc", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);
		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("abd");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult);
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("aa"); reader.Seek(1);
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();

			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult);
			Assert.AreEqual(1, reader.Position);
		}

		[TestMethod]
		public void ShouldReturnHigherErrorPos()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();
			reader = new StringReader("abe");
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult);
			Assert.AreEqual(2, ((UnexpectedCharParseResult)result).Position);

		}




	}
}
