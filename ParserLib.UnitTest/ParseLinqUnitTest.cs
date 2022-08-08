using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseLinqUnitTest
	{
		[TestMethod]
		public void ShouldSelect_SingleParser()
		{
			ISingleParser<char> a;
			ISingleParser<string> parser;
			string result;

			a = Parse.Char('a');

			parser = a.Select(_a => _a + "2");
			result =parser.Parse("ab");
			Assert.AreEqual("a2", result);

			parser = from _a in a
					select _a + "2";

			result = parser.Parse("ab");
			Assert.AreEqual("a2", result);
		}
		[TestMethod]
		public void ShouldSelect_MultipleParser()
		{
			IMultipleParser<char> a;
			ISingleParser<string> parser;
			string result;

			a = Parse.Char('a').OneOrMoreTimes();

			parser = a.Select(_a => string.Concat(_a) + "2");
			result = parser.Parse("aaab");
			Assert.AreEqual("aaa2", result);

			parser = from _a in a
					 select string.Concat(_a) + "2";

			result = parser.Parse("aaab");
			Assert.AreEqual("aaa2", result);
		}

		
		[TestMethod]
		public void ShouldSelectMany_SingleParser_SingleParser()
		{
			ISingleParser<char> a;
			ISingleParser<byte> b;
			ISingleParser<string> parser;


			a = Parse.Char('a');
			b = Parse.Byte();

			parser = a.SelectMany<char, byte, string>(_a => b, (_a, _b) => _a + "_" + _b);
			Assert.AreEqual("a_128", parser.Parse("a128"));

			parser =
				from _a in a
				from _b in b
				select _a + "_" + _b;
			Assert.AreEqual("a_128", parser.Parse("a128"));
		}
		[TestMethod]
		public void ShouldSelectMany_MultipleParser_MultipleParser()
		{
			IMultipleParser<char> a;
			IMultipleParser<byte> b;
			ISingleParser<string> parser;


			a = Parse.Char('a').OneOrMoreTimes();
			b = Parse.Digit().OneOrMoreTimes();

			parser = a.SelectMany<char, byte, string>(_a => b, (_a, _b) => string.Concat(_a) + "_" + string.Concat(_b));
			Assert.AreEqual("aaa_128", parser.Parse("aaa128"));

			parser =
				from _a in a
				from _b in b
				select string.Concat(_a) + "_" + string.Concat(_b);
			Assert.AreEqual("aaa_128", parser.Parse("aaa128"));
		}

		[TestMethod]
		public void ShouldSelectMany_SingleParser_MultipleParser()
		{
			ISingleParser<char> a;
			IMultipleParser<byte> b;
			ISingleParser<string> parser;


			a = Parse.Char('a');
			b = Parse.Digit().OneOrMoreTimes();

			parser = a.SelectMany<char, byte, string>(_a => b, (_a, _b) => string.Concat(_a) + "_" + string.Concat(_b));
			Assert.AreEqual("a_128", parser.Parse("a128"));

			parser =
				from _a in a
				from _b in b
				select _a + "_" + string.Concat(_b);
			Assert.AreEqual("a_128", parser.Parse("a128"));
		}

		[TestMethod]
		public void ShouldSelectMany_MultipleParser_SingleParser()
		{
			IMultipleParser<char> a;
			ISingleParser<byte> b;
			ISingleParser<string> parser;


			a = Parse.Char('a').OneOrMoreTimes();
			b = Parse.Digit();

			parser = a.SelectMany<char, byte, string>(_a => b, (_a, _b) => string.Concat(_a) + "_" +_b);
			Assert.AreEqual("aaa_1", parser.Parse("aaa1"));

			parser =
				from _a in a
				from _b in b
				select string.Concat(_a) + "_" + _b;
			Assert.AreEqual("aaa_1", parser.Parse("aaa1"));
		}


		[TestMethod]
		public void ShouldParseAndTransform()
		{
			ISingleParser<char> a;
			ISingleParser<int> b;
			Tuple<char, int> result;

			a = Parse.Char('a');
			b = from value in Parse.Char('1').OneOrMoreTimes().ToStringParser() select Convert.ToInt32(value);

			ISingleParser<Tuple<char, int>> parser =
				from _a in a
				from _b in b
				select new Tuple<char, int>(_a,_b);

			result = parser.Parse("a111");
			Assert.AreEqual('a', result.Item1);
			Assert.AreEqual(111, result.Item2);
		}
		[TestMethod]
		public void ShouldReturnHigherErrorPos()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult result;

			parser =
				from _a in Parse.String("Item")
				from _b in Parse.Char('(')
				from _c in (Parse.Char('a').Or(Parse.Char('b'))).ZeroOrMoreTimes().ToStringParser()
				from _d in Parse.Char(')')
				select _c;

			reader = new StringReader("Item(abac)");
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult<string>);
			Assert.AreEqual(8, ((UnexpectedCharParseResult)result).Position);

		}

	}

}