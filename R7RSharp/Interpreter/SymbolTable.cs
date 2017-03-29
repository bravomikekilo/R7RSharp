using System;
using System.Collections.Generic;

namespace R7RSharp.Interprerter
{
    public delegate SExp SFunction(SList args);
    public class SymbolTable
    {
        private Dictionary<String ,SFunction> _table;
        public SFunction Get(string key) => _table[key];

        public void Add(string key, SFunction func){
            _table[key] = func;
        }

        public SymbolTable(Dictionary<String, SFunction> table){
            _table = table;
        }
       
    }
}
