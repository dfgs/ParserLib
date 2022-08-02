using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseAnyInRangeUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("abc");
			parser = Parse.AnyInRange('a','c');

			Assert.AreEqual("a", parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}
		[TestMethod]
		public void ShouldParseWithIgnoredChars()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("   abc", ' ');
			parser = Parse.AnyInRange('a', 'c');

			Assert.AreEqual("a", parser.Parse(reader));
			Assert.AreEqual(4, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParse()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("dbc");
			parser = Parse.AnyInRange('a', 'c');

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("a");reader.Seek(1);
			parser = Parse.AnyInRange('a', 'c');

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("abc");
			parser = Parse.AnyInRange('a', 'c');
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult);
			Assert.AreEqual("a", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(1, reader.Position);
		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("dbc");
			parser = Parse.AnyInRange('a', 'c');
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult);
			Assert.AreEqual(null, ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(0, reader.Position);
		}

		
		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("a"); reader.Seek(1);
			parser = Parse.AnyInRange('a', 'c');

			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult);
			Assert.AreEqual(null, ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(1, reader.Position); 
		}
		

		
	}
}
