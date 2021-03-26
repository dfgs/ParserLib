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
			Parser<IPAddress> parser;
			Reader reader;

			reader = new Reader("192.168.0.1");
			parser = Parse.IPAddress();

			Assert.AreEqual(IPAddress.Parse("192.168.0.1"), parser.Parse(reader));
			Assert.AreEqual(11, reader.Position);
		}
		[TestMethod]
		public void ShouldNotParse()
		{
			Parser<IPAddress> parser;
			Reader reader;

			reader = new Reader("192.256.0.1");
			parser = Parse.IPAddress();

			Assert.ThrowsException<UnexpectedCharException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}

		[TestMethod]
		public void ShouldNotParseWhenEOF()
		{
			Parser<IPAddress> parser;
			Reader reader;

			reader = new Reader("192");
			parser = Parse.IPAddress();

			Assert.ThrowsException<EndOfReaderException>(() => parser.Parse(reader));
			Assert.AreEqual(0, reader.Position);
		}


		[TestMethod]
		public void ShouldTryParse()
		{
			Parser<IPAddress> parser;
			Reader reader;
			ParseResult<IPAddress> result;

			reader = new Reader("192.168.0.1");
			parser = Parse.IPAddress();
			result = parser.TryParse(reader);
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(IPAddress.Parse("192.168.0.1"), result.Value);
			Assert.AreEqual(11, reader.Position);
		}

		[TestMethod]
		public void ShouldNotTryParse()
		{
			Parser<IPAddress> parser;
			Reader reader;
			ParseResult<IPAddress> result;

			reader = new Reader("192.256.0.1");
			parser = Parse.IPAddress();
			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(0, reader.Position);
		}

		
		[TestMethod]
		public void ShouldNotTryParseWhenEOF()
		{
			Parser<IPAddress> parser;
			Reader reader;
			ParseResult<IPAddress> result;

			reader = new Reader("192."); reader.Seek(1);
			parser = Parse.IPAddress();

			result = parser.TryParse(reader);
			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual(null, result.Value);
			Assert.AreEqual(1, reader.Position); 

			
		}
		

		
	}
}
