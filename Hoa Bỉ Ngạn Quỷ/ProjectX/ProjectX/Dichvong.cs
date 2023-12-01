using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ProjectX
{
    class Dichvong
    {
        public static string nguon = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static char[] P = nguon.ToCharArray();

        public static string Mahoa (string s, int k)
        {
            char[] banro = s.ToCharArray();
            int maso;
            int l = banro.Length;
            char[] temp = new char[l];
            int[] roso = new int[l];
            int j = 0;
            while (j < l)
            {
                for (int i = 0; i < P.Length; i++)
                {
                    if (P[i] == banro[j])
                    {
                        roso[j] = i;
                        maso = (roso[j] + k) % P.Length;
                        temp[j] = P[maso];
                    }
                }
                j++;
            }

            string banma = new string(temp);
            return banma;
        }


        public static string Giaima (string s, int k)
        {
            char[] banro = s.ToCharArray();
            int maso;
            int l = banro.Length;
            char[] temp = new char[l];
            int[] roso = new int[l];
            int j = 0;
            while (j < l)
            {
                for (int i = 0; i < P.Length; i++)
                {
                    if (P[i] == banro[j])
                    {
                        roso[j] = i;
                        maso = (roso[j] + P.Length - k) % P.Length;
                        temp[j] = P[maso];
                    }
                }
                j++;
            }

            string result = new string(temp);
            return result;
        }
       
    }
}
