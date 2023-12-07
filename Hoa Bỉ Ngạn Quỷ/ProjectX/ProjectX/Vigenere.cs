using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectX
{
    class Vigenere
    {
        public static string nguon = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static char[] P = nguon.ToCharArray();

        public static int[] ChuyenMaKey(string s)
        {
            char[] banro = s.ToUpper().ToCharArray(); // Chuyển hết thành chữ hoa
            int l = banro.Length;
            int[] temp = new int[l];
            int j = 0;
            while (j < l)
            {
                int index = Array.IndexOf(P, banro[j]);
                if (index != -1)
                {
                    temp[j] = index;
                }
                j++;
            }
            return temp;
        }

        public static int[] TaoKhoa(string vao, int[] key)
        {
            char[] banro = vao.ToUpper().ToCharArray();
            int l = banro.Length;
            int[] plant = new int[l];
            for (int i = 0; i < l; i++)
            {
                plant[i] = key[i % key.Length];
            }
            return plant;
        }

        public static string MaHoa(string s, int[] khoa)
        {
            char[] lengt = s.ToCharArray();
            int l = lengt.Length;
            char[] temp = new char[l];
            for (int j = 0; j < l; j++)
            {
                char c = lengt[j];
                if (char.IsWhiteSpace(c))
                {
                    temp[j] = ' '; // Giữ nguyên khoảng trắng trong đầu ra
                }
                else if (!IsLetter(c))
                {
                    temp[j] = c;
                }
                else if (char.IsUpper(c))
                {
                    int index = Array.IndexOf(P, char.ToUpper(c));
                    if (index != -1)
                    {
                        int roso = index;
                        int maso = (roso + khoa[j]) % P.Length;
                        if (maso < 0)
                        {
                            maso += 26; // Đảm bảo giá trị index không âm
                        }
                        temp[j] = P[maso];
                    }
                }
                else if (char.IsLower(c))
                {
                    int index = Array.IndexOf(P, char.ToUpper(c));
                    if (index != -1)
                    {
                        int roso = index;
                        int maso = (roso + khoa[j]) % P.Length;
                        if (maso < 0)
                        {
                            maso += 26; // Đảm bảo giá trị index không âm
                        }
                        temp[j] = char.ToLower(P[maso]);
                    }
                }

            }
            return new string(temp);
        }

        public static string GiaiMa(string s, int[] khoa)
        {
            char[] lengt = s.ToCharArray();
            int l = lengt.Length;
            char[] temp = new char[l];
            for (int j = 0; j < l; j++)
            {
                char c = lengt[j];
                if (char.IsWhiteSpace(c))
                {
                    temp[j] = ' '; // Giữ nguyên khoảng trắng trong đầu ra
                }
                else if (!IsLetter(c))
                {
                    temp[j] = c;
                }
                else if (char.IsUpper(c))
                {
                    int index = Array.IndexOf(P, char.ToUpper(c));
                    if (index != -1)
                    {
                        int roso = index;
                        int maso = ((roso - khoa[j]) + P.Length) % P.Length;
                        temp[j] = P[maso];
                    }
                }
                else if (char.IsLower(c))
                {
                    int index = Array.IndexOf(P, char.ToUpper(c));
                    if (index != -1)
                    {
                        int roso = index;
                        int maso = ((roso - khoa[j]) + P.Length) % P.Length;
                        temp[j] = char.ToLower(P[maso]);
                    }
                }
                else
                {
                    // Không thay đổi ký tự không phải chữ cái
                    temp[j] = c;
                }
            }
            return new string(temp);
        }

        public static bool IsLetter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }
    }
}