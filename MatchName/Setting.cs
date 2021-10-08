using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MatchName
{
    public partial class Setting : Form
    {
        private string prefix = "";
        private string skipSize = "";

        public Setting()
        {
            InitializeComponent();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            skipSize = ConfigurationManager.AppSettings["skipSize"];
            prefix = ConfigurationManager.AppSettings["prefix"];

            Number.Value = decimal.Parse(skipSize);
            PrefixText.Text = prefix;
        }

        private void SavePrefix_Click(object sender, EventArgs e)
        {
            if (PrefixText.Text != prefix)
            {
                SaveConfig(PrefixText.Text, "prefix");
            }

            SaveConfig(Number.Value + "", "skipSize");

            Close();
        }

        private void SaveConfig(string value, string strKey)
        {
            XmlDocument doc = new(); 
            string strFileName = AppDomain.CurrentDomain.BaseDirectory + "MatchName.dll.config";
            doc.Load(strFileName);

            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlAttribute att = nodes[i].Attributes["key"];
                if (att.Value == strKey)
                {
                    att = nodes[i].Attributes["value"];
                    att.Value = value;
                    break;
                }
            }

            doc.Save(strFileName);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
