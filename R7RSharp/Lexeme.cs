using System;
using System.Collections.Generic;
using System.Text;

namespace R7RSharp
{
    public abstract class Lexeme: IEquatable<Lexeme>
    {
        abstract public bool Equals(Lexeme other);
    }


    public class Lex
    {
        public static bool isIden(Lexeme lex) => lex.GetType() == typeof(Iden); 
        public static bool isWhiteSpace(Lexeme lex) => lex.GetType() == typeof(WhiteSpace); 
        public static bool isComment(Lexeme lex) => lex.GetType() == typeof(Comment); 
        public static bool isInt(Lexeme lex) => lex.GetType() == typeof(Int); 
        public static bool isFloat(Lexeme lex) => lex.GetType() == typeof(Float); 
        public static bool isStr(Lexeme lex) => lex.GetType() == typeof(Str); 
        public static bool isKeyword(Lexeme lex) => lex.GetType() == typeof(Keyword); 
        public static bool isEOF(Lexeme lex) => lex.GetType() == typeof(EOF); 
    }

    public class Iden: Lexeme, IEquatable<Lexeme>, IToSExp
    {
        private readonly string Value;
        public string get() => Value;
        public Iden(string value) { Value = value; }
        public override bool Equals(Lexeme other)
        {
            var o = other as Iden;
            return o != null && Value == o.Value;
        }

        public SExp ToSExp() => new SSymbol(Value);
        public override string ToString() => String.Format("{{Iden, Value:{0}}}", Value); 
    }

    public class WhiteSpace: Lexeme, IEquatable<Lexeme>
    {
        public WhiteSpace() { }
        public override bool Equals(Lexeme other)
        {
            var o = other as WhiteSpace;
            return o != null;
        }
        public override string ToString() => "{WhiteSpace}";
    }

    public class EOF: Lexeme, IEquatable<Lexeme>
    {
        public EOF() { }
        public override bool Equals(Lexeme other)
        {
            var o = other as EOF;
            return o != null;
        }
        public override string ToString() => "{EOF}";
    }


    public class Comment: Lexeme, IEquatable<Lexeme>
    {
        private readonly string Value;
        public string get() => Value;
        public Comment(string value) { Value = value; }
        public override bool Equals(Lexeme other)
        {
            var o = other as Comment;
            return o != null && Value == o.Value;
        }
        public override string ToString() => String.Format("{{Comment, Value:{0}}}", Value); 
    }

    public class Int : Lexeme, IEquatable<Lexeme>, IToSExp
    {
        public override bool Equals(Lexeme other)
        {
            var o = other as Int;
            return o != null && Value == o.Value;
        }
        private readonly int Value;
        public int get() => Value;

        public SExp ToSExp() => new SInt(Value);
            

        public Int(int value){ Value = value; }
        public override string ToString() => String.Format("{{Int, Value:{0}}}", Value); 
    }

    public class Float : Lexeme, IEquatable<Lexeme>, IToSExp
    {
        public override bool Equals(Lexeme other)
        {
            var o = other as Float;
            return o != null && Value == o.Value;
        }

        public SExp ToSExp() => new SFloat(Value);

        private double Value;
        public Float(double value){ Value = value; }
        public override string ToString() => String.Format("{{Float, Value:{0}}}", Value); 
    }

    public class Str : Lexeme, IEquatable<Lexeme>, IToSExp
    {
        public override bool Equals(Lexeme other)
        {
            var o = other as Str;
            return o != null && Value == o.Value;
        }

        public SExp ToSExp() => new SString(Value);

        private string Value;
        public Str(string value){ Value = value; }
        public override string ToString() => String.Format("{{Str, Value:{0}}}", Value); 
    }

    public class Keyword : Lexeme, IEquatable<Lexeme>
    {
        public override bool Equals(Lexeme other)
        {
            var o = other as Keyword;
            return o != null && Value == o.Value;
        }
        private readonly R7Lang.KEYWORDS Value;
        public R7Lang.KEYWORDS get() => Value;

        public Keyword(R7Lang.KEYWORDS value){ Value = value; }
        public override string ToString() => String.Format("{{Keyword, Value:{0}}}", Value); 
    }
}
