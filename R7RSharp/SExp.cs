using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace R7RSharp
{
    public enum SExpKind
    {
        List,
        Int,
        Float,
        String,
        Symbol,
    }

    interface IToSExp
    {
        SExp ToSExp(SExp father);
    }

    public abstract class SExp
    {
        public SExp Father { get => Father; set => value = Father; }
        abstract public string ToDot(Counter ct);
    }

    class SRoot: SExp
    {
        public LinkedList<SExp> Children { get => Children; set => Children = value; }
        public SRoot()
        {
            Children = new LinkedList<SExp>();
        }

        public override string ToDot(Counter ct)
        {
            var sb = new StringBuilder("strict digraph {\n");
            var ids = new LinkedList<int>();
            foreach(var i in Children)
            {
                ids.AddLast(ct.GetCount() + 1);
                sb.Append(i.ToDot(ct));
            }
            if(ids.Count > 1)
            {

            }
            sb.Append("}");
            return sb.ToString();
        }

        public override string ToString()
        {
            return String.Join("" , from i in Children select i.ToString()); 
        }

    }

    class SInt: SExp
    {
        public int Value { get => Value; set => Value = value; }
        public SInt(int value ,SExp father)
        {
            this.Value = value;
            this.Father = father;
        }

        public override string ToDot(Counter ct)
        {
            var id = ct.tick();
            return String.Format("{0} [shape=invtriangle, label={0}];", id, Value);
        }

        public override string ToString()
        {
            return String.Format("SInt, Value:{0}\n", Value);
        }

    }

    class SFloat: SExp
    {
        public double Value { get => Value; set => Value = value; }
        public SFloat(double value, SExp father)
        {
            this.Value = value;
            this.Father = father;
        }
        public override string ToDot(Counter ct)
        {
            var id = ct.tick();
            return String.Format("{0} [label={1}]", id, Value);
        }
        public override string ToString()
        {
            return String.Format("SFloat, Value:{0}\n", Value);
        }
    }

    class SString: SExp
    {

        public string Value { get => Value; set => Value = value; }
        public SString(string value, SExp father)
        {
            this.Value = value;
            this.Father = father;
        }

        public override string ToDot(Counter ct)
        {
            var id = ct.tick();
            return String.Format("{0} [shape=box, label={1}];", id, Value);
        }

        public override string ToString()
        {
            return String.Format("SString, Value:{0}\n", Value);
        }
    }

    class SList: SExp
    {
        public LinkedList<SExp> Children { get => Children; set => Children = value; }
        public bool Quoted { get => Quoted; set => Quoted = value; }
        public SList(LinkedList<SExp> member, SExp father, bool quoted = false)
        {
            Father = father;
            Quoted = quoted;
            Children = member;
        }

        public override string ToDot(Counter ct)
        {
            var id = ct.tick();
            var sb = new StringBuilder();
            var ids = new LinkedList<int>();
            foreach(var i in Children)
            {
                ids.AddLast(ct.GetCount() + 1);
                sb.AppendLine(i.ToDot(ct));
            }
            foreach(var j in ids)
            {
                sb.AppendFormat("{0} -> {1} \n;", id, j);
            }
            return sb.ToString();
        }
        public override string ToString()
        {
            return String.Format("List, Quoted:{0}\n", Quoted) + String.Join("", from i in Children select "\t" + i.ToString());
        }
    }

    class SSymbol: SExp
    {
        public string Value { get => Value; set => Value = value; }
        public SSymbol(string value, SExp father)
        {
            this.Value = Value;
            this.Father = father;
        }
        public override string ToDot(Counter ct)
        {
            var id = ct.tick();
            return String.Format("{0} [shape=polygen, sides=5, label={1}]", id, Value) ;
        }
        public override string ToString()
        {
            return String.Format("SSymbol, Value:{0}\n", Value);
        }
    }
}
