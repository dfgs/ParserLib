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
			Parser<string> parser;
			Reader reader;

			reader = new Reader("abc");
			parser = Parse.AnyInRange('a','c');

			Assert.AreEqual("a", parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}
		[TestMethod]
		public void ShouldNotParse()
		{
			Parser<string> parser;
			Reader reader;

			reader = new Reader("dbc");
			parser = Parse.AnyInRange('a', 'c');

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			Parser<string> parser;
			Reader reader;

			reader = new Reader("a");reader.Seek(1);
			parser = Parse.AnyInRange('a', 'c');

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			Parser<string> parser;
			Reader reader;
			IParseResult<string> result;

			reader = new Reader("abc");
			parser = Parse.AnyInRange('a', 'c');
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("a", result.Value);
			Assert.AreEqual(1, reader.Position);
		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			Parser<string> parser;
			Reader reader;
			IParseResult<string> result;

			reader = new Reader("dbc");
			parser = Parse.AnyInRange('a', 'c');
			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(0, reader.Position);
		}

		
		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			Parser<string> parser;
			Reader reader;
			IParseResult<string> result;

			reader = new Reader("a"); reader.Seek(1);
			parser = Parse.AnyInRange('a', 'c');

			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(1, reader.Position); 
		}
		

		
	}
}
