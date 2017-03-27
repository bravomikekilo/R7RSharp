using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace R7RSharp
{
    public class Lexer: IEnumerable<Lexeme>
    {
        
        public bool extendTo(int targetIndex)
        {
            for(int i = cache.Count; i < targetIndex; ++i)
            {
                var n = this.forward();
                if (Lex.isEOF(n)) { return false; }
                else { cache.Add(n); }
            }
            return true;
        }

        public bool extendNext()
        {
            if (cache.Count > 0 && Lex.isEOF(cache[cache.Count - 1])) return false;
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
            if (pos >= content.Length) return new EOF();

            var space = parseWhiteSpace();
            if (space != null) return space;

            var misc = parseMisc();
            if (misc != null) return misc;

            var comment = parserComment();
            if (comment != null) return comment;

            var number = parseNumber();
            if (number != null) return number;

            var str = parseString();
            if (str != null) return str;

            var iden = parseIden();
            if (iden != null) return iden;

            throw new CompilerException("Lexer error, unable to continue");
        }

        private Lexeme parseString()
        {
            if(content[pos] == R7Lang.DQUTO)
            {
                var p = ++pos;
                var f = p;
                var sb = new StringBuilder();
                try
                {
                    for (;;++p)
                    {
                        if (content[p] == R7Lang.BACKSLASH)
                        {
                            if (p > f)
                            {
                                sb.Append(content, f, p - f);
                                ++p;
                                if (R7Lang.ControlTable.ContainsKey(content[p])) { sb.Append(R7Lang.ControlTable[content[p]]); f = ++p; }
                                else throw new CompilerException("string parsing error, Unknown escape character!"); 
                            }
                        }
                        if (content[p] == R7Lang.DQUTO)
                        {
                            if (p > f) sb.Append(content, f, p - f);
                            pos = ++p;
                            return new Str(sb.ToString());
                        }
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    throw new CompilerException("string parsing error, unclosed string found!");
                }
            }
            return null;
        }
        
        private Lexeme parseWhiteSpace()
        {
            if (!Char.IsWhiteSpace(content[pos])) return null;
            for (; pos < content.Length; ++pos) 
            {
                if (!Char.IsWhiteSpace(content[pos])) { return new WhiteSpace(); }
            }
            return new WhiteSpace();
        }
        
        private static readonly Regex NumberPattern = new Regex(@"\G[1-9][0-9]*\.?[0-9]*");
        private Lexeme parseNumber()
        {
            //if (!Char.IsDigit(content[pos])) { return null; }
            //Console.WriteLine("begin to parse number!");
            var match = NumberPattern.Match(content, pos);
            if (match.Success)
            {
                //Console.WriteLine("match a number-like string");
                var temp = match.Value;
                try
                {
                    var value = Int32.Parse(temp);
                    //Console.WriteLine("parse a Int, Length is {0}", match.Length);
                    pos += match.Length;
                    return new Int(value);
                }
                catch (FormatException)
                {
                    try
                    { 
                        var value = Double.Parse(temp);
                        //Console.WriteLine("parse a Float");
                        pos += match.Length;
                        return new Float(value);
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

        private static readonly Regex IdenPattern = new Regex(@"\G[a-zA-Z_!$%&*+-./:<=>?@^_~][0-9a-zA-Z_!$%&*+-./:<=>?@^_~]*");
        private Lexeme parseIden()
        {
            var match = IdenPattern.Match(content, pos);
            if (match.Success)
            {
                var con = content.Substring(pos, match.Length);
                pos += match.Length;
                return new Iden(con);
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
                    var con = content.Substring(pos + 1);
                    pos = content.Length;
                    return new Comment(con);
                }
                else
                {
                    var ret = new Comment(content.Substring(pos + 1, changeLine - pos));
                    pos = ++changeLine;
                    return ret;
                }
            }
            return null;  // TODO: need to add support for #; and block comment
        }

        private Lexeme parseMisc()
        {
            var head = content[pos];
            if (head == R7Lang.LPARE) { ++pos; return new Keyword(R7Lang.KEYWORDS.LPARE); };
            if (head == R7Lang.RPARE) { ++pos; return new Keyword(R7Lang.KEYWORDS.RPARE); };
            if (head == R7Lang.SQUTO) { ++pos; return new Keyword(R7Lang.KEYWORDS.SQUTO); };

            if (head == R7Lang.SHARP)
            {
                try
                {
                    var match = IdenPattern.Match(content, ++pos);
                    if (match.Success)
                    {
                        var con = content.Substring(pos - 1, match.Length+1);
                        pos += match.Length;
                        return new Iden(con);
                    }
                    else --pos;
                }
                catch(System.IndexOutOfRangeException)
                {
                    throw new CompilerException("Lexer error, unable to parse #");
                }

            }
            return null;
        }
    }

    public class LexemeEnum : IEnumerator<Lexeme>
    {
        public Lexeme Current => cache;

        object IEnumerator.Current => this.cache;


        public bool MoveNext()
        {
            if (cache != null && Lex.isEOF(cache)) { return false; }
            ++index;
            cache = producer[index];
            return true;
        }

        public void Reset()
        {
            index = -1;
            cache = null;
        }

        public void Dispose() { }

        public LexemeEnum(Lexer producer)
        {
            this.index = -1;
            this.cache = null;
            this.producer = producer;
        }

        private Lexeme cache;
        private int index;
        private Lexer producer;
    }
}
