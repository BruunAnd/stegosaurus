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
            Stegosaurus.Algorithm.LSBAlgorithm lsbAlgorithm1 = new Stegosaurus.Algorithm.LSBAlgorithm();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.listViewMessageContentFiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
            this.buttonInputBrowse = new System.Windows.Forms.Button();
            this.labelInputFiles = new System.Windows.Forms.Label();
            this.textBoxTextMessage = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.labelTextMesage = new System.Windows.Forms.Label();
            this.EmbedButton = new System.Windows.Forms.Button();
            this.textBoxEncryptionKey = new System.Windows.Forms.TextBox();
            this.labelEncryptionKey = new System.Windows.Forms.Label();
            this.InputBrowseDialog = new System.Windows.Forms.OpenFileDialog();
            this.pictureBoxCarrier = new System.Windows.Forms.PictureBox();
            this.progressBarCapacity = new System.Windows.Forms.ProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.labelCarrierMedia = new System.Windows.Forms.Label();
            this.labelAlgorithmList = new System.Windows.Forms.Label();
            this.comboBoxAlgorithmSelection = new System.Windows.Forms.ComboBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.labelStorageRatio = new System.Windows.Forms.Label();
            this.labelCapacityWarning = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCarrier)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewMessageContentFiles
            // 
            this.listViewMessageContentFiles.AllowDrop = true;
            this.listViewMessageContentFiles.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.listViewMessageContentFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewMessageContentFiles.ContextMenuStrip = this.contextMenuStrip1;
            this.listViewMessageContentFiles.FullRowSelect = true;
            this.listViewMessageContentFiles.GridLines = true;
            this.listViewMessageContentFiles.Location = new System.Drawing.Point(315, 34);
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
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(129, 56);
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
            this.buttonInputBrowse.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonInputBrowse.Location = new System.Drawing.Point(587, 8);
            this.buttonInputBrowse.Name = "buttonInputBrowse";
            this.buttonInputBrowse.Size = new System.Drawing.Size(75, 25);
            this.buttonInputBrowse.TabIndex = 4;
            this.buttonInputBrowse.Text = "Browse";
            this.buttonInputBrowse.UseVisualStyleBackColor = true;
            this.buttonInputBrowse.Click += new System.EventHandler(this.inputBrowseButton_Click);
            // 
            // labelInputFiles
            // 
            this.labelInputFiles.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelInputFiles.AutoSize = true;
            this.labelInputFiles.Location = new System.Drawing.Point(315, 12);
            this.labelInputFiles.Name = "labelInputFiles";
            this.labelInputFiles.Size = new System.Drawing.Size(141, 19);
            this.labelInputFiles.TabIndex = 5;
            this.labelInputFiles.Text = "Message content files";
            // 
            // textBoxTextMessage
            // 
            this.textBoxTextMessage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxTextMessage.Location = new System.Drawing.Point(315, 243);
            this.textBoxTextMessage.Multiline = true;
            this.textBoxTextMessage.Name = "textBoxTextMessage";
            this.textBoxTextMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxTextMessage.Size = new System.Drawing.Size(347, 64);
            this.textBoxTextMessage.TabIndex = 6;
            this.textBoxTextMessage.TextChanged += new System.EventHandler(this.textBoxTextMessage_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(194, 237);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelTextMesage
            // 
            this.labelTextMesage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTextMesage.AutoSize = true;
            this.labelTextMesage.Location = new System.Drawing.Point(315, 221);
            this.labelTextMesage.Name = "labelTextMesage";
            this.labelTextMesage.Size = new System.Drawing.Size(90, 19);
            this.labelTextMesage.TabIndex = 8;
            this.labelTextMesage.Text = "Text message";
            // 
            // EmbedButton
            // 
            this.EmbedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.EmbedButton.Location = new System.Drawing.Point(12, 316);
            this.EmbedButton.Name = "EmbedButton";
            this.EmbedButton.Size = new System.Drawing.Size(154, 50);
            this.EmbedButton.TabIndex = 9;
            this.EmbedButton.Text = "Embed/Extract Button";
            this.EmbedButton.UseVisualStyleBackColor = true;
            this.EmbedButton.Click += new System.EventHandler(this.EmbedButton_Click);
            // 
            // textBoxEncryptionKey
            // 
            this.textBoxEncryptionKey.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxEncryptionKey.Location = new System.Drawing.Point(315, 332);
            this.textBoxEncryptionKey.MaxLength = 2048;
            this.textBoxEncryptionKey.Multiline = true;
            this.textBoxEncryptionKey.Name = "textBoxEncryptionKey";
            this.textBoxEncryptionKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxEncryptionKey.Size = new System.Drawing.Size(347, 22);
            this.textBoxEncryptionKey.TabIndex = 10;
            // 
            // labelEncryptionKey
            // 
            this.labelEncryptionKey.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelEncryptionKey.AutoSize = true;
            this.labelEncryptionKey.Location = new System.Drawing.Point(315, 310);
            this.labelEncryptionKey.Name = "labelEncryptionKey";
            this.labelEncryptionKey.Size = new System.Drawing.Size(99, 19);
            this.labelEncryptionKey.TabIndex = 11;
            this.labelEncryptionKey.Text = "Encryption key";
            // 
            // InputBrowseDialog
            // 
            this.InputBrowseDialog.Multiselect = true;
            // 
            // pictureBoxCarrier
            // 
            this.pictureBoxCarrier.BackColor = System.Drawing.SystemColors.Window;
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
            this.progressBarCapacity.Location = new System.Drawing.Point(12, 287);
            this.progressBarCapacity.Maximum = 101;
            this.progressBarCapacity.Name = "progressBarCapacity";
            this.progressBarCapacity.Size = new System.Drawing.Size(154, 23);
            this.progressBarCapacity.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarCapacity.TabIndex = 13;
            this.progressBarCapacity.Value = 50;
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.Controls.Add(this.pictureBoxCarrier);
            this.panel1.Location = new System.Drawing.Point(13, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 184);
            this.panel1.TabIndex = 14;
            this.panel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelCarrierMedia_DragDrop);
            this.panel1.DragEnter += new System.Windows.Forms.DragEventHandler(this.panelCarrierMedia_DragEnter);
            // 
            // labelCarrierMedia
            // 
            this.labelCarrierMedia.AutoSize = true;
            this.labelCarrierMedia.Location = new System.Drawing.Point(13, 8);
            this.labelCarrierMedia.Name = "labelCarrierMedia";
            this.labelCarrierMedia.Size = new System.Drawing.Size(92, 19);
            this.labelCarrierMedia.TabIndex = 17;
            this.labelCarrierMedia.Text = "Carrier Media";
            // 
            // labelAlgorithmList
            // 
            this.labelAlgorithmList.AutoSize = true;
            this.labelAlgorithmList.Location = new System.Drawing.Point(8, 215);
            this.labelAlgorithmList.Name = "labelAlgorithmList";
            this.labelAlgorithmList.Size = new System.Drawing.Size(167, 19);
            this.labelAlgorithmList.TabIndex = 18;
            this.labelAlgorithmList.Text = "Steganography Algorithm";
            // 
            // comboBoxAlgorithmSelection
            // 
            this.comboBoxAlgorithmSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAlgorithmSelection.FormattingEnabled = true;
            lsbAlgorithm1.CarrierMedia = null;
            lsbAlgorithm1.Key = null;
            this.comboBoxAlgorithmSelection.Items.AddRange(new object[] {
            lsbAlgorithm1});
            this.comboBoxAlgorithmSelection.Location = new System.Drawing.Point(12, 237);
            this.comboBoxAlgorithmSelection.Name = "comboBoxAlgorithmSelection";
            this.comboBoxAlgorithmSelection.Size = new System.Drawing.Size(154, 25);
            this.comboBoxAlgorithmSelection.TabIndex = 19;
            this.comboBoxAlgorithmSelection.SelectionChangeCommitted += new System.EventHandler(this.AlgorithmSelectionCombobox_SelectionChangeCommitted);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.InitialDirectory = "Application.StartupPath";
            this.saveFileDialog.Title = "Save File(s)";
            // 
            // labelStorageRatio
            // 
            this.labelStorageRatio.AutoSize = true;
            this.labelStorageRatio.Location = new System.Drawing.Point(8, 265);
            this.labelStorageRatio.Name = "labelStorageRatio";
            this.labelStorageRatio.Size = new System.Drawing.Size(91, 19);
            this.labelStorageRatio.TabIndex = 20;
            this.labelStorageRatio.Text = "Storage Ratio";
            // 
            // labelCapacityWarning
            // 
            this.labelCapacityWarning.BackColor = System.Drawing.Color.Transparent;
            this.labelCapacityWarning.Location = new System.Drawing.Point(97, 265);
            this.labelCapacityWarning.Name = "labelCapacityWarning";
            this.labelCapacityWarning.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelCapacityWarning.Size = new System.Drawing.Size(69, 19);
            this.labelCapacityWarning.TabIndex = 23;
            this.labelCapacityWarning.Text = "##%";
            this.labelCapacityWarning.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 378);
            this.Controls.Add(this.labelCapacityWarning);
            this.Controls.Add(this.labelStorageRatio);
            this.Controls.Add(this.comboBoxAlgorithmSelection);
            this.Controls.Add(this.labelCarrierMedia);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.progressBarCapacity);
            this.Controls.Add(this.labelEncryptionKey);
            this.Controls.Add(this.textBoxEncryptionKey);
            this.Controls.Add(this.EmbedButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxTextMessage);
            this.Controls.Add(this.labelInputFiles);
            this.Controls.Add(this.buttonInputBrowse);
            this.Controls.Add(this.listViewMessageContentFiles);
            this.Controls.Add(this.labelAlgorithmList);
            this.Controls.Add(this.labelTextMesage);
            this.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(700, 425);
            this.MinimumSize = new System.Drawing.Size(700, 425);
            this.Name = "FormMain";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCarrier)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView listViewMessageContentFiles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ImageList imageListIcons;
        private System.Windows.Forms.Button buttonInputBrowse;
        private System.Windows.Forms.Label labelInputFiles;
        private System.Windows.Forms.TextBox textBoxTextMessage;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelTextMesage;
        private System.Windows.Forms.Button EmbedButton;
        private System.Windows.Forms.Label labelEncryptionKey;
        private System.Windows.Forms.TextBox textBoxEncryptionKey;
        private System.Windows.Forms.OpenFileDialog InputBrowseDialog;
        private System.Windows.Forms.PictureBox pictureBoxCarrier;
        private System.Windows.Forms.ProgressBar progressBarCapacity;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.Label labelCarrierMedia;
        private System.Windows.Forms.Label labelAlgorithmList;
        private System.Windows.Forms.ComboBox comboBoxAlgorithmSelection;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label labelStorageRatio;
        private System.Windows.Forms.Label labelCapacityWarning;
    }
}

