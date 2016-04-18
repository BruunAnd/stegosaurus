using System;
using System.Windows.Forms;
using Stegosaurus.Forms;
using Stegosaurus.Carrier.AudioFormats;
using System.IO;
using System.Linq;
using Stegosaurus.Algorithm;
using Stegosaurus.Carrier;
using System.Text;

namespace Stegosaurus
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Type test = typeof(LSBAlgorithm);
            IStegoAlgorithm algo = (IStegoAlgorithm) Activator.CreateInstance(test);
            algo.CarrierMedia = new ImageCarrier("input.png");
            algo.Key = Encoding.UTF8.GetBytes("key as string");

            StegoMessage simon = new StegoMessage();
            simon.InputFiles.Add(new InputFile("input_file.txt"));
            simon.TextMessage = "Example test message.";
            algo.Embed(simon);

            algo.CarrierMedia.SaveToFile("output.png");


            algo.CarrierMedia = new ImageCarrier("output.png");

            StegoMessage outMessage = algo.Extract();
            MessageBox.Show(outMessage.TextMessage);
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());

        }
    }
}
