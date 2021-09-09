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

namespace AvManager
{
    public partial class MagetSearch : Form
    {
        private int id;
        private List<ShowMagnetSearchResult> model;

        public MagetSearch()
        {
            InitializeComponent();
        }

        public MagetSearch(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private async void MagetSearch_Load(object sender, EventArgs e)
        {
            this.model = await MagnetUrlService.GetScanResultDetail(id);
            ShowList(this.model);
        }

        public void ShowList(List<ShowMagnetSearchResult> model)
        {
            MagnetSearchMainMainPanel.Controls.Clear();

            MagnetList list = new(model);

            MagnetSearchMainMainPanel.Controls.Add(list);

            this.MagnetSearchInfoLabel.Text = $"当前共有 {model.Count} 部";

            list.Dock = DockStyle.Fill;
        }

        private async void MagetnetSearchDownloadBtn_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new();

            if (MagnetSearchMainMainPanel.Controls.Count > 0)
            {
                MagnetList ml = (MagnetList)MagnetSearchMainMainPanel.Controls[0];

                foreach (MagnetItem mi in ml.tableLayout.Controls)
                {
                    if (mi.selectedModel != null)
                    {
                        sb.AppendLine(mi.selectedModel.MagUrl);
                    }
                }

                var res = await OneOneFiveService.AddOneOneFiveTask(sb.ToString());

                if (res.state == true)
                {
                    MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("添加失败", "警告", MessageBoxButtons.OK);
                }
            }
        }

        private void MagnetSearchAllRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (MagnetSearchAllRadio.Checked)
            {
                ShowList(this.model);
            }
        }

        private void MagnetSearchGreaterRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (MagnetSearchGreaterRadio.Checked)
            {
                var tempModel = this.model.Where(x => x.HasGreaterSize && x.MatchFiles != null && x.MatchFiles.Any()).ToList();
                ShowList(tempModel);
            }
        }

        private void MagnetSearchNotExistRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (MagnetSearchNotExistRadio.Checked)
            {
                var tempModel = this.model.Where(x => x.HasGreaterSize && (x.MatchFiles == null || x.MatchFiles.Count <= 0)).ToList();
                ShowList(tempModel);
            }
        }

        private void MagnetSearchCopyBtn_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new();

            if (MagnetSearchMainMainPanel.Controls.Count > 0)
            {
                MagnetList ml = (MagnetList)MagnetSearchMainMainPanel.Controls[0];

                foreach (MagnetItem mi in ml.tableLayout.Controls)
                {
                    if (mi.selectedModel != null)
                    {
                        sb.AppendLine(mi.selectedModel.MagUrl);
                    }
                }

                Clipboard.SetDataObject(sb.ToString());
            }
        }
    }
}
