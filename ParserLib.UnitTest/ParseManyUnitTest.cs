using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseManyUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			Parser<string> a;
			Parser<string> parser;
			char[] result;

			a = Parse.Char('a');
			parser = a.Many();

			result = parser.Parse("aa").ToArray();
			Assert.AreEqual(2, result.Length);
			result = parser.Parse("aaa").ToArray();
			Assert.AreEqual(3, result.Length);
		}
		[TestMethod]
		public void ShouldTryParse()
		{
			Parser<string> a;
			Parser<string> parser;

			a = Parse.Char('a');
			parser = a.Many();

			Assert.IsTrue(parser.TryParse("aa"));
			Assert.IsTrue(parser.TryParse("aaa"));
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			Parser<string> a;
			Parser<string> parser;

			a = Parse.Char('a');
			parser = a.Many();

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(""));
		}

		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			Parser<string> a;
			Parser<string> parser;

			a = Parse.Char('a');
			parser = a.Many();

			Assert.ThrowsException<EndOfReaderException>(() => parser.TryParse(""));
		}

		[TestMethod]
		public void ShouldNotParse()
		{
			Parser<string> a;
			Parser<string> parser;

			a = Parse.Char('a');
			parser = a.Many();

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse("c"));

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse("a"));

		}

	}
}
