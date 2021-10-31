using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseIntUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			IParser<int> parser;
			StringReader reader;

			parser = Parse.Int();

			reader = new StringReader("2555");
			Assert.AreEqual(2555, parser.Parse(reader));
			Assert.AreEqual(4, reader.Position);

			reader = new StringReader("1999");
			Assert.AreEqual(1999, parser.Parse(reader));
			Assert.AreEqual(4, reader.Position);
			
			reader = new StringReader("-99");
			Assert.AreEqual(-99, parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("0");
			Assert.AreEqual(0, parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);

			reader = new StringReader("-0");
			Assert.AreEqual(0, parser.Parse(reader));
			Assert.AreEqual(2, reader.Position);
		}
		

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			IParser<int> parser;
			StringReader reader;

			reader = new StringReader("a");reader.Seek(1);
			parser = Parse.Int();

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<int> parser;
			StringReader reader;
			IParseResult<int> result;

			parser = Parse.Int();

			reader = new StringReader("2555");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(2555, result.Value);
			Assert.AreEqual(4, reader.Position);

			reader = new StringReader("1999");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(1999, result.Value);
			Assert.AreEqual(4, reader.Position);

			reader = new StringReader("-99");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(-99, result.Value);
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("0");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(0, result.Value);
			Assert.AreEqual(1, reader.Position);
		}

		


		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<int> parser;
			StringReader reader;
			IParseResult<int> result;

			parser = Parse.Int();

			reader = new StringReader("a"); reader.Seek(1);
			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(0, result.Value);
			Assert.AreEqual(1, reader.Position); 
		}
		

		
	}
}
