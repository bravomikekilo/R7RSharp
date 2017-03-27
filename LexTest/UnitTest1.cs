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

    class CustomEquality<T> : IEqualityComparer<T> where T : IEquatable<T>
    {
        public bool Equals(T x, T y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }

    public class LexTest1
    {
        private static readonly string SourceOne = "(* (+ 4 5) 3)";
        private static List<Lexeme> LexemeOne = new List<Lexeme>
        {
            new Keyword(R7Lang.KEYWORDS.LPARE),
            new Iden("*"),
            new WhiteSpace(),
            new Keyword(R7Lang.KEYWORDS.LPARE),
            new Iden("+"),
            new WhiteSpace(),
            new Int(4),
            new WhiteSpace(),
            new Int(5),
            new Keyword(R7Lang.KEYWORDS.RPARE),
            new WhiteSpace(),
            new Int(3),
            new Keyword(R7Lang.KEYWORDS.RPARE),
            new EOF()
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
            var a = new List<Lexeme> { new Int(2), new Int(3), new Int(4) };
            var b = new List<Lexeme> { new Int(2), new Int(3), new Int(4) };
            Assert.Equal(a, b, new ListComparer<Lexeme>());
        }
        
        [Fact]
        public void LexemeEqualityTest()
        {
            var a = new Int(2);
            var b = new Int(2);
            Assert.Equal(a, b, new CustomEquality<Lexeme>());
        }
    }
}
