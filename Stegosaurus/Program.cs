using System;
using System.Windows.Forms;
using Stegosaurus.Forms;
using Stegosaurus.Carrier.AudioFormats;
using System.IO;
using System.Linq;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
