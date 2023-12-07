using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectX
{
    class Affine
    {

        public static string nguon = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static char[] P = nguon.ToCharArray();

        public static int USCLN(int a, int b) //Tính ước số chung lớn nhất
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            if (a == 0 || b == 0)
                return a + b;
            while (a != b)
            {
                if (a > b)
                    a = a - b;
                else
                    b = b - a;
            }
            return a;
        }

        public static int A1modm(int a, int m) //Tính a^-1 mod m
        {
            int m0 = m;
            int y = 0, x = 1;

            if (m == 1)
                return 0;

            while (a > 1)
            {
                int q = a / m;
                int t = m;

                m = a % m;
                a = t;
                t = y;

                y = x - q * y;
                x = t;
            }

            if (x < 0)
                x += m0;

            return x;
        }

        public static string Mahoa(string s, int a, int b)
        {
            char[] banro = s.ToCharArray();
            int maso = 0;
            int l = banro.Length;
            char[] temp = new char[l];
            int[] roso = new int[l];
            for (int j = 0; j < l; j++)
            {
                if (char.IsWhiteSpace(banro[j]))
                {
                    temp[j] = ' ';
                    continue;
                } else if (!IsLetter(banro[j]))
                {
                    temp[j] = banro[j];
                    continue;
                }    

                bool isUpperCase = Char.IsUpper(banro[j]);

                for (int i = 0; i < P.Length; i++)
                {
                    if (isUpperCase && Char.IsLower(P[i]))
                        continue;

                    if (!isUpperCase && Char.IsUpper(P[i]))
                        continue;

                    if (Char.ToUpper(P[i]) == Char.ToUpper(banro[j]))
                    {
                        roso[j] = i;
                        maso = ((roso[j] * a) + b) % P.Length;
                        temp[j] = P[maso];
                    }
                }
            }
            string banma = new string(temp);
            return banma;
        }

        public static string Giaima(string s, int a, int b)
        {
            char[] banma = s.ToCharArray();
            int maso = 0;
            int l = banma.Length;
            char[] temp = new char[l];
            int[] roso = new int[l];
            int k = Affine.A1modm(a, P.Length);
            for (int j = 0; j < l; j++)
            {
                if (char.IsWhiteSpace(banma[j]))
                {
                    temp[j] = ' ';
                    continue;
                }
                else if (!IsLetter(banma[j]))
                {
                    temp[j] = banma[j];
                    continue;
                }

                bool isUpperCase = Char.IsUpper(banma[j]);

                for (int i = 0; i < P.Length; i++)
                {
                    if (isUpperCase && Char.IsLower(P[i]))
                        continue;

                    if (!isUpperCase && Char.IsUpper(P[i]))
                        continue;

                    if (Char.ToUpper(P[i]) == Char.ToUpper(banma[j]))
                    {
                        roso[j] = i;
                        maso = ((k + P.Length) * (roso[j] - b + P.Length)) % P.Length;
                        temp[j] = P[maso];
                    }
                }
            }
            string banro = new string(temp);
            return banro;
        }
        public static bool IsLetter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }
        /*    public static int test2()
            {
                int g = P.Length;
                return g;
            }
            public static char[] Sources ()
            {
                return P;
            }
           public static string test3(string s)
           {
               char[] banro = s.ToCharArray();
               int maso = 0;

               char[] temp = new char[banro.Length];
               for (int j = 0; j < banro.Length; j++)
               {
                   for (int i = 0; i < P.Length; i++)
                   {
                       if (P[i] == banro[j])
                       {

                           maso = i;
                       }
                   }
               }

               string ss = maso.ToString();
               return ss;
           }*/
    }
}
