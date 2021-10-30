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
			Assert.AreEqual(null, parser.Parse(reader));
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
		public void ShouldParseWithEnumeration()
		{
			Parser<IEnumerable<byte>> parser;
			Reader reader;
			byte[] result;

			reader = new Reader("1234");
			parser = Parse.Digit().ZeroOrMoreTimes();
			result = parser.Parse(reader).ToArray();

			Assert.AreEqual(4, result.Length);
			Assert.AreEqual(1, result[0]);
			Assert.AreEqual(2, result[1]);
			Assert.AreEqual(3, result[2]);
			Assert.AreEqual(4, reader.Position);

			reader = new Reader("a1234");
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
			Parser<string> parser;
			Reader reader;

			reader = new Reader("ab");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();

			Assert.AreEqual(null, parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			Parser<string> parser;
			Reader reader;
			IParseResult<string> result;

			reader = new Reader("adc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
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
			IParseResult<string> result;

			reader = new Reader("ab"); 
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrMoreTimes();

			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(0, reader.Position);
		}


	}
}
