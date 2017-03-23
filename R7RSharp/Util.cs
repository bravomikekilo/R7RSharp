using System;
using System.Collections.Generic;
using System.Text;

namespace R7RSharp
{
    class Util
    {
        public static bool headIs(string x, int index, char h)
        {
            if (x.Length <= index) return false;
            return x[index] == h;
        }

        public static bool headIs(string x, int index, string h)
        {
            if(x.Length <= index || x.Length - index < h.Length) { return false; }
            int i = index;
            foreach(var c in h)
            {
                if(x[i] != c)
                {
                    return false;
                }
                ++i;
            }
            return true;
        }
    }
}
