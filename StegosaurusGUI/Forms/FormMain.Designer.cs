using System.Windows.Forms;

namespace StegosaurusGUI.Forms
{
    partial class FormMain : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && ( components != null ))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.listViewMessageContentFiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
            this.labelTextMesage = new System.Windows.Forms.Label();
            this.buttonActivateSteganography = new System.Windows.Forms.Button();
            this.imageListSilkIcons = new System.Windows.Forms.ImageList(this.components);
            this.labelEncryptionKey = new System.Windows.Forms.Label();
            this.pictureBoxCarrier = new System.Windows.Forms.PictureBox();
            this.contextMenuStripPictureBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.browseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFromURLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findUniqueSamplesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBarCapacity = new System.Windows.Forms.ProgressBar();
            this.panelPictureBox = new System.Windows.Forms.Panel();
            this.labelCarrierMedia = new System.Windows.Forms.Label();
            this.labelAlgorithmList = new System.Windows.Forms.Label();
            this.comboBoxAlgorithmSelection = new System.Windows.Forms.ComboBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.labelStorageRatio = new System.Windows.Forms.Label();
            this.labelCapacityWarning = new System.Windows.Forms.Label();
            this.textBoxTextMessage = new System.Windows.Forms.RichTextBox();
            this.labelCryptoProvider = new System.Windows.Forms.Label();
            this.comboBoxCryptoProviderSelection = new System.Windows.Forms.ComboBox();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.labelContentDescription = new System.Windows.Forms.Label();
            this.labelContent = new System.Windows.Forms.Label();
            this.labelCarrierDecsription = new System.Windows.Forms.Label();
            this.buttonImportKey = new System.Windows.Forms.Button();
            this.textBoxEncryptionKey = new System.Windows.Forms.RichTextBox();
            this.tabPageAdvanced = new System.Windows.Forms.TabPage();
            this.buttonAddPublicKey = new System.Windows.Forms.Button();
            this.labelMessageSIgning = new System.Windows.Forms.Label();
            this.checkBoxSignMessages = new System.Windows.Forms.CheckBox();
            this.buttonImportAlgorithm = new System.Windows.Forms.Button();
            this.labelAdvancedAlgorithm = new System.Windows.Forms.Label();
            this.propertyGridAlgorithmOptions = new System.Windows.Forms.PropertyGrid();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.labelSignStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCarrier)).BeginInit();
            this.contextMenuStripPictureBox.SuspendLayout();
            this.panelPictureBox.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.tabPageAdvanced.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewMessageContentFiles
            // 
            this.listViewMessageContentFiles.AllowDrop = true;
            this.listViewMessageContentFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewMessageContentFiles.ContextMenuStrip = this.contextMenuStripMain;
            this.listViewMessageContentFiles.FullRowSelect = true;
            this.listViewMessageContentFiles.GridLines = true;
            this.listViewMessageContentFiles.Location = new System.Drawing.Point(410, 44);
            this.listViewMessageContentFiles.Name = "listViewMessageContentFiles";
            this.listViewMessageContentFiles.Size = new System.Drawing.Size(362, 184);
            this.listViewMessageContentFiles.SmallImageList = this.imageListIcons;
            this.listViewMessageContentFiles.TabIndex = 3;
            this.listViewMessageContentFiles.UseCompatibleStateImageBehavior = false;
            this.listViewMessageContentFiles.View = System.Windows.Forms.View.Details;
            this.listViewMessageContentFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewMessageContentFiles_DragDrop);
            this.listViewMessageContentFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewMessageContentFiles_DragEnter);
            this.listViewMessageContentFiles.DragLeave += new System.EventHandler(this.listViewMessageContentFiles_DragLeave);
            this.listViewMessageContentFiles.MouseHover += new System.EventHandler(this.listViewMessageContentFiles_MouseHover);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 212;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Size";
            this.columnHeader2.Width = 129;
            // 
            // contextMenuStripMain
            // 
            this.contextMenuStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFilesToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStripMain.Name = "contextMenuStrip1";
            this.contextMenuStripMain.Size = new System.Drawing.Size(144, 88);
            this.contextMenuStripMain.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripMain_Opening);
            // 
            // addFilesToolStripMenuItem
            // 
            this.addFilesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addFilesToolStripMenuItem.Image")));
            this.addFilesToolStripMenuItem.Name = "addFilesToolStripMenuItem";
            this.addFilesToolStripMenuItem.Size = new System.Drawing.Size(143, 26);
            this.addFilesToolStripMenuItem.Text = "Add files";
            this.addFilesToolStripMenuItem.Click += new System.EventHandler(this.addItemToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(140, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(143, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(143, 26);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // imageListIcons
            // 
            this.imageListIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // labelTextMesage
            // 
            this.labelTextMesage.AutoSize = true;
            this.labelTextMesage.Location = new System.Drawing.Point(406, 231);
            this.labelTextMesage.Name = "labelTextMesage";
            this.labelTextMesage.Size = new System.Drawing.Size(262, 19);
            this.labelTextMesage.TabIndex = 8;
            this.labelTextMesage.Text = "Text message to hide in the carrier media:";
            // 
            // buttonActivateSteganography
            // 
            this.buttonActivateSteganography.Enabled = false;
            this.buttonActivateSteganography.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonActivateSteganography.ImageIndex = 1;
            this.buttonActivateSteganography.ImageList = this.imageListSilkIcons;
            this.buttonActivateSteganography.Location = new System.Drawing.Point(10, 323);
            this.buttonActivateSteganography.Name = "buttonActivateSteganography";
            this.buttonActivateSteganography.Size = new System.Drawing.Size(206, 50);
            this.buttonActivateSteganography.TabIndex = 9;
            this.buttonActivateSteganography.Text = "Extract content";
            this.buttonActivateSteganography.UseVisualStyleBackColor = true;
            this.buttonActivateSteganography.Click += new System.EventHandler(this.buttonActivateSteganography_Click);
            // 
            // imageListSilkIcons
            // 
            this.imageListSilkIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSilkIcons.ImageStream")));
            this.imageListSilkIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSilkIcons.Images.SetKeyName(0, "lock.png");
            this.imageListSilkIcons.Images.SetKeyName(1, "page_key.png");
            this.imageListSilkIcons.Images.SetKeyName(2, "application_xp.png");
            this.imageListSilkIcons.Images.SetKeyName(3, "cog.png");
            this.imageListSilkIcons.Images.SetKeyName(4, "key.png");
            this.imageListSilkIcons.Images.SetKeyName(5, "accept.png");
            this.imageListSilkIcons.Images.SetKeyName(6, "error.png");
            // 
            // labelEncryptionKey
            // 
            this.labelEncryptionKey.AutoSize = true;
            this.labelEncryptionKey.Location = new System.Drawing.Point(8, 231);
            this.labelEncryptionKey.Name = "labelEncryptionKey";
            this.labelEncryptionKey.Size = new System.Drawing.Size(250, 19);
            this.labelEncryptionKey.TabIndex = 11;
            this.labelEncryptionKey.Text = "Encryption or decryption key (optional):";
            // 
            // pictureBoxCarrier
            // 
            this.pictureBoxCarrier.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pictureBoxCarrier.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxCarrier.ContextMenuStrip = this.contextMenuStripPictureBox;
            this.pictureBoxCarrier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxCarrier.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxCarrier.Name = "pictureBoxCarrier";
            this.pictureBoxCarrier.Size = new System.Drawing.Size(390, 184);
            this.pictureBoxCarrier.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCarrier.TabIndex = 12;
            this.pictureBoxCarrier.TabStop = false;
            // 
            // contextMenuStripPictureBox
            // 
            this.contextMenuStripPictureBox.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripPictureBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.browseToolStripMenuItem,
            this.importFromURLToolStripMenuItem,
            this.findUniqueSamplesToolStripMenuItem});
            this.contextMenuStripPictureBox.Name = "contextMenuStripPictureBox";
            this.contextMenuStripPictureBox.Size = new System.Drawing.Size(220, 82);
            this.contextMenuStripPictureBox.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripPictureBox_Opening);
            // 
            // browseToolStripMenuItem
            // 
            this.browseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("browseToolStripMenuItem.Image")));
            this.browseToolStripMenuItem.Name = "browseToolStripMenuItem";
            this.browseToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.browseToolStripMenuItem.Text = "Browse";
            this.browseToolStripMenuItem.Click += new System.EventHandler(this.browseToolStripMenuItem_Click);
            // 
            // importFromURLToolStripMenuItem
            // 
            this.importFromURLToolStripMenuItem.Name = "importFromURLToolStripMenuItem";
            this.importFromURLToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.importFromURLToolStripMenuItem.Text = "Import from URL";
            this.importFromURLToolStripMenuItem.Click += new System.EventHandler(this.importFromURLToolStripMenuItem_Click);
            // 
            // findUniqueSamplesToolStripMenuItem
            // 
            this.findUniqueSamplesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("findUniqueSamplesToolStripMenuItem.Image")));
            this.findUniqueSamplesToolStripMenuItem.Name = "findUniqueSamplesToolStripMenuItem";
            this.findUniqueSamplesToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.findUniqueSamplesToolStripMenuItem.Text = "Find unique samples";
            this.findUniqueSamplesToolStripMenuItem.Click += new System.EventHandler(this.findUniqueSamplesToolStripMenuItem_Click);
            // 
            // progressBarCapacity
            // 
            this.progressBarCapacity.Location = new System.Drawing.Point(410, 342);
            this.progressBarCapacity.Maximum = 101;
            this.progressBarCapacity.Name = "progressBarCapacity";
            this.progressBarCapacity.Size = new System.Drawing.Size(362, 31);
            this.progressBarCapacity.TabIndex = 13;
            this.progressBarCapacity.Value = 50;
            // 
            // panelPictureBox
            // 
            this.panelPictureBox.AllowDrop = true;
            this.panelPictureBox.Controls.Add(this.pictureBoxCarrier);
            this.panelPictureBox.Location = new System.Drawing.Point(10, 44);
            this.panelPictureBox.Name = "panelPictureBox";
            this.panelPictureBox.Size = new System.Drawing.Size(390, 184);
            this.panelPictureBox.TabIndex = 14;
            this.panelPictureBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelCarrierMedia_DragDrop);
            this.panelPictureBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.panelCarrierMedia_DragEnter);
            // 
            // labelCarrierMedia
            // 
            this.labelCarrierMedia.AutoSize = true;
            this.labelCarrierMedia.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCarrierMedia.Location = new System.Drawing.Point(6, 3);
            this.labelCarrierMedia.Name = "labelCarrierMedia";
            this.labelCarrierMedia.Size = new System.Drawing.Size(106, 19);
            this.labelCarrierMedia.TabIndex = 17;
            this.labelCarrierMedia.Text = "Carrier media:";
            // 
            // labelAlgorithmList
            // 
            this.labelAlgorithmList.AutoSize = true;
            this.labelAlgorithmList.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAlgorithmList.Location = new System.Drawing.Point(4, 7);
            this.labelAlgorithmList.Name = "labelAlgorithmList";
            this.labelAlgorithmList.Size = new System.Drawing.Size(186, 19);
            this.labelAlgorithmList.TabIndex = 18;
            this.labelAlgorithmList.Text = "Steganography algorithm:";
            // 
            // comboBoxAlgorithmSelection
            // 
            this.comboBoxAlgorithmSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAlgorithmSelection.FormattingEnabled = true;
            this.comboBoxAlgorithmSelection.Location = new System.Drawing.Point(8, 29);
            this.comboBoxAlgorithmSelection.Name = "comboBoxAlgorithmSelection";
            this.comboBoxAlgorithmSelection.Size = new System.Drawing.Size(182, 25);
            this.comboBoxAlgorithmSelection.TabIndex = 19;
            this.comboBoxAlgorithmSelection.SelectedIndexChanged += new System.EventHandler(this.comboBoxAlgorithmSelection_SelectedIndexChanged);
            // 
            // labelStorageRatio
            // 
            this.labelStorageRatio.AutoSize = true;
            this.labelStorageRatio.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStorageRatio.Location = new System.Drawing.Point(407, 321);
            this.labelStorageRatio.Name = "labelStorageRatio";
            this.labelStorageRatio.Size = new System.Drawing.Size(154, 19);
            this.labelStorageRatio.TabIndex = 20;
            this.labelStorageRatio.Text = "Space used in carrier:";
            // 
            // labelCapacityWarning
            // 
            this.labelCapacityWarning.BackColor = System.Drawing.Color.Transparent;
            this.labelCapacityWarning.Location = new System.Drawing.Point(567, 320);
            this.labelCapacityWarning.Name = "labelCapacityWarning";
            this.labelCapacityWarning.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelCapacityWarning.Size = new System.Drawing.Size(205, 19);
            this.labelCapacityWarning.TabIndex = 23;
            this.labelCapacityWarning.Text = "N/A";
            this.labelCapacityWarning.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxTextMessage
            // 
            this.textBoxTextMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTextMessage.Location = new System.Drawing.Point(410, 253);
            this.textBoxTextMessage.Name = "textBoxTextMessage";
            this.textBoxTextMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBoxTextMessage.Size = new System.Drawing.Size(362, 64);
            this.textBoxTextMessage.TabIndex = 24;
            this.textBoxTextMessage.Text = "";
            this.textBoxTextMessage.TextChanged += new System.EventHandler(this.textBoxTextMessage_TextChanged);
            // 
            // labelCryptoProvider
            // 
            this.labelCryptoProvider.AutoSize = true;
            this.labelCryptoProvider.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCryptoProvider.Location = new System.Drawing.Point(6, 97);
            this.labelCryptoProvider.Name = "labelCryptoProvider";
            this.labelCryptoProvider.Size = new System.Drawing.Size(178, 19);
            this.labelCryptoProvider.TabIndex = 25;
            this.labelCryptoProvider.Text = "Cryptography algorithm:";
            // 
            // comboBoxCryptoProviderSelection
            // 
            this.comboBoxCryptoProviderSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCryptoProviderSelection.FormattingEnabled = true;
            this.comboBoxCryptoProviderSelection.Location = new System.Drawing.Point(9, 119);
            this.comboBoxCryptoProviderSelection.Name = "comboBoxCryptoProviderSelection";
            this.comboBoxCryptoProviderSelection.Size = new System.Drawing.Size(181, 25);
            this.comboBoxCryptoProviderSelection.TabIndex = 26;
            this.comboBoxCryptoProviderSelection.SelectedIndexChanged += new System.EventHandler(this.comboBoxCryptoProviderSelection_SelectedIndexChanged);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControlMain.Controls.Add(this.tabPageMain);
            this.tabControlMain.Controls.Add(this.tabPageAdvanced);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.ImageList = this.imageListSilkIcons;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(796, 432);
            this.tabControlMain.TabIndex = 29;
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.labelContentDescription);
            this.tabPageMain.Controls.Add(this.labelContent);
            this.tabPageMain.Controls.Add(this.labelCarrierDecsription);
            this.tabPageMain.Controls.Add(this.buttonImportKey);
            this.tabPageMain.Controls.Add(this.textBoxEncryptionKey);
            this.tabPageMain.Controls.Add(this.panelPictureBox);
            this.tabPageMain.Controls.Add(this.labelTextMesage);
            this.tabPageMain.Controls.Add(this.listViewMessageContentFiles);
            this.tabPageMain.Controls.Add(this.textBoxTextMessage);
            this.tabPageMain.Controls.Add(this.labelCapacityWarning);
            this.tabPageMain.Controls.Add(this.labelStorageRatio);
            this.tabPageMain.Controls.Add(this.buttonActivateSteganography);
            this.tabPageMain.Controls.Add(this.labelCarrierMedia);
            this.tabPageMain.Controls.Add(this.labelEncryptionKey);
            this.tabPageMain.Controls.Add(this.progressBarCapacity);
            this.tabPageMain.ImageIndex = 2;
            this.tabPageMain.Location = new System.Drawing.Point(4, 29);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMain.Size = new System.Drawing.Size(788, 399);
            this.tabPageMain.TabIndex = 0;
            this.tabPageMain.Text = "Main";
            this.tabPageMain.UseVisualStyleBackColor = true;
            // 
            // labelContentDescription
            // 
            this.labelContentDescription.AutoSize = true;
            this.labelContentDescription.Location = new System.Drawing.Point(406, 22);
            this.labelContentDescription.Name = "labelContentDescription";
            this.labelContentDescription.Size = new System.Drawing.Size(298, 19);
            this.labelContentDescription.TabIndex = 33;
            this.labelContentDescription.Text = "Drag and drop files to hide in the carrier media:";
            // 
            // labelContent
            // 
            this.labelContent.AutoSize = true;
            this.labelContent.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelContent.Location = new System.Drawing.Point(406, 3);
            this.labelContent.Name = "labelContent";
            this.labelContent.Size = new System.Drawing.Size(116, 19);
            this.labelContent.TabIndex = 32;
            this.labelContent.Text = "Content to hide:";
            // 
            // labelCarrierDecsription
            // 
            this.labelCarrierDecsription.AutoSize = true;
            this.labelCarrierDecsription.Location = new System.Drawing.Point(6, 22);
            this.labelCarrierDecsription.Name = "labelCarrierDecsription";
            this.labelCarrierDecsription.Size = new System.Drawing.Size(394, 19);
            this.labelCarrierDecsription.TabIndex = 31;
            this.labelCarrierDecsription.Text = "Drag and drop image or audio file to embed to or extract from.";
            // 
            // buttonImportKey
            // 
            this.buttonImportKey.Enabled = false;
            this.buttonImportKey.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonImportKey.ImageIndex = 4;
            this.buttonImportKey.ImageList = this.imageListSilkIcons;
            this.buttonImportKey.Location = new System.Drawing.Point(222, 323);
            this.buttonImportKey.Name = "buttonImportKey";
            this.buttonImportKey.Size = new System.Drawing.Size(178, 50);
            this.buttonImportKey.TabIndex = 30;
            this.buttonImportKey.Text = "Import RSA key";
            this.buttonImportKey.UseVisualStyleBackColor = true;
            this.buttonImportKey.Click += new System.EventHandler(this.buttonImportKey_Click);
            // 
            // textBoxEncryptionKey
            // 
            this.textBoxEncryptionKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxEncryptionKey.Location = new System.Drawing.Point(10, 253);
            this.textBoxEncryptionKey.Name = "textBoxEncryptionKey";
            this.textBoxEncryptionKey.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBoxEncryptionKey.Size = new System.Drawing.Size(390, 64);
            this.textBoxEncryptionKey.TabIndex = 28;
            this.textBoxEncryptionKey.Text = "";
            this.textBoxEncryptionKey.TextChanged += new System.EventHandler(this.textBoxEncryptionKey_TextChanged);
            // 
            // tabPageAdvanced
            // 
            this.tabPageAdvanced.Controls.Add(this.buttonAddPublicKey);
            this.tabPageAdvanced.Controls.Add(this.labelMessageSIgning);
            this.tabPageAdvanced.Controls.Add(this.checkBoxSignMessages);
            this.tabPageAdvanced.Controls.Add(this.buttonImportAlgorithm);
            this.tabPageAdvanced.Controls.Add(this.labelAdvancedAlgorithm);
            this.tabPageAdvanced.Controls.Add(this.propertyGridAlgorithmOptions);
            this.tabPageAdvanced.Controls.Add(this.buttonGenerate);
            this.tabPageAdvanced.Controls.Add(this.labelAlgorithmList);
            this.tabPageAdvanced.Controls.Add(this.comboBoxCryptoProviderSelection);
            this.tabPageAdvanced.Controls.Add(this.comboBoxAlgorithmSelection);
            this.tabPageAdvanced.Controls.Add(this.labelCryptoProvider);
            this.tabPageAdvanced.ImageIndex = 3;
            this.tabPageAdvanced.Location = new System.Drawing.Point(4, 29);
            this.tabPageAdvanced.Name = "tabPageAdvanced";
            this.tabPageAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAdvanced.Size = new System.Drawing.Size(788, 399);
            this.tabPageAdvanced.TabIndex = 1;
            this.tabPageAdvanced.Text = "Advanced options";
            this.tabPageAdvanced.UseVisualStyleBackColor = true;
            // 
            // buttonAddPublicKey
            // 
            this.buttonAddPublicKey.Location = new System.Drawing.Point(10, 209);
            this.buttonAddPublicKey.Name = "buttonAddPublicKey";
            this.buttonAddPublicKey.Size = new System.Drawing.Size(180, 34);
            this.buttonAddPublicKey.TabIndex = 34;
            this.buttonAddPublicKey.Text = "Add known public key";
            this.buttonAddPublicKey.UseVisualStyleBackColor = true;
            this.buttonAddPublicKey.Click += new System.EventHandler(this.buttonAddPublicKey_Click);
            // 
            // labelMessageSIgning
            // 
            this.labelMessageSIgning.AutoSize = true;
            this.labelMessageSIgning.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessageSIgning.Location = new System.Drawing.Point(5, 187);
            this.labelMessageSIgning.Name = "labelMessageSIgning";
            this.labelMessageSIgning.Size = new System.Drawing.Size(123, 19);
            this.labelMessageSIgning.TabIndex = 33;
            this.labelMessageSIgning.Text = "Message signing:";
            // 
            // checkBoxSignMessages
            // 
            this.checkBoxSignMessages.AutoSize = true;
            this.checkBoxSignMessages.Location = new System.Drawing.Point(10, 249);
            this.checkBoxSignMessages.Name = "checkBoxSignMessages";
            this.checkBoxSignMessages.Size = new System.Drawing.Size(143, 23);
            this.checkBoxSignMessages.TabIndex = 32;
            this.checkBoxSignMessages.Text = "Sign my messages";
            this.checkBoxSignMessages.UseVisualStyleBackColor = true;
            // 
            // buttonImportAlgorithm
            // 
            this.buttonImportAlgorithm.Location = new System.Drawing.Point(8, 60);
            this.buttonImportAlgorithm.Name = "buttonImportAlgorithm";
            this.buttonImportAlgorithm.Size = new System.Drawing.Size(182, 34);
            this.buttonImportAlgorithm.TabIndex = 31;
            this.buttonImportAlgorithm.Text = "Import plugin";
            this.buttonImportAlgorithm.UseVisualStyleBackColor = true;
            this.buttonImportAlgorithm.Click += new System.EventHandler(this.buttonImportAlgorithm_Click);
            // 
            // labelAdvancedAlgorithm
            // 
            this.labelAdvancedAlgorithm.AutoSize = true;
            this.labelAdvancedAlgorithm.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAdvancedAlgorithm.Location = new System.Drawing.Point(196, 7);
            this.labelAdvancedAlgorithm.Name = "labelAdvancedAlgorithm";
            this.labelAdvancedAlgorithm.Size = new System.Drawing.Size(241, 19);
            this.labelAdvancedAlgorithm.TabIndex = 30;
            this.labelAdvancedAlgorithm.Text = "Steganography algorithm settings:";
            // 
            // propertyGridAlgorithmOptions
            // 
            this.propertyGridAlgorithmOptions.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGridAlgorithmOptions.Location = new System.Drawing.Point(199, 29);
            this.propertyGridAlgorithmOptions.Name = "propertyGridAlgorithmOptions";
            this.propertyGridAlgorithmOptions.Size = new System.Drawing.Size(299, 349);
            this.propertyGridAlgorithmOptions.TabIndex = 29;
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(9, 150);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(181, 34);
            this.buttonGenerate.TabIndex = 27;
            this.buttonGenerate.Text = "Generate RSA keypair";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelSignStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 407);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(796, 25);
            this.statusStrip.TabIndex = 30;
            this.statusStrip.Text = "statusStrip1";
            // 
            // labelSignStatus
            // 
            this.labelSignStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.labelSignStatus.Name = "labelSignStatus";
            this.labelSignStatus.Size = new System.Drawing.Size(50, 20);
            this.labelSignStatus.Text = "Ready";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 432);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControlMain);
            this.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 450);
            this.Name = "FormMain";
            this.Text = "Stegosaurus";
            this.contextMenuStripMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCarrier)).EndInit();
            this.contextMenuStripPictureBox.ResumeLayout(false);
            this.panelPictureBox.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.tabPageMain.PerformLayout();
            this.tabPageAdvanced.ResumeLayout(false);
            this.tabPageAdvanced.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView listViewMessageContentFiles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMain;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ImageList imageListIcons;
        private System.Windows.Forms.Label labelTextMesage;
        private System.Windows.Forms.Button buttonActivateSteganography;
        private System.Windows.Forms.Label labelEncryptionKey;
        private System.Windows.Forms.PictureBox pictureBoxCarrier;
        private System.Windows.Forms.ProgressBar progressBarCapacity;
        private System.Windows.Forms.Panel panelPictureBox;
        private System.Windows.Forms.Label labelCarrierMedia;
        private System.Windows.Forms.Label labelAlgorithmList;
        private System.Windows.Forms.ComboBox comboBoxAlgorithmSelection;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label labelStorageRatio;
        private System.Windows.Forms.Label labelCapacityWarning;
        private System.Windows.Forms.RichTextBox textBoxTextMessage;
        private System.Windows.Forms.Label labelCryptoProvider;
        private System.Windows.Forms.ComboBox comboBoxCryptoProviderSelection;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.TabPage tabPageAdvanced;
        private System.Windows.Forms.RichTextBox textBoxEncryptionKey;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.ImageList imageListSilkIcons;
        private System.Windows.Forms.Button buttonImportKey;
        private System.Windows.Forms.Label labelCarrierDecsription;
        private System.Windows.Forms.Label labelContent;
        private System.Windows.Forms.Label labelContentDescription;
        private System.Windows.Forms.PropertyGrid propertyGridAlgorithmOptions;
        private System.Windows.Forms.Label labelAdvancedAlgorithm;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripPictureBox;
        private System.Windows.Forms.ToolStripMenuItem browseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem findUniqueSamplesToolStripMenuItem;
        private System.Windows.Forms.Button buttonImportAlgorithm;
        private System.Windows.Forms.CheckBox checkBoxSignMessages;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel labelSignStatus;
        private System.Windows.Forms.Button buttonAddPublicKey;
        private System.Windows.Forms.Label labelMessageSIgning;
        private System.Windows.Forms.ToolStripMenuItem importFromURLToolStripMenuItem;
    }
}

