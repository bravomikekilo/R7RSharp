using System;
using System.Collections.Generic;
using System.Text;

namespace R7RSharp.Interprerter
{
    class Interpreter
    {
        private SRoot Root;
        private SymbolTable St;

        public Interpreter(SRoot root, SymbolTable preload)
        {
            Root = root;
            St = preload;
        }

        public void EvalRoot() {
            foreach(var i in Root.Children)
            {
                
            }
        }

        private void Eval(SExp input)
        {
            if (input == null) return;
            var root = input as SList;
            if (input == null) return;
        }

    }
}
