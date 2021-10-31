using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseAnyOfUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("abc");
			parser = Parse.AnyOf('c','b','a');

			Assert.AreEqual("a", parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}
		[TestMethod]
		public void ShouldParseWithIgnoredChars()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("   abc", ' ');
			parser = Parse.AnyOf('c', 'b', 'a');

			Assert.AreEqual("a", parser.Parse(reader));
			Assert.AreEqual(4, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParse()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("dbc");
			parser = Parse.AnyOf('c', 'b', 'a');

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("a");reader.Seek(1);
			parser = Parse.AnyOf('c', 'b', 'a');

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			reader = new StringReader("abc");
			parser = Parse.AnyOf('c', 'b', 'a');
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("a", result.Value);
			Assert.AreEqual(1, reader.Position);
		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			reader = new StringReader("dbc");
			parser = Parse.AnyOf('c', 'b', 'a');
			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(0, reader.Position);
		}

		
		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			reader = new StringReader("a"); reader.Seek(1);
			parser = Parse.AnyOf('c', 'b', 'a');

			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(1, reader.Position); 
		}
		

		
	}
}
