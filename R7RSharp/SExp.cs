using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

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
        SExp ToSExp();
    }

    public abstract class SExp
    {
        abstract public string ToDot(Counter ct);
        //abstract public string ToExt();
    }

    public class SRoot: SExp
    {
        public List<SExp> Children;
        public SRoot()
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
        public SInt(int value)
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
        public SFloat(double value)
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
        public SString(string value)
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

    public class SList: SExp, IEnumerable<SExp>
    {
        class SNode{
            public SExp Value;
            public SNode Next;

            public SNode(SExp exp, SNode next){
                this.Value = exp;
                this.Next = next;
            }
        }
        private SNode _head;
        private SNode _last;
        private int count;
        public int Count{get => count;}
        public bool Quoted;

        public SList(bool quoted = false){
            this.Quoted = quoted;
            this._head = null; this._last = null; 
        }
        private SList(SNode head, int count){
            this._head = head;
            this.count = count;
        }


        public void Add(SExp exp){
            var h = new SNode(exp, _head);
            _head = h; ++count;
            if(_last == null) _last = _head;
        }

        public void PushBack(SExp exp){
            if(_head == null) {Add(exp); return;}
            var n = new SNode(exp, null);
            _last.Next = n; _last = n; ++count;
        }

        public SExp head(){
            return _head.Value;
        }

        public SExp Tail(){
            return new SList(_head.Next, count - 1);
        }

        public override string ToDot(Counter ct)
        {
            var id = ct.tick();
            var sb = new StringBuilder();
            var ids = new LinkedList<int>();
            foreach(var i in this)
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
            var sum = String.Join("", from i in this select i.ToString()).Trim();
            return String.Format("List, Quoted:{0}\n", Quoted) + String.Join("",
                    from i in sum.Split('\n') select "  " + i + "\n");
        }

        class SListEn : IEnumerator<SExp>
        {
            public SExp Current => pt.Value;

            object IEnumerator.Current => pt.Value;

            private SNode pt;
            private SNode head;
            public SListEn(SNode head){
                this.head = head;
                this.pt = null;
            }
            public void Dispose(){}

            public bool MoveNext()
            {
                if(head == null) return false;
                if(pt == null) { pt = head; return true; }
                if(pt.Next == null) return false;
                pt = pt.Next; return true;
            }

            public void Reset()
            {
                pt = null;
            }
        }

        public IEnumerator<SExp> GetEnumerator()
        {
            return new SListEn(_head);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class SSymbol: SExp
    {
        public string Value;
        public SSymbol(string value)
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
