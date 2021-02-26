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
			Parser<string> a;
			Parser<string> b;
			Parser<string> parser;

			a = Parse.Char('a');
			b = Parse.Char('b').Then(Parse.Char('c'));
			parser = a.Then(b);

			Assert.AreEqual("abc", parser.Parse("abc"));
		}
		[TestMethod]
		public void ShouldTryParse()
		{
			Parser<string> a;
			Parser<string> b;
			Parser<string> parser;

			a = Parse.Char('a');
			b = Parse.Char('b').Then(Parse.Char('c'));
			parser = a.Then(b);

			Assert.IsTrue(parser.TryParse("abc"));
			Assert.IsFalse(parser.TryParse("acb"));
			Assert.IsFalse(parser.TryParse("cba"));
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			Parser<string> a;
			Parser<string> b;
			Parser<string> parser;

			a = Parse.Char('a');
			b = Parse.Char('b').Then(Parse.Char('c'));
			parser = a.Then(b);

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(""));
		}

		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			Parser<string> a;
			Parser<string> b;
			Parser<string> parser;

			a = Parse.Char('a');
			b = Parse.Char('b').Then(Parse.Char('c'));
			parser = a.Then(b);

			Assert.ThrowsException<EndOfReaderException>(() => parser.TryParse(""));
		}

		[TestMethod]
		public void ShouldNotParse()
		{
			Parser<string> a;
			Parser<string> b;
			Parser<string> parser;

			a = Parse.Char('a');
			b = Parse.Char('b').Then(Parse.Char('c'));
			parser = a.Then(b);

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse("acb"));
			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse("cba"));

		}

	}
}
