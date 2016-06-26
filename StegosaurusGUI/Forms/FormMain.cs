using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Stegosaurus;
using Stegosaurus.Algorithm;
using Stegosaurus.Carrier;
using Stegosaurus.Cryptography;
using Stegosaurus.Exceptions;
using Stegosaurus.Utility;
using StegosaurusGUI.Utility;
using CarrierType = Stegosaurus.Utility.InputTypes.CarrierType;
using ContentType = Stegosaurus.Utility.InputTypes.ContentType;
using IInputType = Stegosaurus.Utility.InputTypes.IInputType;
using Stegosaurus.Archive;

namespace StegosaurusGUI.Forms
{
    public partial class FormMain
    {
        private readonly Dictionary<string, StegoAlgorithmBase> algorithmDictionary = new Dictionary<string, StegoAlgorithmBase>();
        private readonly Dictionary<string, ICryptoProvider> cryptoProviderDictionary = new Dictionary<string, ICryptoProvider>();
        private readonly List<Type> carrierMediaTypes = new List<Type>();

        private StegoMessage stegoMessage = new StegoMessage();
        private InputFolder currentFolder;

        private ICarrierMedia carrierMedia;
        private StegoAlgorithmBase algorithm;
        private ICryptoProvider cryptoProvider;

        private string carrierName;
        private string carrierExtension;

        private bool hasWarnedAdvanced;

        private bool CanEmbed => carrierMedia != null && (!string.IsNullOrEmpty(textBoxTextMessage.Text) || listViewMessageContentFiles.Items.Count > 0);

        private const string ProgramTitle = "Stegosaurus";

        public FormMain()
        {
            InitializeComponent();
            
            cryptoProvider = new AESProvider();
            currentFolder = stegoMessage.RootFolder;
            SetCurrentFolder(stegoMessage.RootFolder);

            // Add algorithms
            AddAlgorithm(typeof(GraphTheoreticAlgorithm));
            AddAlgorithm(typeof(LSBAlgorithm));
            AddAlgorithm(typeof(CommonSampleAlgorithm));

            // Add crypto providers
            AddCryptoProvider(typeof(AESProvider));
            AddCryptoProvider(typeof(RSAProvider));

            // Add carrier media types
            carrierMediaTypes.Add(typeof(ImageCarrier));
            carrierMediaTypes.Add(typeof(AudioCarrier));
          
            // Set default values
            comboBoxAlgorithmSelection.SelectedIndex = 0;
            comboBoxCryptoProviderSelection.SelectedIndex = 0;
        }
        
        /// <summary>
        /// Checks that the files dragged into the panelCarrierMedia control are valid.
        /// Assigns the effect of the Drag and Drop accordingly.
        /// </summary>
        private void panelCarrierMedia_DragEnter(object _sender, DragEventArgs _e)
        {
            _e.Effect = _e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        /// <summary>
        /// Gets the paths of the dropped files and converts the first to CarrierType and calls HandleInput.
        /// </summary>
        private void panelCarrierMedia_DragDrop(object _sender, DragEventArgs _e)
        {
            string[] inputFiles = (string[]) _e.Data.GetData(DataFormats.FileDrop);
            if (inputFiles.Length == 1)
            {
                try
                {
                    IInputType inputContent = new CarrierType(inputFiles[0]);
                    HandleInput(inputContent);
                }
                catch (ArgumentNullException ex)
                {
                    MessageBoxUtility.ShowError(ex.Message, "Unknown error");
                }
                catch (InvalidCarrierFileException ex)
                {
                    MessageBoxUtility.ShowError(ex.Message, "Carrier format error");
                }
                catch (StegoCarrierException ex)
                {
                    MessageBoxUtility.ShowError(ex.Message, "Carrier error");
                }
            }
            else
            {
                MessageBoxUtility.ShowError("Cannot have multiple carrier media.");
            }
        }

        /// <summary>
        /// Assigns the content of the textBoxTextMessage.Text property to the stegoMessage.TextMessage property and updates the  to be the progressBarCapacity control.
        /// </summary>
        private void textBoxTextMessage_TextChanged(object _sender, EventArgs _e)
        {
            stegoMessage.TextMessage = textBoxTextMessage.Text;

            UpdateInterface();
        }

        /// <summary>
        /// Checks that the items dragged into the listViewMessageContentFiles control are valid and changes the effect, and color accordingly.
        /// </summary>
        private void listViewMessageContentFiles_DragEnter(object _sender, DragEventArgs _e)
        {
            if (_e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                _e.Effect = DragDropEffects.Copy;
                listViewMessageContentFiles.BackColor = Color.LightGreen;
            }
            else
            {
                _e.Effect = DragDropEffects.None;
                listViewMessageContentFiles.BackColor = Color.Red;
            }
        }

        /// <summary>
        /// Reverts the color of the listViewMessageContentFiles control to white when the files are dragged out of the conrtrols boundaries. 
        /// </summary>
        private void listViewMessageContentFiles_DragLeave(object _sender, EventArgs _e)
        {
            listViewMessageContentFiles.BackColor = Color.White;
        }

        /// <summary>
        /// Gets the file paths of all dropped files, converts them to the ContentType and calls the HandleInput to handle them further.
        /// </summary>
        private void listViewMessageContentFiles_DragDrop(object _sender, DragEventArgs _e)
        {
            string[] inputFiles = (string[]) _e.Data.GetData(DataFormats.FileDrop);

            foreach (string filePath in inputFiles)
            {
                HandleInput(new ContentType(filePath));
            }

            listViewMessageContentFiles.BackColor = Color.White;
        }
        
        /// <summary>
        /// Ensures that the contextMenuStripMain wont open if no items from listViewMessageContentFiles are selected.
        /// </summary>
        private void contextMenuStripMain_Opening(object _sender, CancelEventArgs _e)
        {
            bool enableIndividualButtons = listViewMessageContentFiles.SelectedItems.Count > 0;
            deleteToolStripMenuItem.Enabled = enableIndividualButtons;
            saveToolStripMenuItem.Enabled = enableIndividualButtons;
        }

        /// <summary>
        /// Allows saving of files from the currentFolder.Items, as selected in the listViewMessageContentFiles control,
        /// to a custom location. If single file is selected user is prompted with dialog to select destination and filename, 
        /// and if multiple files are selected the user is prompted to select a folder to which all selected file will be saved
        /// with their default names.
        /// </summary>
        private void saveToolStripMenuItem_Click(object _sender, EventArgs _e)
        {
            int selectedCountt = listViewMessageContentFiles.SelectedItems.Count;
            bool folderSelected = listViewMessageContentFiles.SelectedItems.Cast<ListViewItem>().Any(i => i.Tag is InputFolder);

            if (folderSelected || selectedCountt > 1)
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                if (fd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                foreach (ListViewItem item in listViewMessageContentFiles.SelectedItems)
                {
                    ArchiveItem archiveItem = item.Tag as ArchiveItem;
                    archiveItem?.SaveTo(Path.Combine(fd.SelectedPath, archiveItem.Name));
                }
            }
            else
            {
                InputFile selected = listViewMessageContentFiles.SelectedItems[0].Tag as InputFile;
                ShowSaveDialog("Save to...", selected.Name, "All files (*.*)|*.*", selected.Content);
            }
            /*int[] fileIndices = GetSelectedContentIndices();
            int selectedCount = fileIndices.Length;
            if (selectedCount == 0)
            {
                MessageBoxUtility.ShowError("You must have items selected to save.", "Save error");
            }
            else if (selectedCount == 1)
            {
                SaveFileDialog sfd = new SaveFileDialog {FileName = currentFolder.Items[fileIndices[0]].Name};

                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                if (sfd.FileName == "")
                {
                    MessageBoxUtility.ShowError("The chosen destination cannot be blank.", "Save error");
                }
                else
                {
                    currentFolder.Items[fileIndices[0]].SaveTo(sfd.FileName);
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
                        string fileName = currentFolder.Items[fileIndices[index]].Name;
                        string saveDestination = Path.Combine(folderBrowserDialog.SelectedPath, fileName);

                        // Ask to overwrite if file exists
                        if (File.Exists(saveDestination))
                        {
                            if (MessageBox.Show($"The file {fileName} already exists. Do you want to overwrite it?", @"File already exists", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                            {
                                continue;
                            }
                        }

                        currentFolder.Items[fileIndices[index]].SaveTo(saveDestination);
                    }
                }
            }*/
        }

        /// <summary>
        /// Handles the deletion of items from the listViewMessageContentFiles control and currentFolder.Items collection.
        /// </summary>
        private void deleteToolStripMenuItem_Click(object _sender, EventArgs _e)
        {
            foreach (ListViewItem item in listViewMessageContentFiles.SelectedItems)
            {
                currentFolder.Items.Remove((ArchiveItem) item.Tag);
                listViewMessageContentFiles.Items.Remove(item);
            }
            /*int[] fileIndices = GetSelectedContentIndices();

            for (int index = fileIndices.Length - 1; index >= 0; index--)
            {
                currentFolder.Items.Remove((ArchiveItem) listViewMessageContentFiles.Items[fileIndices[index]].Tag);
                listViewMessageContentFiles.Items.RemoveAt(fileIndices[index]);
            }*/

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

        /// <summary>
        /// Adds a crypto provider to the cryptoprovider dictionary and combolist
        /// </summary>
        private void AddCryptoProvider(Type _cryptoProviderType)
        {
            ICryptoProvider newCryptoProvider = (ICryptoProvider) Activator.CreateInstance(_cryptoProviderType);
            if (!cryptoProviderDictionary.ContainsKey(newCryptoProvider.Name))
            {
                cryptoProviderDictionary.Add(newCryptoProvider.Name, newCryptoProvider);

                comboBoxCryptoProviderSelection.Items.Add(newCryptoProvider.Name);
            }
        }

        /// <summary>
        /// Updates the cryptoProvider to reflect the chosen item in the comboBoxCryptoProviderSelection control.
        /// </summary>
        private void comboBoxCryptoProviderSelection_SelectedIndexChanged(object _sender, EventArgs _e)
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
        private void comboBoxAlgorithmSelection_SelectedIndexChanged(object _sender, EventArgs _e)
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
        private async void buttonActivateSteganography_Click(object _sender, EventArgs _e)
        {
            if (CanEmbed && string.IsNullOrEmpty(textBoxEncryptionKey.Text))
            {
                if (MessageBox.Show(@"You are about to embed without using an encryption key. Do you want to continue?", @"Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes)
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
                    MessageBoxUtility.ShowError(ex.Message, "Cryptography error");
                }
                catch (StegoMessageException ex)
                {
                    MessageBoxUtility.ShowError(ex.Message, "Message error");
                }
                catch (StegoAlgorithmException ex)
                {
                    MessageBoxUtility.ShowError(ex.Message, "Algorithm error");
                }
                return null;
            });

            // Return if invalid message
            if (extractedMessage == null)
            {
                return;
            }

            stegoMessage = extractedMessage;
            textBoxTextMessage.Text = stegoMessage.TextMessage;
            SetCurrentFolder(stegoMessage.RootFolder);

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

            UpdateInterface();
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

        /// <summary>
        /// Gets an IInputType with a file path. Checks the type of the input and handles it accordingly.
        /// </summary>
        private void HandleInput(IInputType _input)
        {
            if (File.Exists(_input.FilePath))
            {
                if (_input is CarrierType)
                {
                    SetCarrier(new FileInfo(_input.FilePath));
                }
                else if (_input is ContentType)
                {
                    InputFile inputFile = new InputFile(_input.FilePath);
                    currentFolder.Items.Add(inputFile);
                    AddArchiveItem(inputFile);
                }
            }
            else if (Directory.Exists(_input.FilePath) && _input is ContentType)
            {
                InputFolder inputFolder = new InputFolder(new DirectoryInfo(_input.FilePath).Name) { Parent = currentFolder };
                currentFolder.Items.Insert(0, inputFolder);
                FillDirectory(inputFolder, _input.FilePath);
                AddArchiveItem(inputFolder);
            }

            UpdateInterface();
        }

        private void FillDirectory(InputFolder _inputFolder, string _dirPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(_dirPath);

            // Add directories.
            foreach (DirectoryInfo subDirInfo in dirInfo.GetDirectories())
            {
                InputFolder newFolder = new InputFolder(subDirInfo.Name) { Parent = _inputFolder };
                FillDirectory(newFolder, subDirInfo.FullName);
                _inputFolder.Items.Insert(0, newFolder);
            }

            // Add files.
            foreach (FileInfo fileInfo in dirInfo.GetFiles())
            {
                _inputFolder.Items.Add(new InputFile(fileInfo.FullName));
            }
        }

        private void SetCarrier(FileInfo _fileInfo)
        {
            // Set new carrier media.
            labelSignStatus.Text = @"Ready";
            labelSignStatus.ForeColor = Color.Black;
            labelSignStatus.Image = null;

            // Find suitable carrier media
            foreach (Type carrierType in carrierMediaTypes)
            {
                // Skip types that are not ICarrierMedia.
                if (!carrierType.GetInterfaces().Contains(typeof(ICarrierMedia)))
                {
                    continue;
                }

                // Create instance.
                carrierMedia = (ICarrierMedia) Activator.CreateInstance(carrierType);

                // Validate extension
                if (!carrierMedia.IsExtensionCompatible(_fileInfo.Extension.ToLower()))
                {
                    carrierMedia = null;
                }
                else
                {
                    carrierMedia.LoadFromFile(_fileInfo.FullName);
                    pictureBoxCarrier.Image = carrierMedia.Thumbnail;
                    carrierExtension = carrierMedia.OutputExtension;
                    break;
                }
            }

            // Warn user if no suitable carrier was found.
            if (carrierMedia == null)
            {
                pictureBoxCarrier.Image = null;
                MessageBox.Show(@"No suitable carrier media was found for this file.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                carrierName = _fileInfo.Name.Remove(_fileInfo.Name.LastIndexOf('.'));
            }
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
                labelCapacityWarning.Text = @"N/A";
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

        private void buttonGenerate_Click(object _sender, EventArgs _e)
        {
            RSAKeyPair keyPair = RSAProvider.GenerateKeys(2048);
            ShowSaveDialog("Save public key to...", "public_key", "XML File (*.xml)|*.xml", Encoding.UTF8.GetBytes(keyPair.PublicKey));
            ShowSaveDialog("Save private key to...", "private_key", "XML File (*.xml)|*.xml", Encoding.UTF8.GetBytes(keyPair.PrivateKey));
        }

        private void ShowSaveDialog(string _title, string _suggestedName, string _filter, byte[] _content)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = _suggestedName,
                Title = _title,
                Filter = _filter
            };

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            File.WriteAllBytes(sfd.FileName, _content);  
        }

        private void buttonImportKey_Click(object _sender, EventArgs _e)
        {
            OpenFileDialog ofd = new OpenFileDialog {Filter = @"XML files (*.xml)|*.xml|All files (*.*)|*.*"};

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            textBoxEncryptionKey.Text = File.ReadAllText(ofd.FileName);
        }

        private void listViewMessageContentFiles_MouseHover(object _sender, EventArgs _e)
        {
            ShowToolTip(listViewMessageContentFiles, "Right click to delete and save files.");
        }

        private void ShowToolTip(Control _control, string _message)
        {
            new ToolTip().SetToolTip(_control, _message);
        }

        private void browseToolStripMenuItem_Click(object _sender, EventArgs _e)
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

        private void addItemToolStripMenuItem_Click(object _sender, EventArgs _e)
        {
            OpenFileDialog ofd = new OpenFileDialog {Multiselect = true};

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            foreach (string fileName in ofd.FileNames)
            {
                HandleInput(new ContentType(fileName));
            }
        }

        private void contextMenuStripPictureBox_Opening(object _sender, CancelEventArgs _e)
        {
            findUniqueSamplesToolStripMenuItem.Enabled = carrierMedia != null;
        }

        private async void findUniqueSamplesToolStripMenuItem_Click(object _sender, EventArgs _e)
        {
            SetWaitingState(true);

            // Wait for unique sample count
            int numUniqueSamples = await Task.Run(() =>
            {
                return Sample.GetSampleListFrom(carrierMedia, 0).GroupBy(v => v).Count();
            });
            MessageBox.Show($"There are {numUniqueSamples} unique samples.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            SetWaitingState(false);
        }

        private void buttonImportAlgorithm_Click(object _sender, EventArgs _e)
        {
            OpenFileDialog ofd = new OpenFileDialog {Filter = @"Stegosaurus Plugin (*.dll)|*.dll"};

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // Load assembly
            Assembly asm;
            try
            {
                asm = Assembly.LoadFrom(ofd.FileName);
            }
            catch (BadImageFormatException)
            {
                MessageBoxUtility.ShowError("The selected file is not a valid plugin.");
                return;
            }
            Type[] types = asm.GetTypes();
            int numCompatibleTypes = 0;
            foreach (Type type in types)
            {
                if (type.BaseType == typeof(StegoAlgorithmBase))
                {
                    AddAlgorithm(type);
                    numCompatibleTypes++;
                }
                else if (type.GetInterfaces().Contains(typeof(ICarrierMedia)))
                {
                    if (carrierMediaTypes.Contains(type))
                    {
                        carrierMediaTypes.Add(type);
                    }
                    numCompatibleTypes++;
                }
                else if (type.GetInterfaces().Contains(typeof(ICryptoProvider)))
                {
                    AddCryptoProvider(type);
                    numCompatibleTypes++;
                }
            }

            MessageBox.Show($"{numCompatibleTypes} compatible types were found in the plugin.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonAddPublicKey_Click(object _sender, EventArgs _e)
        {
            OpenFileDialog ofd = new OpenFileDialog {Filter = @"XML files (*.xml)|*.xml|All files (*.*)|*.*"};

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
                MessageBox.Show(@"The alias '{alias}' has already been added.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Copy to destination
            PublicKeyList.Add(alias, ofd.FileName);
            File.Copy(ofd.FileName, fileDestination);
        }

        private void textBoxEncryptionKey_TextChanged(object _sender, EventArgs _e)
        {
            cryptoProvider?.SetKey(textBoxEncryptionKey.Text);

            UpdateInterface();
        }

        private void importFromURLToolStripMenuItem_Click(object _sender, EventArgs _e)
        {
            string requestedUrl = Interaction.InputBox("Which URL to import image from?", "Import");
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
                MessageBoxUtility.ShowError("An error occurred while downloading the file.");
            }
            catch (InvalidImageFileException)
            {
                MessageBoxUtility.ShowError("The selected URL is not an image.");
            }
            finally
            {
                File.Delete(tempLocation);
            }
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!hasWarnedAdvanced && tabControlMain.SelectedTab == tabPageAdvanced)
            {
                string warningString = "Advanced Options should not be edited unless you know what you are doing. "
                    + "The default values are recommended for your own safety and the stability of the program.\n\nDo you want to continue?";

                if (MessageBox.Show(warningString, "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    hasWarnedAdvanced = true;
                }
                else
                {
                    tabControlMain.SelectTab(tabPageMain);
                }
            }
        }

        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string folderName = Interaction.InputBox("What name should the folder have?", "Name");
            if (string.IsNullOrWhiteSpace(folderName))
            {
                return;
            }

            // Add new folder.
            InputFolder newFolder = new InputFolder(folderName) {Parent = currentFolder};
            currentFolder.Items.Insert(0, newFolder);

            // Add to list and update.
            AddArchiveItem(newFolder);
            UpdateInterface();
        }

        private void SetCurrentFolder(InputFolder _folder)
        {
            currentFolder = _folder;
            listViewMessageContentFiles.Items.Clear();

            // Set title
            string str = string.Empty;
            GetPathString(currentFolder, ref str);
            Text = $"{ProgramTitle} - {str}";

            // Add items.
            _folder.Items.ForEach(AddArchiveItem);
        }

        private void GetPathString(InputFolder folder, ref string progress)
        {
            if (folder == null)
            {
                return;
            }
            else
            {
                progress = $"/{folder.Name}{progress}";
                GetPathString(folder.Parent, ref progress);
            }
        }

        private void AddArchiveItem(ArchiveItem _archiveItem)
        {
            ListViewItem viewItem = new ListViewItem(_archiveItem.Name) { Tag = _archiveItem };
            viewItem.Tag = _archiveItem;
            if (_archiveItem is InputFile)
            {
                if (_archiveItem.Name.Contains('.'))
                    viewItem.ImageKey = _archiveItem.Name.Substring(_archiveItem.Name.LastIndexOf('.'));
                viewItem.SubItems.Add(SizeFormatter.StringFormatBytes(((InputFile) _archiveItem).Content.Length));

                // Add file icon if it does not exist.
                if (!imageListIcons.Images.ContainsKey(viewItem.ImageKey))
                {
                    imageListIcons.Images.Add(viewItem.ImageKey, IconExtractor.ExtractIcon(viewItem.ImageKey));
                }
            }
            else
            {
                viewItem.ImageIndex = 0;
                viewItem.SubItems.Add("-");
            }

            if (_archiveItem is InputFolder)
                listViewMessageContentFiles.Items.Insert(0, viewItem);
            else
                listViewMessageContentFiles.Items.Add(viewItem);
        }

        private void listViewMessageContentFiles_DoubleClick(object sender, EventArgs e)
        {
            if (listViewMessageContentFiles.SelectedItems.Count != 1)
            {
                return;
            }

            // Get archive item
            InputFolder folder = listViewMessageContentFiles.SelectedItems[0].Tag as InputFolder;
            if (folder != null)
            {
                SetCurrentFolder(folder);
            }
        }

        private void listViewMessageContentFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                if (currentFolder.Parent != null)
                {
                    SetCurrentFolder(currentFolder.Parent);
                }
            }
            else if (e.KeyCode == Keys.Return)
            {
                listViewMessageContentFiles_DoubleClick(null, null);
            }
        }
    }
}
