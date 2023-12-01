using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectX
{
    public class Source
    {
       
        public static char[] StringToCharArray (string s)
        {
            char[] P = s.ToCharArray();
            return P;            
        }
        public static char[] Space ()
        {
            string S = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] P = S.ToCharArray();
            return P;

        }
    }
}
