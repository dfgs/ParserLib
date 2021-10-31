using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseZeroOrMoreTimesUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("adc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();
			Assert.AreEqual(null, parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);

			reader = new StringReader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();
			Assert.AreEqual("abc", parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("abcabc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();
			Assert.AreEqual("abcabc", parser.Parse(reader));
			Assert.AreEqual(6, reader.Position);
		}
		[TestMethod]
		public void ShouldParseWithEnumeration()
		{
			IParser<IEnumerable<byte>> parser;
			StringReader reader;
			byte[] result;

			reader = new StringReader("1234");
			parser = Parse.Digit().ZeroOrMoreTimes();
			result = parser.Parse(reader).ToArray();

			Assert.AreEqual(4, result.Length);
			Assert.AreEqual(1, result[0]);
			Assert.AreEqual(2, result[1]);
			Assert.AreEqual(3, result[2]);
			Assert.AreEqual(4, reader.Position);

			reader = new StringReader("a1234");
			parser = Parse.Digit().ZeroOrMoreTimes();
			result = parser.Parse(reader).ToArray();

			Assert.AreEqual(0, result.Length);
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseIfFuncIsNull()
		{
			Assert.ThrowsException<ArgumentNullException>(() => Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes(null));
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("ab");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();

			Assert.AreEqual(null, parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			reader = new StringReader("adc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(0, reader.Position);

			reader = new StringReader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("abc", result.Value);
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("abcabc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("abcabc", result.Value);
			Assert.AreEqual(6, reader.Position);

		}

		

		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			reader = new StringReader("ab"); 
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();

			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(0, reader.Position);
		}


	}
}
