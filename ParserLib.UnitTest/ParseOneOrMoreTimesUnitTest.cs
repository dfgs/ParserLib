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
			Parser<string> parser;
			Reader reader;

			reader = new Reader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes();

			Assert.AreEqual("abc", parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);

			reader = new Reader("abcabc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes();

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
			parser = Parse.Digit().OneOrMoreTimes();
			result = parser.Parse(reader).ToArray();

			Assert.AreEqual(4, result.Length);
			Assert.AreEqual(1, result[0]);
			Assert.AreEqual(2, result[1]);
			Assert.AreEqual(3, result[2]);
		}

		[TestMethod]
		public void ShouldNotParseIfFuncIsNull()
		{
			Assert.ThrowsException<ArgumentNullException>(() => Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes(null));
		}

		[TestMethod]
		public void ShouldNotParse()
		{
			Parser<string> parser;
			Reader reader;

			reader = new Reader("abd");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes();

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWithEnumeration()
		{
			Parser<IEnumerable<byte>> parser;
			Reader reader;

			reader = new Reader("a1234");
			parser = Parse.Digit().OneOrMoreTimes();
			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			Parser<string> parser;
			Reader reader;

			reader = new Reader("ab"); 
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes();

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			Parser<string> parser;
			Reader reader;
			IParseResult<string> result;

			reader = new Reader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes();
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("abc", result.Value);
			Assert.AreEqual(3, reader.Position);

			reader = new Reader("abcabc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes();
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("abcabc", result.Value);
			Assert.AreEqual(6, reader.Position);

		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			Parser<string> parser;
			Reader reader;
			IParseResult<string> result;

			reader = new Reader("abd");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes();
			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			Parser<string> parser;
			Reader reader;
			IParseResult<string> result;

			reader = new Reader("ab"); 
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).OneOrMoreTimes();

			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(0, reader.Position);
		}


	}
}
