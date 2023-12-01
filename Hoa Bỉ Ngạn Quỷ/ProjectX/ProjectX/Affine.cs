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

        
        public static int USCLN(int a, int b) //Tinh uoc so chung lon nhat
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



        public static int A1modm(int a, int m)//tinh a^-1modX
         {
             int y = 0,y0=0,y1=1,result=0;
             while (a > 0)
             {
                 int r = m % a;
                     if(r==0)
                         break;
                    int q = m/a ;
                    y = y0-(y1*q) ;
                    m = a;
                    a= r;
                    y0= y1;
                    y1 = y;
             }
             if (a == 1) result = ((y + P.Length) % P.Length);
             return result;                               
          }


        public static string Mahoa(string s, int a, int b) //Thuật toán mã hóa Affine
         {
             char[] banro = s.ToCharArray();
             int maso=0;
             int l = banro.Length;
             char[] temp = new char[l];
             int[] roso = new int[l];            
             for (int j = 0; j < l;j++ )
             {
                 for (int i = 0; i < P.Length; i++)
                 {
                     if (P[i] == banro[j])
                     {
                         roso[j] = i;
                         maso = (roso[j] * a + b) % P.Length;
                         temp[j] = P[maso];
                        
                     }
                 }
             }
             string banma = new string(temp);
             return banma;

         }


        public static string Giaima(string s, int a, int b) //Thuật toán giải mã Affine
         {
             char[] banma = s.ToCharArray();
             int maso=0;         
             int l = banma.Length;
             char[] temp = new char[l];
             int[] roso = new int[l];
             int k = Affine.A1modm(a, P.Length);
             for(int j = 0;j<l ; j++)             
             {
                 for (int i = 0; i< P.Length; i++)
                 {
                     if (P[i]==banma[j])
                     {
                         roso[j] = i;
                         maso = ((k+P.Length)*(roso[j]-b+P.Length)) % P.Length;
                         temp[j] = P[maso];                   
                     }
                 }
             }
             string banro = new string(temp);
             return banro ;
         }
        public static string test (int n)
         {
             int t = Affine.A1modm(n, P.Length);
             string s = t.ToString();
             return s;
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
