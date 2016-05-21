using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StegosaurusGUI.Utility
{
    public static class MessageBoxUtility
    {
        public static void ShowError(string _message, string _title = "Error")
        {
            MessageBox.Show(_message, _title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
