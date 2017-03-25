using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace R7RSharp
{
    class Lexer: IEnumerable<Lexeme>
    {
        
        public bool extendTo(int targetIndex)
        {
            for(int i = cache.Count; i < targetIndex; ++i)
            {
                var n = this.forward();
                if (Lexeme.isEOF(n)) { return false; }
                else { cache.Add(n); }
            }
            return true;
        }

        public bool extendNext()
        {
            if (cache.Count > 0 && Lexeme.isEOF(cache[cache.Count - 1])) return false;
            var n = forward();
            cache.Add(n);
            return true;
        }

        public IEnumerator<Lexeme> GetEnumerator()
        {
            return new LexemeEnum(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Lexeme this[int index]
        {
            get
            {
                if (index < cache.Count)
                {
                    return cache[index];
                }
                else
                {
                    for(int i = cache.Count; i < index; ++i)
                    {
                        cache.Add(forward());
                    }
                    var lastLexeme = forward();
                    cache.Add(lastLexeme);
                    return lastLexeme;
                }
            }

        }

        private readonly string content;
        private int pos;
        private List<Lexeme> cache;

        public Lexer(string content)
        {
            cache = new List<Lexeme>();
            pos = 0;
            this.content = content;
        }

        //private bool check(Delegate)
        
        private Lexeme forward() {
            if (pos >= content.Length) return Lexeme.EOF();
            var space = parseWhiteSpace();
            if (space != null) { return space;};
            var comment = parserComment();
            if (comment != null) return comment;
            var number = parseNumber();
            if (number != null) return number;
            var iden = parseIden();
            if (iden != null) return iden;

            return new Lexeme();
        }
        
        private Lexeme parseWhiteSpace()
        {
            if (pos >= content.Length || !Char.IsWhiteSpace(content[pos])) return null;
            for (; pos < content.Length; ++pos) 
            {
                if (!Char.IsWhiteSpace(content[pos])) { return Lexeme.WhiteSpace(); }
            }
            return Lexeme.WhiteSpace();
        }
        
        private static readonly Regex IntPattern = new Regex(@"\G[1-9][0-9]*\.?[0-9]*");
        private Lexeme parseNumber()
        {
            //if (!Char.IsDigit(content[pos])) { return null; }
            //Console.WriteLine("begin to parse number!");
            var match = IntPattern.Match(content, pos);
            if (match.Success)
            {
                //Console.WriteLine("match a number-like string");
                var temp = match.Value;
                try
                {
                    var value = Int32.Parse(temp);
                    //Console.WriteLine("parse a Int, Length is {0}", match.Length);
                    pos += match.Length;
                    return Lexeme.Int(value);
                }
                catch (FormatException)
                {
                    try
                    { 
                        var value = Double.Parse(temp);
                        //Console.WriteLine("parse a Float");
                        pos += match.Length;
                        return Lexeme.Float(value);
                    }
                    catch (FormatException)
                    {
                        //Console.WriteLine("fail to parse the string!");
                        return null;
                    }
                }
            }
            //Console.WriteLine("can't match a number-like string!");
            return null;
        }

        private static readonly Regex IdenPattern = new Regex(@"\G[a-zA-Z_]\w*");
        private Lexeme parseIden()
        {
            var match = IdenPattern.Match(content, pos);
            if (match.Success)
            {
                var con = content.Substring(pos, match.Length);
                if (R7Lang.Keywords.ContainsKey(con))
                {
                    return Lexeme.Keyword(R7Lang.Keywords[con]);
                }
                pos += match.Length;
                return Lexeme.Iden(con);
            }
            return null ;
        }

        private Lexeme parserComment()
        {
            if(content[pos] == R7Lang.SEMICOLON)
            {
                var changeLine = content.IndexOf(R7Lang.CHANGELINE, pos);
                if (changeLine < 0)
                {
                    return Lexeme.Comment(content.Substring(pos + 1));
                }
                else
                {
                    var ret = Lexeme.Comment(content.Substring(pos + 1, changeLine - pos));
                    pos = ++changeLine;
                    return ret;
                }
            }
            return null;  // TODO: need to add support for #; and block comment
        }
    }

    class LexemeEnum : IEnumerator<Lexeme>
    {
        public Lexeme Current => cache;

        object IEnumerator.Current => this.cache;


        public bool MoveNext()
        {
            if (producer.extendNext())
            {
                ++index;
                cache = producer[index];
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            this.index = -1;
            this.cache = new Lexeme();
        }

        public void Dispose() { }

        public LexemeEnum(Lexer producer)
        {
            this.index = -1;
            this.cache = new Lexeme();
            this.producer = producer;
        }

        private Lexeme cache;
        private int index;
        private Lexer producer;
    }
}
