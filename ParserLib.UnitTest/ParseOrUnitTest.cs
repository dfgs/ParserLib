using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseOrUnitTest
	{
		[TestMethod]
		public void ShouldParse_Single_Single()
		{
			ISingleParser<char> a, b;
			ISingleParser<char> parser;
			StringReader reader;

			a = Parse.Char('a');
			b = Parse.Char('b');
			parser = a.Or(b);

			reader = new StringReader("a");
			Assert.AreEqual('a', parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);

			reader = new StringReader("b");
			Assert.AreEqual('b', parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}
		[TestMethod]
		public void ShouldParse_Multiple_Multiple()
		{
			IMultipleParser<char> a, b;
			IMultipleParser<char> parser;
			char[] result;
			StringReader reader;

			a = Parse.Char('a').OneOrMoreTimes();
			b = Parse.Char('b').OneOrMoreTimes();
			parser = a.Or(b);

			reader = new StringReader("aaa");
			result = parser.Parse(reader).ToArray();
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("aaa", string.Concat(result));
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("bbb");
			result = parser.Parse(reader).ToArray();
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual("bbb", string.Concat(result));
			Assert.AreEqual(3, reader.Position);
		}
		
		[TestMethod]
		public void ShouldParse()
		{
			ISingleParser<string> a, b;
			ISingleParser<string> parser;
			StringReader reader;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d')).ToStringParser();
			parser = a.Or(b);

			reader = new StringReader("abc");
			Assert.AreEqual("abc", parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("abd");
			Assert.AreEqual("abd", parser.Parse(reader));
			Assert.AreEqual(3, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParse()
		{
			ISingleParser<string> a, b;
			ISingleParser<string> parser;
			StringReader reader;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d')).ToStringParser();
			parser = a.Or(b);

			reader = new StringReader("abe");
			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			ISingleParser<string> a, b;
			ISingleParser<string> parser;
			StringReader reader;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d')).ToStringParser();
			parser = a.Or(b);

			reader = new StringReader("aab"); reader.Seek(1);
			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			ISingleParser<string> a, b, parser;
			StringReader reader;
			IParseResult result;



			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d')).ToStringParser();
			parser = a.Or(b);

			reader = new StringReader("abc");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("abc", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);

			reader = new StringReader("abd");
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult<string>);
			Assert.AreEqual("abd", ((ISucceededParseResult<string>)result).Value);
			Assert.AreEqual(3, reader.Position);

		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			ISingleParser<string> a, b, parser;
			StringReader reader;
			IParseResult result;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d')).ToStringParser();
			parser = a.Or(b);

			reader = new StringReader("abe");
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<string>);
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			ISingleParser<string> a, b, parser;
			StringReader reader;
			IParseResult result;

			a = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('d')).ToStringParser();
			parser = a.Or(b);

			reader = new StringReader("ab"); reader.Seek(1);
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<string>);
			Assert.AreEqual(1, reader.Position);
		}


		[TestMethod]
		public void ShouldReturnHigherErrorPos()
		{
			ISingleParser<string> a, b, parser;
			StringReader reader;
			IParseResult result;

			a = Parse.Char('a').Then(Parse.Char('a')).Then(Parse.Char('a')).ToStringParser();
			b = Parse.Char('a').Then(Parse.Char('b')).Then(Parse.Char('c')).ToStringParser();

			parser = a.Or(b);
			reader = new StringReader("aae");
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<string>);
			Assert.AreEqual(2, ((UnexpectedCharParseResult)result).Position);


			parser = b.Or(a);
			reader = new StringReader("aae");
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<string>);
			Assert.AreEqual(2, ((UnexpectedCharParseResult)result).Position);
		}




	}
}
