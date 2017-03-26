using System;
using System.Collections.Generic;
using System.Text;

namespace R7RSharp
{
    public class R7Lang
    {
        public static readonly char LPARE = '(';
        public static readonly char RPARE = ')';
        public const char SEMICOLON = ';';
        public static readonly char SHARP = '#';
        public static readonly char SQUTO = '\'';
        public static readonly char BQUTO = '`';
        public static readonly char COMMA = ',';
        public static readonly char DQUTO = '"';
        public static readonly char LSQUARE = '[';
        public static readonly char RSQUARE = ']';
        public static readonly char LCURLY = '{';
        public static readonly char RCURLY = '}';
        public static readonly char BACKSLASH = '\\';
        public static readonly char FORSLASH = '/';
        public static readonly char PLUS = '+';
        public static readonly char MINUS = '-';
        public static readonly char MULTI = '*';
        public static readonly char QUESTION = '?';
        public static readonly char RETURN = '\r';
        public static readonly char CHANGELINE = '\n';
        public static readonly char GT = '>';
        public static readonly char LT = '<';
        public static readonly char EQUAL = '=';
        public static readonly char DOT = '.';
        public static readonly char BANG = '!';
        public const string WhiteSpaceChar = " \t\r\n";
        public enum DELIMITER
        {
            LPARE = 40,
            RPARE = 41,
        }

        public enum KEYWORDS
        {
            define,
            lambda,
        }


        public static bool isWhiteSpaceChar(char a)
        {
            var ret = false;
            foreach (var i in WhiteSpaceChar) { if (i == a) ret = true; }
            return ret;
        }

        public static readonly Dictionary<char, char> ControlTable = new Dictionary<char, char>
            {
                {'n', '\n' },
                {'r', '\r' },
                {'t', '\t' },
                {'\\', '\\' },
                {'"', '"' }
            };
    }
}
