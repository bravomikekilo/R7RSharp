
namespace R7RSharp.Interprerter
{
    abstract class R7Object
    {
        
    }

    class R7List: R7Object
    {
        class R7Node{
            public R7Object Value;
            public R7Node Next;
            public R7Node(R7Object obj, R7Node next){
                this.Value = obj;
                this.Next = next;
            }
        }
        public int count;
        private R7Node _head;
        private R7Node _last;

        public R7List(){
            this._head = null;
            this._last = null;
            this.count = 0;
        }

        public bool isEmpty(){
            return _head == null;
        }

        private R7List(R7Node head, int count){
            this._head = head;
            this.count = count;
        }
        
        public void Add(R7Object obj){
            var h = new R7Node(obj, _head);
            _head = h; ++count;
            if(_last == null) _last = _head;
        }

        public void PushBack(R7Object obj){
            if(_head == null) {Add(obj); return;}
            var n = new R7Node(obj, null);
            _last.Next = n; _last = n; ++count;
        }

        public R7Object Head(){
            return _head.Value;
        }
        public R7List Tail(){
            return new R7List(_head.Next, count - 1); 
        }
        
    }

    class R7Int: R7Object
    {

    }

    class R7Float: R7Object
    {

    }

    class R7String: R7Object
    {

    }

    class R7Port: R7Object
    {

    }

    class R7Vector: R7Object
    {

    }

    class R7Function: R7Object
    {

    }

    class R7Symbol: R7Object
    {

    }

}
