﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseAnyUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			IParser<string> parser;
			Reader reader;

			reader = new Reader("abc");
			parser = Parse.Any();

			Assert.AreEqual("a", parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}
		

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			IParser<string> parser;
			Reader reader;

			reader = new Reader("a"); reader.Seek(1);
			parser = Parse.Any();

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
			parser = Parse.Any();
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("a", result.Value);
			Assert.AreEqual(1, reader.Position);
		}




		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<string> parser;
			Reader reader;
			IParseResult<string> result;

			reader = new Reader("a"); reader.Seek(1);
			parser = Parse.Any();

			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(1, reader.Position);
		}



	}
}
