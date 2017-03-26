using System;
using System.Collections.Generic;
using System.Text;

namespace R7RSharp.Syntax
{
    class Parser
    {
        private LexemeEnum LexPointer;
        public SExp Tree { get => Tree; set => Tree = value; }

        public Parser(Lexer lexer)
        {
            LexPointer = new LexemeEnum(lexer);
            Tree = new SRoot();
        }

        public Parser(LexemeEnum pointer)
        {
            LexPointer = pointer;
            Tree = new SRoot();
        }

        public void buildAll() // build the whole tree
        {
            
        }

        private void JumpWhiteSpace()
        {
            while (Lexeme.isWhiteSpace(LexPointer.Current))
            {
                LexPointer.MoveNext();
            }
        }

        private SList buildList(SExp father)
        {
            return null;
        }

    }
}
