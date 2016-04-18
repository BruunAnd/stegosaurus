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
            /*Type test = typeof(LSBAlgorithm);
            IStegoAlgorithm algo = (IStegoAlgorithm) Activator.CreateInstance(test);
            algo.CarrierMedia = new AudioCarrier("geek.wav");
            algo.Key = Encoding.UTF8.GetBytes("pleb theis");

            StegoMessage simon = new StegoMessage();
            simon.InputFiles.Add(new InputFile("in.txt"));
            simon.TextMessage = "top kek simon";
            algo.Embed(simon);

            algo.CarrierMedia.SaveToFile("kek.wav");


            algo.CarrierMedia = new AudioCarrier("kek.wav");
            var outs = algo.Extract();
            MessageBox.Show(outs.TextMessage);
            */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());

        }
    }
}
