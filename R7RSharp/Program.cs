using System;

namespace R7RSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //var input = "   abc 10.1+1.2 () [] ";
            var input = "  1 ";
            var lex = new Lexer(input);
            foreach(var i in lex)
            {
                Console.WriteLine(i);
            }
        }
    }
}