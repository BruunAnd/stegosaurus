using Stegosaurus.Algorithm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stegosaurus.Forms
{
    public partial class FormEmbeddingProgress : Form
    {
        private StegoMessage message;
        private StegoAlgorithmBase algorithm;

        private CancellationTokenSource cts = new CancellationTokenSource();

        public FormEmbeddingProgress(StegoMessage _message, StegoAlgorithmBase _algorithm)
        {
            InitializeComponent();
            TopMost = true;

            message = _message;
            algorithm = _algorithm;
        }

        public async void Run()
        {
            Progress<int> progress = new Progress<int>(p =>
            {
                if (p <= progressBarMain.Maximum)
                {
                    progressBarMain.Value = p;
                    Text = p + "%";
                }
            });

            // Await execution
            bool result = await Task.Run(() =>
            {
                try
                {
                    algorithm.Embed(message, progress, cts.Token);
                }
                catch (OperationCanceledException)
                {
                    // Form was closed
                    return false;
                }
                return true;
            });

            // Show result
            if (result)
            {
                MessageBox.Show("Message was successfully embedded.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Close();
        }

        private void FormEmbeddingProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            cts.Cancel();
        }
    }
}
