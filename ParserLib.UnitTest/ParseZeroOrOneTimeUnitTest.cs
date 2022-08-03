using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseZeroOrOneTimeUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("adc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(0, reader.Position);

			reader = new StringReader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult);
			Assert.AreEqual("abc", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("abcabc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult);
			Assert.AreEqual("abc", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);
		}
		

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();

			reader = new StringReader("ab");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(0, reader.Position);
			
			reader = new StringReader("a");reader.Seek(1);
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("adc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(0, reader.Position);

			reader = new StringReader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult);
			Assert.AreEqual("abc", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("abcabc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult);
			Assert.AreEqual("abc", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);

		}

		

		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();

			reader = new StringReader("ab");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(0, reader.Position);

			reader = new StringReader("a"); reader.Seek(1);
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(1, reader.Position);
		}

		

	}
}
