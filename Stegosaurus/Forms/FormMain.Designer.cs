using Stegosaurus.Algorithm;

namespace Stegosaurus.Forms
{
    partial class FormMain
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
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
            this.buttonInputBrowse = new System.Windows.Forms.Button();
            this.labelInputFiles = new System.Windows.Forms.Label();
            this.labelTextMesage = new System.Windows.Forms.Label();
            this.buttonActivateSteganography = new System.Windows.Forms.Button();
            this.labelEncryptionKey = new System.Windows.Forms.Label();
            this.pictureBoxCarrier = new System.Windows.Forms.PictureBox();
            this.progressBarCapacity = new System.Windows.Forms.ProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelCarrierMedia = new System.Windows.Forms.Label();
            this.labelAlgorithmList = new System.Windows.Forms.Label();
            this.comboBoxAlgorithmSelection = new System.Windows.Forms.ComboBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.labelStorageRatio = new System.Windows.Forms.Label();
            this.labelCapacityWarning = new System.Windows.Forms.Label();
            this.textBoxTextMessage = new System.Windows.Forms.RichTextBox();
            this.labelCryptoProvider = new System.Windows.Forms.Label();
            this.comboBoxCryptoProviderSelection = new System.Windows.Forms.ComboBox();
            this.buttonCarrierMediaBrowse = new System.Windows.Forms.Button();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.textBoxEncryptionKey = new System.Windows.Forms.RichTextBox();
            this.tabPageAdvanced = new System.Windows.Forms.TabPage();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.imageListSilkIcons = new System.Windows.Forms.ImageList(this.components);
            this.buttonImportKey = new System.Windows.Forms.Button();
            this.contextMenuStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCarrier)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.tabPageAdvanced.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewMessageContentFiles
            // 
            this.listViewMessageContentFiles.AllowDrop = true;
            this.listViewMessageContentFiles.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.listViewMessageContentFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewMessageContentFiles.ContextMenuStrip = this.contextMenuStripMain;
            this.listViewMessageContentFiles.FullRowSelect = true;
            this.listViewMessageContentFiles.GridLines = true;
            this.listViewMessageContentFiles.Location = new System.Drawing.Point(310, 29);
            this.listViewMessageContentFiles.Name = "listViewMessageContentFiles";
            this.listViewMessageContentFiles.Size = new System.Drawing.Size(347, 184);
            this.listViewMessageContentFiles.SmallImageList = this.imageListIcons;
            this.listViewMessageContentFiles.TabIndex = 3;
            this.listViewMessageContentFiles.UseCompatibleStateImageBehavior = false;
            this.listViewMessageContentFiles.View = System.Windows.Forms.View.Details;
            this.listViewMessageContentFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewMessageContentFiles_DragDrop);
            this.listViewMessageContentFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewMessageContentFiles_DragEnter);
            this.listViewMessageContentFiles.DragLeave += new System.EventHandler(this.listViewMessageContentFiles_DragLeave);
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
            this.saveToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStripMain.Name = "contextMenuStrip1";
            this.contextMenuStripMain.Size = new System.Drawing.Size(129, 56);
            this.contextMenuStripMain.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripMain_Opening);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // imageListIcons
            // 
            this.imageListIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // buttonInputBrowse
            // 
            this.buttonInputBrowse.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonInputBrowse.Location = new System.Drawing.Point(582, 3);
            this.buttonInputBrowse.Name = "buttonInputBrowse";
            this.buttonInputBrowse.Size = new System.Drawing.Size(75, 25);
            this.buttonInputBrowse.TabIndex = 4;
            this.buttonInputBrowse.Text = "Browse";
            this.buttonInputBrowse.UseVisualStyleBackColor = true;
            this.buttonInputBrowse.Click += new System.EventHandler(this.buttonInputBrowse_Click);
            // 
            // labelInputFiles
            // 
            this.labelInputFiles.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelInputFiles.AutoSize = true;
            this.labelInputFiles.Location = new System.Drawing.Point(306, 7);
            this.labelInputFiles.Name = "labelInputFiles";
            this.labelInputFiles.Size = new System.Drawing.Size(141, 19);
            this.labelInputFiles.TabIndex = 5;
            this.labelInputFiles.Text = "Message content files";
            // 
            // labelTextMesage
            // 
            this.labelTextMesage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelTextMesage.AutoSize = true;
            this.labelTextMesage.Location = new System.Drawing.Point(306, 215);
            this.labelTextMesage.Name = "labelTextMesage";
            this.labelTextMesage.Size = new System.Drawing.Size(90, 19);
            this.labelTextMesage.TabIndex = 8;
            this.labelTextMesage.Text = "Text message";
            // 
            // buttonActivateSteganography
            // 
            this.buttonActivateSteganography.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonActivateSteganography.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonActivateSteganography.ImageIndex = 1;
            this.buttonActivateSteganography.ImageList = this.imageListSilkIcons;
            this.buttonActivateSteganography.Location = new System.Drawing.Point(7, 278);
            this.buttonActivateSteganography.Name = "buttonActivateSteganography";
            this.buttonActivateSteganography.Size = new System.Drawing.Size(285, 40);
            this.buttonActivateSteganography.TabIndex = 9;
            this.buttonActivateSteganography.Text = "Extract";
            this.buttonActivateSteganography.UseVisualStyleBackColor = true;
            this.buttonActivateSteganography.Click += new System.EventHandler(this.buttonActivateSteganography_Click);
            // 
            // labelEncryptionKey
            // 
            this.labelEncryptionKey.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelEncryptionKey.AutoSize = true;
            this.labelEncryptionKey.Location = new System.Drawing.Point(306, 299);
            this.labelEncryptionKey.Name = "labelEncryptionKey";
            this.labelEncryptionKey.Size = new System.Drawing.Size(31, 19);
            this.labelEncryptionKey.TabIndex = 11;
            this.labelEncryptionKey.Text = "Key";
            // 
            // pictureBoxCarrier
            // 
            this.pictureBoxCarrier.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pictureBoxCarrier.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxCarrier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxCarrier.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxCarrier.Name = "pictureBoxCarrier";
            this.pictureBoxCarrier.Size = new System.Drawing.Size(284, 184);
            this.pictureBoxCarrier.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCarrier.TabIndex = 12;
            this.pictureBoxCarrier.TabStop = false;
            // 
            // progressBarCapacity
            // 
            this.progressBarCapacity.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.progressBarCapacity.Location = new System.Drawing.Point(7, 249);
            this.progressBarCapacity.Maximum = 101;
            this.progressBarCapacity.Name = "progressBarCapacity";
            this.progressBarCapacity.Size = new System.Drawing.Size(285, 23);
            this.progressBarCapacity.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarCapacity.TabIndex = 13;
            this.progressBarCapacity.Value = 50;
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.Controls.Add(this.pictureBoxCarrier);
            this.panel1.Location = new System.Drawing.Point(8, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 184);
            this.panel1.TabIndex = 14;
            this.panel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelCarrierMedia_DragDrop);
            this.panel1.DragEnter += new System.Windows.Forms.DragEventHandler(this.panelCarrierMedia_DragEnter);
            // 
            // labelCarrierMedia
            // 
            this.labelCarrierMedia.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelCarrierMedia.AutoSize = true;
            this.labelCarrierMedia.Location = new System.Drawing.Point(6, 3);
            this.labelCarrierMedia.Name = "labelCarrierMedia";
            this.labelCarrierMedia.Size = new System.Drawing.Size(92, 19);
            this.labelCarrierMedia.TabIndex = 17;
            this.labelCarrierMedia.Text = "Carrier Media";
            // 
            // labelAlgorithmList
            // 
            this.labelAlgorithmList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelAlgorithmList.AutoSize = true;
            this.labelAlgorithmList.Location = new System.Drawing.Point(8, 3);
            this.labelAlgorithmList.Name = "labelAlgorithmList";
            this.labelAlgorithmList.Size = new System.Drawing.Size(168, 19);
            this.labelAlgorithmList.TabIndex = 18;
            this.labelAlgorithmList.Text = "Steganography algorithm:";
            // 
            // comboBoxAlgorithmSelection
            // 
            this.comboBoxAlgorithmSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxAlgorithmSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAlgorithmSelection.FormattingEnabled = true;
            this.comboBoxAlgorithmSelection.Location = new System.Drawing.Point(12, 25);
            this.comboBoxAlgorithmSelection.Name = "comboBoxAlgorithmSelection";
            this.comboBoxAlgorithmSelection.Size = new System.Drawing.Size(158, 25);
            this.comboBoxAlgorithmSelection.TabIndex = 19;
            this.comboBoxAlgorithmSelection.SelectedIndexChanged += new System.EventHandler(this.comboBoxAlgorithmSelection_SelectedIndexChanged);
            // 
            // labelStorageRatio
            // 
            this.labelStorageRatio.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelStorageRatio.AutoSize = true;
            this.labelStorageRatio.Location = new System.Drawing.Point(4, 227);
            this.labelStorageRatio.Name = "labelStorageRatio";
            this.labelStorageRatio.Size = new System.Drawing.Size(91, 19);
            this.labelStorageRatio.TabIndex = 20;
            this.labelStorageRatio.Text = "Storage Ratio";
            // 
            // labelCapacityWarning
            // 
            this.labelCapacityWarning.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelCapacityWarning.BackColor = System.Drawing.Color.Transparent;
            this.labelCapacityWarning.Location = new System.Drawing.Point(117, 227);
            this.labelCapacityWarning.Name = "labelCapacityWarning";
            this.labelCapacityWarning.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelCapacityWarning.Size = new System.Drawing.Size(175, 19);
            this.labelCapacityWarning.TabIndex = 23;
            this.labelCapacityWarning.Text = "N/A";
            this.labelCapacityWarning.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxTextMessage
            // 
            this.textBoxTextMessage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxTextMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTextMessage.Location = new System.Drawing.Point(310, 237);
            this.textBoxTextMessage.Name = "textBoxTextMessage";
            this.textBoxTextMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBoxTextMessage.Size = new System.Drawing.Size(347, 65);
            this.textBoxTextMessage.TabIndex = 24;
            this.textBoxTextMessage.Text = "";
            this.textBoxTextMessage.TextChanged += new System.EventHandler(this.textBoxTextMessage_TextChanged);
            // 
            // labelCryptoProvider
            // 
            this.labelCryptoProvider.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCryptoProvider.AutoSize = true;
            this.labelCryptoProvider.Location = new System.Drawing.Point(8, 53);
            this.labelCryptoProvider.Name = "labelCryptoProvider";
            this.labelCryptoProvider.Size = new System.Drawing.Size(160, 19);
            this.labelCryptoProvider.TabIndex = 25;
            this.labelCryptoProvider.Text = "Cryptography algorithm:";
            // 
            // comboBoxCryptoProviderSelection
            // 
            this.comboBoxCryptoProviderSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxCryptoProviderSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCryptoProviderSelection.FormattingEnabled = true;
            this.comboBoxCryptoProviderSelection.Location = new System.Drawing.Point(12, 75);
            this.comboBoxCryptoProviderSelection.Name = "comboBoxCryptoProviderSelection";
            this.comboBoxCryptoProviderSelection.Size = new System.Drawing.Size(158, 25);
            this.comboBoxCryptoProviderSelection.TabIndex = 26;
            this.comboBoxCryptoProviderSelection.SelectedIndexChanged += new System.EventHandler(this.comboBoxCryptoProviderSelection_SelectedIndexChanged);
            // 
            // buttonCarrierMediaBrowse
            // 
            this.buttonCarrierMediaBrowse.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonCarrierMediaBrowse.Location = new System.Drawing.Point(217, 3);
            this.buttonCarrierMediaBrowse.Name = "buttonCarrierMediaBrowse";
            this.buttonCarrierMediaBrowse.Size = new System.Drawing.Size(75, 25);
            this.buttonCarrierMediaBrowse.TabIndex = 27;
            this.buttonCarrierMediaBrowse.Text = "Browse";
            this.buttonCarrierMediaBrowse.UseVisualStyleBackColor = true;
            this.buttonCarrierMediaBrowse.Click += new System.EventHandler(this.buttonCarrierMediaBrowse_Click);
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
            this.tabControlMain.Size = new System.Drawing.Size(684, 456);
            this.tabControlMain.TabIndex = 29;
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.buttonImportKey);
            this.tabPageMain.Controls.Add(this.textBoxEncryptionKey);
            this.tabPageMain.Controls.Add(this.panel1);
            this.tabPageMain.Controls.Add(this.buttonCarrierMediaBrowse);
            this.tabPageMain.Controls.Add(this.labelTextMesage);
            this.tabPageMain.Controls.Add(this.listViewMessageContentFiles);
            this.tabPageMain.Controls.Add(this.textBoxTextMessage);
            this.tabPageMain.Controls.Add(this.buttonInputBrowse);
            this.tabPageMain.Controls.Add(this.labelCapacityWarning);
            this.tabPageMain.Controls.Add(this.labelInputFiles);
            this.tabPageMain.Controls.Add(this.labelStorageRatio);
            this.tabPageMain.Controls.Add(this.buttonActivateSteganography);
            this.tabPageMain.Controls.Add(this.labelCarrierMedia);
            this.tabPageMain.Controls.Add(this.labelEncryptionKey);
            this.tabPageMain.Controls.Add(this.progressBarCapacity);
            this.tabPageMain.ImageIndex = 2;
            this.tabPageMain.Location = new System.Drawing.Point(4, 29);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMain.Size = new System.Drawing.Size(676, 423);
            this.tabPageMain.TabIndex = 0;
            this.tabPageMain.Text = "Main";
            this.tabPageMain.UseVisualStyleBackColor = true;
            // 
            // textBoxEncryptionKey
            // 
            this.textBoxEncryptionKey.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxEncryptionKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxEncryptionKey.Location = new System.Drawing.Point(310, 321);
            this.textBoxEncryptionKey.Name = "textBoxEncryptionKey";
            this.textBoxEncryptionKey.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBoxEncryptionKey.Size = new System.Drawing.Size(347, 64);
            this.textBoxEncryptionKey.TabIndex = 28;
            this.textBoxEncryptionKey.Text = "";
            // 
            // tabPageAdvanced
            // 
            this.tabPageAdvanced.Controls.Add(this.buttonGenerate);
            this.tabPageAdvanced.Controls.Add(this.labelAlgorithmList);
            this.tabPageAdvanced.Controls.Add(this.comboBoxCryptoProviderSelection);
            this.tabPageAdvanced.Controls.Add(this.comboBoxAlgorithmSelection);
            this.tabPageAdvanced.Controls.Add(this.labelCryptoProvider);
            this.tabPageAdvanced.ImageIndex = 3;
            this.tabPageAdvanced.Location = new System.Drawing.Point(4, 29);
            this.tabPageAdvanced.Name = "tabPageAdvanced";
            this.tabPageAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAdvanced.Size = new System.Drawing.Size(676, 423);
            this.tabPageAdvanced.TabIndex = 1;
            this.tabPageAdvanced.Text = "Advanced options";
            this.tabPageAdvanced.UseVisualStyleBackColor = true;
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(45, 152);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(524, 31);
            this.buttonGenerate.TabIndex = 27;
            this.buttonGenerate.Text = "Generate RSA keys (todo: place this button somewhere else)";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // imageListSilkIcons
            // 
            this.imageListSilkIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSilkIcons.ImageStream")));
            this.imageListSilkIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSilkIcons.Images.SetKeyName(0, "lock.png");
            this.imageListSilkIcons.Images.SetKeyName(1, "lock_open.png");
            this.imageListSilkIcons.Images.SetKeyName(2, "application_xp.png");
            this.imageListSilkIcons.Images.SetKeyName(3, "cog.png");
            // 
            // buttonImportKey
            // 
            this.buttonImportKey.Location = new System.Drawing.Point(488, 391);
            this.buttonImportKey.Name = "buttonImportKey";
            this.buttonImportKey.Size = new System.Drawing.Size(169, 28);
            this.buttonImportKey.TabIndex = 30;
            this.buttonImportKey.Text = "Import (todo: move around)";
            this.buttonImportKey.UseVisualStyleBackColor = true;
            this.buttonImportKey.Click += new System.EventHandler(this.buttonImportKey_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 456);
            this.Controls.Add(this.tabControlMain);
            this.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 450);
            this.Name = "FormMain";
            this.Text = "Stegosaurus BETA";
            this.contextMenuStripMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCarrier)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.tabPageMain.PerformLayout();
            this.tabPageAdvanced.ResumeLayout(false);
            this.tabPageAdvanced.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView listViewMessageContentFiles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMain;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ImageList imageListIcons;
        private System.Windows.Forms.Button buttonInputBrowse;
        private System.Windows.Forms.Label labelInputFiles;
        private System.Windows.Forms.Label labelTextMesage;
        private System.Windows.Forms.Button buttonActivateSteganography;
        private System.Windows.Forms.Label labelEncryptionKey;
        private System.Windows.Forms.PictureBox pictureBoxCarrier;
        private System.Windows.Forms.ProgressBar progressBarCapacity;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelCarrierMedia;
        private System.Windows.Forms.Label labelAlgorithmList;
        private System.Windows.Forms.ComboBox comboBoxAlgorithmSelection;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label labelStorageRatio;
        private System.Windows.Forms.Label labelCapacityWarning;
        private System.Windows.Forms.RichTextBox textBoxTextMessage;
        private System.Windows.Forms.Label labelCryptoProvider;
        private System.Windows.Forms.ComboBox comboBoxCryptoProviderSelection;
        private System.Windows.Forms.Button buttonCarrierMediaBrowse;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.TabPage tabPageAdvanced;
        private System.Windows.Forms.RichTextBox textBoxEncryptionKey;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.ImageList imageListSilkIcons;
        private System.Windows.Forms.Button buttonImportKey;
    }
}

