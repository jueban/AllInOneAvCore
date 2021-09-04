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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace AvManager
{
    public partial class Main : Form
    {
        private bool IsCurrentCombineFinish = false;
        private Process p;
        public delegate void ProcessPb(ProgressBar pb, int value);
        private CancellationTokenSource AutoCombineCts = new();
        private CancellationTokenSource CombinePrepareCts = new();

        public Main()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        #region 全局
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            //准备合并页面
            if (TabControl.SelectedIndex == 2)
            {
                if (e.KeyCode == Keys.Space)
                {
                    e.Handled = true;

                    if (CombinePrepareTree.SelectedNode != null && CombinePrepareTree.SelectedNode.Level != 0)
                    {
                        Process.Start(Setting.POTPLAYEREXEFILELOCATION, @"" + ((MyFileInfo)CombinePrepareTree.SelectedNode.Tag).FullName);
                    }
                }
            }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabControl.SelectedIndex == 3)
            {
                ShowAutoCombineList();
            }
        }

        private void JDuBar(ProgressBar jd, int v)
        {
            if (jd.InvokeRequired)
            {
                jd.Invoke(new ProcessPb(JDuBar), jd, v);
            }
            else
            {
                if (v < jd.Maximum && v >= 0)
                {
                    jd.Value = v;
                }
                else
                {
                    jd.Value = jd.Maximum;
                }
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region 托盘相关
        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NotifyIcon.Visible = true;
                Show();
                WindowState = FormWindowState.Normal;
                Focus();
            }
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            NotifyIcon.Visible = true;
            Show();
            WindowState = FormWindowState.Normal;
            Focus();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                WindowState = FormWindowState.Minimized;
                NotifyIcon.Visible = true;
                Hide();
                return;
            }
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                NotifyIcon.Visible = true;
                Hide();
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要退出？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                NotifyIcon.Visible = false;
                Close();
                Dispose();
                Environment.Exit(Environment.ExitCode);
            }
        }
        #endregion

        #region 去文件夹相关
        private void RemoveFolderText_Click(object sender, EventArgs e)
        {
            CommonHelper.ChooseFolder(FolderBrowserDialog, RemoveFolderText);

            RemoveFolderInfoText.Text = "";
        }

        /// <summary>
        /// 去子文件夹，所有文件放入movefiles文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RemoveFolderConfirmBtn_Click(object sender, EventArgs e)
        {
            if (CommonHelper.CheckFolderChoose(RemoveFolderText))
            {
                Progress<string> progress = new Progress<string>();
                progress.ProgressChanged += UpdateRemoveFolderText;

                await LocalService.RemoveFolder(RemoveFolderText.Text, progress);
            }
        }

        /// <summary>
        /// 去文件夹操作回调更新日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateRemoveFolderText(object sender, string e)
        {
            RemoveFolderInfoText.AppendText(e + Environment.NewLine);
        }
        #endregion

        #region 重命名
        private void RenameText_Click(object sender, EventArgs e)
        {
            CommonHelper.ChooseFolder(FolderBrowserDialog, RenameText);

            RenameInfoText.Text = "";
        }

        private async void RenameBtn_Click(object sender, EventArgs e)
        {
            Progress<string> progress = new();
            progress.ProgressChanged += UpdateRenameInfoText;

            if (CommonHelper.CheckFolderChoose(RenameText))
            {
                await LocalService.Rename(RenameText.Text, progress);
            }
        }

        private void RenameManualBtn_Click(object sender, EventArgs e)
        {
            if (CommonHelper.CheckFolderChoose(RenameText))
            {
                var files = new DirectoryInfo(RenameText.Text).GetFiles().ToList();

                ManualRename mr = new(files, RenameText.Text);

                mr.ShowDialog();
            }
        }

        /// <summary>
        /// 重命名回调更新日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateRenameInfoText(object sender, string e)
        {
            RenameInfoText.AppendText(e + Environment.NewLine);
        }
        #endregion

        #region 合并准备相关
        private void CombinePrepareText_Click(object sender, EventArgs e)
        {
            CommonHelper.ChooseFolder(FolderBrowserDialog, CombinePrepareText);

            CombinePrepareTree.Nodes.Clear();
            CombinePreparePB.Maximum = 0;
            CombinePreparePB.Value = 0;
        }

        private async void ShowCombinePrepareData()
        {
            Progress<(int, int)> progress = new();

            progress.ProgressChanged += UpdateCombinePreparePB;

            CombinePrepareTree.Nodes.Clear();

            if (CommonHelper.CheckFolderChoose(CombinePrepareText))
            {
                var includeThumnail = CombinePrepareCB.Checked;

                var duplicate = await LocalService.GetCombinePrepareData(CombinePrepareText.Text, includeThumnail, Setting.FFMPEGLOCATION, Setting.THUMNAILLOCATION, 5, CombinePrepareCts.Token, progress);

                ShowCombinePrepareTree(duplicate);
            }
        }

        private void UpdateCombinePreparePB(object sender, (int, int) e)
        {
            CombinePreparePB.Maximum = e.Item1;
            CombinePreparePB.Value = e.Item2;
        }

        /// <summary>
        /// 获取重名文件数据,是否带视频截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CombinePrepareListBtn_Click(object sender, EventArgs e)
        {
            ShowCombinePrepareData();
        }

        /// <summary>
        /// 展示重名文件的树结构，大小+空格在前
        /// </summary>
        /// <param name="entity"></param>
        private void ShowCombinePrepareTree(Dictionary<string, List<MyFileInfo>> entity)
        {
            CombinePrepareTree.BeginUpdate();

            foreach (var group in entity)
            {
                TreeNode tempRoot = new TreeNode(group.Key);

                foreach (var item in group.Value)
                {
                    tempRoot.Nodes.Add(new TreeNode()
                    {
                        Text = item.LengthStr + " " + item.FullName,
                        Tag = item
                    });
                }

                tempRoot.Expand();
                CombinePrepareTree.Nodes.Add(tempRoot);
            }

            CombinePrepareTree.EndUpdate();
        }

        /// <summary>
        /// 修改Node名称后，如果跟原名称不相同就当做需要重命名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CombinePrepareTree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Label))
            {
                MyFileInfo currentTag = (MyFileInfo)e.Node.Tag;

                if (!currentTag.FullName.Equals(e.Label.Replace(currentTag.LengthStr, "").Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    currentTag.NewFullName = e.Label.Replace(currentTag.LengthStr, "").Trim();
                    currentTag.ChangeName = true;
                    e.Node.Text = currentTag.LengthStr + " " + currentTag.NewFullName;
                    e.Node.BackColor = Color.Green;
                }
                else
                {
                    currentTag.ChangeName = false;
                    e.Node.BackColor = Color.White;
                }
            }
        }

        /// <summary>
        /// 母结点不让勾选,子节点勾选设置删除状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CombinePrepareTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse && e.Node.Parent == null) 
            {
                e.Node.Checked = false;
                return;
            }

            if (e.Action == TreeViewAction.ByMouse)
            {
                var currentTag = (MyFileInfo)e.Node.Tag;

                if (e.Node.Checked)
                {
                    currentTag.IsDelete = true;
                }
                else
                {
                    currentTag.IsDelete = false;
                }
            }
        }

        private void CombinePrepareTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            e.Node.BeginEdit();
        }

        /// <summary>
        /// 母结点右键点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CombinePrepareTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Clicks == 1 && e.Node.Parent == null)
            {
                CombinePrepareTreeNodeMenu.Show(MousePosition.X, MousePosition.Y);
            }
        }

        /// <summary>
        /// 母结点查看所有子节点的截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 截图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = CombinePrepareTree.SelectedNode;

            if (selectedNode != null && selectedNode.Parent == null)
            {
                List<List<string>> pics = new List<List<string>>();

                foreach (TreeNode tn in selectedNode.Nodes)
                {
                    pics.Add(((MyFileInfo)tn.Tag).Thumnails);
                }

                Thumnail thumnail = new(pics);
                thumnail.ShowDialog();
            }
        }

        /// <summary>
        /// 校验数据后完成重命名，删除，以及生成合并文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CombinePrepareClearBtn_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<MyFileInfo>> data = new();

            foreach (TreeNode tr in CombinePrepareTree.Nodes)
            {
                if (tr.Level == 0)
                {
                    var tempList = new List<MyFileInfo>();

                    foreach (TreeNode subTr in tr.Nodes)
                    {
                        var tempTag = (MyFileInfo)subTr.Tag;

                        tempList.Add(tempTag);
                    }

                    data.Add(tr.Text, tempList);
                }
            }

            int deleteCount = 0;
            int renameCount = 0;
            long deleteSize = 0;

            foreach (var d in data)
            {
                foreach (var i in d.Value)
                {
                    if (i.ChangeName) {
                        renameCount++;
                    }

                    if (i.IsDelete) {
                        deleteCount++;
                        deleteSize += i.Length;
                    }
                }
            }

            var res = MessageBox.Show($"确定要重命名 {renameCount} 个文件，并且删除 {deleteCount} 个文件，总大小 {FileUtility.GetAutoSizeString(deleteSize, 1)} ?", "警告", MessageBoxButtons.YesNo);

            if (res == DialogResult.Yes)
            {
                var ret = LocalService.CombinePrepareDataClear(data);

                if (string.IsNullOrEmpty(ret))
                {
                    res = MessageBox.Show("操作成功", "提示", MessageBoxButtons.OK);
                }
                else
                {
                    res = MessageBox.Show($"操作失败 {ret}", "提示", MessageBoxButtons.OK);
                }

                if (res == DialogResult.OK)
                {
                    ShowCombinePrepareData();
                }
            }
        }

        private void CombinePrepareGenerateBtn_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<string>> data = new();

            foreach (TreeNode tr in CombinePrepareTree.Nodes)
            {
                if (tr.Level == 0)
                {
                    var tempList = new List<string>();

                    foreach (TreeNode subTr in tr.Nodes)
                    {
                        var tempTag = ((MyFileInfo)subTr.Tag).FullName;

                        tempList.Add(tempTag);
                    }

                    data.Add(tr.Text, tempList);
                }
            }

            if (!string.IsNullOrEmpty(LocalService.RealGenerateAutoCombineFile(data, Setting.AUTOCOMBINEFILELOCATION)))
            {
                MessageBox.Show("操作失败", "警告");
            } else
            {
                MessageBox.Show("操作成功", "提示");
            }
        }
        #endregion

        #region 自动合并
        private void ShowAutoCombineList()
        {
            if (Directory.Exists(Setting.AUTOCOMBINEFILELOCATION))
            {
                var files = Directory.GetFiles(Setting.AUTOCOMBINEFILELOCATION);

                ShowPreviewTree(files);
            }
        }

        private void ShowPreviewTree(string[] files)
        {
            AutoCombineListView.Items.Clear();

            AutoCombineListView.BeginUpdate();

            foreach (var file in files)
            {
                ListViewItem lvi = new ListViewItem();
                var list = new List<FileInfo>();
                lvi.SubItems[0].Text = file;

                StreamReader sr = new StreamReader(file);

                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();

                    if (!string.IsNullOrEmpty(line))
                    {
                        var fileName = line.Substring(line.IndexOf("'") + 1);
                        fileName = fileName.Substring(0, fileName.LastIndexOf("'"));

                        if (File.Exists(fileName))
                        {
                            list.Add(new FileInfo(fileName));
                        }
                    }
                }

                sr.Close();

                lvi.SubItems.Add(new ListViewItem.ListViewSubItem() { 
                    Text = FileUtility.GetAutoSizeString(list.Sum(x => x.Length), 1)
                });

                lvi.Tag = list;

                AutoCombineListView.Items.Add(lvi);
            }

            AutoCombineListView.EndUpdate();
        }

        private void AutoCombineSaveText_Click(object sender, EventArgs e)
        {
            CommonHelper.ChooseFolder(FolderBrowserDialog, AutoCombineSaveText);
        }

        private void AutoCombineStartBtn_Click(object sender, EventArgs e)
        {
            AutoCombineCurrentPB.Value = 0;
            AutoCombineTotalPB.Value = 0;
            IsCurrentCombineFinish = false;
            AutoCombineInfoText.Text = "";

            if (CommonHelper.CheckFolderChoose(AutoCombineSaveText) && AutoCombineListView.Items.Count > 0)
            {
                Dictionary<string, List<FileInfo>> autoCombine = new();

                foreach (ListViewItem lvi in AutoCombineListView.Items)
                {
                    List<FileInfo> tempList = new List<FileInfo>();
                    tempList.AddRange(((List<FileInfo>)lvi.Tag));

                    autoCombine.Add(lvi.SubItems[0].Text, tempList);
                }

                StartAutoCombine(autoCombine);
            }
        }

        private void AutoCombineDeleteBtn_Click(object sender, EventArgs e)
        {
            if (AutoCombineListView.Items.Count > 0 && AutoCombineListView.SelectedItems.Count > 0)
            {
                Dictionary<string, List<FileInfo>> deleteFiles = new();

                foreach (ListViewItem lvi in AutoCombineListView.SelectedItems)
                {
                    List<FileInfo> tempList = new List<FileInfo>();
                    tempList.AddRange(((List<FileInfo>)lvi.Tag));

                    deleteFiles.Add(lvi.SubItems[0].Text, tempList);
                }

                var ret = MessageBox.Show($"确定要删除 {deleteFiles.Values.Sum(x => x.Count)} 个文件，总大小 {FileUtility.GetAutoSizeString(deleteFiles.Values.Sum(x => x.ToList().Sum(y => y.Length)),1 )} ?", "警告", MessageBoxButtons.YesNo);

                if (ret == DialogResult.Yes)
                {
                    foreach (var file in deleteFiles)
                    {
                        File.Delete(file.Key);

                        foreach (var f in file.Value)
                        {
                            File.Delete(f.FullName);
                        }
                    }
                }
            }
        }

        private async void StartAutoCombine(Dictionary<string, List<FileInfo>> autoCombine) 
        {
            Progress<int> progress = new();

            progress.ProgressChanged += SetAutoCombineCurrentPBMax;

            int index = 1;
            AutoCombineTotalPB.Maximum = autoCombine.Count;

            foreach (var a in autoCombine)
            {
                p = new Process();
                LocalService.Output += OutputAuto;

                JDuBar(AutoCombineCurrentPB, 0);

                List<string> files = new List<string>();

                a.Value.ForEach(x => files.Add(x.FullName));

                try
                {
                    await LocalService.AutoCombineVideosUsingFFMPEG(files, a.Key, Setting.FFMPEGLOCATION, AutoCombineSaveText.Text + "\\", p, progress, AutoCombineCts.Token);
                }
                catch (OperationCanceledException)
                { 
                    MessageBox.Show("任务取消");
                    return;
                }

                if (IsCurrentCombineFinish)
                {
                    //TODO删除文件
                    IsCurrentCombineFinish = false;

                    AutoCombineListView.Items[index - 1].BackColor = Color.Green;
                }
                else
                {
                    IsCurrentCombineFinish = false;

                    AutoCombineListView.Items[index - 1].BackColor = Color.Red;
                }

                JDuBar(AutoCombineTotalPB, index++);
            }
        }

        private void SetAutoCombineCurrentPBMax(object sender, int e)
        {
            AutoCombineCurrentPB.Maximum = e;
        }

        private void OutputAuto(object sendProcess, DataReceivedEventArgs output)
        {
            if (!String.IsNullOrEmpty(output.Data))
            {
                if (output.Data.StartsWith("frame"))
                {
                    var time = output.Data.Substring(output.Data.IndexOf("time="), 16).Replace("time=", "");
                    var process = LocalService.ConvertDurationToInt(time);

                    AutoCombineInfoText.AppendText(output.Data);

                    JDuBar(AutoCombineCurrentPB, process);
                }
                else if (output.Data.StartsWith("video:"))
                {
                    IsCurrentCombineFinish = true;
                }
            }
        }

        private void AutoCombineCancelBtn_Click(object sender, EventArgs e)
        {
            AutoCombineCts.Cancel();
        }
        #endregion
    }
}
