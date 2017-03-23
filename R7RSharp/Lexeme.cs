using System;
using System.Collections.Generic;
using System.Text;

namespace R7RSharp
{
    class Lexeme
   {
        private LexType kind;
        private object content;

        enum LexType
        {
            Iden,
            WhiteSpace,
            Comment,
            Int,
            Float,
            String,
            KeyWord,
            EOF
        }

        public Lexeme()
        {
            this.content = null;
            this.kind = LexType.EOF;
        }

        public object get()
        {
            return this.content;
        }
        
        // helper function for static functions
        private Lexeme(LexType kind, Object content)
        {
            this.kind = kind;
            this.content = content;
        }
        
        public static Lexeme WhiteSpace() { return new Lexeme(LexType.WhiteSpace, null); }
        public static bool isWhiteSpace(Lexeme x) { return x.kind == LexType.WhiteSpace; }

        public static Lexeme EOF() { return new Lexeme(); }
        public static bool isEOF(Lexeme x) { return x.kind == LexType.EOF; }

        public static Lexeme Comment(string content = "") { return new Lexeme(LexType.Comment, content); }
        public static bool isComment(Lexeme x) { return x.kind == LexType.Comment; }

        public static Lexeme Int(int value) { return new Lexeme(LexType.Int, value); }
        public static bool isInt(Lexeme x) { return x.kind == LexType.Int; }

        public static Lexeme Float(double value) { return new Lexeme(LexType.Float, value); }
        public static bool isFloat(Lexeme x) { return x.kind == LexType.Float; }

        public static Lexeme KeyWord(string content) { return new Lexeme(LexType.KeyWord, content); }
        public static bool isKeyWord(Lexeme x) { return x.kind == LexType.KeyWord; }

        public static Lexeme Iden(string name) { return new Lexeme(LexType.Iden, name); }
        public static bool isIden(Lexeme x) { return x.kind == LexType.Iden; }

   }
}
