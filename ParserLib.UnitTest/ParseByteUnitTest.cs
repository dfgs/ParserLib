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
			Parser<byte> parser;
			Reader reader;

			parser = Parse.Byte();

			reader = new Reader("255");
			Assert.AreEqual(255, parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);

			reader = new Reader("199");
			Assert.AreEqual(199, parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);
			
			reader = new Reader("99");
			Assert.AreEqual(99, parser.Parse(reader));
			Assert.AreEqual(2, reader.Position);

			reader = new Reader("0");
			Assert.AreEqual(0, parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}
		[TestMethod]
		public void ShouldNotParse()
		{
			Parser<byte> parser;
			Reader reader;

			parser = Parse.Byte();

			reader = new Reader("256");
			Assert.AreEqual(25, parser.Parse(reader));
			Assert.AreEqual(2, reader.Position);

			reader = new Reader("300");
			Assert.AreEqual(30, parser.Parse(reader));
			Assert.AreEqual(2, reader.Position);

			reader = new Reader("abc");
			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			Parser<byte> parser;
			Reader reader;

			reader = new Reader("a");reader.Seek(1);
			parser = Parse.Byte();

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			Parser<byte> parser;
			Reader reader;
			ParseResult<byte> result;

			parser = Parse.Byte();

			reader = new Reader("255");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(255, result.Value);
			Assert.AreEqual(3, reader.Position);

			reader = new Reader("199");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(199, result.Value);
			Assert.AreEqual(3, reader.Position);

			reader = new Reader("99");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(99, result.Value);
			Assert.AreEqual(2, reader.Position);

			reader = new Reader("0");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(0, result.Value);
			Assert.AreEqual(1, reader.Position);
		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			Parser<byte> parser;
			Reader reader;
			ParseResult<byte> result;

			parser = Parse.Byte();

			reader = new Reader("256");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(25, result.Value);
			Assert.AreEqual(2, reader.Position);

			reader = new Reader("300");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(30, result.Value);
			Assert.AreEqual(2, reader.Position);

			reader = new Reader("abc");
			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(0, result.Value);
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			Parser<byte> parser;
			Reader reader;
			ParseResult<byte> result;

			parser = Parse.Byte();

			reader = new Reader("a"); reader.Seek(1);
			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(0, result.Value);
			Assert.AreEqual(1, reader.Position); 
		}
		

		
	}
}
