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
			Parser<string> parser;
			Reader reader;

			reader = new Reader("abc");
			parser = Parse.String("abc");

			Assert.AreEqual("abc", parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);
		}
		[TestMethod]
		public void ShouldNotParse()
		{
			Parser<string> parser;
			Reader reader;

			reader = new Reader("abc");
			parser = Parse.String("acb");

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);

			reader = new Reader("abc");
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
			Parser<string> parser;
			Reader reader;

			reader = new Reader("ab");
			parser = Parse.String("abc");

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			Parser<string> parser;
			Reader reader;
			IParseResult<string> result;

			reader = new Reader("abc");
			parser = Parse.String("abc");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("abc", result.Value);
			Assert.AreEqual(3, reader.Position);
		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			Parser<string> parser;
			Reader reader;
			IParseResult<string> result;

			reader = new Reader("abc");
			parser = Parse.String("acb");
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

			reader = new Reader("ab"); reader.Seek(1);
			parser = Parse.String("abc");

			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(1, reader.Position); 
		}
		

		
	}
}
