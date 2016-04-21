using Stegosaurus.Algorithm;
using Stegosaurus.Carrier;
using Stegosaurus.Exceptions;
using Stegosaurus.Extensions;
using Stegosaurus.Extensions.InputExtensions;
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
        ICarrierMedia carrierMedia;
        IStegoAlgorithm algorithm;

        public FormMain()
        {
            InitializeComponent();
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            string[] inputFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            IInputType inputContent;
            foreach (string filePath in inputFiles)
            {
                inputContent = new ContentType(filePath);
                InputHelper(inputContent);
            }

            listView1.BackColor = Color.White;
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
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

        private void inputBrowseButton_Click(object sender, EventArgs e)
        {
            DialogResult result = InputBrowseDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                IInputType inputContent;
                foreach (string fileName in InputBrowseDialog.FileNames)
                {
                    inputContent = new ContentType(fileName);
                    InputHelper(inputContent);
                }
            }
        }

        private void InputBrowseDialog_FileOk(object sender, CancelEventArgs e)
        {
            MessageBox.Show("TODO: file type validation.");
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void panel1_DragLeave(object sender, EventArgs e)
        {

        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            string[] inputFile = (string[]) e.Data.GetData(DataFormats.FileDrop);
            try
            {
                IInputType inputContent = new CarrierType(inputFile[0]);
                InputHelper(inputContent);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (InvalidWaveFileException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show("Invalid file type. Carrier must be picture or .wav audio file.");
            }
            
        }

        private void InputHelper(IInputType _input)
        {
            InputFile inputFile = new InputFile(_input.FilePath);
            FileInfo fileInfo = new FileInfo(_input.FilePath);

            if (_input is ContentType)
            {
                ListViewItem fileItem = new ListViewItem(inputFile.Name);
                fileItem.SubItems.Add(FileSizeExtensions.StringFormatBytes(fileInfo.Length));
                fileItem.ImageKey = fileInfo.Extension;
                if (!imageListIcons.Images.ContainsKey(fileItem.ImageKey))
                    imageListIcons.Images.Add(fileItem.ImageKey, Icon.ExtractAssociatedIcon(_input.FilePath));
                
                stegoMessage.InputFiles.Add(inputFile);
                listView1.Items.Add(fileItem);
            }
            else if (_input is CarrierType)
            {
                if (fileInfo.Extension == ".wav")
                {
                    carrierMedia = new AudioCarrier(_input.FilePath);
                    pictureBox1.Image = Icon.ExtractAssociatedIcon(_input.FilePath).ToBitmap();
                }
                else
                {
                    carrierMedia = new ImageCarrier(_input.FilePath);
                    pictureBox1.Image = Image.FromFile(fileInfo.FullName);
                }
            }
            
        }
        
    }
}
