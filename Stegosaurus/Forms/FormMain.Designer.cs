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
            this.MessageContentFilesListview = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
            this.inputBrowseButton = new System.Windows.Forms.Button();
            this.InputFilesLabel = new System.Windows.Forms.Label();
            this.TextMessageTextbox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.TextMesageLabel = new System.Windows.Forms.Label();
            this.EmbedButton = new System.Windows.Forms.Button();
            this.EncryptionKeyTextbox = new System.Windows.Forms.TextBox();
            this.EncryptionKeyLabel = new System.Windows.Forms.Label();
            this.InputBrowseDialog = new System.Windows.Forms.OpenFileDialog();
            this.CarrierPictureBox = new System.Windows.Forms.PictureBox();
            this.CapacityProgressbar = new System.Windows.Forms.ProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.CarrierMediaLabel = new System.Windows.Forms.Label();
            this.algorithmListLabel = new System.Windows.Forms.Label();
            this.AlgorithmSelectionCombobox = new System.Windows.Forms.ComboBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CarrierPictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MessageContentFilesListview
            // 
            this.MessageContentFilesListview.AllowDrop = true;
            this.MessageContentFilesListview.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.MessageContentFilesListview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.MessageContentFilesListview.ContextMenuStrip = this.contextMenuStrip1;
            this.MessageContentFilesListview.FullRowSelect = true;
            this.MessageContentFilesListview.GridLines = true;
            this.MessageContentFilesListview.Location = new System.Drawing.Point(465, 34);
            this.MessageContentFilesListview.MultiSelect = false;
            this.MessageContentFilesListview.Name = "MessageContentFilesListview";
            this.MessageContentFilesListview.Size = new System.Drawing.Size(347, 184);
            this.MessageContentFilesListview.SmallImageList = this.imageListIcons;
            this.MessageContentFilesListview.TabIndex = 3;
            this.MessageContentFilesListview.UseCompatibleStateImageBehavior = false;
            this.MessageContentFilesListview.View = System.Windows.Forms.View.Details;
            this.MessageContentFilesListview.DragDrop += new System.Windows.Forms.DragEventHandler(this.MessageContentFilesListView_DragDrop);
            this.MessageContentFilesListview.DragEnter += new System.Windows.Forms.DragEventHandler(this.MessageContentFilesListView_DragEnter);
            this.MessageContentFilesListview.DragLeave += new System.EventHandler(this.MessageContentFilesListView_DragLeave);
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
            // inputBrowseButton
            // 
            this.inputBrowseButton.Location = new System.Drawing.Point(737, 8);
            this.inputBrowseButton.Name = "inputBrowseButton";
            this.inputBrowseButton.Size = new System.Drawing.Size(75, 25);
            this.inputBrowseButton.TabIndex = 4;
            this.inputBrowseButton.Text = "Browse";
            this.inputBrowseButton.UseVisualStyleBackColor = true;
            this.inputBrowseButton.Click += new System.EventHandler(this.inputBrowseButton_Click);
            // 
            // InputFilesLabel
            // 
            this.InputFilesLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.InputFilesLabel.AutoSize = true;
            this.InputFilesLabel.Location = new System.Drawing.Point(461, 12);
            this.InputFilesLabel.Name = "InputFilesLabel";
            this.InputFilesLabel.Size = new System.Drawing.Size(141, 19);
            this.InputFilesLabel.TabIndex = 5;
            this.InputFilesLabel.Text = "Message content files";
            this.InputFilesLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // TextMessageTextbox
            // 
            this.TextMessageTextbox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TextMessageTextbox.Location = new System.Drawing.Point(465, 243);
            this.TextMessageTextbox.Multiline = true;
            this.TextMessageTextbox.Name = "TextMessageTextbox";
            this.TextMessageTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextMessageTextbox.Size = new System.Drawing.Size(347, 64);
            this.TextMessageTextbox.TabIndex = 6;
            this.TextMessageTextbox.TextChanged += new System.EventHandler(this.TextMessageTextbox_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(340, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TextMesageLabel
            // 
            this.TextMesageLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TextMesageLabel.AutoSize = true;
            this.TextMesageLabel.Location = new System.Drawing.Point(461, 221);
            this.TextMesageLabel.Name = "TextMesageLabel";
            this.TextMesageLabel.Size = new System.Drawing.Size(90, 19);
            this.TextMesageLabel.TabIndex = 8;
            this.TextMesageLabel.Text = "Text message";
            // 
            // EmbedButton
            // 
            this.EmbedButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.EmbedButton.Location = new System.Drawing.Point(305, 304);
            this.EmbedButton.Name = "EmbedButton";
            this.EmbedButton.Size = new System.Drawing.Size(154, 50);
            this.EmbedButton.TabIndex = 9;
            this.EmbedButton.Text = "Embed/Extract Button";
            this.EmbedButton.UseVisualStyleBackColor = true;
            this.EmbedButton.Click += new System.EventHandler(this.EmbedButton_Click);
            // 
            // EncryptionKeyTextbox
            // 
            this.EncryptionKeyTextbox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.EncryptionKeyTextbox.Location = new System.Drawing.Point(465, 332);
            this.EncryptionKeyTextbox.MaxLength = 2048;
            this.EncryptionKeyTextbox.Multiline = true;
            this.EncryptionKeyTextbox.Name = "EncryptionKeyTextbox";
            this.EncryptionKeyTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.EncryptionKeyTextbox.Size = new System.Drawing.Size(347, 22);
            this.EncryptionKeyTextbox.TabIndex = 10;
            // 
            // EncryptionKeyLabel
            // 
            this.EncryptionKeyLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.EncryptionKeyLabel.AutoSize = true;
            this.EncryptionKeyLabel.Location = new System.Drawing.Point(461, 310);
            this.EncryptionKeyLabel.Name = "EncryptionKeyLabel";
            this.EncryptionKeyLabel.Size = new System.Drawing.Size(99, 19);
            this.EncryptionKeyLabel.TabIndex = 11;
            this.EncryptionKeyLabel.Text = "Encryption key";
            // 
            // InputBrowseDialog
            // 
            this.InputBrowseDialog.Multiselect = true;
            // 
            // CarrierPictureBox
            // 
            this.CarrierPictureBox.BackColor = System.Drawing.SystemColors.Window;
            this.CarrierPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CarrierPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CarrierPictureBox.Location = new System.Drawing.Point(0, 0);
            this.CarrierPictureBox.Name = "CarrierPictureBox";
            this.CarrierPictureBox.Size = new System.Drawing.Size(284, 184);
            this.CarrierPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CarrierPictureBox.TabIndex = 12;
            this.CarrierPictureBox.TabStop = false;
            // 
            // CapacityProgressbar
            // 
            this.CapacityProgressbar.Location = new System.Drawing.Point(303, 195);
            this.CapacityProgressbar.Name = "CapacityProgressbar";
            this.CapacityProgressbar.Size = new System.Drawing.Size(154, 23);
            this.CapacityProgressbar.TabIndex = 13;
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.Controls.Add(this.CarrierPictureBox);
            this.panel1.Location = new System.Drawing.Point(13, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 184);
            this.panel1.TabIndex = 14;
            this.panel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.CarrierMediaPanel_DragDrop);
            this.panel1.DragEnter += new System.Windows.Forms.DragEventHandler(this.CarrierMediaPanel_DragEnter);
            this.panel1.DragLeave += new System.EventHandler(this.CarrierMediaPanel_DragLeave);
            // 
            // CarrierMediaLabel
            // 
            this.CarrierMediaLabel.AutoSize = true;
            this.CarrierMediaLabel.Location = new System.Drawing.Point(13, 8);
            this.CarrierMediaLabel.Name = "CarrierMediaLabel";
            this.CarrierMediaLabel.Size = new System.Drawing.Size(92, 19);
            this.CarrierMediaLabel.TabIndex = 17;
            this.CarrierMediaLabel.Text = "Carrier Media";
            // 
            // algorithmListLabel
            // 
            this.algorithmListLabel.AutoSize = true;
            this.algorithmListLabel.Location = new System.Drawing.Point(299, 141);
            this.algorithmListLabel.Name = "algorithmListLabel";
            this.algorithmListLabel.Size = new System.Drawing.Size(167, 19);
            this.algorithmListLabel.TabIndex = 18;
            this.algorithmListLabel.Text = "Steganography Algorithm";
            // 
            // AlgorithmSelectionCombobox
            // 
            this.AlgorithmSelectionCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AlgorithmSelectionCombobox.FormattingEnabled = true;
            lsbAlgorithm1.CarrierMedia = null;
            lsbAlgorithm1.Key = null;
            this.AlgorithmSelectionCombobox.Items.AddRange(new object[] {
            lsbAlgorithm1});
            this.AlgorithmSelectionCombobox.Location = new System.Drawing.Point(303, 163);
            this.AlgorithmSelectionCombobox.Name = "AlgorithmSelectionCombobox";
            this.AlgorithmSelectionCombobox.Size = new System.Drawing.Size(154, 25);
            this.AlgorithmSelectionCombobox.TabIndex = 19;
            this.AlgorithmSelectionCombobox.SelectionChangeCommitted += new System.EventHandler(this.AlgorithmSelectionCombobox_SelectionChangeCommitted);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 378);
            this.Controls.Add(this.AlgorithmSelectionCombobox);
            this.Controls.Add(this.CarrierMediaLabel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.CapacityProgressbar);
            this.Controls.Add(this.EncryptionKeyLabel);
            this.Controls.Add(this.EncryptionKeyTextbox);
            this.Controls.Add(this.EmbedButton);
            this.Controls.Add(this.TextMesageLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TextMessageTextbox);
            this.Controls.Add(this.InputFilesLabel);
            this.Controls.Add(this.inputBrowseButton);
            this.Controls.Add(this.MessageContentFilesListview);
            this.Controls.Add(this.algorithmListLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(850, 425);
            this.MinimumSize = new System.Drawing.Size(850, 425);
            this.Name = "FormMain";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CarrierPictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView MessageContentFilesListview;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ImageList imageListIcons;
        private System.Windows.Forms.Button inputBrowseButton;
        private System.Windows.Forms.Label InputFilesLabel;
        private System.Windows.Forms.TextBox TextMessageTextbox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label TextMesageLabel;
        private System.Windows.Forms.Button EmbedButton;
        private System.Windows.Forms.Label EncryptionKeyLabel;
        private System.Windows.Forms.TextBox EncryptionKeyTextbox;
        private System.Windows.Forms.OpenFileDialog InputBrowseDialog;
        private System.Windows.Forms.PictureBox CarrierPictureBox;
        private System.Windows.Forms.ProgressBar CapacityProgressbar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.Label CarrierMediaLabel;
        private System.Windows.Forms.Label algorithmListLabel;
        private System.Windows.Forms.ComboBox AlgorithmSelectionCombobox;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

