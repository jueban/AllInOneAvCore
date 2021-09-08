using AvManager.Helper;
using Models;
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
    public partial class MagnetItem : UserControl
    {
        private ShowMagnetSearchResult model;
        public SeedMagnetSearchModel selectedModel;

        public MagnetItem()
        {
            InitializeComponent();
        }

        public MagnetItem(ShowMagnetSearchResult model)
        {
            InitializeComponent();
            this.model = model;
        }

        private async void MagnetItem_Load(object sender, EventArgs e)
        {
            MagnetItemListBox.DisplayMember = "Title";
            MagnetItemListBox.ValueMember = "Tag";

            MagnetItemPicBox.SizeMode = PictureBoxSizeMode.StretchImage;

            if (File.Exists(model.AvModel.LocalPic))
            {
                MagnetItemPicBox.Image = Image.FromFile(model.AvModel.LocalPic);
            }
            else
            {
                using (HttpClient c = new HttpClient())
                {
                    try
                    {
                        using Stream s = await c.GetStreamAsync(model.AvModel.PicUrl);
                        MagnetItemPicBox.Image = Image.FromStream(s);
                    }
                    catch
                    {

                    }
                }
            }

            string info = "";

            if (model.MatchFiles != null && model.MatchFiles.Any())
            {
                switch (model.FileLocation)
                {
                    case FileLocation.Local:
                        info += "本地 " + model.BiggestSizeStr;
                        MagnetItemMainUpperRightPanel.BackColor = Color.Green;
                        MagnetItemInfoLabel.BackColor = Color.Green;
                        break;
                    case FileLocation.OneOneFive:
                        info += "115 " + model.BiggestSizeStr;
                        MagnetItemMainUpperRightPanel.BackColor = Color.Yellow;
                        MagnetItemInfoLabel.BackColor = Color.Yellow;
                        break;
                    case FileLocation.LocalAndOneOneFive:
                        info += "本地/115" + model.BiggestSizeStr;
                        MagnetItemMainUpperRightPanel.BackColor = Color.Blue;
                        MagnetItemInfoLabel.BackColor = Color.Blue;
                        MagnetItemInfoLabel.ForeColor = Color.White;
                        break;
                    default:
                        info += "不存在";
                        break;
                }
            }

            MagnetItemTitleText.Text = model.AvModel.Name;
            MagnetItemInfoLabel.Text = info;

            foreach (var res in model.Magnets)
            {
                ListBoxItem item = new ListBoxItem
                {
                    Title = $"[{res.MagSizeStr}]  {res.Title}",
                    Tag = res,
                };

                MagnetItemListBox.Items.Add(item);
            }
        }

        private void MagnetItemListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MagnetItemListBox.SelectedItem != null)
            {
                this.selectedModel = (SeedMagnetSearchModel)((((ListBoxItem)MagnetItemListBox.SelectedItem).Tag));
            }
            else
            {
                this.selectedModel = null;
            }
        }

        private void MagnetItemPicBox_Click(object sender, EventArgs e)
        {
            MagnetItemListBox.ClearSelected();
        }
    }
}
