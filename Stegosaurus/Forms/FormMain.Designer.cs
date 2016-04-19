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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
            this.inputBrowseButton = new System.Windows.Forms.Button();
            this.InputFilesLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.TextMesageLabel = new System.Windows.Forms.Label();
            this.EmbedButton = new System.Windows.Forms.Button();
            this.EncryptionKeyTextbox = new System.Windows.Forms.TextBox();
            this.EncryptionKeyLabel = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.AllowDrop = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(457, 31);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(347, 184);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
            this.listView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView1_DragEnter);
            this.listView1.DragLeave += new System.EventHandler(this.listView1_DragLeave);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 212;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Size";
            this.columnHeader2.Width = 107;
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
            this.imageListIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // inputBrowseButton
            // 
            this.inputBrowseButton.Location = new System.Drawing.Point(729, 31);
            this.inputBrowseButton.Name = "inputBrowseButton";
            this.inputBrowseButton.Size = new System.Drawing.Size(75, 25);
            this.inputBrowseButton.TabIndex = 4;
            this.inputBrowseButton.Text = "Browse";
            this.inputBrowseButton.UseVisualStyleBackColor = true;
            // 
            // InputFilesLabel
            // 
            this.InputFilesLabel.AutoSize = true;
            this.InputFilesLabel.Location = new System.Drawing.Point(453, 9);
            this.InputFilesLabel.Name = "InputFilesLabel";
            this.InputFilesLabel.Size = new System.Drawing.Size(141, 19);
            this.InputFilesLabel.TabIndex = 5;
            this.InputFilesLabel.Text = "Message content files";
            this.InputFilesLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(457, 240);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(347, 64);
            this.textBox1.TabIndex = 6;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(280, 49);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TextMesageLabel
            // 
            this.TextMesageLabel.AutoSize = true;
            this.TextMesageLabel.Location = new System.Drawing.Point(453, 218);
            this.TextMesageLabel.Name = "TextMesageLabel";
            this.TextMesageLabel.Size = new System.Drawing.Size(90, 19);
            this.TextMesageLabel.TabIndex = 8;
            this.TextMesageLabel.Text = "Text message";
            // 
            // EmbedButton
            // 
            this.EmbedButton.Location = new System.Drawing.Point(297, 301);
            this.EmbedButton.Name = "EmbedButton";
            this.EmbedButton.Size = new System.Drawing.Size(154, 50);
            this.EmbedButton.TabIndex = 9;
            this.EmbedButton.Text = "Embed/Extract Button";
            this.EmbedButton.UseVisualStyleBackColor = true;
            this.EmbedButton.Click += new System.EventHandler(this.EmbedButton_Click);
            // 
            // EncryptionKeyTextbox
            // 
            this.EncryptionKeyTextbox.Location = new System.Drawing.Point(457, 329);
            this.EncryptionKeyTextbox.MaxLength = 2048;
            this.EncryptionKeyTextbox.Multiline = true;
            this.EncryptionKeyTextbox.Name = "EncryptionKeyTextbox";
            this.EncryptionKeyTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.EncryptionKeyTextbox.Size = new System.Drawing.Size(347, 22);
            this.EncryptionKeyTextbox.TabIndex = 10;
            // 
            // EncryptionKeyLabel
            // 
            this.EncryptionKeyLabel.AutoSize = true;
            this.EncryptionKeyLabel.Location = new System.Drawing.Point(453, 307);
            this.EncryptionKeyLabel.Name = "EncryptionKeyLabel";
            this.EncryptionKeyLabel.Size = new System.Drawing.Size(99, 19);
            this.EncryptionKeyLabel.TabIndex = 11;
            this.EncryptionKeyLabel.Text = "Encryption key";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 372);
            this.Controls.Add(this.EncryptionKeyLabel);
            this.Controls.Add(this.EncryptionKeyTextbox);
            this.Controls.Add(this.EmbedButton);
            this.Controls.Add(this.TextMesageLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.InputFilesLabel);
            this.Controls.Add(this.inputBrowseButton);
            this.Controls.Add(this.listView1);
            this.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "Stegosaurus";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ImageList imageListIcons;
        private System.Windows.Forms.Button inputBrowseButton;
        private System.Windows.Forms.Label InputFilesLabel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label TextMesageLabel;
        private System.Windows.Forms.Button EmbedButton;
        private System.Windows.Forms.Label EncryptionKeyLabel;
        private System.Windows.Forms.TextBox EncryptionKeyTextbox;
    }
}

