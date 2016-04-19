using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Stegosaurus.Forms
{
    public partial class FormMain : Form
    {
        StegoMessage stegoMessage = new StegoMessage();

        public FormMain()
        {
            InitializeComponent();
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            InputFile inputFile;
            string[] inputFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string inputFilePath in inputFiles)
            {
                inputFile = new InputFile(inputFilePath);
                stegoMessage.InputFiles.Add(inputFile);
                listView1.Items.Add(inputFile.Name);

            }
            
            label1.Text = string.Join("\n", stegoMessage.InputFiles.Select<InputFile, string>(file => file.Name));

            listView1.BackColor = Color.White;
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                listView1.BackColor = Color.Aquamarine;
            }
            else
            {
                e.Effect = DragDropEffects.None;
                listView1.BackColor = Color.Red;
            }
        }

        private void listView1_DragLeave(object sender, EventArgs e)
        {

            listView1.BackColor = Color.White;
        }
    }
}
