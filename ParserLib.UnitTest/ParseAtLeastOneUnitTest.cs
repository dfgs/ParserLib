using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseAtLeastOneUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			Parser<string> a;
			Parser<string> parser;

			a = Parse.Char('a');
			parser = a.AtLeastOne();

			Assert.AreEqual("a", parser.Parse("a"));
			Assert.AreEqual("a", parser.Parse("ab"));
			Assert.AreEqual("aa", parser.Parse("aab"));
			Assert.AreEqual("aaa", parser.Parse("aaab"));
		}
		[TestMethod]
		public void ShouldTryParse()
		{
			Parser<string> a;
			Parser<string> parser;

			a = Parse.Char('a');
			parser = a.AtLeastOne();

			Assert.IsTrue(parser.TryParse("a").IsSuccess);
			Assert.IsTrue(parser.TryParse("aa").IsSuccess);
			Assert.IsTrue(parser.TryParse("aaa").IsSuccess);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			Parser<string> a;
			Parser<string> parser;

			a = Parse.Char('a');
			parser = a.AtLeastOne();

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(""));
		}

		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			Parser<string> a;
			Parser<string> parser;

			a = Parse.Char('a');
			parser = a.AtLeastOne();

			Assert.ThrowsException<EndOfReaderException>(() => parser.TryParse(""));
		}

		[TestMethod]
		public void ShouldNotParse()
		{
			Parser<string> a;
			Parser<string> parser;

			a = Parse.Char('a');
			parser = a.AtLeastOne();

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse("c"));
		}

		[TestMethod]
		public void ShouldSeekToPreviousPositionWhenTryParse()
		{
			Parser<string> parser;
			Reader reader;


			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).AtLeastOne();
			reader = new Reader("abcabcabd");
			Assert.IsTrue(parser.TryParse(reader).IsSuccess);
			Assert.AreEqual(6, reader.Position);
			Assert.IsFalse(parser.TryParse(reader).IsSuccess);
			Assert.AreEqual(6, reader.Position);

			reader = new Reader("abd");
			Assert.IsFalse(parser.TryParse(reader).IsSuccess);
			Assert.AreEqual(0, reader.Position);
		}


	}
}
