﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
 
    public abstract class Parser<T>:IParser<T>
    {
		public string Description
		{
			get;
			private set;
		}

		protected ParserDelegate<T> parserDelegate;

		public Parser(string Description,ParserDelegate<T> ParserDelegate)
		{
			this.Description = Description;
			this.parserDelegate = ParserDelegate;
		}

		
		public IParseResult TryParse(string Value, params char[] IgnoredChars)
		{
			if (Value == null) throw new ArgumentNullException(nameof(Value));
			return TryParse(new StringReader(Value,IgnoredChars));
		}
		public IParseResult TryParse(IReader Reader, params char[] IncludedChars)
		{
			IParseResult result;
			long position;

			if (Reader == null) throw new ArgumentNullException(nameof(Reader));

			position = Reader.Position;
			result = parserDelegate(Reader, IncludedChars);
			if (!(result is ISucceededParseResult<T>)) Reader.Seek(position);

			return result;
		}

		public override string ToString()
		{
			return Description;
		}


	}


}
