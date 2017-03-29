using System;

namespace R7RSharp.Interprerter {
    public class Interpreter {
        private SRoot Root;
        private SymbolTable St;

        public Interpreter(SRoot root, SymbolTable preload) {
            Root = root;
            St = preload;
        }

        public void EvalRoot() {
            foreach(var i in Root.Children) { Console.WriteLine(Eval(i)); }
        }

        private SExp Eval(SExp input) {
            if (input == null) return null;
            var root = input as SList;
            if(root == null) return input;
            if(root.Count == 0) return null;
            var en = root.GetEnumerator();
            en.MoveNext();
            var sym = Eval(en.Current) as SSymbol;
            if(sym == null) throw new CompilerException("error ,cannot get symbol");
            var args = new SList();
            while(en.MoveNext())
            {
                args.PushBack(Eval(GetCurrent(en)));
            }
            return St.Get(sym.Value)(args);
        }

        private static SExp GetCurrent(System.Collections.Generic.IEnumerator<SExp> en)
        {
            return en.Current;
        }
    }
}
