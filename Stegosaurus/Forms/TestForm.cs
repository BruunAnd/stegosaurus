using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Stegosaurus.Forms
{
    public partial class TestForm : Form
    {
        private string test;

        public TestForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            test = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.MaxLength = 5;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(test);
        }
    }
}
