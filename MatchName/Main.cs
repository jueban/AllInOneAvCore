using Models;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace MatchName
{
    public partial class Main : Form
    {
        private string skipSize = "";
        private string prefix = "";
        private string root = "";
        private List<FileInfo> files = new();

        public Main()
        {
            InitializeComponent();
        }

        private void DescText_Click(object sender, EventArgs e)
        {
            FolderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
            var res = FolderBrowser.ShowDialog();

            if (res == DialogResult.Yes || res == DialogResult.OK)
            {
                DescText.Text = FolderBrowser.SelectedPath.EndsWith(Path.DirectorySeparatorChar) ? FolderBrowser.SelectedPath : FolderBrowser.SelectedPath + Path.DirectorySeparatorChar;
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting setting = new();
            setting.ShowDialog();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            skipSize = ConfigurationManager.AppSettings["skipSize"];
            prefix = ConfigurationManager.AppSettings["prefix"];
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InfoPb.Value = 0;
            InfoPb.Maximum = 0;
            RenamePb.Value = 0;
            RenamePb.Maximum = 0;
            root = "";
            files = new();
            FolderBrowser.RootFolder = Environment.SpecialFolder.MyComputer;
            var res = FolderBrowser.ShowDialog();

            Progress<MatchNameListViewModel> progress = new();
            progress.ProgressChanged += ListViewUpdate;

            if (res == DialogResult.Yes || res == DialogResult.OK)
            {
                RenameListView.BeginUpdate();

                ShowListView(progress);

                RenameListView.EndUpdate();
            }
        }

        private async void 扫描ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (RenameListView.Items.Count > 0 && !string.IsNullOrEmpty(DescText.Text))
            {
                执行ToolStripMenuItem.Enabled = false;

                Progress<MatchNameListViewModel> progress = new();
                progress.ProgressChanged += Scan;
                List<MatchNameListViewModel> files = new();

                foreach (ListViewItem lvi in RenameListView.Items)
                {
                    files.Add((MatchNameListViewModel)lvi.Tag);
                }

                await LocalService.RenameCheck(files, DescText.Text, prefix.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList(), progress);

                执行ToolStripMenuItem.Enabled = true;
            }
            else
            {
                MessageBox.Show("请先打开要重命名的目录获取文件,并设置重命名目录");
            }
        }

        private void 执行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RenameListView.Items.Count > 0 && !string.IsNullOrEmpty(DescText.Text) && InfoPb.Value == InfoPb.Maximum)
            {
                Rename();
            }
            else
            {
                MessageBox.Show("请先打开要重命名的目录获取文件,并设置重命名目录，并等待扫描结束");
            }
        }

        private void ListViewUpdate(object sender, MatchNameListViewModel e)
        {
            ListViewItem lvi = new(e.OriFile.FullName);
            lvi.SubItems.Add("暂无");
            lvi.Tag = e;

            RenameListView.Items.Add(lvi);
        }

        private void ShowListView(IProgress<MatchNameListViewModel> progress)
        {
            root = FolderBrowser.SelectedPath;

            var fis = new DirectoryInfo(root).GetFiles("*.*", SearchOption.AllDirectories);
            var skip = decimal.Parse(skipSize);

            files = fis.Where(x => x.Length >= 1024 * 1024 * 1024 * skip).ToList();

            if (files != null && files.Any())
            {
                InfoLabel.Text = $"共有{files.Count}个文件，总大小{FileUtility.GetAutoSizeString(files.Sum(x => x.Length), 1)}";
                InfoPb.Maximum = files.Count;
                RenamePb.Maximum = files.Count;
                int index = 0;

                foreach (var file in files)
                {
                    MatchNameListViewModel lvm = new();
                    lvm.Index = index++;
                    lvm.OriFile = file;
                    lvm.PossibleFiles = new();

                    progress.Report(lvm);
                }
            }
        }

        private void Scan(object sender, MatchNameListViewModel e)
        {
            RenameListView.Items[e.Index].Tag = e;
            RenameListView.Items[e.Index].SubItems[1].Text = e.DescFile;

            if (e.PossibleFiles.Count > 1)
            {
                RenameListView.Items[e.Index].BackColor = Color.Yellow;
            }

            if (e.PossibleFiles.Count <= 0)
            {
                RenameListView.Items[e.Index].BackColor = Color.Red;
            }

            if (e.PossibleFiles.Count == 1)
            {
                RenameListView.Items[e.Index].BackColor = Color.Green;
            }

            InfoPb.Value += 1;
        }

        private void Rename()
        {
            var notFoundFolder = DescText.Text + "未找到" + Path.DirectorySeparatorChar;

            if (!Directory.Exists(notFoundFolder))
            {
                Directory.CreateDirectory(notFoundFolder);
            }

            foreach (ListViewItem lvi in RenameListView.Items)
            {
                var model = (MatchNameListViewModel)lvi.Tag;

                //未找到
                if (model.PossibleFiles.Count <= 0)
                {
                    FileUtility.TransferFileUsingSystem(new List<string>() { model.OriFile.FullName }, notFoundFolder, true, false);
                }

                //完美匹配
                if (model.PossibleFiles.Count == 1)
                {
                    var match = model.PossibleFiles.FirstOrDefault();

                    var tempFolder = DescText.Text + $"{match.ReleaseDate?.ToString("yyyy-MM-dd")} {match.AvId} {match.Name}" + Path.DirectorySeparatorChar;

                    if (!Directory.Exists(tempFolder))
                    {
                        Directory.CreateDirectory(tempFolder);
                    }

                    FileUtility.TransferFileUsingSystem(new List<string>() { model.OriFile.FullName }, tempFolder + match.AvId + model.OriFile.Extension, true, false);

                    DownloadPic(match.PicUrl, tempFolder + $"{match.AvId}-{match.Name}.jpg");

                    WriteInfo(match.Infos, tempFolder + $"{match.AvId}-{match.Name}.json");

                    GenerateOldFileName(tempFolder + model.OriFile.Name + ".old");
                }

                //找到多个
                if (model.PossibleFiles.Count > 1)
                {
                    var tempFolder = DescText.Text + Path.GetFileNameWithoutExtension(model.OriFile.Name) + Path.DirectorySeparatorChar;

                    if (!Directory.Exists(tempFolder))
                    {
                        Directory.CreateDirectory(tempFolder);
                    }

                    FileUtility.TransferFileUsingSystem(new List<string>() { model.OriFile.FullName }, tempFolder, true, false);

                    foreach (var match in model.PossibleFiles)
                    {
                        DownloadPic(match.PicUrl, tempFolder + $"{match.AvId}-{match.Name}.jpg");

                        WriteInfo(match.Infos, tempFolder + $"{match.AvId}-{match.Name}.json");
                    }
                }

                RenamePb.Value += 1;
            }

            MessageBox.Show("处理完成");
        }

        private void DownloadPic(string url, string file)
        {
            if (!string.IsNullOrWhiteSpace(url) && !File.Exists(file))
            {
                try
                {
                    new WebClient().DownloadFile(url, file);
                }
                catch (Exception)
                {

                }
            }
        }

        private void GenerateOldFileName(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
        }

        private void WriteInfo(string info, string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();

                StreamWriter sw = new(path);
                sw.WriteLine(info);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
