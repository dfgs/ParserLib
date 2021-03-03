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
			Parser<string> parser;
			Reader reader;

			reader = new Reader("adc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();
			Assert.AreEqual("", parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);

			reader = new Reader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();
			Assert.AreEqual("abc", parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);

			reader = new Reader("abcabc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();
			Assert.AreEqual("abcabc", parser.Parse(reader));
			Assert.AreEqual(6, reader.Position);
		}
		

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			Parser<string> parser;
			Reader reader;

			reader = new Reader("ab");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();

			Assert.AreEqual("", parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			Parser<string> parser;
			Reader reader;
			ParseResult<string> result;

			reader = new Reader("adc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("", result.Value);
			Assert.AreEqual(0, reader.Position);

			reader = new Reader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("abc", result.Value);
			Assert.AreEqual(3, reader.Position);

			reader = new Reader("abcabc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("abcabc", result.Value);
			Assert.AreEqual(6, reader.Position);

		}

		

		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			Parser<string> parser;
			Reader reader;
			ParseResult<string> result;

			reader = new Reader("ab"); 
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();

			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("", result.Value);
			Assert.AreEqual(0, reader.Position);
		}


	}
}
