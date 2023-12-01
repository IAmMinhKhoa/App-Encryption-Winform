using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectX
{
    class autocode
    {
        public static string nguon = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static char[] P = nguon.ToCharArray();

        public static int[] ParseStringToIntArray(string line)
        {
            string[] parts = line.Split(",".ToCharArray(), //dấu phẩy là dấu ngăn cách cần bỏ đi
            StringSplitOptions.RemoveEmptyEntries);
            return Array.ConvertAll(parts, int.Parse);
        }
            
        public static int[] chuyenmabanro (string s)
        {
            char[] banro = s.ToCharArray();        
            int l = banro.Length;
            int[] temp = new int[l];
            int[] roso = new int[l];
            int j = 0;
            while (j < l)
            {
                for (int i = 0; i < P.Length; i++)
                {
                    if (P[i] == banro[j])
                    {
                        roso[j] = i;                        
                        temp[j] = roso[j];
                    }
                }
                j++;
            }
            return temp ;
        }

        public static int[] taokhoa(string vao, int k)
        {
            char[] banro = vao.ToCharArray();
            int l = banro.Length;
            int[] temp = new int[l];
            int[] roso = new int[l];
            int j = 0;
            int tg=k;
            int tmp;

            while (j < l)
            {
                for (int i = 0; i < P.Length; i++)
                {
                    if (P[i] == banro[j])
                    {
                        roso[j] = i;
                        temp[j] = roso[j];
                    }
                }
                j++;
            }
                       
            for (int i = 0; i <= l-1; i++)
            {
                tmp = temp[i];
                temp[i] = tg;
                tg = tmp;
            }
            
                return temp;
            
        }

        public static string Mahoa(string s, int k)
        {
            char[] lengt = s.ToCharArray();
            int l = lengt.Length;
            int[] khoa = taokhoa(s, k);                      
            int[] roso = new int[l];
            char[] temp = new char[l];
            int maso;           
            for (int j = 0; j < l; j++)
            {
                for (int i = 0; i < P.Length; i++)
                {
                    if (P[i] == lengt[j])
                    {
                        roso[j] = i;
                        maso = (roso[j]+khoa[j])%P.Length;
                        temp[j] = P[maso];                        
                    }
                }
            }

            string tg = new string(temp);
            return tg;

        }


        public static string Giaima(string s,int[] khoa)
        {
            char[] lengt = s.ToCharArray();
            int l = lengt.Length;
          // int[] khoa = taokhoa(s, k);
            int[] roso = new int[l];
            char[] temp = new char[l];
            int maso;
            for (int j = 0; j < l; j++)
            {
                for (int i = 0; i < P.Length; i++)
                {
                    if (P[i] == lengt[j])
                    {
                        roso[j] = i;
                        maso = ((roso[j]+P.Length)-khoa[j])%P.Length;
                        temp[j] = P[maso];
                    }
                }
            }

            string tg = new string(temp);
            return tg;
           

        }
    }
}
