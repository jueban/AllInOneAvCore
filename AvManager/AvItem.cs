using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AvManager
{
    public partial class AvItem : UserControl
    {
        public string AvId { get; set; }
        public string AvName { get; set; }
        public string ImageStr { get; set; }
        public int AvDBId { get; set; }
        public Panel Panel
        {
            get
            {
                return AvItemBottomPanel;
            }
        }

        public delegate void AvItemClickHandle(object sender, EventArgs e);
        public event AvItemClickHandle AvItemClicked;

        public AvItem()
        {
            InitializeComponent();
        }

        public AvItem(string name, string img, string id, int avDbId)
        {
            InitializeComponent();

            AvItemInfoLabel.Text = name;
            AvName = name;
            ImageStr = img;
            AvId = id;
            AvDBId = avDbId;
        }

        private void AvItemPicBox_Click(object sender, EventArgs e)
        {
            AvItemClicked?.Invoke(sender, new EventArgs());
        }

        private void AvItem_Load(object sender, EventArgs e)
        {
            ShowImage();
        }

        private async void ShowImage()
        {
            using (HttpClient c = new HttpClient())
            {
                using (Stream s = await c.GetStreamAsync(this.ImageStr))
                {
                    AvItemPicBox.Image = Image.FromStream(s);
                }
            }
        }
    }
}
