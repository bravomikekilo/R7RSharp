using System;
using System.Collections.Generic;
using System.Text;

namespace R7RSharp.Interprerter {
    class Interpreter {
        private SRoot Root;
        private SymbolTable St;

        public Interpreter(SRoot root, SymbolTable preload) {
            Root = root;
            St = preload;
        }

        public void EvalRoot() {
            foreach(var i in Root.Children) {
                Eval(i);
            }
        }

        private R7Object Eval(SExp input) {
            if (input == null) return null;
            var root = input as SList;
            if(root.Children.Count == 0) return null;
            var en = root.Children.GetEnumerator();
            var sym = Eval(en.Current);
            var args = new R7List();
            while(en.MoveNext()){
                args.PushBack(Eval(en.Current));
            }
            return null;
        }
    }
}
