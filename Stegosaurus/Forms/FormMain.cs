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
        StegoMessage stegoMessage = new StegoMessage("", new List<InputFile>());

        public FormMain()
        {
            InitializeComponent();
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            string[] inputFiles = (string[]) e.Data.GetData(DataFormats.FileDrop);
            foreach (string inputFilePath in inputFiles)
            {
                stegoMessage.InputFiles.Add(new InputFile(inputFilePath));
            }

            label1.Text = string.Join("\n", stegoMessage.InputFiles.Select<InputFile, string>(inputFile => inputFile.Name));

        }
        
    }
}
