﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseExceptUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			ISingleParser<char> parser;
			StringReader reader;

			reader = new StringReader("dbc");
			parser = Parse.Except('c','b','a');

			Assert.AreEqual('d', parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}
		[TestMethod]
		public void ShouldNotParse()
		{
			ISingleParser<char> parser;
			StringReader reader;

			reader = new StringReader("abc");
			parser = Parse.Except('c', 'b', 'a');

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			ISingleParser<char> parser;
			StringReader reader;

			reader = new StringReader("a");reader.Seek(1);
			parser = Parse.Except('c', 'b', 'a');

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<char> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("dbc");
			parser = Parse.Except('c', 'b', 'a');
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<char>);
			Assert.AreEqual('d', ((ISucceededParseResult<char>)result).Value);
			Assert.AreEqual(1, reader.Position);
		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			IParser<char> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("abc");
			parser = Parse.Except('c', 'b', 'a');
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<char>);
			Assert.AreEqual(0, reader.Position);
		}

		
		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<char> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("a"); reader.Seek(1);
			parser = Parse.Except('c', 'b', 'a');

			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<char>);
			Assert.AreEqual(1, reader.Position); 
		}
		

		
	}
}
