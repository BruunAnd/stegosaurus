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
            List<InputFile> inputFileList = new List<InputFile>();
            foreach (string inputFilePath in inputFiles)
            {
                inputFileList.Add(new InputFile(inputFilePath));
            }


        }
    }
}
