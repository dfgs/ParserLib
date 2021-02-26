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
				select _a.ToString() + "2";

			Assert.AreEqual("a2", parser.Parse("ab"));
		}
		[TestMethod]
		public void ShouldParseMany()
		{
			Parser<string> a;
			Parser<string> b;

			//"aaa".SelectMany()

			a = Parse.Char('a');
			b = Parse.Char('b').Many();
			//"adedaz".SelectMany();

			Parser<string> parser =
				from _a in a
				from _b in b
				select _a.ToString() + "2" + _b;

			Assert.AreEqual("a2bb", parser.Parse("abb"));
		}

		//*/
	}

}