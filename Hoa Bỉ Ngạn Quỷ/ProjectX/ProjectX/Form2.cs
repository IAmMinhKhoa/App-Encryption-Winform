using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProjectX
{
    public partial class Form2 : Form
    {
        public Form2(string strText)
        {
            InitializeComponent();
            textBox1.Text = strText;
        }
        private void closeform() { this.Dispose(); }
        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.Text);
            closeform();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
