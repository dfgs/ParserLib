using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseReaderIncludesUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			IParser<string> parser;
			StringReader reader;

			reader = new StringReader("abc def",' ');
			parser = Parse.Except(' ').OneOrMoreTimes().ReaderIncludes(' ').ToStringParser();

			// should not ignore ' ' and read abcdef
			Assert.AreEqual("abc", parser.Parse(reader));
		}
		




	}
}
