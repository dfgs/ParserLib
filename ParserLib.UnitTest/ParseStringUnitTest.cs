using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseStringUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("abc");
			parser = Parse.String("abc");

			Assert.AreEqual("abc", parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);
		}
		[TestMethod]
		public void ShouldNotParse()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("abc");
			parser = Parse.String("acb");

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);

			reader = new StringReader("abc");
			parser = Parse.String("abd");

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}
		[TestMethod]
		public void ShouldNotParseNull()
		{
			Assert.ThrowsException<ArgumentNullException>(() => Parse.String(null));
		}
		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("ab");
			parser = Parse.String("abc");

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			reader = new StringReader("abc");
			parser = Parse.String("abc");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("abc", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);
		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			reader = new StringReader("abc");
			parser = Parse.String("acb");
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<string>);
			Assert.AreEqual(0, reader.Position);
		}

		
		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			reader = new StringReader("ab"); reader.Seek(1);
			parser = Parse.String("abc");

			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<string>);
			Assert.AreEqual(1, reader.Position); 
		}

		[TestMethod]
		public void ShouldReturnHigherErrorPos()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			parser = Parse.String("abc");
			reader = new StringReader("abe");
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<string>);
			Assert.AreEqual(2, ((UnexpectedCharParseResult<string>)result).Position);

		}



	}
}
