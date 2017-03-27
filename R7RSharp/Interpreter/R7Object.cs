using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace R7RSharp.Interprerter
{
    abstract class R7Object
    {
        
    }

    class R7List: R7Object
    {
        public LinkedList<R7Object> children; 
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
