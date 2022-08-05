using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseOneOrMoreTimesUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			ISingleParser<string> parser;
			StringReader reader;

			reader = new StringReader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes().ToStringParser();

			Assert.AreEqual("abc", parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("abcabc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes().ToStringParser();

			Assert.AreEqual("abcabc", parser.Parse(reader));
			Assert.AreEqual(6, reader.Position);
		}
		[TestMethod]
		public void ShouldParseWithEnumeration()
		{
			IMultipleParser<byte> parser;
			StringReader reader;
			byte[] result;

			reader = new StringReader("1234");
			parser = Parse.Digit().OneOrMoreTimes();
			result = parser.Parse(reader).ToArray();

			Assert.AreEqual(4, result.Length);
			Assert.AreEqual(1, result[0]);
			Assert.AreEqual(2, result[1]);
			Assert.AreEqual(3, result[2]);
		}

		/*[TestMethod]
		public void ShouldNotParseIfFuncIsNull()
		{
			Assert.ThrowsException<ArgumentNullException>(() => Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes(null));
		}*/

		[TestMethod]
		public void ShouldNotParse()
		{
			ISingleParser<string> parser;
			StringReader reader;

			reader = new StringReader("abd");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes().ToStringParser();

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWithEnumeration()
		{
			IMultipleParser<byte> parser;
			StringReader reader;

			reader = new StringReader("a1234");
			parser = Parse.Digit().OneOrMoreTimes();
			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			ISingleParser<string> parser;
			StringReader reader;

			reader = new StringReader("ab"); 
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes().ToStringParser();

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			reader = new StringReader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("abc", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("abcabc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("abcabc", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(6, reader.Position);

		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			reader = new StringReader("abd");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<string>);
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			reader = new StringReader("ab"); 
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes().ToStringParser();

			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<string>);
			Assert.AreEqual(0, reader.Position);
		}
		[TestMethod]
		public void ShouldReturnHigherErrorPos()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			parser = Parse.Char('a').OneOrMoreTimes().Then(Parse.Char('b')).ToStringParser();
			reader = new StringReader("aaac");
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<string>);
			Assert.AreEqual(3, ((UnexpectedCharParseResult<string>)result).Position);

		}

	}
}
