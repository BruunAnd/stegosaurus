using System;
using System.IO;
using System.Windows.Forms;
using Stegosaurus.Cryptography;
using StegosaurusGUI.Forms;

namespace StegosaurusGUI
{
    static class Program
    {
        public static readonly string KnownKeysFolder = "Known Keys";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // Load public keys from known keys folder
            if (Directory.Exists(KnownKeysFolder))
            {
                foreach (FileInfo file in new DirectoryInfo(KnownKeysFolder).GetFiles("*.xml"))
                {
                    PublicKeyList.Add(Path.GetFileNameWithoutExtension(file.FullName), file.FullName);
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

    }
}
