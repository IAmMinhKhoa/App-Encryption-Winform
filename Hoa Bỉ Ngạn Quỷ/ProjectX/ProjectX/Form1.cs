using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Threading;
using System.Text.RegularExpressions;

namespace ProjectX
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private bool active_key = false;
        private bool decryptable = true;
        int p, q, n, e, d, phi_n;

        #region Bat dau he ma dich vong
        //-----------------------------Bắt sự kiện cho tab Dịch vòng------------------------------------//
        private void Dichvong_btnClear_Click(object sender, EventArgs e)//Button reset tab dịch vòng
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }
        private void Button1_Click(object sender, EventArgs e)//Button Chọn file bản rõ
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(open.FileName);
                textBox1.Text = file.ReadToEnd();
                file.Close();
            }
        }

        private void Button3_Click(object sender, EventArgs e)//Button Lưu file bản mã
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (save.ShowDialog()==DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(save.FileName);
                file.Write(textBox5.Text);
                file.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)//Button mã hóa dịch vòng
        {
            mahoadichvong();
        }

        private void mahoadichvong()
        {
            int key = 0;
            if (textBox1.Text == "")   //chưa nhập bản rõ
            {
                MessageBox.Show("Vui lòng nhập bản rõ!", "Thông Báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox5.Text = "";
                return;
            }

            if (textBox3.Text == "") //chưa nhập khoá k
            {
                MessageBox.Show("Vui lòng nhập khoá để mã hoá!", "Thông Báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox5.Text = "";
                return;
            }
            else if (ContainsNonAlphanumericCharacters(textBox3.Text)) //Khoá k chỉ được chứa kí tự là chữ hoặc số
            {
                MessageBox.Show("Khoá K chỉ được chứa kí tự là chữ hoặc số!", "Thông Báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox3.Text = "";
                textBox5.Text = "";
                return;
            }

            if (IsNumber(textBox3.Text))  //khoá là số
            {
                key = int.Parse(textBox3.Text);
            }
            else //khoá là chuỗi
            {
                string message = "Khoá K bạn vừa nhập là chuỗi! \nỨng dụng sẽ quy đổi như sau:\n" +
                                    "- Các kí tự là chữ trong chuỗi sẽ được quy đổi thành số thứ tự trong bảng chữ cái Alphabet.\n" +
                                    "- Khoá K sẽ được tính bằng tổng các số sau khi đã quy đổi với các kí tự là số nếu có trong chuỗi.";
                MessageBox.Show(message, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                key = CalculateStringSum(textBox3.Text);
            }

            textBox5.Text = Dichvong.Mahoa(textBox1.Text, key);
            textBox15.Text = key.ToString();

        }

        static bool ContainsNonAlphanumericCharacters(string input)
        {
            // Kiểm tra xem chuỗi có chứa kí tự không phải chữ cái, số, khoảng trắng, dấu cộng hay dấu trừ hay không
            return Regex.IsMatch(input.Trim(), @"[^a-zA-Z0-9\s\+\-]+");
        }

        static bool ContainNumber(string input)
        {
            // Kiểm tra xem chuỗi có chứa số không
            return input.Any(c => char.IsDigit(c));
        }

        static bool IsNumber(string input)
        {
            // Kiểm tra xem chuỗi có phải là số nguyên
            int number;
            bool isNumeric = int.TryParse(input, out number);
            return isNumeric;
        }

        static int CalculateStringSum(string input)
        {
            int sum = 0;
            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    int letterValue = char.ToUpper(c) - 'A' + 1;
                    sum += letterValue;
                }
                else if (char.IsDigit(c))
                {
                    int digitValue = int.Parse(c.ToString());
                    sum += digitValue;
                }
            }
            if(sum > 26)
            {
                sum = sum % 26;
            }
            return sum;
        }



        private void button4_Click(object sender, EventArgs e)//Button giải mã dịch vòng
        {
            giaimadichvong();
        }
        private void giaimadichvong()
        {
            int key = 0;
            if (textBox4.Text == "")   //chưa nhập bản rõ
            {
                MessageBox.Show("Vui lòng nhập bản mã!", "Thông Báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox6.Text = "";
                return;
            }

            if (textBox2.Text == "") //chưa nhập khoá k
            {
                MessageBox.Show("Vui lòng nhập khoá để giải hoá!", "Thông Báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox6.Text = "";
                return;
            }
            else if (ContainsNonAlphanumericCharacters(textBox2.Text)) //Khoá k chỉ được chứa kí tự là chữ hoặc số
            {
                MessageBox.Show("Khoá K chỉ được chứa kí tự là chữ hoặc số!", "Thông Báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Text = "";
                textBox6.Text = "";
                return;
            }

            if (IsNumber(textBox2.Text))  //khoá là số
            {
                key = Convert.ToInt32(textBox2.Text);
            }
            else //khoá là chuỗi 
            {
                string message = "Khoá K bạn vừa nhập là chuỗi! \nỨng dụng sẽ quy đổi như sau:\n" +
                                    "- Các kí tự là chữ trong chuỗi sẽ được quy đổi thành số thứ tự trong bảng chữ cái Alphabet.\n" +
                                    "- Khoá K sẽ được tính bằng tổng các số sau khi đã quy đổi với các kí tự là số nếu có trong chuỗi.";
                MessageBox.Show(message, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                key = CalculateStringSum(textBox2.Text);
            }
            textBox6.Text = Dichvong.Giaima(textBox4.Text, key);       
            textBox16.Text = key.ToString();
        }

        private void button5_Click(object sender, EventArgs e)//Button chọn file bản mã
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(open.FileName);
                textBox4.Text = file.ReadToEnd();
                file.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)//Button lưu file bản mã
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(save.FileName);
                file.Write(textBox6.Text);
                file.Close();
            }
        }
        //---------------------------------Kết thúc đoạn mã Dịch vòng-----------------------------------//
        #endregion

        #region Bat dau he ma affine
        //Bắt sự kiện cho tab AFFINE      
        private void Affine_btnBrowerBanro_Click(object sender, EventArgs e) //Button Chọn file bản rõ
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(open.FileName);
                Affine_txtBanro.Text = file.ReadToEnd();
                file.Close();
            }
        }

        private void Affine_btnSaveBanro_Click(object sender, EventArgs e) //Button Lưu file bản rõ
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(save.FileName);
                file.Write(textBox8.Text);
                file.Close();
            }
        }

        private void Affine_btnBrowerBanma_Click(object sender, EventArgs e)//Button chọn file bản mã
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(open.FileName);
                Affine_txtBanma.Text = file.ReadToEnd();
                file.Close();
            }
        }

        private void Affine_btnSaveBanma_Click(object sender, EventArgs e)//Button lưu file bản mã
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(save.FileName);
                file.Write(textBox7.Text);
                file.Close();
            }
        }

        private void Affine_btnClear_Click(object sender, EventArgs e)//Button Clear affine
        {
            Affine_txtBanma.Text = " ";
            Affine_txtBanro.Text = " ";
            Affine_txtKhoaAbanma.Text = " ";
            Affine_txtKhoaAbanro.Text = " ";
            Affine_txtKhoaBbanma.Text = " ";
            Affine_txtKhoaBbanro.Text = " ";
            textBox7.Text = " ";
            textBox8.Text = " ";
        }

        private void Affine_btnMahoa_Click(object sender, EventArgs e)//Button mã hóa Affine
        {
            try
            {
                textBox8.Text = "";
                if (Affine_txtBanro.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập bản rõ!", "Error =.=!");
                    return;
                }
                if (Affine_txtKhoaAbanro.Text == "" || Affine_txtKhoaBbanro.Text == "")
                {
                    MessageBox.Show("Nhập đầy đủ giá trị a và b của khoá trước khi mã hoã!", "Error =.=!");
                    return;
                }
                int keya = Convert.ToInt32(Affine_txtKhoaAbanro.Text);
                int keyb = Convert.ToInt32(Affine_txtKhoaBbanro.Text);
                int usc = Affine.USCLN(keya, Affine.P.Length);
                if (keya >= 26 && keyb >= 26)
                {
                    MessageBox.Show("Giá trị a và b không được vượt quá không gian Z(26)", "Error =.=!");
                    Affine_txtKhoaAbanro.Text = "";
                    Affine_txtKhoaBbanro.Text = "";
                    return;
                } 
                else if (keya >= 26)
                {
                    MessageBox.Show("Giá trị a không được vượt quá không gian Z(214)", "Error =.=!");
                    Affine_txtKhoaAbanro.Text = "";
                    return;
                } else if (keyb >= 26)
                {
                    MessageBox.Show("Giá trị b không được vượt quá không gian Z(214)", "Error =.=!");
                    Affine_txtKhoaBbanro.Text = "";
                    return;
                }


                if (keya <= 0 && keyb <= 0)
                {
                    MessageBox.Show("Giá trị a và b phải lớn hơn hoặc bằng 1.", "Error =.=!");
                    Affine_txtKhoaAbanro.Text = "";
                    Affine_txtKhoaBbanro.Text = "";
                    return;
                }
                else if (keya <= 0)
                {
                    MessageBox.Show("Giá trị a phải lớn hơn hoặc bằng 1.", "Error =.=!");
                    Affine_txtKhoaAbanro.Text = "";
                    return;

                } else if (keyb <= 0)
                {
                    MessageBox.Show("Giá trị b phải lớn hơn hoặc bằng 1.", "Error =.=!");
                    Affine_txtKhoaBbanro.Text = "";
                    return;
                }

                if (usc != 1)
                {
                    MessageBox.Show("Hệ số a = " + keya + " không phù hợp.\nNhập khóa a sao cho USCLN(a,26)=1", "Error =.=!");
                    Affine_txtKhoaAbanro.Text = "";
                   
                    return;

                }

                textBox8.Text = Affine.Mahoa(Affine_txtBanro.Text.Trim(), keya, keyb);

            }
            catch (Exception)
            {
                MessageBox.Show("Nhập lại khóa(Nhập số nguyên)!", "Error =.=! "); 
                Affine_txtKhoaAbanro.Text = "";
                Affine_txtKhoaBbanro.Text = "";
            }                                       
        }

        private void Affine_btnGiaima_Click(object sender, EventArgs e)//Button giải mã Affine
        {
            try
            {
                textBox7.Text = "";
                if (Affine_txtBanma.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập bản mã!", "Error =.=!");
                    return;
                }
                if (Affine_txtKhoaAbanma.Text == "" || Affine_txtKhoaBbanma.Text == "")
                {
                    MessageBox.Show("Nhập đầy đủ giá trị a và b của khoá trước khi giả mã!", "Error =.=!");
                    return;
                }
                int keya = Convert.ToInt32(Affine_txtKhoaAbanma.Text);
                int keyb = Convert.ToInt32(Affine_txtKhoaBbanma.Text);
                int usc = Affine.USCLN(keya, Affine.P.Length);
                if (keya >= 26)
                {
                    MessageBox.Show("Giá trị a không được vượt quá không gian Z(26)", "Error =.=!");
                    Affine_txtKhoaAbanma.Text = "";
                    return;
                }
                else if (keyb >= 26)
                {
                    MessageBox.Show("Giá trị b không được vượt quá không gian Z(26)", "Error =.=!");
                    Affine_txtKhoaBbanma.Text = "";
                    return;
                } else if (keya >= 26 && keyb >= 26)
                {
                    MessageBox.Show("Giá trị a và b không được vượt quá không gian Z(26)", "Error =.=!");
                    Affine_txtKhoaAbanma.Text = "";
                    Affine_txtKhoaBbanma.Text = "";
                    return;
                }

                if (keya < 0)
                {
                    MessageBox.Show("Giá trị a phải lớn hơn hoặc bằng 1.", "Error =.=!");
                    Affine_txtKhoaAbanma.Text = "";
                    return;

                }
                else if (keyb < 0)
                {
                    MessageBox.Show("Giá trị b phải lớn hơn hoặc bằng 1.", "Error =.=!");
                    Affine_txtKhoaBbanma.Text = "";
                    return;
                }
                else if (keya < 0 && keyb < 0)
                {
                    MessageBox.Show("Giá trị a và b phải lớn hơn hoặc bằng 1.", "Error =.=!");
                    Affine_txtKhoaAbanma.Text = "";
                    Affine_txtKhoaBbanma.Text = "";
                    return;
                }

                if (usc != 1)
                {
                    MessageBox.Show("Hệ số a = " + keya + " không phù hợp.\nNhập khóa a sao cho USCLN(a,26)=1", "Error =.=!");
                    Affine_txtKhoaAbanma.Text = "";

                    return;

                }

                textBox7.Text = Affine.Giaima(Affine_txtBanma.Text.Trim(), keya, keyb);
            }

            catch (Exception)
            {
                MessageBox.Show("Nhập lại khóa(Nhập số nguyên)!", "Error =.=! "); 
                Affine_txtBanro.Text = "";
                Affine_txtKhoaAbanro.Text = "";
                Affine_txtKhoaBbanro.Text = "";
            }  
        }
        //----------------------------Ket thua ma Affine-------------------------------//
        #endregion

        #region Bat dau he ma Hill
        //----------------------------Bat dau ma Hill----------------------------------//
        private void Hill_btnBrowerBanro_Click(object sender, EventArgs e)//Button chọn file bản rõ
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(open.FileName);
                Hill_txtBanro.Text = file.ReadToEnd();
                file.Close();
            }
        }

        private void Hill_btnLuuFileBanro_Click(object sender, EventArgs e)//Button lưu file bản rõ
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(save.FileName);
                file.Write(textBox13.Text);
                file.Close();
            }
        }

        private void Hill_btnChonfileBanma_Click(object sender, EventArgs e)//Button chọn file bản mã
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(open.FileName);
                Hill_txtBanma.Text = file.ReadToEnd();
                file.Close();
            }
        }

        private void Hill_btnLuuFilebanma_Click(object sender, EventArgs e)//Button lưu file bản mã
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(save.FileName);
                file.Write(textBox14.Text);
                file.Close();
            }
        }

        private void Hill_btnClear_Click(object sender, EventArgs e)//Button Clear all textbox
        {
            Hill_txtBanro.Text = "";
            Hill_txtBanma.Text = "";
            Hill_txtKhoaAbanro.Text = "";
            Hill_txtKhoaBbanro.Text = "";
            Hill_txtKhoaCbanro.Text = "";
            Hill_txtKhoaDbanro.Text = "";
            Hill_txtKhoaAbanma.Text = "";
            Hill_txtKhoaBbanma.Text = "";
            Hill_txtKhoaCbanma.Text = "";
            Hill_txtKhoaDbanma.Text = "";                                                                                        
        }

        private void Hill_btnMahoa_Click(object sender, EventArgs e)//Button mã hóa Hill
        {
            try
            {
                int keya = Convert.ToInt32(Hill_txtKhoaAbanro.Text);
                int keyb = Convert.ToInt32(Hill_txtKhoaBbanro.Text);
                int keyc = Convert.ToInt32(Hill_txtKhoaCbanro.Text);
                int keyd = Convert.ToInt32(Hill_txtKhoaDbanro.Text);
                if (keya >= 214 || keya < 0 || keyb >= 214 || keyb < 0 || keyc >= 214 || keyc < 0 || keyd >= 214 || keyd < 0)
                {
                    MessageBox.Show("Lỗi nhập khóa.\n Nhập số nguyên không được vượt quá không gian Z(214)", "Error =.=!");
                    Hill_txtBanro.Text = "";
                    Hill_txtKhoaAbanro.Text = "";
                    Hill_txtKhoaBbanro.Text = "";
                    Hill_txtKhoaCbanro.Text = "";
                    Hill_txtKhoaDbanro.Text = "";
                }
                float detk = Hill.DetK(keya, keyb, keyc, keyd);
                float Check = Hill.USCLN(detk, 214);
                if (Check != 1)
                {
                    MessageBox.Show("Ma trận không hợp lệ, không tồn tại MT khả nghịch.\nNhập lại MT sao cho (|DetK|,214)=1 ", "Error =.=!");
                    Hill_txtBanro.Text = "";
                    Hill_txtKhoaAbanro.Text = "";
                    Hill_txtKhoaBbanro.Text = "";
                    Hill_txtKhoaCbanro.Text = "";
                    Hill_txtKhoaDbanro.Text = "";
                }
                int[] arc = Hill.NhanBietXau(Hill_txtBanro.Text);
                float[,] khoa = Hill.Taokhoa(keya, keyb, keyc, keyd);
                /*float[] arr = Hill.Mahoa(arc, khoa);
                string result = (arr == null) ? null : arr.Skip(1).Aggregate(arr[0].ToString(), (s, i) => s + "," + i.ToString());*/
                textBox13.Text = Hill.Mahoa(arc, khoa);
            }
            catch (Exception)
            {
                MessageBox.Show("Nhập giá trị khóa là số nguyên", "Error =.=!");
            }              
        }

         private void Hill_btnGiaima_Click(object sender, EventArgs e)//Button Giải mã Hill
        {
            try
            {
                int keya = Convert.ToInt32(Hill_txtKhoaAbanma.Text);
                int keyb = Convert.ToInt32(Hill_txtKhoaBbanma.Text);
                int keyc = Convert.ToInt32(Hill_txtKhoaCbanma.Text);
                int keyd = Convert.ToInt32(Hill_txtKhoaDbanma.Text);
                if (keya >= 214 || keya < 0 || keyb >= 214 || keyb < 0 || keyc >= 214 || keyc < 0 || keyd >= 214 || keyd < 0)
                {
                    MessageBox.Show("Lỗi nhập khóa.\n Nhập số nguyên không được vượt quá không gian Z(214)", "Error =.=!");
                    Hill_txtBanma.Text = "";
                    Hill_txtKhoaAbanma.Text = "";
                    Hill_txtKhoaBbanma.Text = "";
                    Hill_txtKhoaCbanma.Text = "";
                    Hill_txtKhoaDbanma.Text = "";
                }
                float detk = Hill.DetK(keya, keyb, keyc, keyd);
                int detkToint = Convert.ToInt32(detk);
                float Check = Hill.USCLN(detk, 214);
                if (Check != 1)
                {
                    MessageBox.Show("Ma trận không hợp lệ, không tồn tại MT khả nghịch.\nNhập lại MT sao cho (|DetK|,214)=1 ", "Error =.=!");
                    Hill_txtBanma.Text = "";
                    Hill_txtKhoaAbanma.Text = "";
                    Hill_txtKhoaBbanma.Text = "";
                    Hill_txtKhoaCbanma.Text = "";
                    Hill_txtKhoaDbanma.Text = "";
                }
                float EclidDetK = Hill.Euclid_Extended(detkToint, 214);
                float[,] MtBuDaiSo = Hill.MTbuDaiSo(keya, keyb, keyc, keyd);
                float[,] MTkhaNghich = Hill.Kmutru1(EclidDetK, MtBuDaiSo);
                int[] arc = Hill.NhanBietXau(Hill_txtBanma.Text);
                // float[] arr = Hill.Giaima(arc, MTkhaNghich);
                //string result = (arr == null) ? null : arr.Skip(1).Aggregate(arr[0].ToString(), (s, i) => s + "," + i.ToString());
               textBox14.Text = Hill.Giaima(arc, MTkhaNghich);
                /* for (int i = 0; i < MTkhaNghich.GetLength(0); i++)
                 {
                     for (int j = 0; j < MTkhaNghich.GetLength(1); j++)
                     {
                         Hill_txtBanro.Text += MTkhaNghich[i, j] + " ";
                     }
                 }*/
            }
            catch (Exception)
            {
                MessageBox.Show("Nhập giá trị khóa là số nguyên", "Error =.=!");                
            }
        }

        //------------------Kết thúc mã Hill---------------------------//
        #endregion

        #region Bat dau ma tu sinh
        //--------------------------Bắt đầu mã tự sinh-------------------------------//

        private void AutoCode_btnChonfilebanro_Click(object sender, EventArgs e)//Button chọn file bản rõ
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(open.FileName);
                AutoCode_txtBanro.Text = file.ReadToEnd();
                file.Close();
            }
        }

        private void AutoCode_btnLuufilebanro_Click(object sender, EventArgs e)//Button lưu file bản rõ
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(save.FileName);
                file.Write(textBox11.Text);
                file.Close();
            }
        }

        private void AutoCode_btnChonfilebanma_Click(object sender, EventArgs e)//Button chọn flie bản mã
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(open.FileName);
                AutoCode_txtBanma.Text = file.ReadToEnd();
                file.Close();
            }
        }

        private void AutoCode_btnLuufilebanma_Click(object sender, EventArgs e)//Button lưu file bản mã
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(save.FileName);
                file.Write(textBox12.Text);
                file.Close();
            }
        }

        private void AutoCode_btnClear_Click(object sender, EventArgs e)//Button Clear textbox
        {
            AutoCode_txtBanma.Text = "";
            AutoCode_txtBanro.Text = "";
            AutoCode_txtKhoaBanma.Text = "";
            AutoCode_txtKhoaBanro.Text = "";        
        }

        private void AutoCode_btnMahoa_Click(object sender, EventArgs e)//Button Ma hoa tự sinh
        {

            try
            {
                int key = Convert.ToInt32(AutoCode_txtKhoaBanro.Text);
                if (key >= 214 || key < 0)
                {
                    MessageBox.Show("Giá trị khóa không được vượt quá không gian Z(214)", "Error =.=!");
                    AutoCode_txtBanro.Text = "";
                    AutoCode_txtKhoaBanro.Text ="";
                }
                int[] arr = autocode.taokhoa(AutoCode_txtBanro.Text, key);
                string result = (arr == null) ? null : arr.Skip(1).Aggregate(arr[0].ToString(), (s, i) => s + "," + i.ToString());
                textBox11.Text = autocode.Mahoa(AutoCode_txtBanro.Text, key);
                Form2 frm = new Form2(result);
                frm.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Nhập lại khóa(Nhập số nguyên)!", "Error =.=! ");
                AutoCode_txtBanro.Text = "";
                AutoCode_txtKhoaBanro.Text = "";
            }

        }

        private void AutoCode_btnGiaima_Click(object sender, EventArgs e)//Button Giải mã tự sinh
        {
            try
            {
                int[] keyb = autocode.ParseStringToIntArray(AutoCode_txtKhoaBanma.Text);
                textBox12.Text = autocode.Giaima(AutoCode_txtBanma.Text, keyb);
            }
            catch (Exception)
            {
                MessageBox.Show("Nhập lại khóa.\nNhập dãy số nguyên(1,2,7,2,5...) có số phần tử bằng số phần tử bản mã!", "Error =.=! ");
                AutoCode_txtBanma.Text = "";
                AutoCode_txtKhoaBanma.Text = "";
            }
        }
        #endregion

        #region Bat dau he ma Vigenere
        private void Vigenere_btnMahoa_Click(object sender, EventArgs e)
        {
               
                int[] arc = Vigenere.chuyenmakey(Vigenere_txtKhoaBanro.Text);
                int[] dongkhoa = Vigenere.taokhoa(Vigenere_txtBanro.Text, arc);
                //string result = (arr == null) ? null : arr.Skip(1).Aggregate(arr[0].ToString(), (s, i) => s + "," + i.ToString());
                textBox9.Text = Vigenere.Mahoa(Vigenere_txtBanro.Text, dongkhoa);
            
            
        }
        private void Vigenere_btnClear_Click(object sender, EventArgs e)
        {
            Vigenere_txtBanma.Text = "";
            Vigenere_txtKhoaBanro.Text = "";
            Vigenere_txtBanro.Text = "";
            Vigenere_txtKhoaBanma.Text = "";
        }       
        private void Vigenere_btnGiaima_Click(object sender, EventArgs e)
        {
            int[] arc = Vigenere.chuyenmakey(Vigenere_txtKhoaBanma.Text);
            int[] dongkhoa = Vigenere.taokhoa(Vigenere_txtBanma.Text, arc);
            //string result = (arr == null) ? null : arr.Skip(1).Aggregate(arr[0].ToString(), (s, i) => s + "," + i.ToString());
            textBox10.Text = Vigenere.Giaima(Vigenere_txtBanma.Text, dongkhoa);
        }

        private void Vigenere_btnChonfileBanro_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(open.FileName);
                Vigenere_txtBanro.Text = file.ReadToEnd();
                file.Close();
            }
        }
    
        private void Vigenere_btnChonFileBanma_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(open.FileName);
                Vigenere_txtBanma.Text = file.ReadToEnd();
                file.Close();
            }
        }

        private void Vigenere_btnLuuFileBanro_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(save.FileName);
                file.Write(textBox9.Text);
                file.Close();
            }
        }

        private void Vigenere_btnLuuFileBanma_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(save.FileName);
                file.Write(textBox10.Text);
                file.Close();
            }
        }

        #endregion

        #region Bat dau he ma RSA
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void label16_Click(object sender, EventArgs e)
        {
            string a = txtSHA1.Text;
            txtSHA2.Text = EncodeSHA1(a);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            btnkhoatudongmoi.Hide();
            radioButton1.Checked = true;
             HamMaTranKhoa();
            MaTran();
           
            btnGiaiMa4.Enabled = false;
            btnMaHoa4.Enabled = false;
            pictureBox2.Hide();
        }

        private void btntaokhoa1_Click(object sender, EventArgs e)
        {
            if (txtp.Text == "" || txtq.Text == "")
                MessageBox.Show("Bạn phải nhập đủ 2 số ", "Thông Báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                p = Convert.ToInt16(txtp.Text);
                q = Convert.ToInt16(txtq.Text);
                if (p == q)
                {
                    MessageBox.Show("Bạn phải nhập 2 số khác nhau ", " Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtq.Focus();
                }
                else
                {
                    if (!kiemTraNguyenTo(p) || p <= 1)
                    {
                        MessageBox.Show("Bạn phải nhập số nguyên  tố [p] lớn hơn 1 ", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtp.Focus();
                    }
                    else
                    {
                        if (!kiemTraNguyenTo(q) || q <= 1)
                        {
                            MessageBox.Show("Bạn phải nhập số nguyên  tố [q] lớn hơn 1 ", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtq.Focus();
                        }
                        else
                        {
                            taoKhoa();
                            txtp.Text = "" + p;
                            txtq.Text = "" + q;
                            txtn.Text = "" + n;
                            txtn2.Text = "" + phi_n;
                            txtd.Text = "" + d;
                            active_key = true;
                        }
                    }
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            btntaokhoa1.Enabled = true;
            btntaokhoamoi1.Enabled = true;
            btnkhoatudongmoi.Hide();
            btntaokhoamoi1.Show();
            reset();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            btntaokhoa1.Enabled = false;
            btntaokhoamoi1.Enabled = false;
            btntaokhoamoi1.Hide();
            btnkhoatudongmoi.Show();
            reset_tudong();
            p = q = 0;
            do
            {
                p = soNgauNhien();
                q = soNgauNhien();
            }
            while (p == q || !kiemTraNguyenTo(p) || !kiemTraNguyenTo(q));
            taoKhoa();
            txtp.Text = "" + p;
            txtq.Text = "" + q;
            txtn.Text = "" + n;
            txtn2.Text = "" + phi_n;
            txtd.Text = "" + d;

            active_key = true;
        }
        private void reset()
        {
            txtq.Text = "";
            txtp.Text = "";
            txtn2.Text = "";
            txtn.Text = "";
            txte.Text = "";
            txtd.Text = "";

            txtd.Enabled = false;
            txte.Enabled = false;
            txtn.Enabled = false;
            txtn2.Enabled = false;
            txtp.Enabled = true;
            txtq.Enabled = true;
            q = p = 0;
        }
        private void reset_tudong()
        {
            txtq.Text = "";
            txtp.Text = "";
            txtn2.Text = "";
            txtn.Text = "";
            txte.Text = "";
            txtd.Text = "";
            txtd.Enabled = false;
            txte.Enabled = false;
            txtn.Enabled = false;
            txtn2.Enabled = false;
            txtp.Enabled = false;
            txtq.Enabled = false;

        }

        private void btnkhoatudongmoi_Click(object sender, EventArgs e)
        {
            reset_tudong();
            p = q = 0;
            do
            {
                p = soNgauNhien();
                q = soNgauNhien();
            }
            while (p == q || !kiemTraNguyenTo(p) || !kiemTraNguyenTo(q));
            taoKhoa();
            txtp.Text = "" + p;
            txtq.Text = "" + q;
            txtn.Text = "" + n;
            txtn2.Text = "" + phi_n;
            txtd.Text = "" + d;

            active_key = true;
        }

        private void btntaokhoamoi1_Click(object sender, EventArgs e)
        {
            reset();
            this.btnmahoamoi1_Click(sender, e);
        }

        private void btnmahoamoi1_Click(object sender, EventArgs e)
        {
            txt_banma1.Text = "";
            txt_banro1.Text = "";
            txt_giaima.Text = "";
            txt_ma_ban_ma1.Text = "";
            txt_ma_ban_ro1.Text = "";
            txt_ma_giai_ma.Text = "";
            decryptable = false;
        }
        public string EncodeSHA1(string pass)
        {

            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] bs = System.Text.Encoding.UTF8.GetBytes(pass);

            bs = sha1.ComputeHash(bs);

            System.Text.StringBuilder s = new System.Text.StringBuilder();

            foreach (byte b in bs)
            {

                s.Append(b.ToString("x1").ToLower());

            }

            pass = s.ToString();

            return pass;

        }

        private void btnmahoa1_Click(object sender, EventArgs e)
        {
            if (active_key == false)
                MessageBox.Show("Bạn phải tạo khóa trước ", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                try
                {
                    MaHoa(txt_banro1.Text);
                    decryptable = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        private void btngiaima_Click(object sender, EventArgs e)
        {

            if (txt_banma2.Text == "")
                MessageBox.Show("Bạn phải nhập khóa trước ", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                try
                {
                    GiaiMa(txt_banma2.Text);
                    if (txt_banro2.Text == txt_giaima.Text)
                    {
                        MessageBox.Show("chứ kí đúng :)");
                    }
                    else MessageBox.Show("chữ kí sai hoặc là không đúng người gửi, hoặc là nội dung đã bị sửa hoặc cả 2");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
        private void button11_Click_1(object sender, EventArgs e)
        {
            int a1 = 3;
            string a = "";
            a += (char)a1;
            MessageBox.Show(a);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFile = new OpenFileDialog();
            string ma = "";

            if (oFile.ShowDialog() == DialogResult.OK)
            {

                string a = oFile.FileName;
                byte[] by = System.IO.File.ReadAllBytes(a);
                for (int i = 0; i < by.Length; i++)
                {
                    ma += by[i].ToString();
                }
                MessageBox.Show(by.Length.ToString());
              
                label16_Click(sender, e);
                txt_banro2.Text = txtSHA2.Text;
            }
        }

        private void label29_Click(object sender, EventArgs e)
        {
           
        }
        #endregion


        #region He ma RSA
        private int soNgauNhien()
        {
            Random rd = new Random();
            return rd.Next(11, 101);
        }
        private bool kiemTraNguyenTo(int i)
        {
            bool kiemtra = true;
            for (int j = 2; j < i; j++)
                if (i % j == 0)
                {
                    kiemtra = false;
                    break;
                }
            return kiemtra;
        }
        private bool nguyenToCungNhau(int a, int b)
        {
            bool kiemtra = true;
            for (int i = 2; i <= a; i++)
                if (a % i == 0 && b % i == 0)
                    kiemtra = false;
            return kiemtra;
        }

        private void btnFile1_Click(object sender, EventArgs e)
        {

            OpenFileDialog oFile = new OpenFileDialog();
            string ma = "";

            if (oFile.ShowDialog() == DialogResult.OK)
            {

                string a = oFile.FileName;
                byte[] by = System.IO.File.ReadAllBytes(a);
                for (int i = 0; i < by.Length; i++)
                {
                    ma += by[i].ToString();
                }
                MessageBox.Show(by.Length.ToString());
                txtSHA1.Text = ma;
                label16_Click(sender, e);
                txt_banro1.Text = txtSHA2.Text;
            }
        }

        private void taoKhoa()
        {
            //Tinh n=p*q
            n = p * q;

            //Tính Phi(n)=(p-1)*(q-1)
            phi_n = (p - 1) * (q - 1);

            //Tính e là một số ngẫu nhiên có giá trị 0< e <phi(n) và là số nguyên tố cùng nhau với Phi(n)
            do
            {
                Random rd = new Random();
                e = rd.Next(2, phi_n);
            }
            while (!nguyenToCungNhau(e, phi_n));
            txte.Text = Convert.ToString(e);
            //Tính d
            d = 0;
            int i = 2;
            while (((1 + i * phi_n) % e) != 0 || d <= 0)
            {
                i++;
                d = (1 + i * phi_n) / e;
            }

        }
        public int mod(int m, int e, int n)
        {


            //Sử dụng thuật toán "bình phương nhân"
            //Chuyển e sang hệ nhị phân
            int[] a = new int[100];
            int k = 0;
            do
            {
                a[k] = e % 2;
                k++;
                e = e / 2;
            }
            while (e != 0);
            //Quá trình lấy dư
            int kq = 1;
            for (int i = k - 1; i >= 0; i--)
            {
                kq = (kq * kq) % n;
                if (a[i] == 1)
                    kq = (kq * m) % n;
            }
            return kq;



            /* cách khác
             * int kq = 1;
             * while ( e > 0)
             * {
             *    if((e & 1) == 1)
             *     {
             *         kq = (kq + m)%n;
             *     }
             *     e = e >> 1;
             *     m = (m * m) % n;
             * }
             * return kq
             * */
        }

        private void groupBox12_Enter(object sender, EventArgs e)
        {

        }

        private void btThoatRSA_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Close(); 
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(save.FileName);
                file.Write(txt_banma1.Text);
                file.Close();
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(save.FileName);
                file.Write(txt_giaima.Text);
                file.Close();
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(save.FileName);
                file.Write(txtXuatVanBan1.Text);
                file.Close();
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {

            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(open.FileName);
                txtXuatVanBan.Text = file.ReadToEnd();
                file.Close();
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(save.FileName);
                file.Write(txtNhapVanBan1.Text);
                file.Close();
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }

        private void Vigenere_txtBanro_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label53_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Hill_txtKhoaDbanro_TextChanged(object sender, EventArgs e)
        {

        }

        private void Hill_txtKhoaCbanro_TextChanged(object sender, EventArgs e)
        {

        }

        private void Hill_txtKhoaBbanro_TextChanged(object sender, EventArgs e)
        {

        }

        private void Hill_txtKhoaAbanro_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txt_banro2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_banma2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_giaima_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSHA2_TextChanged(object sender, EventArgs e)
        {

        }

        public string chuoi(int[] a) //Lấy chuỗi từ mảng ra xâu S
        {
            string s = "";


            for (int i = 0; i < a.Length - 1; i++)
            {
                s = s + a[i].ToString() + "-";
            }
            s = s + a[a.Length - 1].ToString();
            return s;
        }
        public void MaHoa(string s) // mã hóa
        {
            taoKhoa();
            // Chuyen xau thanh ma Unicode
            int[] nguyen = new int[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                nguyen[i] = (int)s[i];
            }
            txt_ma_ban_ro1.Text = chuoi(nguyen);
            //Mảng a chứa các kí tự đã mã hóa
            int[] a = new int[nguyen.Length];
            for (int i = 0; i < nguyen.Length; i++)
            {
                a[i] = mod(nguyen[i], d, n);
            }
            txt_ma_ban_ma1.Text = chuoi(a);
            //Chuyển sang kiểu kí tự trong bảng mã Unicode
            string str = "";
            for (int i = 0; i < nguyen.Length; i++)
            {
                str = str + (char)a[i];
            }
            byte[] data = Encoding.Unicode.GetBytes(str);
            txt_banma1.Text = Convert.ToBase64String(data);
        }
        public void GiaiMa(string s)
        {
            //Lấy mã Unicode của từng kí tự mã hóa
            string giaima = Encoding.Unicode.GetString(Convert.FromBase64String(s));
            int[] b = new int[giaima.Length];
            for (int i = 0; i < giaima.Length; i++)
            {
                b[i] = (int)giaima[i];
            }
            //Giải mã
            int[] c = new int[b.Length];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = mod(b[i], e, n);
            }
            txt_ma_giai_ma.Text = chuoi(c);
            string str = "";
            for (int i = 0; i < c.Length; i++)
            {
                str = str + (char)c[i];
            }
            txt_giaima.Text = str;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
        #endregion



        #region Bat dau he ma  Ff
        string NhapMaTran = ""; //Chuỗi ma trận nhập vào
        string BangChuCai; //bảng chữ cái
        char[,] MaTranKhoa = new char[5, 5]; //ma trận
        char[,] DiaChiNhiPhan = new char[200, 2]; int DemDiaChi = 0; //địa chỉ từng ký tự trên ma trận theo dạng nhị phân
        char[,] DiaChiNhiPhanKQ = new char[200, 2];//Địa chỉ của kết quả sau khi giải mã hoặc mã hóa
        int[] DiaChiNhiPhanCapDoi = new int[4];//cho những nơi của bộ đôi.
        Random random = new Random();
        private void button14_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(open.FileName);
                txtNhapVanBan.Text = file.ReadToEnd();
                file.Close();
            }
        }
        public string HamDiaChiNhiPhanKQ()
        {
            string ChuoiDiaChiNhiPhanKQ = "";

            for (int i = 0; i < DemDiaChi; i++)
            {
                ChuoiDiaChiNhiPhanKQ += DiaChiNhiPhanKQ[i, 0].ToString() + DiaChiNhiPhanKQ[i, 1].ToString();
            }

            return ChuoiDiaChiNhiPhanKQ;
        }

        //Playfair mã hóa bộ đôi một cách riêng biệt.

        public void GiaiMa()
        {
            for (int i = 0; i < DemDiaChi; i++)
            {
                //Tìm địa điểm của bộ đôi.
                HamDiaChiNhiPhanCapDoi(DiaChiNhiPhan[i, 0], DiaChiNhiPhan[i, 1]);

                if (DiaChiNhiPhanCapDoi[0] == DiaChiNhiPhanCapDoi[2]) // nếu chúng ở trên cùng một dòng,
                {
                    DiaChiNhiPhanKQ[i, 0] = MaTranKhoa[DiaChiNhiPhanCapDoi[0], (DiaChiNhiPhanCapDoi[1] + 4) % 5];
                    DiaChiNhiPhanKQ[i, 1] = MaTranKhoa[DiaChiNhiPhanCapDoi[2], (DiaChiNhiPhanCapDoi[3] + 4) % 5];
                }
                else if (DiaChiNhiPhanCapDoi[1] == DiaChiNhiPhanCapDoi[3]) // nếu chúng ở trong cùng một cột,
                {
                    DiaChiNhiPhanKQ[i, 0] = MaTranKhoa[(DiaChiNhiPhanCapDoi[0] + 4) % 5, DiaChiNhiPhanCapDoi[1]];
                    DiaChiNhiPhanKQ[i, 1] = MaTranKhoa[(DiaChiNhiPhanCapDoi[2] + 4) % 5, DiaChiNhiPhanCapDoi[3]];
                }
                else //Nếu hàng và cột khác nhau,
                {
                    DiaChiNhiPhanKQ[i, 0] = MaTranKhoa[DiaChiNhiPhanCapDoi[0], DiaChiNhiPhanCapDoi[3]];
                    DiaChiNhiPhanKQ[i, 1] = MaTranKhoa[DiaChiNhiPhanCapDoi[2], DiaChiNhiPhanCapDoi[1]];
                }
            }
        }

        //Playfair mã hóa bộ đôi một cách riêng biệt.
        public void MaHoa()
        {
            for (int i = 0; i < DemDiaChi; i++)
            {
                //Tìm DiaChiNhiPhanCapDoi của bộ đôi.
                HamDiaChiNhiPhanCapDoi(DiaChiNhiPhan[i, 0], DiaChiNhiPhan[i, 1]);

                if (DiaChiNhiPhanCapDoi[0] == DiaChiNhiPhanCapDoi[2]) // aynı satırda iseler,
                {
                    DiaChiNhiPhanKQ[i, 0] = MaTranKhoa[DiaChiNhiPhanCapDoi[0], (DiaChiNhiPhanCapDoi[1] + 1) % 5];
                    DiaChiNhiPhanKQ[i, 1] = MaTranKhoa[DiaChiNhiPhanCapDoi[2], (DiaChiNhiPhanCapDoi[3] + 1) % 5];
                }
                else if (DiaChiNhiPhanCapDoi[1] == DiaChiNhiPhanCapDoi[3]) // aynı stunda iseler,
                {
                    DiaChiNhiPhanKQ[i, 0] = MaTranKhoa[(DiaChiNhiPhanCapDoi[0] + 1) % 5, DiaChiNhiPhanCapDoi[1]];
                    DiaChiNhiPhanKQ[i, 1] = MaTranKhoa[(DiaChiNhiPhanCapDoi[2] + 1) % 5, DiaChiNhiPhanCapDoi[3]];
                }
                else //satır ve stunları farklı ise,
                {
                    DiaChiNhiPhanKQ[i, 0] = MaTranKhoa[DiaChiNhiPhanCapDoi[0], DiaChiNhiPhanCapDoi[3]];
                    DiaChiNhiPhanKQ[i, 1] = MaTranKhoa[DiaChiNhiPhanCapDoi[2], DiaChiNhiPhanCapDoi[1]];
                }
            }
        }

        //Tìm dòng và trình tự của cặp mong muốn trong ma trận
        public void HamDiaChiNhiPhanCapDoi(char ch1, char ch2)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (MaTranKhoa[i, j] == ch1)
                    {
                        DiaChiNhiPhanCapDoi[0] = i;
                        DiaChiNhiPhanCapDoi[1] = j;
                    }
                    else if (MaTranKhoa[i, j] == ch2)
                    {
                        DiaChiNhiPhanCapDoi[2] = i;
                        DiaChiNhiPhanCapDoi[3] = j;
                    }
                }
            }
        }

        //Tạo một chuỗi nhị phân của văn bản đầu vào.
        public void HamDiaChiNhiPhan(string KyTuLoai)
        {
            int j = 0;

            for (int i = 0; i < KyTuLoai.Length;)
            {
                DiaChiNhiPhan[j, 0] = KyTuLoai[i];

                if (i == KyTuLoai.Length - 1)
                {
                    DiaChiNhiPhan[j, 1] = 'w';
                    j++;
                    break;
                }
                else if (KyTuLoai[i] != KyTuLoai[i + 1])
                {
                    DiaChiNhiPhan[j, 1] = KyTuLoai[i + 1];
                    i += 2;
                }
                else
                {
                    DiaChiNhiPhan[j, 1] = 'x';
                    i++;
                }

                j++;
            }

            DemDiaChi = j;
        }

        public void HamNhapMaTran()
        {
            int k = 0;
            int j = 0;
            int index = 0;

            BangChuCai = "abcdefghiklmnopqrstuvwxyz";
            int i = BangChuCai.Length;

            while (i > 0)
            {
                index = random.Next(0, i);
                MaTranKhoa[k, j] = BangChuCai[index];
                BangChuCai = BangChuCai.Remove(index, 1);

                i--;
                j++;

                if (j % 5 == 0)
                {
                    k++;
                    j = 0;
                }
            }
        }

        public void HamMaTranKhoa()
        {
            int j = 0;
            int k = 0;
            int i = NhapMaTran.Length;
            int index1 = 0;

            BangChuCai = "abcdefghiklmnopqrstuvwxyz";
            while (i > 0)
            {

                index1 = BangChuCai.IndexOf(NhapMaTran[NhapMaTran.Length - i]);
                if (index1 >= 0)
                {
                    string MT = "" + NhapMaTran[NhapMaTran.Length - i];
                    string TD = "i";
                    if (MT == "j")
                    {
                        MaTranKhoa[k, j] = TD[0];
                    }
                    else
                    {
                        MaTranKhoa[k, j] = NhapMaTran[NhapMaTran.Length - i];

                    }
                    BangChuCai = BangChuCai.Remove(index1, 1);
                    j++;
                }

                i--;

                if (j == 5)
                {
                    k++;
                    j = 0;
                }
            }

            i = 0;

            while (i < BangChuCai.Length)
            {
                MaTranKhoa[k, j] = BangChuCai[i];

                i++;
                j++;

                if (j % 5 == 0)
                {
                    k++;
                    j = 0;
                }
            }
        }

        public void MaTran()
        {
            txtMaTran.Text = "";

            for (int i = 1; i < 6; i++)
            {
                for (int j = 1; j < 6; j++)
                {
                    string MT = "" + MaTranKhoa[i - 1, j - 1];
                    if (MT == "i" || MT == "j")
                    {
                        txtMaTran.Text += "i/j\r\t";
                    }
                    else
                    {
                        txtMaTran.Text += MaTranKhoa[i - 1, j - 1] + "\r\t";
                    }
                }
                txtMaTran.Text += "\r\n\r\n";
            }
        }

        //kiểm tra các ký tự không xác định trong văn bản.
        public string LoaiBoKyTuDB(string KyTuLoai)
        {
            for (int i = KyTuLoai.Length - 1; i >= 0; i--)
            {
                /*if (!(KyTuLoai[i] == '.' || KyTuLoai[i] == ',' ||
					KyTuLoai[i] == ':' || KyTuLoai[i] == ' ' ||
					(KyTuLoai[i] >= 97 && KyTuLoai[i] <= 122) ||
					KyTuLoai[i] == 'ü' || KyTuLoai[i] == 'ö' ||
					KyTuLoai[i] == 'ç' || KyTuLoai[i] == 'ş' ||
					KyTuLoai[i] == 'ğ' || KyTuLoai[i] == 'ı'))
				{
					KyTuLoai = KyTuLoai.Remove(i, 1);
				}*/
                if (KyTuLoai[i] == 'j')
                {
                    KyTuLoai = "i";
                }
                else
                {
                    if (KyTuLoai[i] == ' ')
                    {
                        //MessageBox.Show("Văn bản đầu vào phải là chữ in thường viết liền không dấu.", "Lỗi nhập dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        pictureBox2.Show();
                        KyTuLoai = KyTuLoai.Remove(i, 1);
                    }


                }
            }
            return KyTuLoai;
        }

        private void txtNhapMaTran_TextChanged(object sender, EventArgs e)
        {

        }



        private void txtNhapVanBan_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnRandomKhoa_Click(object sender, EventArgs e)
        {

            HamNhapMaTran();
            MaTran();
        }

        private void tabPage7_Click_1(object sender, EventArgs e)
        {

        }



        private void txtNhapMaTran_TextChanged_1(object sender, EventArgs e)
        {
            if (txtNhapMaTran.Text == "")
            {
                //txtNhapVanBan.Enabled = false;
                //txtXuatVanBan.Enabled = false;
                NhapMaTran = "";
            }
            else
            {
                //txtNhapVanBan.Enabled = true;
                //txtXuatVanBan.Enabled = true;
                txtNhapMaTran.Text = LoaiBoKyTuDB(txtNhapMaTran.Text);
                txtNhapMaTran.Select(txtNhapMaTran.Text.Length, 0);
                NhapMaTran = txtNhapMaTran.Text;
            }
            HamMaTranKhoa();
            MaTran();
        }

        private void txtNhapVanBan_TextChanged_1(object sender, EventArgs e)
        {
            if (txtNhapVanBan.Text == "")
            {
                btnGiaiMa4.Enabled = false;
                btnMaHoa4.Enabled = false;
            }
            else
            {
                txtNhapVanBan.Text = LoaiBoKyTuDB(txtNhapVanBan.Text);
                txtNhapVanBan.Select(txtNhapVanBan.Text.Length, 0);
                btnGiaiMa4.Enabled = true;
                btnMaHoa4.Enabled = true;
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Hill_txtKhoaAbanma_TextChanged(object sender, EventArgs e)
        {

        }

        private void Hill_txtKhoaBbanma_TextChanged(object sender, EventArgs e)
        {

        }

        private void Hill_txtKhoaCbanma_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void AutoCode_txtKhoaBanma_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtXuatVanBan_TextChanged(object sender, EventArgs e)
        {
            if (txtXuatVanBan.Text == "")
            {
                btnGiaiMa4.Enabled = false;
                btnMaHoa4.Enabled = false;
            }
            else
            {
                txtXuatVanBan.Text = LoaiBoKyTuDB(txtXuatVanBan.Text);
                txtXuatVanBan.Select(txtXuatVanBan.Text.Length, 0);
                btnGiaiMa4.Enabled = true;
                btnMaHoa4.Enabled = true;
            }
        }

        private void Hill_txtKhoaDbanma_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDuyChuyen_Click(object sender, EventArgs e)
        {
            txtNhapVanBan.Text = txtXuatVanBan.Text;
            txtXuatVanBan.Text = "";
        }

        private void btnMaHoa4_Click(object sender, EventArgs e)
        {
            HamDiaChiNhiPhan(txtNhapVanBan.Text);
            MaHoa();
            txtXuatVanBan1.Text = HamDiaChiNhiPhanKQ();
        }

        private void btnGiaiMa4_Click(object sender, EventArgs e)
        {


            HamDiaChiNhiPhan(txtXuatVanBan.Text);
            GiaiMa();
            txtNhapVanBan1.Text = HamDiaChiNhiPhanKQ();
        }


        #endregion
    }
}
