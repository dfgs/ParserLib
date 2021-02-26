using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParserExtensionsUnitTest
	{
		[TestMethod]
		public void ShouldCheckParseParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => Parse.Char('a').Parse((string)null));
			Assert.ThrowsException<ArgumentNullException>(() => Parse.Char('a').Parse((IReader)null));
		}
		[TestMethod]
		public void ShouldCheckOrParameters()
		{
			Assert.ThrowsException<ArgumentNullException>(() => Parse.Char('a').Or(null));
		}

	}
}
