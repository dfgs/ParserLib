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
			Parser<string> a;

			a = Parse.Char('a');

			Parser<string> parser =
				from _a in a
				select _a + "2";

			Assert.AreEqual("a2", parser.Parse("ab"));
		}
		[TestMethod]
		public void ShouldParseMany()
		{
			Parser<string> a;
			Parser<string> b;


			a = Parse.Char('a');
			b = Parse.Char('b').OneOrMoreTimes();

			Parser<string> parser =
				from _a in a
				from _b in b
				select _a + "2" + _b;

			Assert.AreEqual("a2bb", parser.Parse("abb"));
		}

		[TestMethod]
		public void ShouldParseAndTransform()
		{
			Parser<string> a;
			Parser<int> b;
			Tuple<string, int> result;

			a = Parse.Char('a');
			b = from value in Parse.Char('1').OneOrMoreTimes() select Convert.ToInt32(value);

			Parser<Tuple<string,int>> parser =
				from _a in a
				from _b in b
				select new Tuple<string, int>(_a,_b);

			result = parser.Parse("a111");
			Assert.AreEqual("a", result.Item1);
			Assert.AreEqual(111, result.Item2);
		}


	}

}