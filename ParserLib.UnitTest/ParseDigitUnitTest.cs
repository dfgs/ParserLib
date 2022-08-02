using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseDigitUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			IParser<byte> parser;
			StringReader reader;

			reader = new StringReader("0123456789");
			parser = Parse.Digit();

			for (byte t = 0; t < 10; t++)
			{
				Assert.AreEqual(t, parser.Parse(reader));
				Assert.AreEqual(t+1, reader.Position);
			}
		}
		[TestMethod]
		public void ShouldNotParse()
		{
			IParser<byte> parser;
			StringReader reader;

			reader = new StringReader("abc");
			parser = Parse.Digit();

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			IParser<byte> parser;
			StringReader reader;

			reader = new StringReader("a");reader.Seek(1);
			parser = Parse.Digit();

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<byte> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("0123456789");
			parser = Parse.Digit();

			for (byte t = 0; t < 10; t++)
			{
				result = parser.TryParse(reader);
				Assert.IsTrue(result is ISucceededParseResult);
				Assert.AreEqual(t, ((ISucceededParseResult<string>)result).Value);
				Assert.AreEqual(t+1, reader.Position);
			}
		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			IParser<byte> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("abc");
			parser = Parse.Digit();
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult);
			Assert.AreEqual(0, ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(0, reader.Position);
		}

		
		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<byte> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("a"); reader.Seek(1);
			parser = Parse.Digit();

			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult);
			Assert.AreEqual(0, ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(1, reader.Position); 
		}
		

		
	}
}
