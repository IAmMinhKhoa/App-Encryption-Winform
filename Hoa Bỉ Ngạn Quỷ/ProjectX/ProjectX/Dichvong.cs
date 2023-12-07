using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ProjectX
{
    class Dichvong
    {
        public static string nguon = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        public static char[] P = nguon.ToCharArray();

        public static string Mahoa(string s, int k)
        {
            char[] banro = s.ToCharArray();
            int maso;
            int l = banro.Length;
            char[] temp = new char[l];
            int[] roso = new int[l];
            int j = 0;
            while (j < l)
            {
                bool isLowercase = char.IsLower(banro[j]); // Kiểm tra xem kí tự trong bản rõ có phải là chữ thường hay không
                if (char.IsWhiteSpace(banro[j])) // Kiểm tra xem kí tự trong bản rõ có phải là khoảng trắng hay không
                {
                    temp[j] = ' '; // Giữ nguyên khoảng trắng trong đầu ra
                }
                else
                {
                    char banroChar = banro[j];
                    char banmaChar;
                    if (char.IsUpper(banroChar))
                    {
                        banmaChar = P[(Array.IndexOf(P, char.ToUpper(banroChar)) + k) % 26];
                    }
                    else if (char.IsLower(banroChar))
                    {
                        banmaChar = P[(Array.IndexOf(P, char.ToLower(banroChar)) + k) % 26 + 26];
                    }
                    else
                    {
                        banmaChar = banroChar; // Kí tự không phải chữ cái, giữ nguyên trong đầu ra
                    }

                    temp[j] = banmaChar;
                }
                j++;
            }

            string banma = new string(temp);
            return banma;
        }

        public static string Giaima(string s, int k)
        {
            char[] banma = s.ToCharArray();
            int maso;
            int l = banma.Length;
            char[] temp = new char[l];
            int[] roso = new int[l];
            int j = 0;
            while (j < l)
            {
                bool isLowercase = char.IsLower(banma[j]); // Kiểm tra xem kí tự trong bản mã có phải là chữ thường hay không
                if (char.IsWhiteSpace(banma[j])) // Kiểm tra xem kí tự trong bản mã có phải là khoảng trắng hay không
                {
                    temp[j] = ' '; // Giữ nguyên khoảng trắng trong đầu ra
                }
                else
                {
                    char banmaChar = banma[j];
                    char banroChar;
                    if (char.IsUpper(banmaChar))
                    {
                        banroChar = P[(Array.IndexOf(P, char.ToUpper(banmaChar)) + P.Length - k) % 26];
                    }
                    else if (char.IsLower(banmaChar))
                    {
                        banroChar = P[(Array.IndexOf(P, char.ToLower(banmaChar)) + P.Length - k) % 26 + 26];
                    }
                    else
                    {
                        banroChar = banmaChar; // Kí tự không phải chữ cái, giữ nguyên trong đầu ra
                    }

                    temp[j] = banroChar;
                }
                j++;
            }

            string result = new string(temp);
            return result;
        }
    }
}
