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

                FileInfo fileInfo = new FileInfo(inputFilePath);
                ListViewItem fileItem = new ListViewItem(inputFile.Name);
                fileItem.SubItems.Add($"{fileInfo.Length} B");
                fileItem.ImageKey = fileInfo.Extension;

                if (!imageListIcons.Images.ContainsKey(fileItem.ImageKey))
                    imageListIcons.Images.Add(fileItem.ImageKey, Icon.ExtractAssociatedIcon(inputFilePath));

                listView1.Items.Add(fileItem);
            }
            
            listView1.BackColor = Color.White;
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                listView1.BackColor = Color.LightGreen;
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

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To Be Implemented.");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            stegoMessage.TextMessage = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(stegoMessage.TextMessage);
        }

        private void EmbedButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO: initiate algorithm.");
        }
        
    }
}
