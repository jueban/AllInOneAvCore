
namespace AvManager
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.NotifyIconExitMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.RemoveFolderTab = new System.Windows.Forms.TabPage();
            this.RemoveFolderMainPanel = new System.Windows.Forms.Panel();
            this.RemoveFolderInfoText = new System.Windows.Forms.TextBox();
            this.RemoveFolderUpperPanel = new System.Windows.Forms.Panel();
            this.RemoveFolderConfirmBtn = new System.Windows.Forms.Button();
            this.RemoveFolderText = new System.Windows.Forms.TextBox();
            this.RenameTab = new System.Windows.Forms.TabPage();
            this.RenameMainPanel = new System.Windows.Forms.Panel();
            this.RenameInfoText = new System.Windows.Forms.TextBox();
            this.RenameUpperPanel = new System.Windows.Forms.Panel();
            this.RenameManualBtn = new System.Windows.Forms.Button();
            this.RenameBtn = new System.Windows.Forms.Button();
            this.RenameText = new System.Windows.Forms.TextBox();
            this.CombinePrepareTab = new System.Windows.Forms.TabPage();
            this.CombinePrepareMainPanel = new System.Windows.Forms.Panel();
            this.CombinePrepareMainMainPanel = new System.Windows.Forms.Panel();
            this.CombinePrepareTree = new System.Windows.Forms.TreeView();
            this.CombinePrepareMainBottomPanel = new System.Windows.Forms.Panel();
            this.CombinePreparePB = new System.Windows.Forms.ProgressBar();
            this.CombinePrepareUpperPanel = new System.Windows.Forms.Panel();
            this.CombinePrepareGenerateBtn = new System.Windows.Forms.Button();
            this.CombinePrepareClearBtn = new System.Windows.Forms.Button();
            this.CombinePrepareListBtn = new System.Windows.Forms.Button();
            this.CombinePrepareCB = new System.Windows.Forms.CheckBox();
            this.CombinePrepareText = new System.Windows.Forms.TextBox();
            this.AutoCombine = new System.Windows.Forms.TabPage();
            this.AutoCombineMainPanel = new System.Windows.Forms.Panel();
            this.AutoCombineMainMainPanel = new System.Windows.Forms.Panel();
            this.AutoCombineListView = new System.Windows.Forms.ListView();
            this.AutoCombineListViewName = new System.Windows.Forms.ColumnHeader();
            this.AutoCombineListViewSize = new System.Windows.Forms.ColumnHeader();
            this.AutoCombineMainRightPanel = new System.Windows.Forms.Panel();
            this.AutoCombineInfoText = new System.Windows.Forms.TextBox();
            this.AutoCombineBottomPanel = new System.Windows.Forms.Panel();
            this.AutoCombineCurrentPB = new System.Windows.Forms.ProgressBar();
            this.AutoCombineDeleteBtn = new System.Windows.Forms.Button();
            this.AutoCombineTotalPB = new System.Windows.Forms.ProgressBar();
            this.AutoCombineUpperPanel = new System.Windows.Forms.Panel();
            this.AutoCombineCancelBtn = new System.Windows.Forms.Button();
            this.AutoCombineStartBtn = new System.Windows.Forms.Button();
            this.AutoCombineSaveText = new System.Windows.Forms.TextBox();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.CombinePrepareTreeNodeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.截图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NotifyIconExitMenu.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.RemoveFolderTab.SuspendLayout();
            this.RemoveFolderMainPanel.SuspendLayout();
            this.RemoveFolderUpperPanel.SuspendLayout();
            this.RenameTab.SuspendLayout();
            this.RenameMainPanel.SuspendLayout();
            this.RenameUpperPanel.SuspendLayout();
            this.CombinePrepareTab.SuspendLayout();
            this.CombinePrepareMainPanel.SuspendLayout();
            this.CombinePrepareMainMainPanel.SuspendLayout();
            this.CombinePrepareMainBottomPanel.SuspendLayout();
            this.CombinePrepareUpperPanel.SuspendLayout();
            this.AutoCombine.SuspendLayout();
            this.AutoCombineMainPanel.SuspendLayout();
            this.AutoCombineMainMainPanel.SuspendLayout();
            this.AutoCombineMainRightPanel.SuspendLayout();
            this.AutoCombineBottomPanel.SuspendLayout();
            this.AutoCombineUpperPanel.SuspendLayout();
            this.CombinePrepareTreeNodeMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.ContextMenuStrip = this.NotifyIconExitMenu;
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "notifyIcon";
            this.NotifyIcon.Visible = true;
            this.NotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseClick);
            this.NotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // NotifyIconExitMenu
            // 
            this.NotifyIconExitMenu.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.NotifyIconExitMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Exit});
            this.NotifyIconExitMenu.Name = "notifyIconExitMenu";
            this.NotifyIconExitMenu.Size = new System.Drawing.Size(127, 38);
            // 
            // Exit
            // 
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(126, 34);
            this.Exit.Text = "退出";
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.TabControl);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(1486, 741);
            this.MainPanel.TabIndex = 1;
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.RemoveFolderTab);
            this.TabControl.Controls.Add(this.RenameTab);
            this.TabControl.Controls.Add(this.CombinePrepareTab);
            this.TabControl.Controls.Add(this.AutoCombine);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Location = new System.Drawing.Point(0, 0);
            this.TabControl.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(1486, 741);
            this.TabControl.TabIndex = 0;
            this.TabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // RemoveFolderTab
            // 
            this.RemoveFolderTab.Controls.Add(this.RemoveFolderMainPanel);
            this.RemoveFolderTab.Controls.Add(this.RemoveFolderUpperPanel);
            this.RemoveFolderTab.Location = new System.Drawing.Point(4, 37);
            this.RemoveFolderTab.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.RemoveFolderTab.Name = "RemoveFolderTab";
            this.RemoveFolderTab.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.RemoveFolderTab.Size = new System.Drawing.Size(1478, 700);
            this.RemoveFolderTab.TabIndex = 0;
            this.RemoveFolderTab.Text = "去文件夹";
            this.RemoveFolderTab.UseVisualStyleBackColor = true;
            // 
            // RemoveFolderMainPanel
            // 
            this.RemoveFolderMainPanel.Controls.Add(this.RemoveFolderInfoText);
            this.RemoveFolderMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoveFolderMainPanel.Location = new System.Drawing.Point(6, 64);
            this.RemoveFolderMainPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.RemoveFolderMainPanel.Name = "RemoveFolderMainPanel";
            this.RemoveFolderMainPanel.Size = new System.Drawing.Size(1466, 631);
            this.RemoveFolderMainPanel.TabIndex = 1;
            // 
            // RemoveFolderInfoText
            // 
            this.RemoveFolderInfoText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoveFolderInfoText.Location = new System.Drawing.Point(0, 0);
            this.RemoveFolderInfoText.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.RemoveFolderInfoText.Multiline = true;
            this.RemoveFolderInfoText.Name = "RemoveFolderInfoText";
            this.RemoveFolderInfoText.ReadOnly = true;
            this.RemoveFolderInfoText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.RemoveFolderInfoText.Size = new System.Drawing.Size(1466, 631);
            this.RemoveFolderInfoText.TabIndex = 0;
            // 
            // RemoveFolderUpperPanel
            // 
            this.RemoveFolderUpperPanel.Controls.Add(this.RemoveFolderConfirmBtn);
            this.RemoveFolderUpperPanel.Controls.Add(this.RemoveFolderText);
            this.RemoveFolderUpperPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.RemoveFolderUpperPanel.Location = new System.Drawing.Point(6, 5);
            this.RemoveFolderUpperPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.RemoveFolderUpperPanel.Name = "RemoveFolderUpperPanel";
            this.RemoveFolderUpperPanel.Size = new System.Drawing.Size(1466, 59);
            this.RemoveFolderUpperPanel.TabIndex = 0;
            // 
            // RemoveFolderConfirmBtn
            // 
            this.RemoveFolderConfirmBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveFolderConfirmBtn.Location = new System.Drawing.Point(1317, 8);
            this.RemoveFolderConfirmBtn.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.RemoveFolderConfirmBtn.Name = "RemoveFolderConfirmBtn";
            this.RemoveFolderConfirmBtn.Size = new System.Drawing.Size(139, 44);
            this.RemoveFolderConfirmBtn.TabIndex = 1;
            this.RemoveFolderConfirmBtn.Text = "确定";
            this.RemoveFolderConfirmBtn.UseVisualStyleBackColor = true;
            this.RemoveFolderConfirmBtn.Click += new System.EventHandler(this.RemoveFolderConfirmBtn_Click);
            // 
            // RemoveFolderText
            // 
            this.RemoveFolderText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveFolderText.Location = new System.Drawing.Point(6, 13);
            this.RemoveFolderText.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.RemoveFolderText.Name = "RemoveFolderText";
            this.RemoveFolderText.PlaceholderText = "点击选择需要去文件夹的目录，文件会移动到所选目录下的\'movefiles\'文件夹";
            this.RemoveFolderText.Size = new System.Drawing.Size(1293, 34);
            this.RemoveFolderText.TabIndex = 0;
            this.RemoveFolderText.Click += new System.EventHandler(this.RemoveFolderText_Click);
            // 
            // RenameTab
            // 
            this.RenameTab.Controls.Add(this.RenameMainPanel);
            this.RenameTab.Controls.Add(this.RenameUpperPanel);
            this.RenameTab.Location = new System.Drawing.Point(4, 37);
            this.RenameTab.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.RenameTab.Name = "RenameTab";
            this.RenameTab.Size = new System.Drawing.Size(1478, 700);
            this.RenameTab.TabIndex = 2;
            this.RenameTab.Text = "重命名";
            this.RenameTab.UseVisualStyleBackColor = true;
            // 
            // RenameMainPanel
            // 
            this.RenameMainPanel.Controls.Add(this.RenameInfoText);
            this.RenameMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenameMainPanel.Location = new System.Drawing.Point(0, 59);
            this.RenameMainPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.RenameMainPanel.Name = "RenameMainPanel";
            this.RenameMainPanel.Size = new System.Drawing.Size(1478, 641);
            this.RenameMainPanel.TabIndex = 1;
            // 
            // RenameInfoText
            // 
            this.RenameInfoText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenameInfoText.Location = new System.Drawing.Point(0, 0);
            this.RenameInfoText.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.RenameInfoText.Multiline = true;
            this.RenameInfoText.Name = "RenameInfoText";
            this.RenameInfoText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.RenameInfoText.Size = new System.Drawing.Size(1478, 641);
            this.RenameInfoText.TabIndex = 0;
            // 
            // RenameUpperPanel
            // 
            this.RenameUpperPanel.Controls.Add(this.RenameManualBtn);
            this.RenameUpperPanel.Controls.Add(this.RenameBtn);
            this.RenameUpperPanel.Controls.Add(this.RenameText);
            this.RenameUpperPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.RenameUpperPanel.Location = new System.Drawing.Point(0, 0);
            this.RenameUpperPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.RenameUpperPanel.Name = "RenameUpperPanel";
            this.RenameUpperPanel.Size = new System.Drawing.Size(1478, 59);
            this.RenameUpperPanel.TabIndex = 0;
            // 
            // RenameManualBtn
            // 
            this.RenameManualBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RenameManualBtn.Location = new System.Drawing.Point(1324, 8);
            this.RenameManualBtn.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.RenameManualBtn.Name = "RenameManualBtn";
            this.RenameManualBtn.Size = new System.Drawing.Size(139, 43);
            this.RenameManualBtn.TabIndex = 2;
            this.RenameManualBtn.Text = "手动重名";
            this.RenameManualBtn.UseVisualStyleBackColor = true;
            this.RenameManualBtn.Click += new System.EventHandler(this.RenameManualBtn_Click);
            // 
            // RenameBtn
            // 
            this.RenameBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RenameBtn.Location = new System.Drawing.Point(1173, 8);
            this.RenameBtn.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.RenameBtn.Name = "RenameBtn";
            this.RenameBtn.Size = new System.Drawing.Size(139, 43);
            this.RenameBtn.TabIndex = 1;
            this.RenameBtn.Text = "自动重名";
            this.RenameBtn.UseVisualStyleBackColor = true;
            this.RenameBtn.Click += new System.EventHandler(this.RenameBtn_Click);
            // 
            // RenameText
            // 
            this.RenameText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RenameText.Location = new System.Drawing.Point(6, 12);
            this.RenameText.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.RenameText.Name = "RenameText";
            this.RenameText.PlaceholderText = "点击选择需要重命名的目录，唯一确定的放入Fin，需要手动处理的放入TempFin";
            this.RenameText.Size = new System.Drawing.Size(1144, 34);
            this.RenameText.TabIndex = 0;
            this.RenameText.Click += new System.EventHandler(this.RenameText_Click);
            // 
            // CombinePrepareTab
            // 
            this.CombinePrepareTab.Controls.Add(this.CombinePrepareMainPanel);
            this.CombinePrepareTab.Controls.Add(this.CombinePrepareUpperPanel);
            this.CombinePrepareTab.Location = new System.Drawing.Point(4, 37);
            this.CombinePrepareTab.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.CombinePrepareTab.Name = "CombinePrepareTab";
            this.CombinePrepareTab.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.CombinePrepareTab.Size = new System.Drawing.Size(1478, 700);
            this.CombinePrepareTab.TabIndex = 1;
            this.CombinePrepareTab.Text = "合并准备";
            this.CombinePrepareTab.UseVisualStyleBackColor = true;
            // 
            // CombinePrepareMainPanel
            // 
            this.CombinePrepareMainPanel.Controls.Add(this.CombinePrepareMainMainPanel);
            this.CombinePrepareMainPanel.Controls.Add(this.CombinePrepareMainBottomPanel);
            this.CombinePrepareMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CombinePrepareMainPanel.Location = new System.Drawing.Point(6, 64);
            this.CombinePrepareMainPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.CombinePrepareMainPanel.Name = "CombinePrepareMainPanel";
            this.CombinePrepareMainPanel.Size = new System.Drawing.Size(1466, 631);
            this.CombinePrepareMainPanel.TabIndex = 1;
            // 
            // CombinePrepareMainMainPanel
            // 
            this.CombinePrepareMainMainPanel.Controls.Add(this.CombinePrepareTree);
            this.CombinePrepareMainMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CombinePrepareMainMainPanel.Location = new System.Drawing.Point(0, 0);
            this.CombinePrepareMainMainPanel.Name = "CombinePrepareMainMainPanel";
            this.CombinePrepareMainMainPanel.Size = new System.Drawing.Size(1466, 572);
            this.CombinePrepareMainMainPanel.TabIndex = 2;
            // 
            // CombinePrepareTree
            // 
            this.CombinePrepareTree.CheckBoxes = true;
            this.CombinePrepareTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CombinePrepareTree.FullRowSelect = true;
            this.CombinePrepareTree.LabelEdit = true;
            this.CombinePrepareTree.Location = new System.Drawing.Point(0, 0);
            this.CombinePrepareTree.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.CombinePrepareTree.Name = "CombinePrepareTree";
            this.CombinePrepareTree.Size = new System.Drawing.Size(1466, 572);
            this.CombinePrepareTree.TabIndex = 0;
            this.CombinePrepareTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.CombinePrepareTree_AfterLabelEdit);
            this.CombinePrepareTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.CombinePrepareTree_AfterCheck);
            this.CombinePrepareTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.CombinePrepareTree_NodeMouseClick);
            this.CombinePrepareTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.CombinePrepareTree_NodeMouseDoubleClick);
            // 
            // CombinePrepareMainBottomPanel
            // 
            this.CombinePrepareMainBottomPanel.Controls.Add(this.CombinePreparePB);
            this.CombinePrepareMainBottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CombinePrepareMainBottomPanel.Location = new System.Drawing.Point(0, 572);
            this.CombinePrepareMainBottomPanel.Name = "CombinePrepareMainBottomPanel";
            this.CombinePrepareMainBottomPanel.Size = new System.Drawing.Size(1466, 59);
            this.CombinePrepareMainBottomPanel.TabIndex = 1;
            // 
            // CombinePreparePB
            // 
            this.CombinePreparePB.Location = new System.Drawing.Point(6, 10);
            this.CombinePreparePB.Name = "CombinePreparePB";
            this.CombinePreparePB.Size = new System.Drawing.Size(1457, 40);
            this.CombinePreparePB.TabIndex = 0;
            // 
            // CombinePrepareUpperPanel
            // 
            this.CombinePrepareUpperPanel.Controls.Add(this.CombinePrepareGenerateBtn);
            this.CombinePrepareUpperPanel.Controls.Add(this.CombinePrepareCB);
            this.CombinePrepareUpperPanel.Controls.Add(this.CombinePrepareClearBtn);
            this.CombinePrepareUpperPanel.Controls.Add(this.CombinePrepareListBtn);
            this.CombinePrepareUpperPanel.Controls.Add(this.CombinePrepareText);
            this.CombinePrepareUpperPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CombinePrepareUpperPanel.Location = new System.Drawing.Point(6, 5);
            this.CombinePrepareUpperPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.CombinePrepareUpperPanel.Name = "CombinePrepareUpperPanel";
            this.CombinePrepareUpperPanel.Size = new System.Drawing.Size(1466, 59);
            this.CombinePrepareUpperPanel.TabIndex = 0;
            // 
            // CombinePrepareGenerateBtn
            // 
            this.CombinePrepareGenerateBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CombinePrepareGenerateBtn.Location = new System.Drawing.Point(1314, 5);
            this.CombinePrepareGenerateBtn.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.CombinePrepareGenerateBtn.Name = "CombinePrepareGenerateBtn";
            this.CombinePrepareGenerateBtn.Size = new System.Drawing.Size(139, 44);
            this.CombinePrepareGenerateBtn.TabIndex = 2;
            this.CombinePrepareGenerateBtn.Text = "生成";
            this.CombinePrepareGenerateBtn.UseVisualStyleBackColor = true;
            this.CombinePrepareGenerateBtn.Click += new System.EventHandler(this.CombinePrepareGenerateBtn_Click);
            // 
            // CombinePrepareClearBtn
            // 
            this.CombinePrepareClearBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CombinePrepareClearBtn.Location = new System.Drawing.Point(1163, 5);
            this.CombinePrepareClearBtn.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.CombinePrepareClearBtn.Name = "CombinePrepareClearBtn";
            this.CombinePrepareClearBtn.Size = new System.Drawing.Size(139, 44);
            this.CombinePrepareClearBtn.TabIndex = 1;
            this.CombinePrepareClearBtn.Text = "整理";
            this.CombinePrepareClearBtn.UseVisualStyleBackColor = true;
            this.CombinePrepareClearBtn.Click += new System.EventHandler(this.CombinePrepareClearBtn_Click);
            // 
            // CombinePrepareListBtn
            // 
            this.CombinePrepareListBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CombinePrepareListBtn.Location = new System.Drawing.Point(1013, 5);
            this.CombinePrepareListBtn.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.CombinePrepareListBtn.Name = "CombinePrepareListBtn";
            this.CombinePrepareListBtn.Size = new System.Drawing.Size(139, 44);
            this.CombinePrepareListBtn.TabIndex = 0;
            this.CombinePrepareListBtn.Text = "展示";
            this.CombinePrepareListBtn.UseVisualStyleBackColor = true;
            this.CombinePrepareListBtn.Click += new System.EventHandler(this.CombinePrepareListBtn_Click);
            // 
            // CombinePrepareCB
            // 
            this.CombinePrepareCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CombinePrepareCB.AutoSize = true;
            this.CombinePrepareCB.Location = new System.Drawing.Point(912, 12);
            this.CombinePrepareCB.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.CombinePrepareCB.Name = "CombinePrepareCB";
            this.CombinePrepareCB.Size = new System.Drawing.Size(80, 32);
            this.CombinePrepareCB.TabIndex = 1;
            this.CombinePrepareCB.Text = "截屏";
            this.CombinePrepareCB.UseVisualStyleBackColor = true;
            // 
            // CombinePrepareText
            // 
            this.CombinePrepareText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CombinePrepareText.Location = new System.Drawing.Point(6, 10);
            this.CombinePrepareText.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.CombinePrepareText.Name = "CombinePrepareText";
            this.CombinePrepareText.PlaceholderText = "点击选择合并文件所在的目录，勾选为删除，双击重命名,点击‘整理’完成重命名及删除";
            this.CombinePrepareText.Size = new System.Drawing.Size(894, 34);
            this.CombinePrepareText.TabIndex = 0;
            this.CombinePrepareText.Click += new System.EventHandler(this.CombinePrepareText_Click);
            // 
            // AutoCombine
            // 
            this.AutoCombine.Controls.Add(this.AutoCombineMainPanel);
            this.AutoCombine.Controls.Add(this.AutoCombineBottomPanel);
            this.AutoCombine.Controls.Add(this.AutoCombineUpperPanel);
            this.AutoCombine.Location = new System.Drawing.Point(4, 37);
            this.AutoCombine.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AutoCombine.Name = "AutoCombine";
            this.AutoCombine.Size = new System.Drawing.Size(1478, 700);
            this.AutoCombine.TabIndex = 3;
            this.AutoCombine.Text = "自动合并";
            this.AutoCombine.UseVisualStyleBackColor = true;
            // 
            // AutoCombineMainPanel
            // 
            this.AutoCombineMainPanel.Controls.Add(this.AutoCombineMainMainPanel);
            this.AutoCombineMainPanel.Controls.Add(this.AutoCombineMainRightPanel);
            this.AutoCombineMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoCombineMainPanel.Location = new System.Drawing.Point(0, 59);
            this.AutoCombineMainPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AutoCombineMainPanel.Name = "AutoCombineMainPanel";
            this.AutoCombineMainPanel.Size = new System.Drawing.Size(1478, 582);
            this.AutoCombineMainPanel.TabIndex = 2;
            // 
            // AutoCombineMainMainPanel
            // 
            this.AutoCombineMainMainPanel.Controls.Add(this.AutoCombineListView);
            this.AutoCombineMainMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoCombineMainMainPanel.Location = new System.Drawing.Point(0, 0);
            this.AutoCombineMainMainPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AutoCombineMainMainPanel.Name = "AutoCombineMainMainPanel";
            this.AutoCombineMainMainPanel.Size = new System.Drawing.Size(1107, 582);
            this.AutoCombineMainMainPanel.TabIndex = 2;
            // 
            // AutoCombineListView
            // 
            this.AutoCombineListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.AutoCombineListViewName,
            this.AutoCombineListViewSize});
            this.AutoCombineListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoCombineListView.FullRowSelect = true;
            this.AutoCombineListView.HideSelection = false;
            this.AutoCombineListView.Location = new System.Drawing.Point(0, 0);
            this.AutoCombineListView.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AutoCombineListView.Name = "AutoCombineListView";
            this.AutoCombineListView.Size = new System.Drawing.Size(1107, 582);
            this.AutoCombineListView.TabIndex = 0;
            this.AutoCombineListView.UseCompatibleStateImageBehavior = false;
            this.AutoCombineListView.View = System.Windows.Forms.View.Details;
            // 
            // AutoCombineListViewName
            // 
            this.AutoCombineListViewName.Text = "名称";
            this.AutoCombineListViewName.Width = 500;
            // 
            // AutoCombineListViewSize
            // 
            this.AutoCombineListViewSize.Text = "大小";
            this.AutoCombineListViewSize.Width = 80;
            // 
            // AutoCombineMainRightPanel
            // 
            this.AutoCombineMainRightPanel.Controls.Add(this.AutoCombineInfoText);
            this.AutoCombineMainRightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.AutoCombineMainRightPanel.Location = new System.Drawing.Point(1107, 0);
            this.AutoCombineMainRightPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AutoCombineMainRightPanel.Name = "AutoCombineMainRightPanel";
            this.AutoCombineMainRightPanel.Size = new System.Drawing.Size(371, 582);
            this.AutoCombineMainRightPanel.TabIndex = 1;
            // 
            // AutoCombineInfoText
            // 
            this.AutoCombineInfoText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoCombineInfoText.Location = new System.Drawing.Point(0, 0);
            this.AutoCombineInfoText.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AutoCombineInfoText.Multiline = true;
            this.AutoCombineInfoText.Name = "AutoCombineInfoText";
            this.AutoCombineInfoText.Size = new System.Drawing.Size(371, 582);
            this.AutoCombineInfoText.TabIndex = 0;
            // 
            // AutoCombineBottomPanel
            // 
            this.AutoCombineBottomPanel.Controls.Add(this.AutoCombineCurrentPB);
            this.AutoCombineBottomPanel.Controls.Add(this.AutoCombineDeleteBtn);
            this.AutoCombineBottomPanel.Controls.Add(this.AutoCombineTotalPB);
            this.AutoCombineBottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.AutoCombineBottomPanel.Location = new System.Drawing.Point(0, 641);
            this.AutoCombineBottomPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AutoCombineBottomPanel.Name = "AutoCombineBottomPanel";
            this.AutoCombineBottomPanel.Size = new System.Drawing.Size(1478, 59);
            this.AutoCombineBottomPanel.TabIndex = 1;
            // 
            // AutoCombineCurrentPB
            // 
            this.AutoCombineCurrentPB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AutoCombineCurrentPB.Location = new System.Drawing.Point(17, 7);
            this.AutoCombineCurrentPB.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AutoCombineCurrentPB.Name = "AutoCombineCurrentPB";
            this.AutoCombineCurrentPB.Size = new System.Drawing.Size(1296, 21);
            this.AutoCombineCurrentPB.TabIndex = 3;
            // 
            // AutoCombineDeleteBtn
            // 
            this.AutoCombineDeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AutoCombineDeleteBtn.Location = new System.Drawing.Point(1324, 10);
            this.AutoCombineDeleteBtn.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AutoCombineDeleteBtn.Name = "AutoCombineDeleteBtn";
            this.AutoCombineDeleteBtn.Size = new System.Drawing.Size(139, 44);
            this.AutoCombineDeleteBtn.TabIndex = 2;
            this.AutoCombineDeleteBtn.Text = "删除";
            this.AutoCombineDeleteBtn.UseVisualStyleBackColor = true;
            this.AutoCombineDeleteBtn.Click += new System.EventHandler(this.AutoCombineDeleteBtn_Click);
            // 
            // AutoCombineTotalPB
            // 
            this.AutoCombineTotalPB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AutoCombineTotalPB.Location = new System.Drawing.Point(17, 31);
            this.AutoCombineTotalPB.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AutoCombineTotalPB.Name = "AutoCombineTotalPB";
            this.AutoCombineTotalPB.Size = new System.Drawing.Size(1296, 21);
            this.AutoCombineTotalPB.TabIndex = 0;
            // 
            // AutoCombineUpperPanel
            // 
            this.AutoCombineUpperPanel.Controls.Add(this.AutoCombineCancelBtn);
            this.AutoCombineUpperPanel.Controls.Add(this.AutoCombineStartBtn);
            this.AutoCombineUpperPanel.Controls.Add(this.AutoCombineSaveText);
            this.AutoCombineUpperPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.AutoCombineUpperPanel.Location = new System.Drawing.Point(0, 0);
            this.AutoCombineUpperPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AutoCombineUpperPanel.Name = "AutoCombineUpperPanel";
            this.AutoCombineUpperPanel.Size = new System.Drawing.Size(1478, 59);
            this.AutoCombineUpperPanel.TabIndex = 0;
            // 
            // AutoCombineCancelBtn
            // 
            this.AutoCombineCancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AutoCombineCancelBtn.Location = new System.Drawing.Point(1324, 8);
            this.AutoCombineCancelBtn.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AutoCombineCancelBtn.Name = "AutoCombineCancelBtn";
            this.AutoCombineCancelBtn.Size = new System.Drawing.Size(139, 44);
            this.AutoCombineCancelBtn.TabIndex = 2;
            this.AutoCombineCancelBtn.Text = "取消";
            this.AutoCombineCancelBtn.UseVisualStyleBackColor = true;
            this.AutoCombineCancelBtn.Click += new System.EventHandler(this.AutoCombineCancelBtn_Click);
            // 
            // AutoCombineStartBtn
            // 
            this.AutoCombineStartBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AutoCombineStartBtn.Location = new System.Drawing.Point(1163, 8);
            this.AutoCombineStartBtn.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AutoCombineStartBtn.Name = "AutoCombineStartBtn";
            this.AutoCombineStartBtn.Size = new System.Drawing.Size(139, 44);
            this.AutoCombineStartBtn.TabIndex = 1;
            this.AutoCombineStartBtn.Text = "开始";
            this.AutoCombineStartBtn.UseVisualStyleBackColor = true;
            this.AutoCombineStartBtn.Click += new System.EventHandler(this.AutoCombineStartBtn_Click);
            // 
            // AutoCombineSaveText
            // 
            this.AutoCombineSaveText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AutoCombineSaveText.Location = new System.Drawing.Point(6, 13);
            this.AutoCombineSaveText.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AutoCombineSaveText.Name = "AutoCombineSaveText";
            this.AutoCombineSaveText.PlaceholderText = "点击选择合并后文件保存目录，自动读取AutoCombie目录中合并文件";
            this.AutoCombineSaveText.Size = new System.Drawing.Size(1145, 34);
            this.AutoCombineSaveText.TabIndex = 0;
            this.AutoCombineSaveText.Click += new System.EventHandler(this.AutoCombineSaveText_Click);
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.FileName = "openFileDialog1";
            // 
            // CombinePrepareTreeNodeMenu
            // 
            this.CombinePrepareTreeNodeMenu.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.CombinePrepareTreeNodeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.截图ToolStripMenuItem});
            this.CombinePrepareTreeNodeMenu.Name = "CombinePrepareTreeNodeMenu";
            this.CombinePrepareTreeNodeMenu.Size = new System.Drawing.Size(127, 38);
            // 
            // 截图ToolStripMenuItem
            // 
            this.截图ToolStripMenuItem.Name = "截图ToolStripMenuItem";
            this.截图ToolStripMenuItem.Size = new System.Drawing.Size(126, 34);
            this.截图ToolStripMenuItem.Text = "截图";
            this.截图ToolStripMenuItem.Click += new System.EventHandler(this.截图ToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1486, 741);
            this.Controls.Add(this.MainPanel);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AvManager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.NotifyIconExitMenu.ResumeLayout(false);
            this.MainPanel.ResumeLayout(false);
            this.TabControl.ResumeLayout(false);
            this.RemoveFolderTab.ResumeLayout(false);
            this.RemoveFolderMainPanel.ResumeLayout(false);
            this.RemoveFolderMainPanel.PerformLayout();
            this.RemoveFolderUpperPanel.ResumeLayout(false);
            this.RemoveFolderUpperPanel.PerformLayout();
            this.RenameTab.ResumeLayout(false);
            this.RenameMainPanel.ResumeLayout(false);
            this.RenameMainPanel.PerformLayout();
            this.RenameUpperPanel.ResumeLayout(false);
            this.RenameUpperPanel.PerformLayout();
            this.CombinePrepareTab.ResumeLayout(false);
            this.CombinePrepareMainPanel.ResumeLayout(false);
            this.CombinePrepareMainMainPanel.ResumeLayout(false);
            this.CombinePrepareMainBottomPanel.ResumeLayout(false);
            this.CombinePrepareUpperPanel.ResumeLayout(false);
            this.CombinePrepareUpperPanel.PerformLayout();
            this.AutoCombine.ResumeLayout(false);
            this.AutoCombineMainPanel.ResumeLayout(false);
            this.AutoCombineMainMainPanel.ResumeLayout(false);
            this.AutoCombineMainRightPanel.ResumeLayout(false);
            this.AutoCombineMainRightPanel.PerformLayout();
            this.AutoCombineBottomPanel.ResumeLayout(false);
            this.AutoCombineUpperPanel.ResumeLayout(false);
            this.AutoCombineUpperPanel.PerformLayout();
            this.CombinePrepareTreeNodeMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.ContextMenuStrip NotifyIconExitMenu;
        private System.Windows.Forms.ToolStripMenuItem Exit;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage RemoveFolderTab;
        private System.Windows.Forms.TabPage CombinePrepareTab;
        private System.Windows.Forms.Panel RemoveFolderMainPanel;
        private System.Windows.Forms.Panel RemoveFolderUpperPanel;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.Button RemoveFolderConfirmBtn;
        private System.Windows.Forms.TextBox RemoveFolderText;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
        private System.Windows.Forms.TextBox RemoveFolderInfoText;
        private System.Windows.Forms.Panel CombinePrepareMainPanel;
        private System.Windows.Forms.Panel CombinePrepareUpperPanel;
        private System.Windows.Forms.TextBox CombinePrepareText;
        private System.Windows.Forms.CheckBox CombinePrepareCB;
        private System.Windows.Forms.Button CombinePrepareListBtn;
        private System.Windows.Forms.Button CombinePrepareClearBtn;
        private System.Windows.Forms.TreeView CombinePrepareTree;
        private System.Windows.Forms.ContextMenuStrip CombinePrepareTreeNodeMenu;
        private System.Windows.Forms.ToolStripMenuItem 截图ToolStripMenuItem;
        private System.Windows.Forms.Button CombinePrepareGenerateBtn;
        private System.Windows.Forms.TabPage RenameTab;
        private System.Windows.Forms.Panel RenameMainPanel;
        private System.Windows.Forms.Panel RenameUpperPanel;
        private System.Windows.Forms.TextBox RenameInfoText;
        private System.Windows.Forms.TextBox RenameText;
        private System.Windows.Forms.Button RenameBtn;
        private System.Windows.Forms.Button RenameManualBtn;
        private System.Windows.Forms.TabPage AutoCombine;
        private System.Windows.Forms.Panel AutoCombineUpperPanel;
        private System.Windows.Forms.Panel AutoCombineBottomPanel;
        private System.Windows.Forms.Panel AutoCombineMainPanel;
        private System.Windows.Forms.Button AutoCombineStartBtn;
        private System.Windows.Forms.TextBox AutoCombineSaveText;
        private System.Windows.Forms.ProgressBar AutoCombineTotalPB;
        private System.Windows.Forms.Button AutoCombineDeleteBtn;
        private System.Windows.Forms.ProgressBar AutoCombineCurrentPB;
        private System.Windows.Forms.ListView AutoCombineListView;
        private System.Windows.Forms.ColumnHeader AutoCombineListViewName;
        private System.Windows.Forms.ColumnHeader AutoCombineListViewSize;
        private System.Windows.Forms.Panel AutoCombineMainMainPanel;
        private System.Windows.Forms.Panel AutoCombineMainRightPanel;
        private System.Windows.Forms.TextBox AutoCombineInfoText;
        private System.Windows.Forms.Button AutoCombineCancelBtn;
        private System.Windows.Forms.Panel CombinePrepareMainMainPanel;
        private System.Windows.Forms.Panel CombinePrepareMainBottomPanel;
        private System.Windows.Forms.ProgressBar CombinePreparePB;
    }
}

