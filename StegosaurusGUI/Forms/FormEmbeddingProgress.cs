using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Media;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
                catch (StegoCryptoException ex)
                {
                    MessageBoxUtility.ShowError(ex.Message, "Crypto error");
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
                buttonUpload.Enabled = true;
                if (carrierMedia is ImageCarrier)
                {
                    buttonUpload.Enabled = true;
                }
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

            string question = "Are you sure you want to " + (embeddingComplete ? "close without saving?" : "cancel the embedding process?");
            if (!fileSaved && MessageBox.Show(question, "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }

            cts.Cancel();
        }

        private async void buttonUpload_Click(object sender, EventArgs e)
        {
            buttonUpload.Enabled = false;
            UseWaitCursor = true;

            try
            {
                // Create new post request.
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://api.imgur.com/3/image");
                request.Headers.Add("Authorization", "Client-ID 5fff7f7ba29c837");
                request.Method = "POST";

                // Convert Image to Base64 string.
                string postData;
                using (MemoryStream tempStream = new MemoryStream())
                {
                    carrierMedia.Encode();
                    ImageCarrier imageCarrier = carrierMedia as ImageCarrier;
                    imageCarrier?.ImageData.Save(tempStream, ImageFormat.Png);
                    postData = Convert.ToBase64String(tempStream.ToArray());
                }
                byte[] encodedData = new ASCIIEncoding().GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = encodedData.Length;

                // Write data to request stream.
                Stream writer = request.GetRequestStream();
                writer.Write(encodedData, 0, encodedData.Length);

                // Get response from request.
                HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync();
                string responseString;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    responseString = reader.ReadToEnd();
                }

                // Extract link from result.
                Match regexMatch = new Regex("\"link\":\"(.*?)\"").Match(responseString);
                if (regexMatch.Success)
                {
                    string uploadedUrl = regexMatch.Groups[1].Value.Replace("\\", "");
                    if (uploadedUrl.EndsWith(".png"))
                    {
                        string extractedLink = regexMatch.Groups[1].Value.Replace("\\", "");
                        if (MessageBox.Show("Image has been uploaded. Do you want to copy the link to your clipboard?", "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Clipboard.SetText(extractedLink);
                        }
                    }
                    else
                    {
                        MessageBoxUtility.ShowError("During the upload process, the image was compressed.\n\nPlease use a smaller image.");
                    }
                }
                else
                {
                    MessageBoxUtility.ShowError("Could not upload data.");
                }
            }
            catch (WebException)
            {
                MessageBoxUtility.ShowError("A web exception occurred. Ensure that you are connected to the internet.");
            }
            catch (Exception ex)
            {
                MessageBoxUtility.ShowError($"An unhandled exception occurred:\n{ex.Message}");
            }
            finally
            {
                buttonUpload.Enabled = true;
                UseWaitCursor = false;
            }
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
