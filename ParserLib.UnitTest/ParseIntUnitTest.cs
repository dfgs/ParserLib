﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseIntUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			Parser<int> parser;
			Reader reader;

			parser = Parse.Int();

			reader = new Reader("2555");
			Assert.AreEqual(2555, parser.Parse(reader));
			Assert.AreEqual(4, reader.Position);

			reader = new Reader("1999");
			Assert.AreEqual(1999, parser.Parse(reader));
			Assert.AreEqual(4, reader.Position);
			
			reader = new Reader("-99");
			Assert.AreEqual(-99, parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);

			reader = new Reader("0");
			Assert.AreEqual(0, parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);

			reader = new Reader("-0");
			Assert.AreEqual(0, parser.Parse(reader));
			Assert.AreEqual(2, reader.Position);
		}
		

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			Parser<int> parser;
			Reader reader;

			reader = new Reader("a");reader.Seek(1);
			parser = Parse.Int();

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			Parser<int> parser;
			Reader reader;
			ParseResult<int> result;

			parser = Parse.Int();

			reader = new Reader("2555");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(2555, result.Value);
			Assert.AreEqual(4, reader.Position);

			reader = new Reader("1999");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(1999, result.Value);
			Assert.AreEqual(4, reader.Position);

			reader = new Reader("-99");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(-99, result.Value);
			Assert.AreEqual(3, reader.Position);

			reader = new Reader("0");
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(0, result.Value);
			Assert.AreEqual(1, reader.Position);
		}

		


		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			Parser<int> parser;
			Reader reader;
			ParseResult<int> result;

			parser = Parse.Int();

			reader = new Reader("a"); reader.Seek(1);
			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(0, result.Value);
			Assert.AreEqual(1, reader.Position); 
		}
		

		
	}
}