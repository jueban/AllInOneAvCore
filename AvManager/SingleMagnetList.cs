using Models;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace AvManager
{
    public partial class SingleMagnetList : Form
    {
        private List<SeedMagnetSearchModel> seeds = new List<SeedMagnetSearchModel>();

        public SingleMagnetList()
        {
            InitializeComponent();
        }

        public SingleMagnetList(List<SeedMagnetSearchModel> seeds)
        {
            InitializeComponent();
            this.seeds = seeds;
        }

        private async void SingleMagnetListListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (SingleMagnetListListView.SelectedItems.Count > 0)
            {
                var url = ((SeedMagnetSearchModel)SingleMagnetListListView.SelectedItems[0].Tag).MagUrl;

                var res = await OneOneFiveService.AddOneOneFiveTask(url);

                if (res.state == true)
                {
                    var close = MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK);

                    if (close == DialogResult.OK)
                    {
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("添加失败 " + res.error_msg, "警告", MessageBoxButtons.OK);
                }
            }
        }

        private void SingleMagnetList_Load(object sender, EventArgs e)
        {
            SingleMagnetListListView.Items.Clear();

            SingleMagnetListListView.BeginUpdate();

            foreach (var seed in seeds)
            {
                seed.MagSizeStr = FileUtility.GetAutoSizeString(seed.MagSize, 1);

                ListViewItem lvi = new();
                lvi.Tag = seed;
                lvi.SubItems[0].Text = seed.Title;
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Text = seed.MagSizeStr
                });
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Text = seed.Date.ToString("yyyy-MM-dd HH:mm:ss")
                });
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Text = seed.CompleteCount + ""
                });

                SingleMagnetListListView.Items.Add(lvi);
            }

            SingleMagnetListListView.EndUpdate();
        }
    }
}
