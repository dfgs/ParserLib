using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseByteUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			IParser<byte> parser;
			StringReader reader;

			parser = Parse.Byte();

			reader = new StringReader("255");
			Assert.AreEqual(255, parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("199");
			Assert.AreEqual(199, parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);
			
			reader = new StringReader("99");
			Assert.AreEqual(99, parser.Parse(reader));
			Assert.AreEqual(2, reader.Position);

			reader = new StringReader("0");
			Assert.AreEqual(0, parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}
		[TestMethod]
		public void ShouldNotParse()
		{
			IParser<byte> parser;
			StringReader reader;

			parser = Parse.Byte();

			reader = new StringReader("256");
			Assert.AreEqual(25, parser.Parse(reader));
			Assert.AreEqual(2, reader.Position);

			reader = new StringReader("300");
			Assert.AreEqual(30, parser.Parse(reader));
			Assert.AreEqual(2, reader.Position);

			reader = new StringReader("abc");
			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			IParser<byte> parser;
			StringReader reader;

			reader = new StringReader("a");reader.Seek(1);
			parser = Parse.Byte();

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<byte> parser;
			StringReader reader;
			IParseResult<byte> result;

			parser = Parse.Byte();

			reader = new StringReader("255");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(255, result.Value);
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("199");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(199, result.Value);
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("99");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(99, result.Value);
			Assert.AreEqual(2, reader.Position);

			reader = new StringReader("0");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(0, result.Value);
			Assert.AreEqual(1, reader.Position);
		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			IParser<byte> parser;
			StringReader reader;
			IParseResult<byte> result;

			parser = Parse.Byte();

			reader = new StringReader("256");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(25, result.Value);
			Assert.AreEqual(2, reader.Position);

			reader = new StringReader("300");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(30, result.Value);
			Assert.AreEqual(2, reader.Position);

			reader = new StringReader("abc");
			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(0, result.Value);
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<byte> parser;
			StringReader reader;
			IParseResult<byte> result;

			parser = Parse.Byte();

			reader = new StringReader("a"); reader.Seek(1);
			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(0, result.Value);
			Assert.AreEqual(1, reader.Position); 
		}
		

		
	}
}
