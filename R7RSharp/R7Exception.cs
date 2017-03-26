using System;
using System.Collections.Generic;
using System.Text;

namespace R7RSharp
{
    class R7Exception: System.Exception
    {
        
    }

    public class CompilerException: Exception
    {
        public CompilerException() : base() { }
        public CompilerException(string message) : base(message) { }
        public CompilerException(string message, System.Exception inner) : base(message, inner) { }
        
    }
}
