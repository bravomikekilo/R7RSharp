using System;
using System.Collections.Generic;
using Xunit;
using R7RSharp;

namespace LexTest
{
    class ListComparer<T> : IEqualityComparer<List<T>> where T: IEquatable<T>
    {
        public bool Equals(List<T> x, List<T> y)
        {
            IEnumerator<T> enX = x.GetEnumerator();
            IEnumerator<T> enY = y.GetEnumerator();
            while (true)
            {
                bool hasNextX = enX.MoveNext();
                bool hasNextY = enY.MoveNext();
                if (!hasNextX || !hasNextY)
                {
                    return (hasNextY == hasNextX);
                }
                if (!enX.Current.Equals(enY.Current)) return false;
            } 
        }

        public int GetHashCode(List<T> obj)
        {
            return obj.GetHashCode();
        }
    }

    public class LexTest1
    {
        private static readonly string SourceOne = "(* (+ 4 5) 3)";
        private static List<Lexeme> LexemeOne = new List<Lexeme>
        {
            Lexeme.Iden("("),
            Lexeme.Iden("*"),
            Lexeme.WhiteSpace(),
            Lexeme.Iden("("),
            Lexeme.Iden("+"),
            Lexeme.WhiteSpace(),
            Lexeme.Int(4),
            Lexeme.WhiteSpace(),
            Lexeme.Int(5),
            Lexeme.Iden(")"),
            Lexeme.WhiteSpace(),
            Lexeme.Int(3),
            Lexeme.Iden(")"),
            Lexeme.EOF()
        };

        public static List<Lexeme> getAll(string source)
        {
            var lexer = new Lexer(source);
            var ret = new List<Lexeme>();
            foreach (var i in lexer)
            {
                ret.Add(i);
            }
            return ret;
        }

        [Fact]
        public void LexerTest1()
        {
            Assert.Equal(LexemeOne, getAll(SourceOne), new ListComparer<Lexeme>());
        }

        [Fact]
        public void ListEqualityTest()
        {
            var a = new List<Lexeme> { Lexeme.Int(2), Lexeme.Int(3), Lexeme.Int(4) };
            var b = new List<Lexeme> { Lexeme.Int(2), Lexeme.Int(3), Lexeme.Int(4) };
            Assert.Equal(a, b, new ListComparer<Lexeme>());
        }
        
        [Fact]
        public void LexemeEqualityTest()
        {
            var a = Lexeme.Int(2);
            var b = Lexeme.Int(2);
            Assert.Equal(a, b);
        }
    }
}
