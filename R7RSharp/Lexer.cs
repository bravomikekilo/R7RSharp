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
            var n = forward();
            if (Lexeme.isEOF(n)) { return false; }
            else { cache.Add(n); }
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
            pos = -1;
            this.content = content;
        }

        private bool check(Delegate)
        
        private Lexeme forward() {
            var isSpace = parseWhiteSpace();
            if (isSpace) { return Lexeme.WhiteSpace();};
            if(pos >= content.Length) { return Lexeme.EOF();}
            var comment = parserComment();
            if (comment != null) return comment;

            return new Lexeme();
        }
        
        private bool parseWhiteSpace()
        {
            pos = pos < 0 ? 0 : pos;
            if (pos >= content.Length || !R7Lang.isWhiteSpaceChar(content[pos])) return false;
            for (; pos < content.Length; ++pos) 
            {
                if (!R7Lang.isWhiteSpaceChar(content[pos])) { return true; }
            }
            return false;
        }
        
        private Lexeme parseNumber()
        {
            return null;
        }

        private static readonly Regex IntPattern = new Regex("^[]");

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
