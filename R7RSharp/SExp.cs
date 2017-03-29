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
        public SExp Father;
        public SExp(SExp father) { Father = father; }
        abstract public string ToDot(Counter ct);
    }

    public class SRoot: SExp
    {
        public List<SExp> Children;
        public SRoot():base(null)
        {
            Children = new List<SExp>();
            Console.WriteLine("prepare to build a link list");
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
            return String.Join("body \n" , from i in Children select i.ToString()); 
        }

    }

    public class SInt: SExp
    {
        public int Value;
        public SInt(int value ,SExp father):base(father)
        {
            this.Value = value;
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

    public class SFloat: SExp
    {
        public double Value;
        public SFloat(double value, SExp father):base(father)
        {
            this.Value = value;
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

    public class SString: SExp
    {

        public string Value; 
        public SString(string value, SExp father):base(father)
        {
            this.Value = value;
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

    public class SList: SExp
    {
        class SNode{
            public SExp Value;
        }
        public LinkedList<SExp> Children;
        public bool Quoted;
        public SList(LinkedList<SExp> member, 
            SExp father, bool quoted = false):base(father)
        {
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
            var sum = String.Join("", from i in Children select i.ToString()).Trim();
            return String.Format("List, Quoted:{0}\n", Quoted) + String.Join("",
                    from i in sum.Split('\n') select "  " + i + "\n");
        }
    }

    public class SSymbol: SExp
    {
        public string Value;
        public SSymbol(string value, SExp father):base(father)
        {
            this.Value = value;
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
