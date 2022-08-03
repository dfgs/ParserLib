using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace ParserLib.UnitTest
{
	[TestClass]
	public class ParseIPAddressUnitTest
	{
		[TestMethod]
		public void ShouldParse()
		{
			IParser<IPAddress> parser;
			StringReader reader;

			reader = new StringReader("192.168.0.1");
			parser = Parse.IPAddress();

			Assert.AreEqual(IPAddress.Parse("192.168.0.1"), parser.Parse(reader));
			Assert.AreEqual(11, reader.Position);
		}
		[TestMethod]
		public void ShouldNotParse()
		{
			IParser<IPAddress> parser;
			StringReader reader;

			reader = new StringReader("192.256.0.1");
			parser = Parse.IPAddress();

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			IParser<IPAddress> parser;
			StringReader reader;

			reader = new StringReader("192");
			parser = Parse.IPAddress();

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			IParser<IPAddress> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("192.168.0.1");
			parser = Parse.IPAddress();
			result = parser.TryParse(reader);
			Assert.IsTrue(result is ISucceededParseResult);
			Assert.AreEqual(IPAddress.Parse("192.168.0.1"), ((ISucceededParseResult<IPAddress>)result).Value);
			Assert.AreEqual(11, reader.Position);
		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			IParser<IPAddress> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("192.256.0.1");
			parser = Parse.IPAddress();
			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult);
			Assert.AreEqual(0, reader.Position);
		}

		
		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			IParser<IPAddress> parser;
			StringReader reader;
			IParseResult result;

			reader = new StringReader("192."); reader.Seek(1);
			parser = Parse.IPAddress();

			result = parser.TryParse(reader);
			Assert.IsFalse(result is ISucceededParseResult);
			Assert.AreEqual(1, reader.Position); 

			
		}
		

		
	}
}
