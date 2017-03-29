using System;
using System.Collections.Generic;
using System.Text;

namespace R7RSharp.Syntax
{
    public class Parser
    {
        private LexemeEnum LexPointer;
        public SRoot Root; 

        public Parser(Lexer lexer)
        {
            LexPointer = new LexemeEnum(lexer);
            new Parser(LexPointer);
        }

        public Parser(LexemeEnum pointer)
        {
            LexPointer = pointer;
            LexPointer.MoveNext();
            Root = new SRoot();
        }

        public void buildAll() // build the whole tree
        {
            Console.WriteLine("begin to build the whole tree");
            while (true)
            {
                JumpWhiteSpace();
                //Console.WriteLine("now the Lexeme is {0}", LexPointer.Current);
                var single = buildSingle(Root);
                if (single != null)
                {
                    Root.Children.Add(single);
                    continue;
                }
                //Console.WriteLine("For List \nnow the Lexeme is {0}", LexPointer.Current);
                var list = buildList(Root);
                if (list != null)
                {
                    Root.Children.Add(list);
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
            if (!LexPointer.Current.Equals(R7Lang.LEX_LPARE)) return null;
            LexPointer.MoveNext();
            var n = new SList(new LinkedList<SExp>(), father);
            while (true)
            {
                JumpWhiteSpace();
                var single = buildSingle(n);
                if (single != null)
                {
                    //Console.WriteLine("Root get a single {0}", single.ToString());
                    n.Children.AddLast(single);
                    continue;
                }

                var list = buildList(n);
                if (list != null)
                {
                    //Console.WriteLine("Root get a single {0}", list.ToString());
                    n.Children.AddLast(list);
                    continue;
                }
                break;
            }
            //Console.WriteLine("now the Lexeme is {0}", LexPointer.Current);
            if (!LexPointer.Current.Equals(R7Lang.LEX_RPARE)) throw new CompilerException("parser error, Unclosed (");
            LexPointer.MoveNext();
            
            return n;
        }

        private SExp buildSingle(SExp father)
        {
            var cur = LexPointer.Current as IToSExp;
            if (cur != null)
            {
                Console.WriteLine(cur);
                LexPointer.MoveNext();
                return cur.ToSExp(father);
            }
            return null;
        }

    }
}
