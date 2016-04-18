using System;
using System.Windows.Forms;
using Stegosaurus.Forms;
using Stegosaurus.Carrier.AudioFormats;
using System.IO;
using System.Linq;
using Stegosaurus.Algorithm;
using Stegosaurus.Carrier;

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
            /*File.Delete("simon.png");
            Type test = typeof(LSBAlgorithm);
            IStegoAlgorithm algo = (IStegoAlgorithm) Activator.CreateInstance(test);
            algo.CarrierMedia = new ImageCarrier("blank.png");

            StegoMessage simon = new StegoMessage();
            simon.InputFiles.Add(new InputFile("in.txt"));
            simon.TextMessage = "top kek simon";
            algo.Embed(simon);

            algo.CarrierMedia.SaveToFile("simon.png");


            algo.CarrierMedia = new ImageCarrier("simon.png");
            var outs = algo.Extract();
            MessageBox.Show(outs.TextMessage);*/

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());

        }
    }
}
