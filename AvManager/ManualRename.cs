using AvManager.Helper;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace AvManager
{
    public partial class ManualRename : Form
    {
        private List<FileInfo> fis = new List<FileInfo>();
        private string currentFolder = "";

        #region 行为
        public ManualRename()
        {
            InitializeComponent();
        }

        public ManualRename(List<FileInfo> fis, string currentFolder)
        {
            InitializeComponent();
            this.fis = fis;
            this.currentFolder = currentFolder;
        }

        private void ManualRename_Load(object sender, EventArgs e)
        {
            ManualRenamePositionCombo.DataSource = Enum.GetNames(typeof(RenameLocation));
            ManualRenamePositionCombo.SelectedIndex = ManualRenamePositionCombo.FindString(RenameLocation.Fin.ToString());

            for (int i = 0; i <= 10; i++)
            {
                ManualRenameEpisodeCombo.Items.Add(i);
            }
            ManualRenameEpisodeCombo.SelectedIndex = 0;

            ManualRenameListView.BeginUpdate();

            foreach (var fi in fis)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems.Insert(0, new ListViewItem.ListViewSubItem()
                {
                    Text = FileUtility.GetAutoSizeString(fi.Length, 1)
                });

                lvi.SubItems.Insert(1, new ListViewItem.ListViewSubItem()
                {
                    Text = fi.Name.Replace(fi.Extension, "")
                });

                lvi.Tag = fi;

                ManualRenameListView.Items.Add(lvi);
            }

            ManualRenameListView.EndUpdate();
        }

        private void ManualRenameListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListView.SelectedIndexCollection indexes = ManualRenameListView.SelectedIndices;
                if (indexes.Count > 0)
                {
                    ResetInfos();
                    int index = indexes[0];
                }
            }
            catch
            {

            }
        }

        private void avItemClicked(object sender, EventArgs e)
        {
            AvItemClick(sender);
        }

        private void ResetPanelManualRenameMainMainPanel()
        {
            var controls = ManualRenameMainMainPanel.Controls;

            foreach (var c in controls)
            {
                AvItem av = (AvItem)c;

                av.Panel.BackColor = Color.Empty;
            }
        }

        private void ManualRenamePositionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            AppendPostfix();
        }

        private void ManualRenameEpisodeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            AppendPostfix();
        }

        private void ManualRenameChineseCB_CheckedChanged(object sender, EventArgs e)
        {
            AppendPostfix();
        }

        private void ManualRenameMatchBtn_Click(object sender, EventArgs e)
        {
            if (CommonHelper.CheckFolderChoose(ManualRenameMatchText))
            {
                Match(ManualRenameMatchText.Text);
            }
        }

        private void ManualRenameSearchBtn_Click(object sender, EventArgs e)
        {
            if (CommonHelper.CheckFolderChoose(ManualRenameMatchText))
            {
                SearchJavLibrary();
            }
        }

        private void ManualRenameConfirmBtn_Click(object sender, EventArgs e)
        {
            if (CommonHelper.CheckFolderChoose(ManualRenameMatchText) && ManualRenameListView.SelectedItems.Count > 0)
            {
                ConfirmMethod();
            }
        }

        private void ManualRename_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void ManualRename_KeyDown(object sender, KeyEventArgs e)
        {
            if (ManualRenameListView.SelectedItems.Count > 0)
            {
                var file = ((FileInfo)ManualRenameListView.SelectedItems[0].Tag);

                if (e.KeyCode == Keys.Delete)
                {
                    var ret = MessageBox.Show($"确定要删除文件 {file.FullName}, 大小 {FileUtility.GetAutoSizeString(file.Length, 1)} ?", "警告", MessageBoxButtons.YesNo);

                    if (ret == DialogResult.Yes)
                    {
                        File.Delete(file.FullName);

                        MoveListViewItem();
                    }
                }

                if (e.KeyCode == Keys.Space)
                {
                    Process.Start(Setting.POTPLAYEREXEFILELOCATION, @"" + file.FullName);
                }
            }
        }
        #endregion

        #region 方法
        private async void ResetInfos()
        {
            ManualRenameMainMainPanel.Controls.Clear();

            this.Text = "手动重命名 - " + ManualRenameListView.SelectedItems[0].SubItems[1].Text;
            ManualRenamePositionCombo.SelectedIndex = ManualRenamePositionCombo.FindString(RenameLocation.Fin.ToString());
            ManualRenameEpisodeCombo.SelectedIndex = 0;
            ManualRenameChineseCB.Checked = false;
            ManualRenameFinalText.Text = ManualRenameListView.SelectedItems[0].SubItems[1].Text;

            var possibleName = await LocalService.GetPossibleAvName(ManualRenameListView.SelectedItems[0].SubItems[1].Text);

            Match(possibleName);
        }

        private async void Match(string avId)
        {
            var avModel = await JavLibraryService.GetAvModelByAvId(avId);

            ManualRenameMatchText.Text = !string.IsNullOrEmpty(avId) ? avId : ManualRenameListView.SelectedItems[0].SubItems[1].Text;

            if (avModel != null && avModel.Any())
            {
                ShowAv(avModel);
            }
        }

        private void ShowAv(List<AvModel> avs)
        {
            ManualRenameMainMainPanel.Controls.Clear();
            var count = avs.Count;

            var outerWidth = ManualRenameMainMainPanel.Width - 10;
            var outerHeight = ManualRenameMainMainPanel.Height;

            var singleWidth = (int)(outerWidth / 3);
            var singleHeight = (int)(singleWidth / 1.5);

            int x = 2, y = 2;

            while (count > 0)
            {
                count -= 3;

                foreach (var av in avs.Take(3))
                {
                    AvItem item = new AvItem(av.Name, av.PicUrl, av.AvId, av.Id)
                    {
                        Width = singleWidth,
                        Height = singleHeight,
                        Location = new Point(x, y)
                    };

                    item.AvItemClicked += new AvItem.AvItemClickHandle(avItemClicked);

                    ManualRenameMainMainPanel.Controls.Add(item);

                    x += singleWidth + 5;
                }

                x = 2;
                y += singleHeight + 2;

                avs = avs.Skip(3).ToList();
            }

            if (ManualRenameMainMainPanel.Controls.Count > 0)
            {
                AvItemClick(ManualRenameMainMainPanel.Controls[0].Controls[0].Controls[0]);
            }
        }

        private void AvItemClick(object sender)
        {
            ResetPanelManualRenameMainMainPanel();

            PictureBox pb = (PictureBox)sender;
            AvItem av = (AvItem)pb.Parent.Parent;

            av.Panel.BackColor = Color.Blue;

            SetName(av.AvName, av.AvId);
        }

        private async void ConfirmMethod()
        {
            ManualRenameModel model = new ManualRenameModel();

            foreach (var control in ManualRenameMainMainPanel.Controls)
            {
                AvItem temp = (AvItem)control;
                if (temp.Panel.BackColor == Color.Blue)
                {
                    model.episode = int.Parse(ManualRenameEpisodeCombo.Text);
                    model.language = ManualRenameChineseCB.Checked ? RenamneLanguage.Chinese : RenamneLanguage.Janpanese;
                    model.location = (RenameLocation)Enum.Parse(typeof(RenameLocation), ManualRenamePositionCombo.Text);
                    model.rootFolder = this.currentFolder;
                    model.moveFile = ((FileInfo)ManualRenameListView.SelectedItems[0].Tag).FullName;
                    model.avDbId = temp.AvDBId;

                    if (await LocalService.ManualRename(model))
                    {
                        MoveListViewItem();
                    }

                    break;
                }
            }
        }

        private void SetName(string avName, string id)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(id);
            sb.Append("-" + avName);

            ManualRenameFinalText.Text = sb.ToString();
        }

        private void AppendPostfix()
        {
            AvItem item = null;

            foreach (var control in ManualRenameMainMainPanel.Controls)
            {
                AvItem temp = (AvItem)control;
                if (temp.Panel.BackColor == Color.Blue)
                {
                    item = temp;
                }
            }

            if (item != null)
            {
                AvItemClick(item.Controls[0].Controls[0]);

                StringBuilder sb = new StringBuilder();

                if (ManualRenameEpisodeCombo.Text != "0")
                {
                    sb.Append("-" + ManualRenameEpisodeCombo.Text);
                }

                if (ManualRenameChineseCB.Checked)
                {
                    sb.Append("-C");
                }

                ManualRenameFinalText.Text = ManualRenameFinalText.Text + sb.ToString();
            }
        }

        private async void SearchJavLibrary()
        {
            var ret = await LocalService.GetJavLibrarySearchResult(ManualRenameMatchText.Text);

            if (ret != null && ret.Any())
            {
                MessageBox.Show($"更新了 {ret.Count} 个记录", "提示", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show($"没有找到记录", "警告", MessageBoxButtons.OK);
            }
        }

        private void MoveListViewItem()
        {
            var index = ManualRenameListView.SelectedItems[0].Index;
            ManualRenameListView.Items.Remove(ManualRenameListView.SelectedItems[0]);

            if (index + 1 < ManualRenameListView.Items.Count)
            {
                ManualRenameListView.Items[index + 1].Selected = true;
            }
            else
            {
                if (ManualRenameListView.Items.Count > 0)
                {
                    ManualRenameListView.Items[0].Selected = true;
                }
            }
        }
        #endregion
    }
}