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

        public static bool charIn(char x, string a)
        {
            foreach(var i in a)
            {
                if (i == x) return true;
            }
            return false;
        }
    }

    class SExpVisualizer
    {
        private SExp root;
        private StringBuilder content;
        private string name;

        public SExpVisualizer(SExp root, string name)
        {
            this.root = root;
            this.name = name;
            this.content = new StringBuilder();
        }
    }

    class Counter
    {
        private int count;
        public int GetCount() { return count; }
        public int tick() { return ++count; }
        public int jump(int length) { count += length; return count; }
        public Counter() { this.count = 0; }
        public void Reset() { count = 0; }
    }
}
