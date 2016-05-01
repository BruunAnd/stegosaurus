using Stegosaurus.Algorithm;
using Stegosaurus.Carrier;
using Stegosaurus.Exceptions;
using Stegosaurus.Utility;
using Stegosaurus.Utility.Extensions;
using Stegosaurus.Utility.InputTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Stegosaurus.Cryptography;

namespace Stegosaurus.Forms
{
    public partial class FormMain : Form
    {
        private readonly Dictionary<string, IStegoAlgorithm> algorithmDictionary = new Dictionary<string, IStegoAlgorithm>();
        private readonly Dictionary<string, ICryptoProvider> cryptoProviderDictionary = new Dictionary<string, ICryptoProvider>();

        private StegoMessage stegoMessage = new StegoMessage();
        private ICarrierMedia carrierMedia;
        private IStegoAlgorithm algorithm;
        private ICryptoProvider cryptoProvider;

        private string carrierName;
        private string carrierExtension;

        private bool CanEmbed => carrierMedia != null && (!string.IsNullOrEmpty(textBoxTextMessage.Text) || listViewMessageContentFiles.Items.Count > 0);

        public FormMain()
        {
            InitializeComponent();

            cryptoProvider = new AESProvider();

            // Add algorithms
            AddAlgorithm(typeof(LSBAlgorithm));
            AddAlgorithm(typeof(GraphTheoreticAlgorithm));

            // Add crypto providers
            AddCryptoProvider(typeof(AESProvider));
            AddCryptoProvider(typeof(TripleDESProvider));
            AddCryptoProvider(typeof(RSAProvider));

            // Set default values
            comboBoxAlgorithmSelection.SelectedIndex = 0;
            comboBoxCryptoProviderSelection.SelectedIndex = 0;
        }
        
        #region Carrier Media Handling
        /// <summary>
        /// Checks that the files dragged into the panelCarrierMedia control are valid and assigs the effect of the Drag&Drop accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelCarrierMedia_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        /// <summary>
        /// Gets the paths of the dropped files and converts the first to CarrierType and calls InputHelper.
        /// TODO: Implement chack for multiple dropped files and show apropriate error.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelCarrierMedia_DragDrop(object sender, DragEventArgs e)
        {
            string[] inputFile = (string[]) e.Data.GetData(DataFormats.FileDrop);
            if (inputFile.Length == 1)
            {
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
            else
            {
                MessageBox.Show("Cannot have multiple carrier medias.", "Error");
            }

        }

        /// <summary>
        /// Opens a dialog where the user can browse for a file to make the carrier media.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCarrierMediaBrowse_Click(object sender, EventArgs e)
        {
            openFileDialogBrowseInput.Multiselect = false;
            DialogResult result = openFileDialogBrowseInput.ShowDialog();

            if (result != DialogResult.OK)
                return;

           InputHelper(new CarrierType(openFileDialogBrowseInput.FileName));
        }

        #endregion

        #region StegoMessage Content Handling
        /// <summary>
        /// Assigns the content of the textBoxTextMessage.Text property to the stegoMessage.TextMessage property and updates the  to be the progressBarCapacity control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxTextMessage_TextChanged(object sender, EventArgs e)
        {
            stegoMessage.TextMessage = textBoxTextMessage.Text;

            UpdateCapacityBar();
            UpdateButtonText();
        }

        /// <summary>
        /// Checks that the items dragged into the listViewMessageContentFiles control are valid and changes the effect, and color accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewMessageContentFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                listViewMessageContentFiles.BackColor = Color.LightGreen;
            }
            else
            {
                e.Effect = DragDropEffects.None;
                listViewMessageContentFiles.BackColor = Color.Red;
            }
        }

        /// <summary>
        /// Reverts the color of the listViewMessageContentFiles control to white when the files are dragged out of the conrtrols boundaries. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewMessageContentFiles_DragLeave(object sender, EventArgs e)
        {
            listViewMessageContentFiles.BackColor = Color.White;
        }

        /// <summary>
        /// Gets the file paths of all dropped files, converts them to the ContentType and calls the InputHelper to handle them further.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewMessageContentFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] inputFiles = (string[]) e.Data.GetData(DataFormats.FileDrop);

            foreach (string filePath in inputFiles)
            {
                InputHelper(new ContentType(filePath));
            }

            listViewMessageContentFiles.BackColor = Color.White;
        }

        /// <summary>
        /// Opens a dialog where the user can browse for files to add to the message content.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonInputBrowse_Click(object sender, EventArgs e)
        {
            openFileDialogBrowseInput.Multiselect = true;
            DialogResult result = openFileDialogBrowseInput.ShowDialog();

            if (result != DialogResult.OK)
                return;

            foreach (string fileName in openFileDialogBrowseInput.FileNames)
            {
                InputHelper(new ContentType(fileName));
            }
        }
        
        /// <summary>
        /// Ensures that the contextMenuStripMain wont open if no items from listViewMessageContentFiles are selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStripMain_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = listViewMessageContentFiles.SelectedItems.Count == 0;
        }

        /// <summary>
        /// Allows saving of files from the stegoMessage.InputFiles, as selected in the listViewMessageContentFiles control,
        /// to a custom location. If single file is selected user is prompted with dialog to select destination and filename, 
        /// and if multiple files are selected the user is prompted to select a folder to which all selected file will be saved
        /// with their default names.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] fileIndices = GetSelectedContentIndices();
            int len = fileIndices.Length;
            if (len == 0)
            {
                MessageBox.Show("You must have items selected to save.", "Save Error");
            }
            if (len == 1)
            {
                saveFileDialog.FileName = stegoMessage.InputFiles[fileIndices[0]].Name;

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                if (saveFileDialog.FileName == "")
                {
                    MessageBox.Show("The chosen destination cannot be blank.", "Save Error");
                }
                else
                {
                    stegoMessage.InputFiles[fileIndices[0]].SaveTo(saveFileDialog.FileName);
                }
            }
            else
            {
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                    return;

                if (folderBrowserDialog.SelectedPath == "")
                {
                    MessageBox.Show("The chosen destination cannot be blank.", "Save Error");
                }
                else
                {
                    foreach (int index in fileIndices)
                    {
                        stegoMessage.InputFiles[fileIndices[index]].SaveTo($"{folderBrowserDialog.SelectedPath}\\{stegoMessage.InputFiles[fileIndices[index]].Name}");
                    }
                }
            }
        }

        /// <summary>
        /// Handles the deletion of items from the listViewMessageContentFiles control and stegoMessage.InputFiles collection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] fileIndices = GetSelectedContentIndices();

            for (int index = fileIndices.Length - 1; index >= 0; index--)
            {
                stegoMessage.InputFiles.RemoveAt(fileIndices[index]);
                listViewMessageContentFiles.Items.RemoveAt(fileIndices[index]);
            }

            UpdateCapacityBar();
            UpdateButtonText();
        }
        
        /// <summary>
        /// Returns an array of integers containing the indices of all selected items in the listViewMessageContentFiles control.
        /// </summary>
        /// <returns></returns>
        private int[] GetSelectedContentIndices()
        {
            int[] indices = new int[listViewMessageContentFiles.SelectedIndices.Count];
            listViewMessageContentFiles.SelectedIndices.CopyTo(indices, 0);
            return indices;
        }
        #endregion

        #region Cryptography Handling
        /// <summary>
        /// Adds a crypto provider to the cryptoprovider dictionary and combolist
        /// </summary>
        private void AddCryptoProvider(Type cryptoProviderType)
        {
            ICryptoProvider newCryptoProvider = (ICryptoProvider) Activator.CreateInstance(cryptoProviderType);
            cryptoProviderDictionary.Add(newCryptoProvider.Name, newCryptoProvider);
            comboBoxCryptoProviderSelection.Items.Add(newCryptoProvider.Name);
        }

        /// <summary>
        /// Updates the cryptoProvider to reflect the chosen item in the comboBoxCryptoProviderSelection control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxCryptoProviderSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            cryptoProvider = cryptoProviderDictionary[comboBoxCryptoProviderSelection.Text];
            cryptoProvider.CryptoKey = textBoxEncryptionKey.Text;

            algorithm.CryptoProvider = cryptoProvider;
        }

        //TODO: Implement Key size limit for textboxEncryptionKey.
        #endregion

        #region Steganography Handling
        /// <summary>
        /// Add an algorithm to the algorithm dictionary and combolist
        /// </summary>
        private void AddAlgorithm(Type algorithmType)
        {
            IStegoAlgorithm stegoAlgorithm = (IStegoAlgorithm) Activator.CreateInstance(algorithmType);
            algorithmDictionary.Add(stegoAlgorithm.Name, stegoAlgorithm);
            comboBoxAlgorithmSelection.Items.Add(stegoAlgorithm.Name);
        }

        /// <summary>
        /// Assigns the chosen algorithm to the algorithm variable and supplies the algorithm with the current carrierMedia. At last the progressBarCapacity is updated.
        /// </summary>
        private void comboBoxAlgorithmSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            algorithm = algorithmDictionary[comboBoxAlgorithmSelection.Text];
            algorithm.CarrierMedia = carrierMedia;
            algorithm.CryptoProvider = cryptoProvider;
            UpdateCapacityBar();
        }

        /// <summary>
        /// Updates the Embed/Extract buttons text to reflect action the button will activate.
        /// </summary>
        private void UpdateButtonText()
        {
            buttonActivateSteganography.Text = CanEmbed ? "Embed" : "Extract";
        }

        /// <summary>
        /// Checks whether stegoMessage contains content to be embedded, and runs the algoritm.Extract or algorithm.Embed methods accordingly.
        /// If the Extract method was called the interface is updated according to the new content of stegoMessage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonActivateSteganography_Click(object sender, EventArgs e)
        {
            if (carrierMedia == null)
            {
                MessageBox.Show("You must supply a carrier media.");
                return;
            }

            if (CanEmbed)
            {
                try
                {
                    Embed();
                    MessageBox.Show("Message was succesfully embedded.", "Success", MessageBoxButtons.OK);
                }
                catch (StegoAlgorithmException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    Extract();
                    MessageBox.Show("Message was succesfully extracted.", "Success", MessageBoxButtons.OK);
                }
                catch (StegoAlgorithmException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Extract hidden content from CarrierMedia.
        /// </summary>
        private void Extract()
        {
            algorithm.CarrierMedia = carrierMedia;
            algorithm.CryptoProvider.CryptoKey = textBoxEncryptionKey.Text;
            stegoMessage = algorithm.Extract();
            if (stegoMessage.InputFiles.Count != 0)
            {
                foreach (InputFile file in stegoMessage.InputFiles)
                {
                    ListViewItem fileItem = new ListViewItem(file.Name);
                    fileItem.SubItems.Add(FileSizeExtensions.StringFormatBytes(file.Content.LongLength));
                    fileItem.ImageKey = file.Name.Substring(file.Name.LastIndexOf('.'));
                    if (!imageListIcons.Images.ContainsKey(fileItem.ImageKey))
                        imageListIcons.Images.Add(fileItem.ImageKey, IconExtractor.ExtractIcon(fileItem.ImageKey));

                    listViewMessageContentFiles.Items.Add(fileItem);
                    UpdateButtonText();
                    UpdateCapacityBar();
                }
            }
            textBoxTextMessage.Text = stegoMessage.TextMessage;
        }

        /// <summary>
        /// Embed hidden content into CarrierMedia.
        /// </summary>
        private void Embed()
        {
            algorithm.CarrierMedia = carrierMedia;
            algorithm.CryptoProvider.CryptoKey = textBoxEncryptionKey.Text;
            algorithm.Embed(stegoMessage);
            algorithm.CarrierMedia.SaveToFile($"Stego-{carrierName}{carrierExtension}");
        }
        #endregion
        
        /// <summary>
        /// Gets and IInputType with a file path. Checks the type of the input and handles it accordingly.
        /// </summary>
        /// <param name="_input"></param>
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
                listViewMessageContentFiles.Items.Add(fileItem);
            }
            else if (_input is CarrierType)
            {
                if (fileInfo.Extension == ".wav")
                {
                    carrierMedia = new AudioCarrier(_input.FilePath);
                    pictureBoxCarrier.Image = Icon.ExtractAssociatedIcon(_input.FilePath)?.ToBitmap();
                    carrierExtension = fileInfo.Extension;
                }
                else
                {
                    carrierMedia = new ImageCarrier(_input.FilePath);
                    pictureBoxCarrier.Image = ((ImageCarrier) carrierMedia).InnerImage;
                    carrierExtension = ".png";
                }
                carrierName = fileInfo.Name.Remove(fileInfo.Name.LastIndexOf('.'));
            }

            UpdateCapacityBar();
            UpdateButtonText();
        }
        
        /// <summary>
        /// Checks the size of the message content and the capacity of the carrierMedia and updates the progressBarCapacity and labelCapacityWarning controls accordingly.
        /// </summary>
        private void UpdateCapacityBar()
        {
            decimal ratio, max = progressBarCapacity.Maximum;
            long size = stegoMessage.GetCompressedSize();
            if (carrierMedia != null)
            {
                algorithm.CarrierMedia = carrierMedia;
                long capacity = algorithm.ComputeBandwidth();
                if (capacity >= size)
                {
                    ratio = 100 * ((decimal) size / capacity);
                }
                else
                {
                    ratio = max;
                }
            }
            else
            {
                ratio = max;
            }
            if (ratio == max)
            {
                labelCapacityWarning.Text = $"< 100%";
                labelCapacityWarning.ForeColor = Color.Red;
            }
            else
            {

                labelCapacityWarning.Text = $"{ratio :.##}%";
                labelCapacityWarning.ForeColor = SystemColors.ControlText;
            }
            progressBarCapacity.Value = (int) ratio;
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            RSAKeyPair keyPair = RSAProvider.GenerateKeys(2048);
            ShowSaveDialog("Save public key to...", "public_key", "XML File (*.xml)|*.xml", Encoding.UTF8.GetBytes(keyPair.PublicKey));
            ShowSaveDialog("Save private key to...", "private_key", "XML File (*.xml)|*.xml", Encoding.UTF8.GetBytes(keyPair.PrivateKey));
        }

        private void ShowSaveDialog(string title, string suggestedName, string filter, byte[] content)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = suggestedName;
            sfd.Title = title;
            sfd.Filter = filter;

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            File.WriteAllBytes(sfd.FileName, content);
        }

        
    }
}
