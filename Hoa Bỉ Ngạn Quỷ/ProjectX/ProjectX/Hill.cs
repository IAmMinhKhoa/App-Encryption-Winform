using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectX
{
    class Hill
    {
        public static string nguon = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static char[] P = nguon.ToCharArray();
        #region Build Func Hill Encrypt 
        public static float DetK(float a, float b, float c, float d)//Tinh DetK của ma trận cấp II
        {
            float k = Math.Abs((a * d) - (b * c));
            return k;
        }

        public static int Euclid_Extended (int a, int m)//tinh a^-1modX
        {
            int y = 0, y0 = 0, y1 = 1, result = 0;
            while (a > 0)
            {
                int r = m % a;
                if (r == 0)
                    break;
                int q = m / a;
                y = y0 - (y1 * q);
                m = a;
                a = r;
                y0 = y1;
                y1 = y;
            }
            if (a == 1) result = ((y+P.Length)%P.Length);
            
            return result;
        }

        public static float USCLN(float a, float b) //Tinh USCLN
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
        public static float[,] Taokhoa(int a, int b, int c, int d)//Tạo ma trận cấp 2 từ 4 textbox
        {
            float[,] key = new float[2, 2];
            key[0, 0] = a;
            key[0, 1] = b;
            key[1, 0] = c;
            key[1, 1] = d;
            return key;
        }
        public static float[,] MTbuDaiSo(int a, int b, int c, int d)//Tính ma trận bù đại số
        {
            float[,] s = new float[2, 2];
            s[0, 0] = d;
            s[0, 1] = -b;
            s[1, 0] = -c;
            s[1, 1] = a;
            
            return s;

        }
        public static float[,] Kmutru1 (float detK,float[,] ksao)//Tính ma trận nghịch đảo
        {
            float[,] result = new float[2,2];
            result[0, 0] = ((detK + P.Length) * ksao[0, 0]) % P.Length;
            float k12 = detK*ksao[0,1];
            while (k12 < 0)
            {
                k12 = k12 + P.Length;
            }
            result[0, 1] = k12 % P.Length;
            float k21 = detK * ksao[1, 0];
            while (k21 < 0)
            {
                k21 = k21 + P.Length;
            }
            result[1, 0] = k21 % P.Length;
            result[1, 1] = ((detK + P.Length) * ksao[1, 1]) % P.Length;
            return result;
        }
        public static float[] NhanMT(float[] a, float[,] b)
        {
            float[] c = new float[2];
            c[0] = (a[0] * b[0, 0] + a[1] * b[1, 0]) % P.Length;
            c[1] = (a[0] * b[0,1] + a[1] * b[1,1]) % P.Length;
            return c; 
        }
        public static int[] NhanBietXau (string s)
        {
            char[] banro = s.ToCharArray();            
            int l = banro.Length;
            int[] A = new int[banro.Length +1];            
            int[] temp = new int[l];
            int[] roso = new int[l];  
            
            for (int j = 0; j < l;j++ )
            {
                for (int i = 0; i < P.Length; i++)
                {
                    if (P[i] == banro[j])
                    {
                        roso[j] = i;
                        temp[j] = roso[j];
                    }                  
                }               
            }
            if (l % 2 != 0)
            {
                for (int k = 0; k < l; k++)
                {
                    A[k] = temp[k];
                }
                A[A.Length - 1] = 213;

                return A;
            }
                
            else return temp;
                            
            
        }
        public static float[,] TinhMTkhaNghich(float k, float[,] Ksao)
        {
            float[,] result = new float[2, 2];
            result[0, 0] = (k * Ksao[0, 0]) % P.Length;
            result[0, 1] = (k * Ksao[0, 1]) % P.Length;
            result[1, 0] = (k * Ksao[1, 0]) % P.Length;
            result[1, 1] = (k * Ksao[1, 1]) % P.Length;
            return result;
        }
        #endregion

       
        public static string Mahoa(int[] banro, float[,] khoa)//Code mã hóa Hill
        {
            float[] tmp = new float[banro.Length];
            float[] a= new float[2];
            float[] c = new float[2];
            char[] temp = new char[banro.Length];
            float[] roso = new float[banro.Length];
            float maso=0;
            for (int i = 0; i < banro.Length;i++ )
            {
                a[0] = (banro[i] ) % P.Length;
                a[1] = (banro[i+1]) % P.Length;
                c = NhanMT(a, khoa);
                tmp[i] = (c[0] % P.Length) ;
                tmp[i+1] = (c[1] % P.Length);
                i++;
            }
         
            for (int j = 0; j < banro.Length; j++)
            {                                                             
                        maso = tmp[j];
                        temp[j] = P[Convert.ToInt32(maso)];                  
             }
           
            string result = new string(temp);
            return result;
        }

        public static string Giaima(int[] banma, float[,] khoa)//Code giải mã Hill
        {
            float[] tmp = new float[banma.Length];
            float[] a= new float[2];
            float[] c = new float[2];
            float maso = 0;
            char[] temp = new char[banma.Length];
            for (int i = 0; i < banma.Length;i++ )
            {
                a[0] = (banma[i] ) % P.Length;
                a[1] = (banma[i+1]) % P.Length;
                c = NhanMT(a, khoa);
                tmp[i] = (c[0] % P.Length) ;
                tmp[i+1] = (c[1] % P.Length);
                i++;
            }

            for (int j = 0; j < banma.Length; j++)
            {
                maso = tmp[j];
                temp[j] = P[Convert.ToInt32(maso)];
            }

            string result = new string(temp);
            return result;
        }
        
     


        }
}
