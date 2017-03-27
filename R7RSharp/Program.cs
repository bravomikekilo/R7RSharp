using System;
using System.Collections.Generic;

namespace R7RSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //var input = "   abc 10.1+1.2 () [] ";
            var input = @"(* (+ 4 5) 3 2)";
            var lex = new Lexer(input);
            foreach(var i in lex)
            {
                Console.WriteLine(i);
            }
            var LexE = new LexemeEnum(lex);
            var parser = new Syntax.Parser(LexE);
            parser.buildAll();
            Console.WriteLine(parser.Root);
        }
    }
}