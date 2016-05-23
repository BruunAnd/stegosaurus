using System;
using System.Diagnostics;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stegosaurus;
using Stegosaurus.Algorithm;
using Stegosaurus.Carrier;
using Stegosaurus.Exceptions;
using StegosaurusGUI.Utility;

namespace StegosaurusGUI.Forms
{
    public partial class FormEmbeddingProgress
    {
        
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        private bool embeddingComplete, fileSaved, errorOccurred;

        private string name, extension;
        private ICarrierMedia carrierMedia;

        public FormEmbeddingProgress()
        {
            InitializeComponent();
        }

        public async Task Run(StegoMessage _message, StegoAlgorithmBase _algorithm, string _name, string _extension)
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
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool result = await Task.Run(() =>
            {
                try
                {
                    _algorithm.Embed(_message, progress, cts.Token);
                    name = _name;
                    extension = _extension;
                    carrierMedia = _algorithm.CarrierMedia;
                    return true;
                }
                catch (OperationCanceledException)
                {
                    // Form was closed
                    return false;
                }
                catch (StegoAlgorithmException ex)
                {
                    MessageBoxUtility.ShowError(ex.Message, "Algorithm error");
                    errorOccurred = true;
                    return false;
                }
                finally
                {
                    // Should not be necessary, just an added stability measure.
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            });
            sw.Stop();

            if (result)
            {
                SystemSounds.Hand.Play();
                labelStatus.Text = $"Embedding complete! ({sw.Elapsed.TotalSeconds} seconds)";
                embeddingComplete = true;
                buttonCancel.Enabled = false;
                buttonSaveAs.Enabled = true;
            }
            else
            {
                Close();
            }
        }

        private void FormEmbeddingProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (errorOccurred)
            {
                return;
            }

            string question = "Are you sure you want to " + ( embeddingComplete ? "close without saving?" : "cancel the embedding process?" );
            if (!fileSaved && MessageBox.Show(question, "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) !=DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }

            cts.Cancel();
        }

        private void FormEmbeddingProgress_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = $"stego-{name}",
                Filter = $"Default extension (*{extension})|*{extension}|All files (*.*)|*.*"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            carrierMedia.SaveToFile(sfd.FileName);
            fileSaved = true;
            Close();
        }
    }
}
