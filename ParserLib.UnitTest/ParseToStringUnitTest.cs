using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseToStringUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			ISingleParser<string> parser;
			StringReader reader;

			parser = Parse.Byte().ToStringParser();

			reader = new StringReader("255");
			Assert.AreEqual("255", parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("199");
			Assert.AreEqual("199", parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);
			
			reader = new StringReader("99");
			Assert.AreEqual("99", parser.Parse(reader));
			Assert.AreEqual(2, reader.Position);

			reader = new StringReader("0");
			Assert.AreEqual("0", parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}
		[TestMethod]
		public void ShouldNotParse()
		{
			ISingleParser<string> parser;
			StringReader reader;

			parser = Parse.Byte().ToStringParser();

			reader = new StringReader("256");
			Assert.AreEqual("25", parser.Parse(reader));
			Assert.AreEqual(2, reader.Position);

			reader = new StringReader("300");
			Assert.AreEqual("30", parser.Parse(reader));
			Assert.AreEqual(2, reader.Position);

			reader = new StringReader("abc");
			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			ISingleParser<string> parser;
			StringReader reader;

			reader = new StringReader("a");reader.Seek(1);
			parser = Parse.Byte().ToStringParser();

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			parser = Parse.Byte().ToStringParser();

			reader = new StringReader("255");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("255", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("199");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("199", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("99");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("99", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(2, reader.Position);

			reader = new StringReader("0");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("0", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(1, reader.Position);
		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			parser = Parse.Byte().ToStringParser();

			reader = new StringReader("256");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("25", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(2, reader.Position);

			reader = new StringReader("300");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("30", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(2, reader.Position);

			reader = new StringReader("abc");
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<string>);
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			parser = Parse.Byte().ToStringParser();

			reader = new StringReader("a"); reader.Seek(1);
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<string>);
			Assert.AreEqual(1, reader.Position); 
		}
		

		
	}
}
