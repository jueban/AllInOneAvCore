
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
            this.CombinePrepareTab = new System.Windows.Forms.TabPage();
            this.CombinePrepareMainPanel = new System.Windows.Forms.Panel();
            this.CombinePrepareTree = new System.Windows.Forms.TreeView();
            this.CombinePrepareUpperPanel = new System.Windows.Forms.Panel();
            this.CombinePrepareGenerateBtn = new System.Windows.Forms.Button();
            this.CombinePrepareClearBtn = new System.Windows.Forms.Button();
            this.CombinePrepareListBtn = new System.Windows.Forms.Button();
            this.CombinePrepareCB = new System.Windows.Forms.CheckBox();
            this.CombinePrepareText = new System.Windows.Forms.TextBox();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.CombinePrepareTreeNodeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.截图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RenameTab = new System.Windows.Forms.TabPage();
            this.RenameUpperPanel = new System.Windows.Forms.Panel();
            this.RenameMainPanel = new System.Windows.Forms.Panel();
            this.RenameInfoText = new System.Windows.Forms.TextBox();
            this.RenameText = new System.Windows.Forms.TextBox();
            this.RenameBtn = new System.Windows.Forms.Button();
            this.RenameManualBtn = new System.Windows.Forms.Button();
            this.NotifyIconExitMenu.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.RemoveFolderTab.SuspendLayout();
            this.RemoveFolderMainPanel.SuspendLayout();
            this.RemoveFolderUpperPanel.SuspendLayout();
            this.CombinePrepareTab.SuspendLayout();
            this.CombinePrepareMainPanel.SuspendLayout();
            this.CombinePrepareUpperPanel.SuspendLayout();
            this.CombinePrepareTreeNodeMenu.SuspendLayout();
            this.RenameTab.SuspendLayout();
            this.RenameUpperPanel.SuspendLayout();
            this.RenameMainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.ContextMenuStrip = this.NotifyIconExitMenu;
            this.NotifyIcon.Text = "notifyIcon";
            this.NotifyIcon.Visible = true;
            this.NotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseClick);
            this.NotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // NotifyIconExitMenu
            // 
            this.NotifyIconExitMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Exit});
            this.NotifyIconExitMenu.Name = "notifyIconExitMenu";
            this.NotifyIconExitMenu.Size = new System.Drawing.Size(101, 26);
            // 
            // Exit
            // 
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(100, 22);
            this.Exit.Text = "退出";
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.TabControl);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(800, 450);
            this.MainPanel.TabIndex = 1;
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.RemoveFolderTab);
            this.TabControl.Controls.Add(this.RenameTab);
            this.TabControl.Controls.Add(this.CombinePrepareTab);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Location = new System.Drawing.Point(0, 0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(800, 450);
            this.TabControl.TabIndex = 0;
            // 
            // RemoveFolderTab
            // 
            this.RemoveFolderTab.Controls.Add(this.RemoveFolderMainPanel);
            this.RemoveFolderTab.Controls.Add(this.RemoveFolderUpperPanel);
            this.RemoveFolderTab.Location = new System.Drawing.Point(4, 26);
            this.RemoveFolderTab.Name = "RemoveFolderTab";
            this.RemoveFolderTab.Padding = new System.Windows.Forms.Padding(3);
            this.RemoveFolderTab.Size = new System.Drawing.Size(792, 420);
            this.RemoveFolderTab.TabIndex = 0;
            this.RemoveFolderTab.Text = "去文件夹";
            this.RemoveFolderTab.UseVisualStyleBackColor = true;
            // 
            // RemoveFolderMainPanel
            // 
            this.RemoveFolderMainPanel.Controls.Add(this.RemoveFolderInfoText);
            this.RemoveFolderMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoveFolderMainPanel.Location = new System.Drawing.Point(3, 39);
            this.RemoveFolderMainPanel.Name = "RemoveFolderMainPanel";
            this.RemoveFolderMainPanel.Size = new System.Drawing.Size(786, 378);
            this.RemoveFolderMainPanel.TabIndex = 1;
            // 
            // RemoveFolderInfoText
            // 
            this.RemoveFolderInfoText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoveFolderInfoText.Location = new System.Drawing.Point(0, 0);
            this.RemoveFolderInfoText.Multiline = true;
            this.RemoveFolderInfoText.Name = "RemoveFolderInfoText";
            this.RemoveFolderInfoText.ReadOnly = true;
            this.RemoveFolderInfoText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.RemoveFolderInfoText.Size = new System.Drawing.Size(786, 378);
            this.RemoveFolderInfoText.TabIndex = 0;
            // 
            // RemoveFolderUpperPanel
            // 
            this.RemoveFolderUpperPanel.Controls.Add(this.RemoveFolderConfirmBtn);
            this.RemoveFolderUpperPanel.Controls.Add(this.RemoveFolderText);
            this.RemoveFolderUpperPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.RemoveFolderUpperPanel.Location = new System.Drawing.Point(3, 3);
            this.RemoveFolderUpperPanel.Name = "RemoveFolderUpperPanel";
            this.RemoveFolderUpperPanel.Size = new System.Drawing.Size(786, 36);
            this.RemoveFolderUpperPanel.TabIndex = 0;
            // 
            // RemoveFolderConfirmBtn
            // 
            this.RemoveFolderConfirmBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveFolderConfirmBtn.Location = new System.Drawing.Point(706, 5);
            this.RemoveFolderConfirmBtn.Name = "RemoveFolderConfirmBtn";
            this.RemoveFolderConfirmBtn.Size = new System.Drawing.Size(75, 27);
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
            this.RemoveFolderText.Location = new System.Drawing.Point(5, 7);
            this.RemoveFolderText.Name = "RemoveFolderText";
            this.RemoveFolderText.PlaceholderText = "点击选择需要去文件夹的目录，文件会移动到所选目录下的\'movefiles\'文件夹";
            this.RemoveFolderText.Size = new System.Drawing.Size(695, 23);
            this.RemoveFolderText.TabIndex = 0;
            this.RemoveFolderText.Click += new System.EventHandler(this.RemoveFolderText_Click);
            // 
            // CombinePrepareTab
            // 
            this.CombinePrepareTab.Controls.Add(this.CombinePrepareMainPanel);
            this.CombinePrepareTab.Controls.Add(this.CombinePrepareUpperPanel);
            this.CombinePrepareTab.Location = new System.Drawing.Point(4, 26);
            this.CombinePrepareTab.Name = "CombinePrepareTab";
            this.CombinePrepareTab.Padding = new System.Windows.Forms.Padding(3);
            this.CombinePrepareTab.Size = new System.Drawing.Size(792, 420);
            this.CombinePrepareTab.TabIndex = 1;
            this.CombinePrepareTab.Text = "合并准备";
            this.CombinePrepareTab.UseVisualStyleBackColor = true;
            // 
            // CombinePrepareMainPanel
            // 
            this.CombinePrepareMainPanel.Controls.Add(this.CombinePrepareTree);
            this.CombinePrepareMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CombinePrepareMainPanel.Location = new System.Drawing.Point(3, 39);
            this.CombinePrepareMainPanel.Name = "CombinePrepareMainPanel";
            this.CombinePrepareMainPanel.Size = new System.Drawing.Size(786, 378);
            this.CombinePrepareMainPanel.TabIndex = 1;
            // 
            // CombinePrepareTree
            // 
            this.CombinePrepareTree.CheckBoxes = true;
            this.CombinePrepareTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CombinePrepareTree.FullRowSelect = true;
            this.CombinePrepareTree.LabelEdit = true;
            this.CombinePrepareTree.Location = new System.Drawing.Point(0, 0);
            this.CombinePrepareTree.Name = "CombinePrepareTree";
            this.CombinePrepareTree.Size = new System.Drawing.Size(786, 378);
            this.CombinePrepareTree.TabIndex = 0;
            this.CombinePrepareTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.CombinePrepareTree_AfterLabelEdit);
            this.CombinePrepareTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.CombinePrepareTree_AfterCheck);
            this.CombinePrepareTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.CombinePrepareTree_NodeMouseClick);
            this.CombinePrepareTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.CombinePrepareTree_NodeMouseDoubleClick);
            // 
            // CombinePrepareUpperPanel
            // 
            this.CombinePrepareUpperPanel.Controls.Add(this.CombinePrepareGenerateBtn);
            this.CombinePrepareUpperPanel.Controls.Add(this.CombinePrepareClearBtn);
            this.CombinePrepareUpperPanel.Controls.Add(this.CombinePrepareListBtn);
            this.CombinePrepareUpperPanel.Controls.Add(this.CombinePrepareCB);
            this.CombinePrepareUpperPanel.Controls.Add(this.CombinePrepareText);
            this.CombinePrepareUpperPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CombinePrepareUpperPanel.Location = new System.Drawing.Point(3, 3);
            this.CombinePrepareUpperPanel.Name = "CombinePrepareUpperPanel";
            this.CombinePrepareUpperPanel.Size = new System.Drawing.Size(786, 36);
            this.CombinePrepareUpperPanel.TabIndex = 0;
            // 
            // CombinePrepareGenerateBtn
            // 
            this.CombinePrepareGenerateBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CombinePrepareGenerateBtn.Location = new System.Drawing.Point(696, 3);
            this.CombinePrepareGenerateBtn.Name = "CombinePrepareGenerateBtn";
            this.CombinePrepareGenerateBtn.Size = new System.Drawing.Size(75, 27);
            this.CombinePrepareGenerateBtn.TabIndex = 2;
            this.CombinePrepareGenerateBtn.Text = "生成";
            this.CombinePrepareGenerateBtn.UseVisualStyleBackColor = true;
            this.CombinePrepareGenerateBtn.Click += new System.EventHandler(this.CombinePrepareGenerateBtn_Click);
            // 
            // CombinePrepareClearBtn
            // 
            this.CombinePrepareClearBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CombinePrepareClearBtn.Location = new System.Drawing.Point(615, 3);
            this.CombinePrepareClearBtn.Name = "CombinePrepareClearBtn";
            this.CombinePrepareClearBtn.Size = new System.Drawing.Size(75, 27);
            this.CombinePrepareClearBtn.TabIndex = 1;
            this.CombinePrepareClearBtn.Text = "整理";
            this.CombinePrepareClearBtn.UseVisualStyleBackColor = true;
            this.CombinePrepareClearBtn.Click += new System.EventHandler(this.CombinePrepareClearBtn_Click);
            // 
            // CombinePrepareListBtn
            // 
            this.CombinePrepareListBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CombinePrepareListBtn.Location = new System.Drawing.Point(534, 3);
            this.CombinePrepareListBtn.Name = "CombinePrepareListBtn";
            this.CombinePrepareListBtn.Size = new System.Drawing.Size(75, 27);
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
            this.CombinePrepareCB.Location = new System.Drawing.Point(477, 9);
            this.CombinePrepareCB.Name = "CombinePrepareCB";
            this.CombinePrepareCB.Size = new System.Drawing.Size(51, 21);
            this.CombinePrepareCB.TabIndex = 1;
            this.CombinePrepareCB.Text = "截屏";
            this.CombinePrepareCB.UseVisualStyleBackColor = true;
            // 
            // CombinePrepareText
            // 
            this.CombinePrepareText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CombinePrepareText.Location = new System.Drawing.Point(5, 7);
            this.CombinePrepareText.Name = "CombinePrepareText";
            this.CombinePrepareText.PlaceholderText = "点击选择合并文件所在的目录，勾选为删除，双击重命名,点击‘整理’完成重命名及删除";
            this.CombinePrepareText.Size = new System.Drawing.Size(464, 23);
            this.CombinePrepareText.TabIndex = 0;
            this.CombinePrepareText.Click += new System.EventHandler(this.CombinePrepareText_Click);
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.FileName = "openFileDialog1";
            // 
            // CombinePrepareTreeNodeMenu
            // 
            this.CombinePrepareTreeNodeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.截图ToolStripMenuItem});
            this.CombinePrepareTreeNodeMenu.Name = "CombinePrepareTreeNodeMenu";
            this.CombinePrepareTreeNodeMenu.Size = new System.Drawing.Size(101, 26);
            // 
            // 截图ToolStripMenuItem
            // 
            this.截图ToolStripMenuItem.Name = "截图ToolStripMenuItem";
            this.截图ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.截图ToolStripMenuItem.Text = "截图";
            this.截图ToolStripMenuItem.Click += new System.EventHandler(this.截图ToolStripMenuItem_Click);
            // 
            // RenameTab
            // 
            this.RenameTab.Controls.Add(this.RenameMainPanel);
            this.RenameTab.Controls.Add(this.RenameUpperPanel);
            this.RenameTab.Location = new System.Drawing.Point(4, 26);
            this.RenameTab.Name = "RenameTab";
            this.RenameTab.Size = new System.Drawing.Size(792, 420);
            this.RenameTab.TabIndex = 2;
            this.RenameTab.Text = "重命名";
            this.RenameTab.UseVisualStyleBackColor = true;
            // 
            // RenameUpperPanel
            // 
            this.RenameUpperPanel.Controls.Add(this.RenameManualBtn);
            this.RenameUpperPanel.Controls.Add(this.RenameBtn);
            this.RenameUpperPanel.Controls.Add(this.RenameText);
            this.RenameUpperPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.RenameUpperPanel.Location = new System.Drawing.Point(0, 0);
            this.RenameUpperPanel.Name = "RenameUpperPanel";
            this.RenameUpperPanel.Size = new System.Drawing.Size(792, 36);
            this.RenameUpperPanel.TabIndex = 0;
            // 
            // RenameMainPanel
            // 
            this.RenameMainPanel.Controls.Add(this.RenameInfoText);
            this.RenameMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenameMainPanel.Location = new System.Drawing.Point(0, 36);
            this.RenameMainPanel.Name = "RenameMainPanel";
            this.RenameMainPanel.Size = new System.Drawing.Size(792, 384);
            this.RenameMainPanel.TabIndex = 1;
            // 
            // RenameInfoText
            // 
            this.RenameInfoText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenameInfoText.Location = new System.Drawing.Point(0, 0);
            this.RenameInfoText.Multiline = true;
            this.RenameInfoText.Name = "RenameInfoText";
            this.RenameInfoText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.RenameInfoText.Size = new System.Drawing.Size(792, 384);
            this.RenameInfoText.TabIndex = 0;
            // 
            // RenameText
            // 
            this.RenameText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RenameText.Location = new System.Drawing.Point(8, 7);
            this.RenameText.Name = "RenameText";
            this.RenameText.PlaceholderText = "点击选择需要重命名的目录，唯一确定的放入Fin，需要手动处理的放入TempFin";
            this.RenameText.Size = new System.Drawing.Size(614, 23);
            this.RenameText.TabIndex = 0;
            this.RenameText.Click += new System.EventHandler(this.RenameText_Click);
            // 
            // RenameBtn
            // 
            this.RenameBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RenameBtn.Location = new System.Drawing.Point(628, 5);
            this.RenameBtn.Name = "RenameBtn";
            this.RenameBtn.Size = new System.Drawing.Size(75, 26);
            this.RenameBtn.TabIndex = 1;
            this.RenameBtn.Text = "自动重名";
            this.RenameBtn.UseVisualStyleBackColor = true;
            this.RenameBtn.Click += new System.EventHandler(this.RenameBtn_Click);
            // 
            // RenameManualBtn
            // 
            this.RenameManualBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RenameManualBtn.Location = new System.Drawing.Point(709, 5);
            this.RenameManualBtn.Name = "RenameManualBtn";
            this.RenameManualBtn.Size = new System.Drawing.Size(75, 26);
            this.RenameManualBtn.TabIndex = 2;
            this.RenameManualBtn.Text = "手动重名";
            this.RenameManualBtn.UseVisualStyleBackColor = true;
            this.RenameManualBtn.Click += new System.EventHandler(this.RenameManualBtn_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MainPanel);
            this.KeyPreview = true;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AvManager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.NotifyIconExitMenu.ResumeLayout(false);
            this.MainPanel.ResumeLayout(false);
            this.TabControl.ResumeLayout(false);
            this.RemoveFolderTab.ResumeLayout(false);
            this.RemoveFolderMainPanel.ResumeLayout(false);
            this.RemoveFolderMainPanel.PerformLayout();
            this.RemoveFolderUpperPanel.ResumeLayout(false);
            this.RemoveFolderUpperPanel.PerformLayout();
            this.CombinePrepareTab.ResumeLayout(false);
            this.CombinePrepareMainPanel.ResumeLayout(false);
            this.CombinePrepareUpperPanel.ResumeLayout(false);
            this.CombinePrepareUpperPanel.PerformLayout();
            this.CombinePrepareTreeNodeMenu.ResumeLayout(false);
            this.RenameTab.ResumeLayout(false);
            this.RenameUpperPanel.ResumeLayout(false);
            this.RenameUpperPanel.PerformLayout();
            this.RenameMainPanel.ResumeLayout(false);
            this.RenameMainPanel.PerformLayout();
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
    }
}

