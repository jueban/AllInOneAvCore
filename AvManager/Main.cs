using AvManager.Helper;
using DAL;
using Models;
using Newtonsoft.Json;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace AvManager
{
    public partial class Main : Form
    {
        #region 变量
        public delegate void ProcessPb(ProgressBar pb, int value);
        private bool IsCurrentCombineFinish = false;
        private Process p;
        private CancellationTokenSource AutoCombineCts = new();
        private CancellationTokenSource CombinePrepareCts = new();
        private bool OnlyCancelCurrentCombineTask = false;
        private bool IsManualChangeSearchListBox = true;
        private bool UpdateVideoCombo = true;
        public int VideoPage = 50;
        #endregion

        #region 全局
        public Main()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

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

            //整理页面
            if (TabControl.SelectedIndex == 4)
            {
                if (e.KeyCode == Keys.Space)
                {
                    e.Handled = true;

                    if (ClearTreeView.SelectedNode != null && ClearTreeView.SelectedNode.Level != 0)
                    {
                        Process.Start(Setting.POTPLAYEREXEFILELOCATION, @"" + ((MyFileInfo)ClearTreeView.SelectedNode.Tag).FullName);
                    }
                }
            }

            //播放文件夹
            if (TabControl.SelectedIndex == 6)
            {
                if (e.KeyCode == Keys.Back)
                {
                    if (PlayFolderListView.SelectedItems.Count > 0)
                    {
                        foreach (ListViewItem lvi in PlayFolderListView.SelectedItems)
                        {
                            var file = (FileInfo)lvi.Tag;
    
                            SettingService.SetPlayHistoryNotPlayed(file.Name);

                            lvi.BackColor = Color.White;
                        }
                    }
                }

                if (e.KeyCode == Keys.Space)
                {
                    e.Handled = true;

                    if (PlayFolderListView.SelectedItems.Count > 0)
                    {
                        if (PlayFolderListView.SelectedItems.Count > 1)
                        {
                            List<string> files = new();

                            foreach (ListViewItem lvi in PlayFolderListView.SelectedItems)
                            {
                                var file = (FileInfo)lvi.Tag;
                                files.Add(file.FullName);

                                SettingService.InsertPlayHistory(new PlayHistory()
                                {
                                    FileName = file.Name,
                                    PlayTimes = 1,
                                    LastPlay = DateTime.Now,
                                    SetNotPlayed = false
                                });

                                lvi.BackColor = Color.Green;
                            }

                            var playList = LocalService.GeneratePotPlayerPlayList(files, Setting.POTPLAYERPLAYLISTLOCATION);

                            Process.Start(Setting.POTPLAYEREXEFILELOCATION, playList);

                        }
                        else
                        {
                            var file = (FileInfo)PlayFolderListView.SelectedItems[0].Tag;
                            Process.Start(Setting.POTPLAYEREXEFILELOCATION, @"" + file.FullName);

                            SettingService.InsertPlayHistory(new PlayHistory()
                            {
                                FileName = file.Name,
                                PlayTimes = 1,
                                LastPlay = DateTime.Now,
                                SetNotPlayed = false
                            });

                            PlayFolderListView.SelectedItems[0].BackColor = Color.Green;
                        }
                    }
                }
            }

            //影片库相关
            if (TabControl.SelectedIndex == 8)
            {
                if (VideoListView.SelectedItems.Count > 0 && e.KeyCode == Keys.Space)
                {
                    List<MyFileInfo> files = new();

                    foreach (ListViewItem lvi in VideoListView.SelectedItems)
                    {
                        var model = ((VideoModel)lvi.Tag);

                        var first = model.FileInfo.FirstOrDefault();
                        if (first != null && first.IsRemote == false)
                        {
                            files.Add(first);
                        }
                    }

                    Process.Start(Setting.POTPLAYEREXEFILELOCATION, @"" + LocalService.GeneratePotPlayerPlayList(files.Select(x => x.FullName).ToList(), Setting.POTPLAYERPLAYLISTLOCATION));

                    foreach (var fi in files)
                    {
                        SettingService.InsertPlayHistory(new PlayHistory
                        {
                            FileName = fi.Name,
                            LastPlay = DateTime.Now,
                            PlayTimes = 1,
                            SetNotPlayed = false
                        });
                    }
                }
            }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //自动合并相关
            if (TabControl.SelectedIndex == 3)
            {
                ShowAutoCombineList();
            }

            //搜索相关
            if (TabControl.SelectedIndex == 5)
            {
                SearchSiteComboBox.SelectedIndex = 0;
            }

            //设置相关
            if (TabControl.SelectedIndex == 9)
            {
                InitSetting();
            }

            //影片库相关
            if (TabControl.SelectedIndex == 8)
            {
                InitVideo();
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

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            NotifyIcon.Visible = false;
            Close();
            Dispose();
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
            if (e.Button == MouseButtons.Right && e.Clicks == 1 && e.Node.Level == 0)
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
                    if (!OnlyCancelCurrentCombineTask)
                    {
                        MessageBox.Show("任务取消");
                        return;
                    }
                }

                if (IsCurrentCombineFinish)
                {
                    //TODO删除文件
                    IsCurrentCombineFinish = false;

                    AutoCombineListView.Items[index - 1].BackColor = Color.Green;
                }
                else
                {
                    //TODO删除文件
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

                    if (AutoCombineCurrentPB.Value - AutoCombineCurrentPB.Maximum >= 100)
                    {
                        OnlyCancelCurrentCombineTask = true;
                        AutoCombineCts.Cancel();
                    }
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

        #region 整理
        private void ClearRefreshBtn_Click(object sender, EventArgs e)
        {
            ShowClearTree();
        }

        private void ClearClearBtn_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<MyFileInfo>> data = new();

            foreach (TreeNode tr in ClearTreeView.Nodes)
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
                    if (i.ChangeName)
                    {
                        renameCount++;
                    }

                    if (i.IsDelete)
                    {
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
                    ShowClearTree();
                }
            }
        }

        private void ClearInteliCB_CheckedChanged(object sender, EventArgs e)
        {
            if (ClearTreeView.Nodes.Count > 0)
            {
                if (ClearInteliCB.Checked)
                {
                    foreach (TreeNode parent in ClearTreeView.Nodes)
                    {
                        if (parent.Level == 0)
                        {
                            Dictionary<MyFileInfo, TreeNode> tempFiles = new();

                            foreach (TreeNode child in parent.Nodes)
                            {
                                tempFiles.Add(((MyFileInfo)child.Tag), child);
                            }

                            //有中文就选中其他删除,如果没有保留文件大小最大的
                            var chineseKey = tempFiles.Keys.FirstOrDefault(x => x.IsChinese == true);

                            if (chineseKey != null)
                            {
                                foreach (var key in tempFiles)
                                {
                                    if (key.Key != chineseKey)
                                    {
                                        ((MyFileInfo)key.Value.Tag).IsDelete = true;
                                        key.Value.Checked = true;
                                    }
                                }
                            }
                            else
                            {
                                var biggestSize = tempFiles.Keys.Max(x => x.Length);
                                var biggestKey = tempFiles.Keys.FirstOrDefault(x => x.Length == biggestSize);

                                foreach (var key in tempFiles)
                                {
                                    if (key.Key != biggestKey)
                                    {
                                        ((MyFileInfo)key.Value.Tag).IsDelete = true;
                                        key.Value.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (TreeNode tn in ClearTreeView.Nodes)
                    {
                        if (tn.Level == 0)
                        {
                            foreach (TreeNode sub in tn.Nodes)
                            {
                                ((MyFileInfo)sub.Tag).IsDelete = false;
                                sub.Checked = false;
                            }
                        }
                    }
                }
            }
        }

        private async void ShowClearTree()
        {
            ClearTreeView.Nodes.Clear();
            ClearInteliCB.Checked = false;

            var avs = await LocalService.GetDuplicateAvFile();

            if (avs != null && avs.Any())
            {
                foreach (var group in avs)
                {
                    TreeNode tempRoot = new(group.Key);

                    foreach (var item in group.Value)
                    {
                        var tempNode = new TreeNode()
                        {
                            Text = item.LengthStr + " " + item.FullName,
                            Tag = item,
                            BackColor = item.IsChinese ? Color.Green : Color.White
                        };

                        tempRoot.Nodes.Add(tempNode);
                    }

                    tempRoot.Expand();
                    ClearTreeView.Nodes.Add(tempRoot);
                }

                ClearTreeView.EndUpdate();
            }
            else
            {
                MessageBox.Show("没有重名文件");
            }
        }

        private void ClearTreeView_AfterCheck(object sender, TreeViewEventArgs e)
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

        private void ClearTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Label))
            {
                MyFileInfo currentTag = (MyFileInfo)e.Node.Tag;

                if (!currentTag.FullName.Equals(e.Label.Replace(currentTag.LengthStr, "").Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    currentTag.NewFullName = e.Label.Replace(currentTag.LengthStr, "").Trim();
                    currentTag.ChangeName = true;
                    e.Node.Text = currentTag.LengthStr + " " + currentTag.NewFullName;
                    e.Node.BackColor = Color.Yellow;
                }
                else
                {
                    currentTag.ChangeName = false;
                    e.Node.BackColor = Color.White;
                }
            }
        }

        private void ClearTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            e.Node.BeginEdit();
        }
        #endregion

        #region 播放文件夹
        private void PlayFolderTxt_MouseClick(object sender, MouseEventArgs e)
        {
            CommonHelper.ChooseFolder(FolderBrowserDialog, PlayFolderTxt);
        }

        private void PlayFolderBtn_Click(object sender, EventArgs e)
        {
            if (CommonHelper.CheckFolderChoose(PlayFolderTxt))
            {
                ShowPlayFolderListView();
            }
        }

        private void PlayFolderListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (PlayFolderListView.SelectedItems.Count > 0)
            {
                var file = (FileInfo)PlayFolderListView.SelectedItems[0].Tag;
                Process.Start(Setting.POTPLAYEREXEFILELOCATION, file.FullName);
                SettingService.InsertPlayHistory(new PlayHistory()
                {
                    FileName = file.Name,
                    PlayTimes = 1,
                    LastPlay = DateTime.Now,
                    SetNotPlayed = false
                });

                PlayFolderListView.SelectedItems[0].BackColor = Color.Green;
            }
        }

        private void PlayFolderListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Clicks == 1 && PlayFolderListView.SelectedItems.Count > 0)
            {
                List<FileInfo> list = new();

                foreach (ListViewItem lvi in PlayFolderListView.SelectedItems)
                {
                    list.Add(((FileInfo)lvi.Tag));
                }

                var ret = MessageBox.Show($"确定要删除 {list.Count} 个文件，总大小 {FileUtility.GetAutoSizeString(list.Sum(x => x.Length), 1)} ?", "警告", MessageBoxButtons.YesNo);

                if (ret == DialogResult.Yes)
                {
                    list.ForEach(x => x.Delete());

                    ShowPlayFolderListView();
                }
            }
        }

        private async void ShowPlayFolderListView()
        {
            PlayFolderListView.Items.Clear();

            var setting = await SettingService.GetSetting();
            var exclude = setting.ExcludeFolder;
            var filters = setting.AvNameFilter.Split(',').ToList();

            int limitSize = 200;
            List<FileInfo> files = new List<FileInfo>();

            FileUtility.GetFilesRecursive(PlayFolderTxt.Text, exclude, filters, EverythingService.Extensions, files, limitSize);

            PlayFolderListView.BeginUpdate();

            foreach (var file in files.OrderByDescending(x => x.LastWriteTime))
            {
                ListViewItem lvi = new(file.Directory.Name);

                lvi.Tag = file;

                lvi.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Text = file.Name
                });

                lvi.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Text = FileUtility.GetAutoSizeString(file.Length, 1)
                });

                lvi.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Text = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss")
                });

                var playHistory = SettingService.GetPlayHistory(file.Name);

                if (playHistory != null && playHistory.SetNotPlayed == false)
                {
                    lvi.BackColor = Color.Green;
                }

                PlayFolderListView.Items.Add(lvi);
            }

            PlayFolderListView.EndUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MagetSearch search = new();
            search.ShowDialog();
        }

        private async void PlayFolderMoveBtn_Click(object sender, EventArgs e)
        {
            if (PlayFolderListView.SelectedItems.Count > 0)
            {
                var choose = FolderBrowserDialog.ShowDialog();

                if ((choose == DialogResult.Yes || choose == DialogResult.OK) && FolderBrowserDialog.SelectedPath != null)
                {
                    List<FileInfo> list = new();

                    foreach (ListViewItem lvi in PlayFolderListView.SelectedItems)
                    {
                        list.Add(((FileInfo)lvi.Tag));
                    }

                    var ret = MessageBox.Show($"是否要移动 {PlayFolderListView.SelectedItems.Count} 个文件，大小 {FileUtility.GetAutoSizeString(list.Sum(x => x.Length), 1)} ?", "警告", MessageBoxButtons.YesNo);

                    if (ret == DialogResult.Yes)
                    {
                        await Task.Run(() =>
                        {
                            FileUtility.TransferFileUsingSystem(list.Select(x => x.FullName).ToList(), FolderBrowserDialog.SelectedPath, true, false);
                        });
                    }

                    ShowPlayFolderListView();
                }
            }
        }
        #endregion

        #region 搜索相关
        private void SearchUpdateBtn_Click(object sender, EventArgs e)
        {
            InitSearchUI();
        }

        private async void SearchSearchBtn_Click(object sender, EventArgs e)
        {
            ScanPageModel param = new();
            Progress<string> stringProgress = new();
            Progress<(string, int, int)> intProgress = new();
            intProgress.ProgressChanged += SearchProgressChanged;

            param.Page = (int)SearchPageUpDown.Value;
            param.Order = SearchAscRadio.Checked ? "ASC" : "DESC";           

            foreach (ListBox lb in SearchUpperTablePanel.Controls)
            {
                if (lb.SelectedItems.Count > 0)
                {
                    foreach (ListBoxItem lbi in lb.SelectedItems)
                    {
                        param.Url += (string)lbi.Tag + ",";
                        param.Name += lbi.Title + ",";
                    }

                    param.Url = param.Url[0..^1];

                    param.Name = param.Name[0..^1];
                    if (param.Name.Length > 20)
                    {
                        param.Name = param.Name[0..20] + "...";
                    }
                }
            }

            ListViewItem lvi = new();
            ProgressBar pb = new();
            var key = (SearchListView.Items.Count + 1) + "temp";

            lvi.SubItems[0].Text = param.Name;
            lvi.SubItems.Add(0 + "");
            lvi.SubItems.Add("");
            lvi.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            lvi.SubItems.Add(key);

            SearchListView.Items.Add(lvi);

            Rectangle r = lvi.SubItems[2].Bounds;
            pb.SetBounds(r.X, r.Y, r.Width, r.Height);
            pb.Minimum = 0;
            pb.Maximum = param.Url.Split(',').Length;
            pb.Value = 0;
            pb.Name = key;
            SearchListView.Controls.Add(pb);

            if (SearchSiteComboBox.Text == "JavLibrary")
            {
                await MagnetUrlService.SearchJavLibrary(param.Url, param.Page, param.Name, param.Order, stringProgress, key, intProgress);
            }
            else if (SearchSiteComboBox.Text == "JavBus")
            {
                await MagnetUrlService.SearchJavBus(param.Url, param.Page, param.Name, stringProgress);
            }
        }

        private void SearchProgressChanged(object sender, (string, int, int) e)
        {
            ProgressBar pb;

            pb = SearchListView.Controls.OfType<ProgressBar>().FirstOrDefault(x => x.Name == e.Item1);
            if (pb != null)
            {
                pb.Value = e.Item3;
            }
        }

        private void SearchListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 2 && e.Button == MouseButtons.Left && SearchListView.SelectedItems.Count > 0)
            {
                var result = ((ScanResult)SearchListView.SelectedItems[0].Tag);

                if (result != null)
                {
                    MagetSearch ms = new(result.Id);
                    ms.ShowDialog();
                }
            }
        }

        private async void SearchListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1 && e.Button == MouseButtons.Right && SearchListView.SelectedItems.Count > 0)
            {
                var result = ((ScanResult)SearchListView.SelectedItems[0].Tag);

                if (result != null)
                {
                    await new ScanDAL().DeleteSeedMagnetSearchModelById(result.Id);
                }
            }
        }

        private void SearchPageListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsManualChangeSearchListBox)
            {
                IsManualChangeSearchListBox = false;
                SearchActressListBox.ClearSelected();
                SearchCategoryListBox.ClearSelected();
                SearchPrefixListBox.ClearSelected();
            }
            IsManualChangeSearchListBox = true;
        }

        private void SearchActressListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsManualChangeSearchListBox)
            {
                IsManualChangeSearchListBox = false;
                SearchPageListBox.ClearSelected();
                SearchCategoryListBox.ClearSelected();
                SearchPrefixListBox.ClearSelected();
            }
            IsManualChangeSearchListBox = true;
        }

        private void SearchCategoryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsManualChangeSearchListBox)
            {
                IsManualChangeSearchListBox = false;
                SearchActressListBox.ClearSelected();
                SearchPageListBox.ClearSelected();
                SearchPrefixListBox.ClearSelected();
            }
            IsManualChangeSearchListBox = true;
        }

        private void SearchPrefixListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsManualChangeSearchListBox)
            {
                IsManualChangeSearchListBox = false;
                SearchActressListBox.ClearSelected();
                SearchCategoryListBox.ClearSelected();
                SearchPageListBox.ClearSelected();
            }
            IsManualChangeSearchListBox = true;
        }

        private void SearchSiteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSearchListBox();
        }

        private void InitSearchUI()
        {
            UpdateSearchListBox();
        }

        private async void UpdateSearchListBox()
        {
            if (string.IsNullOrEmpty(SearchSiteComboBox.Text))
            {
                return;
            }

            var enumValue = (WebScanUrlSite)Enum.Parse(typeof(WebScanUrlSite), SearchSiteComboBox.Text);

            var favi = await MagnetUrlService.GetScanPageMode(enumValue);

            var pageDrop = favi.Drops.Where(x => x.Title == "选择页面").ToList();
            var actressDrop = favi.Drops.Where(x => x.Title == "选择演员").ToList();
            var catrgoryDrop = favi.Drops.Where(x => x.Title == "选择类别").ToList();
            var prefixDrop = favi.Drops.Where(x => x.Title == "选择前缀").ToList();

            SearchPageListBox.DisplayMember = "Title";
            SearchPageListBox.ValueMember = "Tag";

            SearchCategoryListBox.DisplayMember = "Title";
            SearchCategoryListBox.ValueMember = "Tag";

            SearchActressListBox.DisplayMember = "Title";
            SearchActressListBox.ValueMember = "Tag";

            SearchPrefixListBox.DisplayMember = "Title";
            SearchPrefixListBox.ValueMember = "Tag";

            SearchPageListBox.Items.Clear();
            SearchCategoryListBox.Items.Clear();
            SearchActressListBox.Items.Clear();
            SearchPrefixListBox.Items.Clear();

            foreach (var drop in pageDrop)
            {
                foreach (var sub in drop.Items)
                {
                    SearchPageListBox.Items.Add(new ListBoxItem{ Title = sub.Text, Tag = sub.Value });
                }
            }

            foreach (var drop in actressDrop)
            {
                foreach (var sub in drop.Items)
                {
                    SearchCategoryListBox.Items.Add(new ListBoxItem { Title = sub.Text, Tag = sub.Value });
                }
            }

            foreach (var drop in catrgoryDrop)
            {
                foreach (var sub in drop.Items)
                {
                    SearchActressListBox.Items.Add(new ListBoxItem { Title = sub.Text, Tag = sub.Value });
                }
            }

            foreach (var drop in prefixDrop)
            {
                foreach (var sub in drop.Items)
                {
                    SearchPrefixListBox.Items.Add(new ListBoxItem { Title = sub.Text, Tag = sub.Value });
                }
            }

            ShowSearchListView();
        }

        private async void ShowSearchListView()
        {
            SearchListView.Controls.Clear();
            SearchListView.Items.Clear();

            var items = await new ScanDAL().GetSeedMagnetSearchModelAll();

            SearchListView.BeginUpdate();

            foreach (var item in items.OrderBy(x => x.StartTime))
            {
                ListViewItem lvi = new();
                ProgressBar pb = new();

                lvi.SubItems[0].Text = item.Name;
                lvi.SubItems.Add(item.MagUrlObj.Count + "");
                lvi.SubItems.Add("");
                lvi.SubItems.Add(item.DateStr);
                lvi.SubItems.Add(item.Id + "");
                lvi.Tag = item;

                SearchListView.Items.Add(lvi);

                Rectangle r = lvi.SubItems[2].Bounds;
                pb.SetBounds(r.X, r.Y, r.Width, r.Height);
                pb.Minimum = 1;
                pb.Maximum = 10;
                pb.Value = 10;
                pb.Name = item.Id + "";
                SearchListView.Controls.Add(pb);
            }

            SearchListView.EndUpdate();
        }
        #endregion

        #region 影片库相关
        private void VideoFirstBtn_Click(object sender, EventArgs e)
        {
            if (int.TryParse(VideoCurrentText.Text, out int current) && int.TryParse(VideoTotalText.Text, out int total))
            {
                if (total > 0)
                {
                    VideoCurrentText.Text = 1 + "";
                }
            }
        }

        private void VideoPreBtn_Click(object sender, EventArgs e)
        {
            if (int.TryParse(VideoCurrentText.Text, out int current) && int.TryParse(VideoTotalText.Text, out int total))
            {
                if (current - 1 >= 1)
                {
                    VideoCurrentText.Text = (current - 1) + "";
                }
            }
        }

        private async void VideoCurrentText_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(VideoCurrentText.Text, out int current) && int.TryParse(VideoTotalText.Text, out int total))
            {
                if (current >= 1 && current <= total)
                {
                    ShowVideos(await GetVideoModels(current, VideoPage, VideoOnlyExistCB.Checked));
                }
            }
        }

        private void VideoNextBtn_Click(object sender, EventArgs e)
        {
            if (int.TryParse(VideoCurrentText.Text, out int current) && int.TryParse(VideoTotalText.Text, out int total))
            {
                if (current + 1 <= total)
                {
                    VideoCurrentText.Text = (current + 1) + "";
                }
            }
        }

        private void VideoLastBtn_Click(object sender, EventArgs e)
        {
            if (int.TryParse(VideoCurrentText.Text, out int current) && int.TryParse(VideoTotalText.Text, out int total))
            {
                if (total > 0)
                {
                    VideoCurrentText.Text = total + "";
                }
            }
        }

        private async void VideoActressCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            VideoCategoryCombo.SelectedIndexChanged -= VideoCategoryCombo_SelectedIndexChanged;
            VideoPrefixCombo.SelectedIndexChanged -= VideoPrefixCombo_SelectedIndexChanged;
            VideoDirectorCombo.SelectedIndexChanged -= VideoDirectorCombo_SelectedIndexChanged;

            VideoCategoryCombo.SelectedIndex = 0;
            VideoPrefixCombo.SelectedIndex = 0;
            VideoDirectorCombo.SelectedIndex = 0;

            ShowVideos(await GetVideoModels(1, VideoPage, VideoOnlyExistCB.Checked));

            VideoCategoryCombo.SelectedIndexChanged += VideoCategoryCombo_SelectedIndexChanged;
            VideoPrefixCombo.SelectedIndexChanged += VideoPrefixCombo_SelectedIndexChanged;
            VideoDirectorCombo.SelectedIndexChanged += VideoDirectorCombo_SelectedIndexChanged;
        }

        private async void VideoCategoryCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            VideoActressCombo.SelectedIndexChanged -= VideoActressCombo_SelectedIndexChanged;
            VideoPrefixCombo.SelectedIndexChanged -= VideoPrefixCombo_SelectedIndexChanged;
            VideoDirectorCombo.SelectedIndexChanged -= VideoDirectorCombo_SelectedIndexChanged;

            VideoActressCombo.SelectedIndex = 0;
            VideoPrefixCombo.SelectedIndex = 0;
            VideoDirectorCombo.SelectedIndex = 0;

            ShowVideos(await GetVideoModels(1, VideoPage, VideoOnlyExistCB.Checked));

            VideoActressCombo.SelectedIndexChanged += VideoActressCombo_SelectedIndexChanged;
            VideoPrefixCombo.SelectedIndexChanged += VideoPrefixCombo_SelectedIndexChanged;
            VideoDirectorCombo.SelectedIndexChanged += VideoDirectorCombo_SelectedIndexChanged;
        }

        private async void VideoPrefixCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            VideoCategoryCombo.SelectedIndexChanged -= VideoCategoryCombo_SelectedIndexChanged;
            VideoActressCombo.SelectedIndexChanged -= VideoActressCombo_SelectedIndexChanged;
            VideoDirectorCombo.SelectedIndexChanged -= VideoDirectorCombo_SelectedIndexChanged;

            VideoCategoryCombo.SelectedIndex = 0;
            VideoActressCombo.SelectedIndex = 0;
            VideoDirectorCombo.SelectedIndex = 0;

            ShowVideos(await GetVideoModels(1, VideoPage, VideoOnlyExistCB.Checked));

            VideoCategoryCombo.SelectedIndexChanged += VideoCategoryCombo_SelectedIndexChanged;
            VideoActressCombo.SelectedIndexChanged += VideoActressCombo_SelectedIndexChanged;
            VideoDirectorCombo.SelectedIndexChanged += VideoDirectorCombo_SelectedIndexChanged;
        }

        private async void VideoDirectorCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            VideoCategoryCombo.SelectedIndexChanged -= VideoCategoryCombo_SelectedIndexChanged;
            VideoPrefixCombo.SelectedIndexChanged -= VideoPrefixCombo_SelectedIndexChanged;
            VideoActressCombo.SelectedIndexChanged -= VideoActressCombo_SelectedIndexChanged;

            VideoCategoryCombo.SelectedIndex = 0;
            VideoPrefixCombo.SelectedIndex = 0;
            VideoActressCombo.SelectedIndex = 0;

            ShowVideos(await GetVideoModels(1, VideoPage, VideoOnlyExistCB.Checked));

            VideoCategoryCombo.SelectedIndexChanged += VideoCategoryCombo_SelectedIndexChanged;
            VideoPrefixCombo.SelectedIndexChanged += VideoPrefixCombo_SelectedIndexChanged;
            VideoActressCombo.SelectedIndexChanged += VideoActressCombo_SelectedIndexChanged;
        }

        private void VideoListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void InitVideo()
        {
            if (UpdateVideoCombo)
            {
                var business = new JavLibraryDAL();
                var setting = await SettingService.GetSetting();

                VideoListView.Items.Clear();
                VideoTotalText.Text = 0 + "";
                VideoCurrentText.Text = 0 + "";
                VideoOnlyExistCB.Checked = true;

                var actress = await business.GetCommonJavLibraryModelByType(CommonModelType.Actress);
                var category = await business.GetCommonJavLibraryModelByType(CommonModelType.Category);
                var prefix = setting.Prefix.Split(',').ToList();
                var director = await business.GetCommonJavLibraryModelByType(CommonModelType.Director);

                var actressDS = actress.Select(x => x.Name).ToList();
                actressDS.Insert(0, "全部");

                var categoryDS = category.Select(x => x.Name).ToList();
                categoryDS.Insert(0, "全部");

                var prefixDS = prefix;
                prefixDS.Insert(0, "全部");

                var directorDS = director.Select(x => x.Name).ToList();
                directorDS.Insert(0, "全部");

                VideoActressCombo.SelectedIndexChanged -= VideoActressCombo_SelectedIndexChanged;
                VideoCategoryCombo.SelectedIndexChanged -= VideoCategoryCombo_SelectedIndexChanged;
                VideoPrefixCombo.SelectedIndexChanged -= VideoPrefixCombo_SelectedIndexChanged;
                VideoDirectorCombo.SelectedIndexChanged -= VideoDirectorCombo_SelectedIndexChanged;

                VideoActressCombo.BeginUpdate();
                VideoCategoryCombo.BeginUpdate();
                VideoPrefixCombo.BeginUpdate();
                VideoDirectorCombo.BeginUpdate();

                VideoActressCombo.DataSource = actressDS;
                VideoCategoryCombo.DataSource = categoryDS;
                VideoPrefixCombo.DataSource = prefixDS;
                VideoDirectorCombo.DataSource = directorDS;

                VideoActressCombo.EndUpdate();
                VideoCategoryCombo.EndUpdate();
                VideoPrefixCombo.EndUpdate();
                VideoDirectorCombo.EndUpdate();

                VideoActressCombo.SelectedIndexChanged += VideoActressCombo_SelectedIndexChanged;
                VideoCategoryCombo.SelectedIndexChanged += VideoCategoryCombo_SelectedIndexChanged;
                VideoPrefixCombo.SelectedIndexChanged += VideoPrefixCombo_SelectedIndexChanged;
                VideoDirectorCombo.SelectedIndexChanged += VideoDirectorCombo_SelectedIndexChanged;

                UpdateVideoCombo = false;
            }

            VideoPageSizeUpDown.Value = VideoPage;
        }

        private void VideoListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Clicks == 1 && VideoListView.SelectedItems.Count > 0)
            {
                GenerateVideoMenu(MousePosition);
            }
        }

        private void GenerateVideoMenu(Point mousePosition)
        {
            VideoMenu = new ContextMenuStrip();

            if (VideoListView.SelectedItems.Count > 0)
            {
                var model = (VideoModel)VideoListView.SelectedItems[0].Tag;

                if (model != null)
                {
                    ToolStripItem actressItem = new ToolStripMenuItem("看演员");
                    ToolStripItem categoryItem = new ToolStripMenuItem("看类型");
                    ToolStripItem prefixItem = new ToolStripMenuItem("看前缀");
                    ToolStripItem directorItem = new ToolStripMenuItem("看导演");
                    ToolStripItem seedItem = new ToolStripMenuItem("搜种子");
                    //ToolStripItem faviitem = new ToolStripMenuItem("添加收藏");
                    ToolStripItem openitem = new ToolStripMenuItem("打开网页");

                    var actress = model.AvModel.InfoObj.Where(x => x.Type == CommonModelType.Actress).ToList();
                    var category = model.AvModel.InfoObj.Where(x => x.Type == CommonModelType.Category).ToList();
                    var prefix = model.AvModel.Name.Split('-')[0].ToUpper();
                    var director = model.AvModel.InfoObj.Where(x => x.Type == CommonModelType.Director).ToList();

                    if (actress != null)
                    {
                        VideoMenu.Items.Add(actressItem);

                        if (actress.Count > 1)
                        {
                            foreach (var act in actress)
                            {
                                ToolStripItem temp = new ToolStripMenuItem(act.Name)
                                {
                                    Tag = actress.FirstOrDefault()
                                };
                                temp.Click += new EventHandler(ChangeActressCombo);

                                ((ToolStripDropDownItem)actressItem).DropDownItems.Add(temp);
                            }
                        }
                        else
                        {
                            actressItem.Tag = actress.FirstOrDefault();
                            actressItem.Click += new EventHandler(ChangeActressCombo);
                        }
                    }

                    if (category != null)
                    {
                        VideoMenu.Items.Add(categoryItem);

                        if (category.Count > 1)
                        {
                            foreach (var act in category)
                            {
                                ToolStripItem temp = new ToolStripMenuItem(act.Name)
                                {
                                    Tag = category.FirstOrDefault()
                                };
                                temp.Click += new EventHandler(ChangeCategoryCombo);

                                ((ToolStripDropDownItem)categoryItem).DropDownItems.Add(temp);
                            }
                        }
                        else
                        {
                            categoryItem.Tag = category.FirstOrDefault();
                            categoryItem.Click += new EventHandler(ChangeCategoryCombo);
                        }
                    }

                    if (director != null)
                    {
                        VideoMenu.Items.Add(directorItem);

                        if (director.Count > 1)
                        {
                            foreach (var act in director)
                            {
                                ToolStripItem temp = new ToolStripMenuItem(act.Name)
                                {
                                    Tag = director.FirstOrDefault()
                                };
                                temp.Click += new EventHandler(ChangeDirectorCombo);

                                ((ToolStripDropDownItem)directorItem).DropDownItems.Add(temp);
                            }
                        }
                        else
                        {
                            directorItem.Tag = director.FirstOrDefault();
                            directorItem.Click += new EventHandler(ChangeDirectorCombo);
                        }
                    }

                    if (!string.IsNullOrEmpty(prefix))
                    {
                        VideoMenu.Items.Add(prefixItem);
                        prefixItem.Tag = model.AvModel.AvId.Split('-')[0].ToUpper();
                        prefixItem.Click += new EventHandler(ChangePrefixCombo);
                    }

                    VideoMenu.Items.Add(seedItem);
                    seedItem.Tag = model.AvModel.AvId.ToUpper();
                    seedItem.Click += new EventHandler(SearchSeedMenu);

                    VideoMenu.Items.Add(openitem);
                    openitem.Tag = model.AvModel.Url;
                    openitem.Click += new EventHandler(OpenItemMenu);
                }
            }

            VideoMenu.Show(mousePosition.X, mousePosition.Y);
        }

        private void ChangeActressCombo(object sender, EventArgs e)
        {
            var model = (CommonModel)((ToolStripDropDownItem)sender).Tag;

            if (model != null)
            {
                VideoActressCombo.SelectedIndex = VideoActressCombo.Items.IndexOf(model.Name);
            }
        }

        private void ChangeCategoryCombo(object sender, EventArgs e)
        {
            var model = (CommonModel)((ToolStripDropDownItem)sender).Tag;

            if (model != null)
            {
                VideoCategoryCombo.SelectedIndex = VideoCategoryCombo.Items.IndexOf(model.Name);
            }
        }

        private void ChangePrefixCombo(object sender, EventArgs e)
        {
            var model = (CommonModel)((ToolStripDropDownItem)sender).Tag;

            if (model != null)
            {
                VideoPrefixCombo.SelectedIndex = VideoPrefixCombo.Items.IndexOf(model.Name);
            }
        }

        private void ChangeDirectorCombo(object sender, EventArgs e)
        {
            var model = (CommonModel)((ToolStripDropDownItem)sender).Tag;

            if (model != null)
            {
                VideoDirectorCombo.SelectedIndex = VideoDirectorCombo.Items.IndexOf(model.Name);
            }
        }

        private void SearchSeedMenu(object sender, EventArgs e)
        {

        }

        private void OpenItemMenu(object sender, EventArgs e)
        {
            var model = (string)((ToolStripDropDownItem)sender).Tag;

            if (!string.IsNullOrEmpty(model))
            {
                Process.Start(Win32Helper.GetExeLocation("Chrome.exe"), model);
            }
        }

        private async Task<List<VideoModel>> GetVideoModels(int page, int size, bool onlyExists)
        {
            Progress<(int, int)> progress = new();
            progress.ProgressChanged += VideoPbUpdate;

            List<VideoModel> ret = new();
            if (onlyExists)
            {
                CommonModelType modelType = CommonModelType.None;
                var modelName = "";

                if (VideoActressCombo.SelectedIndex > 0)
                {
                    modelType = CommonModelType.Actress;
                    modelName = (string)VideoActressCombo.SelectedItem;
                }

                if (VideoCategoryCombo.SelectedIndex > 0)
                {
                    modelType = CommonModelType.Category;
                    modelName = (string)VideoCategoryCombo.SelectedItem;
                }

                if (VideoPrefixCombo.SelectedIndex > 0)
                {
                    modelType = CommonModelType.Prefix;
                    modelName = (string)VideoPrefixCombo.SelectedItem;
                }

                if (VideoDirectorCombo.SelectedIndex > 0)
                {
                    modelType = CommonModelType.Director;
                    modelName = (string)VideoDirectorCombo.SelectedItem;
                }

                var res = await LocalService.GetVideoModel(onlyExists, page, size, modelType, modelName, progress);
                ret = res.Item1;

                UpdateVideoUINumber(page, res.Item2);
            }

            return ret;
        }

        private async void ShowVideos(List<VideoModel> videos)
        {
            ImageList.Images.Clear();
            VideoListView.Items.Clear();

            VideoListView.BeginUpdate();

            var setting = await SettingService.GetSetting();

            foreach (var video in videos)
            {
                var img = setting.JavLibraryImageFolder + video.AvModel.AvId + "-" + video.AvModel.Name + ".jpg";

                if (File.Exists(img))
                {
                    ImageList.Images.Add(video.AvModel.Id + "", Image.FromFile(img));
                }

                var sizeStr = (video.FileInfo != null && video.FileInfo.Any()) ? $"[{video.FileInfo[0].LengthStr}] " : "";
                ListViewItem lvi = new(sizeStr + video.AvModel.Name);
                lvi.Tag = video;
                lvi.ImageIndex = ImageList.Images.IndexOfKey(video.AvModel.Id + "");

                VideoListView.Items.Add(lvi);
            }

            VideoListView.EndUpdate();
        }

        private void UpdateVideoUINumber(int current, int total)
        {
            VideoCurrentText.Text = current + "";
            VideoTotalText.Text = total + "";
        }

        private void VideoClearRedisBtn_Click(object sender, EventArgs e)
        {
            var ret = MessageBox.Show("是否要清理Redis缓存，会导致下次加载变慢?", "警告", MessageBoxButtons.YesNo);

            if (ret == DialogResult.Yes)
            {
                if (RedisService.HExists("video", "all"))
                {
                    RedisService.DeleteHash("video", "all");
                }

                UpdateVideoCombo = true;
            }
        }

        private void VideoPageSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            VideoPage = (int)VideoPageSizeUpDown.Value;
        }

        private void VideoListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (VideoListView.SelectedItems.Count > 0)
            {
                var file = ((VideoModel)VideoListView.SelectedItems[0].Tag).FileInfo.FirstOrDefault();

                if (file.IsRemote == false)
                {
                    Process.Start(Setting.POTPLAYEREXEFILELOCATION, file.FullName);

                    SettingService.InsertPlayHistory(new PlayHistory()
                    {
                        FileName = file.Name,
                        LastPlay = DateTime.Now,
                        PlayTimes = 1,
                        SetNotPlayed = false
                    });
                }
            }
        }

        private void VideoPbUpdate(object sender, (int, int) e)
        {
            VideoProgressBar.Maximum = e.Item2;
            JDuBar(VideoProgressBar, e.Item1);
        }
        #endregion

        #region 设置
        private async void SettingCookieCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Progress<string> progress = new();
            var setting = await SettingService.GetSetting();

            var enumValue = (JavLibraryGetCookieMode)Enum.Parse(typeof(JavLibraryGetCookieMode), SettingCookieCombo.Text);
            setting.JavLibrarySettings.CookieMode = enumValue;

            await SettingService.SaveSetting(setting, progress);

            InitSetting();
        }

        private async void SettingMagnetSiteCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Progress<string> progress = new();
            var setting = await SettingService.GetSetting();

            var enumValue = (SearchSeedSiteEnum)Enum.Parse(typeof(SearchSeedSiteEnum), SettingMagnetSiteCombo.Text);
            setting.MagSearchSite = enumValue;

            await SettingService.SaveSetting(setting, progress);

            InitSetting();
        }

        private async void SettingSaveFaviBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SettingPreText.Text))
            {
                var res = await MagnetUrlService.GetFaviUrl(SettingPreText.Text);

                await MagnetUrlService.SaveFaviUrl(res);

                InitSetting();
            }
        }

        private async void SettingSavePrefixBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SettingPreText.Text))
            {
                Progress<string> progress = new();
                var setting = await SettingService.GetSetting();

                setting.Prefix += "," + SettingPreText.Text;

                await SettingService.SaveSetting(setting, progress);

                InitSetting();
            }
        }

        private async void SettingSaveBarkBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SettingBarkText.Text))
            {
                Progress<string> progress = new();
                var setting = await SettingService.GetSetting();

                setting.BarkId = SettingBarkText.Text;

                await SettingService.SaveSetting(setting, progress);

                InitSetting();
            }
        }

        private async void InitSetting()
        {
            var setting = await SettingService.GetSetting();

            SettingPreText.Text = "";
            SettingFaviText.Text = "";

            SettingCookieCombo.SelectedIndexChanged -= SettingCookieCombo_SelectedIndexChanged;
            SettingMagnetSiteCombo.SelectedIndexChanged -= SettingMagnetSiteCombo_SelectedIndexChanged;

            SettingCookieCombo.Text = setting.JavLibrarySettings.CookieMode.ToString();
            SettingMagnetSiteCombo.Text = setting.MagSearchSite.ToString();
            SettingBarkText.Text = setting.BarkId;

            SettingCookieCombo.SelectedIndexChanged += SettingCookieCombo_SelectedIndexChanged;
            SettingMagnetSiteCombo.SelectedIndexChanged += SettingMagnetSiteCombo_SelectedIndexChanged;
        }
        #endregion
    }
}
