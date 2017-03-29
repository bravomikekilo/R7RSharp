
namespace R7RSharp.R7Lib{
    public class R7Std{
        public static SExp Add(SList args){
            double s = 0;
            foreach(var i in args){
                var num = i as SInt;
                if(num != null) {
                    s += num.Value;
                    continue;
                }
                var fl = i as SFloat;
                if(fl != null){ 
                    s += fl.Value;
                    continue;
                }
            }
            return new SFloat(s);
        }
    }
}