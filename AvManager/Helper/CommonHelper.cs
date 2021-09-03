using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AvManager.Helper
{
    public class CommonHelper
    {
        public static void ChooseFolder(FolderBrowserDialog dialog, TextBox textBox)
        {
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;

            var res = dialog.ShowDialog();

            if (res == DialogResult.Yes || res == DialogResult.OK)
            {
                textBox.Text = dialog.SelectedPath;
            }
        }

        public static bool CheckFolderChoose(TextBox textBox)
        {
            return !string.IsNullOrEmpty(textBox.Text);
        }
    }
}
