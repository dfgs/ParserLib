using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseZeroOrOneTimeUnitTest
	{
		[TestMethod]
		public void ShouldParseSingleItem()
		{
			IParser<char> parser;
			StringReader reader;
			IParseResult<char> result;

			reader = new StringReader("a");
			parser = Parse.Char('a').ZeroOrOneTime();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<char>);
			Assert.AreEqual('a', ((ISucceededParseResult<char>)result).Value);
			Assert.AreEqual(1, reader.Position);
		
		}
		[TestMethod]
		public void ShouldNotParseSingleItem()
		{
			IParser<char> parser;
			StringReader reader;
			IParseResult<char> result;

			reader = new StringReader("d");
			parser = Parse.Char('a').ZeroOrOneTime();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<char>);
			Assert.AreEqual(0, ((ISucceededParseResult<char>)result).EnumerateValue().Count());
			Assert.AreEqual(0, reader.Position);

		}
		[TestMethod]
		public void ShouldParseComplex()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			reader = new StringReader("adc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(0, reader.Position);

			reader = new StringReader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("abc", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("abcabc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("abc", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);
		}
		

		[TestMethod]
		public void ShouldNotParseComplexWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();

			reader = new StringReader("ab");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(0, reader.Position);
			
			reader = new StringReader("a");reader.Seek(1);
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParseComplex()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			reader = new StringReader("adc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(0, reader.Position);

			reader = new StringReader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("abc", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("abcabc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("abc", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);

		}

		

		[TestMethod]
		public void ShouldNotTryParseComplexWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();

			reader = new StringReader("ab");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(0, reader.Position);

			reader = new StringReader("a"); reader.Seek(1);
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(1, reader.Position);
		}




		[TestMethod]
		public void ShouldParseSimple()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			reader = new StringReader("dbc");
			parser = Parse.Char('a').ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(0, reader.Position);

			reader = new StringReader("abc");
			parser = Parse.Char('a').ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("a", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(1, reader.Position);

			reader = new StringReader("aa");
			parser = Parse.Char('a').ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("a", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldNotParseSimpleWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();

			reader = new StringReader("ab");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(0, reader.Position);

			reader = new StringReader("a"); reader.Seek(1);
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParseSimple()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			reader = new StringReader("adc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(0, reader.Position);

			reader = new StringReader("abc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("abc", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("abcabc");
			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("abc", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);

		}



		[TestMethod]
		public void ShouldNotTryParseSimpleWhenEOF()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			parser = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ZeroOrOneTime().ToStringParser();

			reader = new StringReader("ab");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(0, reader.Position);

			reader = new StringReader("a"); reader.Seek(1);
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(1, reader.Position);
		}


	}
}
