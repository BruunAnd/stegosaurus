using System;
using System.Windows.Forms;
using Stegosaurus.Forms;
using Stegosaurus.Carrier.AudioFormats;

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
            WaveFile wave = new WaveFile("daisy.wav");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
