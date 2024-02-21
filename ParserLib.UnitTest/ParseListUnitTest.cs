using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseListUnitTest
	{
		[TestMethod]
		public void ShouldParseOneItem()
		{
			IMultipleParser<string> parser;
			StringReader reader;
			string[] result;

			reader = new StringReader("abc");
			parser = Parse.String("abc").List(Parse.Char(','));

			result = parser.Parse(reader).ToArray();

			Assert.AreEqual(1, result.Length);
			Assert.AreEqual("abc", result[0]);
			Assert.AreEqual(3, reader.Position);
		}

		[TestMethod]
		public void ShouldParseTwoItems()
		{
			IMultipleParser<string> parser;
			StringReader reader;
			string[] result;

			reader = new StringReader("abc,abc");
			parser = Parse.String("abc").List(Parse.Char(','));

			result = parser.Parse(reader).ToArray();

			Assert.AreEqual(2, result.Length);
			Assert.AreEqual("abc", result[0]);
			Assert.AreEqual("abc", result[1]);
			Assert.AreEqual(7, reader.Position);
		}
		[TestMethod]
		public void ShouldParseFourItems()
		{
			IMultipleParser<string> parser;
			StringReader reader;
			string[] result;

			reader = new StringReader("abc,abc,abc,abc");
			parser = Parse.String("abc").List(Parse.Char(','));

			result = parser.Parse(reader).ToArray();

			Assert.AreEqual(4, result.Length);
			Assert.AreEqual("abc", result[0]);
			Assert.AreEqual("abc", result[1]);
			Assert.AreEqual("abc", result[2]);
			Assert.AreEqual("abc", result[3]);
			Assert.AreEqual(15, reader.Position);
		}

		[TestMethod]
		public void ShouldParseOneItemWithIncorrectSeparator()
		{
			IMultipleParser<string> parser;
			StringReader reader;
			string[] result;

			reader = new StringReader("abc;");
			parser = Parse.String("abc").List(Parse.Char(','));

			result = parser.Parse(reader).ToArray();

			Assert.AreEqual(1, result.Length);
			Assert.AreEqual("abc", result[0]);
			Assert.AreEqual(3, reader.Position);
		}

		[TestMethod]
		public void ShouldParseTwoItemWithIncorrectSeparator()
		{
			IMultipleParser<string> parser;
			StringReader reader;
			string[] result;

			reader = new StringReader("abc,abc;abc");
			parser = Parse.String("abc").List(Parse.Char(','));

			result = parser.Parse(reader).ToArray();

			Assert.AreEqual(2, result.Length);
			Assert.AreEqual("abc", result[0]);
			Assert.AreEqual("abc", result[1]);
			Assert.AreEqual(7, reader.Position);
		}
		[TestMethod]
		public void ShouldParseTwoItemWithIncorrectItemAtTheEnd()
		{
			IMultipleParser<string> parser;
			StringReader reader;
			string[] result;

			reader = new StringReader("abc,abc,abd");
			parser = Parse.String("abc").List(Parse.Char(','));

			result = parser.Parse(reader).ToArray();

			Assert.AreEqual(2, result.Length);
			Assert.AreEqual("abc", result[0]);
			Assert.AreEqual("abc", result[1]);
			Assert.AreEqual(7, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParse()
		{
			IMultipleParser<string> parser;
			StringReader reader;
	
			reader = new StringReader("abd");
			parser = Parse.String("abc").List(Parse.Char(','));

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}


		
		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			IMultipleParser<string> parser;
			StringReader reader;

			reader = new StringReader("ab");
			parser = Parse.String("abc").List(Parse.Char(','));

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IMultipleParser<string> parser;
			StringReader reader;
			IParseResult result;
			string[] values;

			reader = new StringReader("abc");
			parser = Parse.String("abc").List(Parse.Char(','));
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			values = ((ISucceededParseResult<string>)result).EnumerateValue().ToArray();
			Assert.AreEqual(1, values.Length);
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("abc,abc");
			parser = Parse.String("abc").List(Parse.Char(','));
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			values = ((ISucceededParseResult<string>)result).EnumerateValue().ToArray();
			Assert.AreEqual(2, values.Length);
			Assert.AreEqual(7, reader.Position);

		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("abd");
			parser = Parse.String("abc").List(Parse.Char(','));
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

			reader = new StringReader("ab");
			parser = Parse.String("abc").List(Parse.Char(','));

			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<string>);
			Assert.AreEqual(0, reader.Position);
		}
		[TestMethod]
		public void ShouldReturnHigherErrorPos()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			parser = Parse.String("abcd").List(Parse.Char(','));
			reader = new StringReader("abce");
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<string>);
			Assert.AreEqual(3, ((UnexpectedCharParseResult)result).Position);

		}
		//*/

	}
}
