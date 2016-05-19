using Stegosaurus.Algorithm;
using Stegosaurus.Carrier;
using Stegosaurus.Exceptions;
using Stegosaurus.Utility;
using Stegosaurus.Utility.InputTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Stegosaurus.Cryptography;
using System.Threading.Tasks;
using Stegosaurus.Algorithm.CommonSample;
using System.Linq;
using System.Net;
using System.Reflection;
using Microsoft.VisualBasic;

namespace Stegosaurus.Forms
{
    public partial class FormMain : Form
    {
        private readonly Dictionary<string, StegoAlgorithmBase> algorithmDictionary = new Dictionary<string, StegoAlgorithmBase>();
        private readonly Dictionary<string, ICryptoProvider> cryptoProviderDictionary = new Dictionary<string, ICryptoProvider>();

        private StegoMessage stegoMessage = new StegoMessage();
        private ICarrierMedia carrierMedia;
        private StegoAlgorithmBase algorithm;
        private ICryptoProvider cryptoProvider;

        private string carrierName;
        private string carrierExtension;

        private bool CanEmbed => carrierMedia != null && (!string.IsNullOrEmpty(textBoxTextMessage.Text) || listViewMessageContentFiles.Items.Count > 0);

        public FormMain()
        {
            InitializeComponent();

            cryptoProvider = new AESProvider();

            // Add algorithms
            AddAlgorithm(typeof(GTARewrite));
            AddAlgorithm(typeof(LSBAlgorithm));
            AddAlgorithm(typeof(GraphTheoreticAlgorithm));
            AddAlgorithm(typeof(CommonSampleAlgorithm));

            // Add crypto providers
            AddCryptoProvider(typeof(AESProvider));
            //AddCryptoProvider(typeof(TripleDESProvider));
            AddCryptoProvider(typeof(RSAProvider));

            // Set default values
            comboBoxAlgorithmSelection.SelectedIndex = 0;
            comboBoxCryptoProviderSelection.SelectedIndex = 0;
        }
        
        #region Carrier Media Handling
        /// <summary>
        /// Checks that the files dragged into the panelCarrierMedia control are valid.
        /// Assigns the effect of the Drag and Drop accordingly.
        /// </summary>
        private void panelCarrierMedia_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        /// <summary>
        /// Gets the paths of the dropped files and converts the first to CarrierType and calls HandleInput.
        /// </summary>
        private void panelCarrierMedia_DragDrop(object sender, DragEventArgs e)
        {
            string[] inputFiles = (string[]) e.Data.GetData(DataFormats.FileDrop);
            if (inputFiles.Length == 1)
            {
                try
                {
                    IInputType inputContent = new CarrierType(inputFiles[0]);
                    HandleInput(inputContent);
                }
                catch (ArgumentNullException ex)
                {
                    ShowError(ex.Message, "Unknown error.");
                }
                catch (InvalidCarrierFileException ex)
                {
                    ShowError(ex.Message, "Invalid file.");
                }
            }
            else
            {
                ShowError("Cannot have multiple carrier media.");
            }
        }

        private void ShowError(string message, string title = "Error")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region StegoMessage Content Handling
        /// <summary>
        /// Assigns the content of the textBoxTextMessage.Text property to the stegoMessage.TextMessage property and updates the  to be the progressBarCapacity control.
        /// </summary>
        private void textBoxTextMessage_TextChanged(object sender, EventArgs e)
        {
            stegoMessage.TextMessage = textBoxTextMessage.Text;

            UpdateInterface();
        }

        /// <summary>
        /// Checks that the items dragged into the listViewMessageContentFiles control are valid and changes the effect, and color accordingly.
        /// </summary>
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
        private void listViewMessageContentFiles_DragLeave(object sender, EventArgs e)
        {
            listViewMessageContentFiles.BackColor = Color.White;
        }

        /// <summary>
        /// Gets the file paths of all dropped files, converts them to the ContentType and calls the HandleInput to handle them further.
        /// </summary>
        private void listViewMessageContentFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] inputFiles = (string[]) e.Data.GetData(DataFormats.FileDrop);

            foreach (string filePath in inputFiles)
            {
                HandleInput(new ContentType(filePath));
            }

            listViewMessageContentFiles.BackColor = Color.White;
        }
        
        /// <summary>
        /// Ensures that the contextMenuStripMain wont open if no items from listViewMessageContentFiles are selected.
        /// </summary>
        private void contextMenuStripMain_Opening(object sender, CancelEventArgs e)
        {
            bool enableIndividualButtons = listViewMessageContentFiles.SelectedItems.Count > 0;
            deleteToolStripMenuItem.Enabled = enableIndividualButtons;
            saveToolStripMenuItem.Enabled = enableIndividualButtons;
        }

        /// <summary>
        /// Allows saving of files from the stegoMessage.InputFiles, as selected in the listViewMessageContentFiles control,
        /// to a custom location. If single file is selected user is prompted with dialog to select destination and filename, 
        /// and if multiple files are selected the user is prompted to select a folder to which all selected file will be saved
        /// with their default names.
        /// </summary>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] fileIndices = GetSelectedContentIndices();
            int selectedCount = fileIndices.Length;
            if (selectedCount == 0)
            {
                ShowError("You must have items selected to save.", "Save error");
            }
            else if (selectedCount == 1)
            {
                SaveFileDialog sfd = new SaveFileDialog {FileName = stegoMessage.InputFiles[fileIndices[0]].Name};

                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                if (sfd.FileName == "")
                {
                    ShowError("The chosen destination cannot be blank.", "Save error");
                }
                else
                {
                    stegoMessage.InputFiles[fileIndices[0]].SaveTo(sfd.FileName);
                }
            }
            else
            {
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                if (string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
                {
                    MessageBox.Show(@"The chosen destination cannot be blank.", @"Save Error");
                }
                else
                {
                    foreach (int index in fileIndices)
                    {
                        string fileName = stegoMessage.InputFiles[fileIndices[index]].Name;
                        string saveDestination = Path.Combine(folderBrowserDialog.SelectedPath, fileName);

                        // Ask to overwrite if file exists
                        if (File.Exists(saveDestination))
                        {
                            if (MessageBox.Show($"The file {fileName} already exists. Do you want to overwrite it?", @"File already exists", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                            {
                                continue;
                            }
                        }

                        stegoMessage.InputFiles[fileIndices[index]].SaveTo(saveDestination);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the deletion of items from the listViewMessageContentFiles control and stegoMessage.InputFiles collection.
        /// </summary>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] fileIndices = GetSelectedContentIndices();

            for (int index = fileIndices.Length - 1; index >= 0; index--)
            {
                stegoMessage.InputFiles.RemoveAt(fileIndices[index]);
                listViewMessageContentFiles.Items.RemoveAt(fileIndices[index]);
            }

            UpdateInterface();
        }
        
        /// <summary>
        /// Returns an array of integers containing the indices of all selected items in the listViewMessageContentFiles control.
        /// </summary>
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
        private void AddCryptoProvider(Type _cryptoProviderType)
        {
            ICryptoProvider newCryptoProvider = (ICryptoProvider) Activator.CreateInstance(_cryptoProviderType);
            cryptoProviderDictionary.Add(newCryptoProvider.Name, newCryptoProvider);

            comboBoxCryptoProviderSelection.Items.Add(newCryptoProvider.Name);
        }

        /// <summary>
        /// Updates the cryptoProvider to reflect the chosen item in the comboBoxCryptoProviderSelection control.
        /// </summary>
        private void comboBoxCryptoProviderSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            cryptoProvider = cryptoProviderDictionary[comboBoxCryptoProviderSelection.Text];
            cryptoProvider.SetKey(textBoxEncryptionKey.Text);

            algorithm.CryptoProvider = cryptoProvider;

            buttonImportKey.Enabled = cryptoProvider is RSAProvider;
        }

        private void SetWaitingState(bool _isWaiting)
        {
            UseWaitCursor = _isWaiting;
            tabControlMain.Enabled = !_isWaiting; 
        }
        #endregion

        #region Steganography Handling
        /// <summary>
        /// Add an algorithm to the algorithm dictionary and combolist
        /// </summary>
        private void AddAlgorithm(Type _algorithmType)
        {
            StegoAlgorithmBase stegoAlgorithm = (StegoAlgorithmBase) Activator.CreateInstance(_algorithmType);
            if (!algorithmDictionary.ContainsKey(stegoAlgorithm.Name))
            {
                algorithmDictionary.Add(stegoAlgorithm.Name, stegoAlgorithm);
                comboBoxAlgorithmSelection.Items.Add(stegoAlgorithm.Name);
            }
        }

        /// <summary>
        /// Assigns the chosen algorithm to the algorithm variable and supplies the algorithm with the current carrierMedia.
        /// At last the progressBarCapacity is updated.
        /// </summary>
        private void comboBoxAlgorithmSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            algorithm = algorithmDictionary[comboBoxAlgorithmSelection.Text];
            algorithm.CarrierMedia = carrierMedia;
            algorithm.CryptoProvider = cryptoProvider;

            propertyGridAlgorithmOptions.SelectedObject = algorithm;

            UpdateInterface();
        }

        /// <summary>
        /// Checks whether stegoMessage contains content to be embedded, and runs the algoritm.Extract or algorithm.Embed methods accordingly.
        /// If the Extract method was called the interface is updated according to the new content of stegoMessage.
        /// </summary>
        private async void buttonActivateSteganography_Click(object sender, EventArgs e)
        {
            if (CanEmbed && string.IsNullOrEmpty(textBoxEncryptionKey.Text))
            {
                if (MessageBox.Show(@"You are about to embed without using an encryption key. Do you want to continue?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    textBoxEncryptionKey.Focus();
                    return;
                }
            }

            SetWaitingState(true);
            // Embed or extract
            if (CanEmbed)
            {
                stegoMessage.PrivateSigningKey = null;

                // Check if we need to use a private signing key
                if (checkBoxSignMessages.Checked)
                {
                    OpenFileDialog ofd = new OpenFileDialog
                    {
                        Title = @"Choose your private signing key",
                        Filter = @"Private Key (*.xml)|*.xml"
                    };

                    if (ofd.ShowDialog() != DialogResult.OK)
                    {
                        SetWaitingState(false);
                        return;
                    }

                    stegoMessage.PrivateSigningKey = File.ReadAllText(ofd.FileName);
                }

                // Embedding happens in another thread
                await Embed();
            }
            else
            {
                // Extraction happens in another thread
                await Extract();
            }
            SetWaitingState(false);
        }

        /// <summary>
        /// Extract hidden content from CarrierMedia.
        /// </summary>
        private async Task Extract()
        {
            algorithm.CarrierMedia = carrierMedia;
            algorithm.CryptoProvider.SetKey(textBoxEncryptionKey.Text);

            // Wait for StegoMessage
            StegoMessage extractedMessage = await Task.Run(() =>
            {
                try
                {
                    return algorithm.Extract();
                }
                catch (StegoCryptoException ex)
                {
                    ShowError(ex.Message, "Cryptography error");
                }
                catch (StegoMessageException ex)
                {
                    ShowError(ex.Message, "Message error");
                }
                catch (StegoAlgorithmException ex)
                {
                    ShowError(ex.Message, "Algorithm error");
                }
                return null;
            });

            // Return if invalid message
            if (extractedMessage == null)
            {
                return;
            }

            stegoMessage = extractedMessage;

            // Check if message is signed
            if (stegoMessage.SignState == StegoMessage.StegoMessageSignState.SignedByKnown)
            {
                labelSignStatus.Image = imageListSilkIcons.Images[5];
                labelSignStatus.ForeColor = Color.DarkGreen;
                labelSignStatus.Text = $"This message has been signed by {stegoMessage.SignedBy}.";
            }
            else
            {
                labelSignStatus.Image = imageListSilkIcons.Images[6];
                labelSignStatus.ForeColor = Color.DarkRed;
                labelSignStatus.Text = stegoMessage.SignState == StegoMessage.StegoMessageSignState.Unsigned ? "This message is unsigned." : "This message has been signed with an unknown key.";
            }

            // Add files
            foreach (InputFile file in stegoMessage.InputFiles)
            {
                ListViewItem fileItem = new ListViewItem(file.Name);
                fileItem.SubItems.Add(SizeFormatter.StringFormatBytes(file.Content.LongLength));
                fileItem.ImageKey = file.Name.Substring(file.Name.LastIndexOf('.'));
                if (!imageListIcons.Images.ContainsKey(fileItem.ImageKey))
                {
                    imageListIcons.Images.Add(fileItem.ImageKey, IconExtractor.ExtractIcon(fileItem.ImageKey));
                }

                listViewMessageContentFiles.Items.Add(fileItem);
                UpdateInterface();
            }

            textBoxTextMessage.Text = stegoMessage.TextMessage;
        }

        /// <summary>
        /// Embed hidden content into CarrierMedia.
        /// </summary>
        private async Task Embed()
        {
            algorithm.CarrierMedia = carrierMedia;
            algorithm.CryptoProvider.SetKey(textBoxEncryptionKey.Text);

            FormEmbeddingProgress progressForm = new FormEmbeddingProgress();
            progressForm.Show(this);

            await progressForm.Run(stegoMessage, algorithm, carrierName, carrierExtension);
        }
        #endregion
        
        /// <summary>
        /// Gets an IInputType with a file path. Checks the type of the input and handles it accordingly.
        /// </summary>
        private void HandleInput(IInputType _input)
        {
            InputFile inputFile = new InputFile(_input.FilePath);
            FileInfo fileInfo = new FileInfo(_input.FilePath);

            if (_input is ContentType)
            {
                ListViewItem fileItem = new ListViewItem(inputFile.Name);
                fileItem.SubItems.Add(SizeFormatter.StringFormatBytes(fileInfo.Length));
                fileItem.ImageKey = fileInfo.Extension;
                if (!imageListIcons.Images.ContainsKey(fileItem.ImageKey))
                    imageListIcons.Images.Add(fileItem.ImageKey, Icon.ExtractAssociatedIcon(_input.FilePath));

                stegoMessage.InputFiles.Add(inputFile);
                listViewMessageContentFiles.Items.Add(fileItem);
            }
            else if (_input is CarrierType)
            {
                labelSignStatus.Text = "Ready";
                labelSignStatus.ForeColor = Color.Black;
                labelSignStatus.Image = null;

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

            UpdateInterface();
        }
        
        /// <summary>
        /// Checks the size of the message content and the capacity of the carrierMedia and updates the progressBarCapacity and labelCapacityWarning controls accordingly.
        /// </summary>
        private void UpdateInterface()
        {
            long size = stegoMessage.GetCompressedSize() + cryptoProvider?.HeaderSize ?? 0;
            if (carrierMedia == null)
            {
                progressBarCapacity.Value = progressBarCapacity.Maximum;
                labelCapacityWarning.Text = "N/A";
                labelCapacityWarning.ForeColor = Color.Black;
                return;
            }

            // Calculate capacity, size and ratio of those
            algorithm.CarrierMedia = carrierMedia;
            long capacity = algorithm.ComputeBandwidth();
            double ratio = 100 * ((double) size / capacity);

            // Update progressbar
            progressBarCapacity.Value = size >= capacity ? progressBarCapacity.Maximum : (int) ratio;

            // Update label
            labelCapacityWarning.Text = $"{ratio:#.##}% ({SizeFormatter.StringFormatBytes(size)}/{SizeFormatter.StringFormatBytes(capacity)})";
            labelCapacityWarning.ForeColor = size > capacity ? Color.Red : Color.Green;

            // Update button
            buttonActivateSteganography.ImageIndex = CanEmbed ? 0 : 1;
            buttonActivateSteganography.Enabled = (size <= capacity) && carrierMedia != null;
            buttonActivateSteganography.Text = CanEmbed ? "Embed content" : "Extract content";
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

        private void buttonImportKey_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog {Filter = @"XML files (*.xml)|*.xml|All files (*.*)|*.*"};

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            textBoxEncryptionKey.Text = File.ReadAllText(ofd.FileName);
        }

        private void listViewMessageContentFiles_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(listViewMessageContentFiles, "Right click to delete and save files.");
        }

        private void ShowToolTip(Control _control, string _message)
        {
            new ToolTip().SetToolTip(_control, _message);
        }

        private void browseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Multiselect = false,
                Filter = @"InnerImage files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.gif, *.bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.gif; *.bmp|Audio files (*.wav)|*.wav"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            HandleInput(new CarrierType(ofd.FileName));
        }

        private void addItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            foreach (string fileName in ofd.FileNames)
            {
                HandleInput(new ContentType(fileName));
            }
        }

        private void contextMenuStripPictureBox_Opening(object sender, CancelEventArgs e)
        {
            findUniqueSamplesToolStripMenuItem.Enabled = carrierMedia != null;
        }

        private async void findUniqueSamplesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetWaitingState(true);

            // Wait for unique sample count
            int numUniqueSamples = await Task.Run(() =>
            {
                return Sample.GetSampleListFrom(carrierMedia, 0).GroupBy(v => v).Count();
            });
            MessageBox.Show($"There are {numUniqueSamples} unique samples.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            SetWaitingState(false);
        }

        private void buttonImportAlgorithm_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "DLL files (*.dll)|*.dll";

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // Load assembly
            Assembly asm = Assembly.LoadFrom(ofd.FileName);
            Type[] types = asm.GetTypes();
            foreach (Type type in types)
            {
                if (type.BaseType == typeof(StegoAlgorithmBase))
                {
                    AddAlgorithm(type);
                }
            }
        }

        private void buttonAddPublicKey_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // Get an alias for this key
            string alias = Interaction.InputBox("What alias do you want to give to this key?", "Alias", Path.GetFileNameWithoutExtension(ofd.FileName));
            if (string.IsNullOrWhiteSpace(alias))
            {
                return;
            }

            // Create known keys folder
            if (!Directory.Exists(Program.KnownKeysFolder))
            {
                Directory.CreateDirectory(Program.KnownKeysFolder);
            }

            // Check if file exists
            string fileDestination = Path.Combine(Program.KnownKeysFolder, $"{alias}.xml");
            if (File.Exists(fileDestination))
            {
                MessageBox.Show("The alias '{alias}' has already been added.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Copy to destination
            PublicKeyList.Add(alias, ofd.FileName);
            File.Copy(ofd.FileName, fileDestination);
        }

        private void textBoxEncryptionKey_TextChanged(object sender, EventArgs e)
        {
            cryptoProvider?.SetKey(textBoxEncryptionKey.Text);

            UpdateInterface();
        }

        private void importFromURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string requestedUrl = Interaction.InputBox("Which URL to import image from?", "Import", "");
            if (string.IsNullOrWhiteSpace(requestedUrl))
            {
                return;
            }

            // Download image with webclient
            WebClient webClient = new WebClient();
            string tempLocation = Path.GetTempFileName();
            try
            {
                webClient.DownloadFile(requestedUrl, tempLocation);

                HandleInput(new CarrierType(tempLocation));
            }
            catch (WebException)
            {
                ShowError("An error occurred while downloading the file.");
            }
            catch (InvalidImageFileException)
            {
                ShowError("The selected URL is not an image.");
            }
            finally
            {
                File.Delete(tempLocation);
            }
        }

    }
}
