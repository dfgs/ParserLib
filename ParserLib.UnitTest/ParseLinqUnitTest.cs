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
		public void ShouldParse()
		{
			IParser<string> a;

			a = Parse.Char('a');

			IParser<string> parser =
				from _a in a
				select _a + "2";

			Assert.AreEqual("a2", parser.Parse("ab"));
		}
		[TestMethod]
		public void ShouldParseMany()
		{
			IParser<string> a;
			IParser<string> b;


			a = Parse.Char('a');
			b = Parse.Char('b').OneOrMoreTimes();

			IParser<string> parser =
				from _a in a
				from _b in b
				select _a + "2" + _b;

			Assert.AreEqual("a2bb", parser.Parse("abb"));
		}

		[TestMethod]
		public void ShouldParseAndTransform()
		{
			IParser<string> a;
			IParser<int> b;
			Tuple<string, int> result;

			a = Parse.Char('a');
			b = from value in Parse.Char('1').OneOrMoreTimes() select Convert.ToInt32(value);

			IParser<Tuple<string,int>> parser =
				from _a in a
				from _b in b
				select new Tuple<string, int>(_a,_b);

			result = parser.Parse("a111");
			Assert.AreEqual("a", result.Item1);
			Assert.AreEqual(111, result.Item2);
		}
		[TestMethod]
		public void ShouldReturnHigherErrorPos()
		{
			IParser<string> parser;
			StringReader reader;
			IParseResult<string> result;

			parser =
				from _a in Parse.String("Item")
				from _b in Parse.Char('(')
				from _c in (Parse.Char('a').Or(Parse.Char('b'))).ZeroOrMoreTimes()
				from _d in Parse.Char(')')
				select _c;

			reader = new StringReader("Item(abac)");
			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(8, ((UnexpectedCharParseResult<string>)result).Position);

		}

	}

}