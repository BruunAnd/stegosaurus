using Stegosaurus.Algorithm;
using Stegosaurus.Carrier;
using Stegosaurus.Exceptions;
using Stegosaurus.Utility.Extensions;
using Stegosaurus.Utility.InputTypes;
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
        IStegoAlgorithm algorithm = new LSBAlgorithm();

        public FormMain()
        {
            InitializeComponent();
        }

        private void MessageContentFilesListView_DragDrop(object sender, DragEventArgs e)
        {
            string[] inputFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            IInputType inputContent;
            foreach (string filePath in inputFiles)
            {
                inputContent = new ContentType(filePath);
                InputHelper(inputContent);
            }

            MessageContentFilesListview.BackColor = Color.White;
        }

        private void MessageContentFilesListView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                MessageContentFilesListview.BackColor = Color.LightGreen;
            }
            else
            {
                e.Effect = DragDropEffects.None;
                MessageContentFilesListview.BackColor = Color.Red;
            }
        }

        private void MessageContentFilesListView_DragLeave(object sender, EventArgs e)
        {
            MessageContentFilesListview.BackColor = Color.White;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To Be Implemented.");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void TextMessageTextbox_TextChanged(object sender, EventArgs e)
        {
            stegoMessage.TextMessage = TextMessageTextbox.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(stegoMessage.TextMessage);
        }

        private void EmbedButton_Click(object sender, EventArgs e)
        {
            if (carrierMedia == null)
            {
                MessageBox.Show("You must supply a carrier media.");
            }
            else
            {
                if (stegoMessage.InputFiles.Count == 0 && string.IsNullOrEmpty(stegoMessage.TextMessage))
                {
                    algorithm.CarrierMedia = carrierMedia;
                    algorithm.Key = Encoding.UTF8.GetBytes(EncryptionKeyTextbox.Text);
                    stegoMessage = algorithm.Extract();

                }
                else
                {
                    algorithm.CarrierMedia = carrierMedia;
                    algorithm.Key = Encoding.UTF8.GetBytes(EncryptionKeyTextbox.Text);
                    algorithm.Embed(stegoMessage);
                    algorithm.CarrierMedia.SaveToFile("new.png");
                }
            }

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

        private void FormMain_Load(object sender, EventArgs e)
        {
            
        }

        private void CarrierMediaPanel_DragEnter(object sender, DragEventArgs e)
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

        private void CarrierMediaPanel_DragLeave(object sender, EventArgs e)
        {

        }

        private void CarrierMediaPanel_DragDrop(object sender, DragEventArgs e)
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
                MessageContentFilesListview.Items.Add(fileItem);
            }
            else if (_input is CarrierType)
            {
                if (fileInfo.Extension == ".wav")
                {
                    carrierMedia = new AudioCarrier(_input.FilePath);
                    CarrierPictureBox.Image = Icon.ExtractAssociatedIcon(_input.FilePath).ToBitmap();
                }
                else
                {
                    carrierMedia = new ImageCarrier(_input.FilePath);
                    CarrierPictureBox.Image = Image.FromFile(fileInfo.FullName);
                }
            }
            
        }

        private void AlgorithmSelectionCombobox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            algorithm = (IStegoAlgorithm) AlgorithmSelectionCombobox.SelectedItem;
        }
    }
}
