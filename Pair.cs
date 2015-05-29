using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManWP
{
   public class Pair
    {
       public int Start { get; set; }
       public int End { get; set; }
       public Pair(int start, int end)
       {
           Start = start; End = end;
       }

       public Pair()
       {
           // TODO: Complete member initialization
       }
        public bool Intersect(int s, int e)
       {
           bool contains = false;
           if ((e>=Start&&Start>=s) || (s>=Start&&End>=e) ||(s<=End&&e>=End) ||(Start>=s&&End<=e))
           {
               contains = true;
           }
           return contains;
       }
    }
}
