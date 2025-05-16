using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoReasoningGUI
{
    public partial class Form2 : Form
    {
        private Form1 form1;
        public Form2(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }

        private void prevPage_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            form1.Visible = true;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            form1.Close();
        }
    }
}
