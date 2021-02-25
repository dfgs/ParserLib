using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLib
{
    public delegate IParseResult<T> Parser<T>(IReader<T> Reader);
}
