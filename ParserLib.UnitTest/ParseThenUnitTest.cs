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
			Reader reader;

			reader = new Reader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));

			Assert.AreEqual("abc", parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);
		}
		[TestMethod]
		public void ShouldNotParse()
		{
			IParser<string> parser;
			Reader reader;

			reader = new Reader("abd");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			IParser<string> parser;
			Reader reader;

			reader = new Reader("aab"); reader.Seek(1);
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<string> parser;
			Reader reader;
			IParseResult<string> result;

			reader = new Reader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("abc", result.Value);
			Assert.AreEqual(3, reader.Position);
		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			IParser<string> parser;
			Reader reader;
			IParseResult<string> result;

			reader = new Reader("abd");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));
			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<string> parser;
			Reader reader;
			IParseResult<string> result;

			reader = new Reader("aa"); reader.Seek(1);
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c'));

			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(1, reader.Position);
		}




	}
}
