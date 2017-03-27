using System;
using System.Collections.Generic;
using System.Text;

namespace R7RSharp.Syntax
{
    class Parser
    {
        private LexemeEnum LexPointer;
        public SRoot Root { get => Root; set => Root = value; }

        public Parser(Lexer lexer)
        {
            LexPointer = new LexemeEnum(lexer);
            Root = new SRoot();
        }

        public Parser(LexemeEnum pointer)
        {
            LexPointer = pointer;
            Root = new SRoot();
        }

        public void buildAll() // build the whole tree
        {
            while (true)
            {
                JumpWhiteSpace();

                var single = buildSingle(Root);
                if (single != null)
                {
                    Root.Children.AddLast(single);
                    continue;
                }

                var list = buildList(Root);
                if (list != null)
                {
                    Root.Children.AddLast(list);
                    continue;
                }
                break;
            }
        }

        private void JumpWhiteSpace()
        {
            while (Lex.isWhiteSpace(LexPointer.Current) || Lex.isComment(LexPointer.Current))
            {
                LexPointer.MoveNext();
            }
        }

        private SList buildList(SExp father)
        {
            if (LexPointer.Current != R7Lang.LEX_LPARE) return null;
            LexPointer.MoveNext();
            var n = new SList(new LinkedList<SExp>(), father);
            while (true)
            {
                JumpWhiteSpace();
                var single = buildSingle(n);
                if (single != null)
                {
                    n.Children.AddLast(single);
                    continue;
                }

                var list = buildList(n);
                if (list != null)
                {
                    n.Children.AddLast(list);
                    continue;
                }
                break;
            }
            if (LexPointer.Current != R7Lang.LEX_RPARE) throw new CompilerException("parser error, Unclosed (");
            LexPointer.MoveNext();
            
            return null;
        }

        private SExp buildSingle(SExp father)
        {
            var cur = LexPointer.Current as IToSExp;
            if (cur != null) return cur.ToSExp(father);
            return null;
        }

    }
}
