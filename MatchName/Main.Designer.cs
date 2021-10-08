
namespace MatchName
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
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.扫描ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.执行ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.InfoPb = new System.Windows.Forms.ToolStripProgressBar();
            this.InfoLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.MainMainPanel = new System.Windows.Forms.Panel();
            this.RenameListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.MainTopPanel = new System.Windows.Forms.Panel();
            this.DescText = new System.Windows.Forms.TextBox();
            this.RenamePb = new System.Windows.Forms.ToolStripProgressBar();
            this.MenuStrip.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.MainMainPanel.SuspendLayout();
            this.MainTopPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开ToolStripMenuItem,
            this.扫描ToolStripMenuItem1,
            this.执行ToolStripMenuItem,
            this.设置ToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(800, 25);
            this.MenuStrip.TabIndex = 0;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.打开ToolStripMenuItem.Text = "打开";
            this.打开ToolStripMenuItem.Click += new System.EventHandler(this.打开ToolStripMenuItem_Click);
            // 
            // 扫描ToolStripMenuItem1
            // 
            this.扫描ToolStripMenuItem1.Name = "扫描ToolStripMenuItem1";
            this.扫描ToolStripMenuItem1.Size = new System.Drawing.Size(44, 21);
            this.扫描ToolStripMenuItem1.Text = "扫描";
            this.扫描ToolStripMenuItem1.Click += new System.EventHandler(this.扫描ToolStripMenuItem1_Click);
            // 
            // 执行ToolStripMenuItem
            // 
            this.执行ToolStripMenuItem.Name = "执行ToolStripMenuItem";
            this.执行ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.执行ToolStripMenuItem.Text = "执行";
            this.执行ToolStripMenuItem.Click += new System.EventHandler(this.执行ToolStripMenuItem_Click);
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            this.设置ToolStripMenuItem.Click += new System.EventHandler(this.设置ToolStripMenuItem_Click);
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.InfoPb,
            this.InfoLabel,
            this.RenamePb});
            this.StatusStrip.Location = new System.Drawing.Point(0, 428);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(800, 22);
            this.StatusStrip.TabIndex = 1;
            this.StatusStrip.Text = "statusStrip1";
            // 
            // InfoPb
            // 
            this.InfoPb.Name = "InfoPb";
            this.InfoPb.Size = new System.Drawing.Size(100, 16);
            // 
            // InfoLabel
            // 
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.MainMainPanel);
            this.MainPanel.Controls.Add(this.MainTopPanel);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 25);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(800, 403);
            this.MainPanel.TabIndex = 2;
            // 
            // MainMainPanel
            // 
            this.MainMainPanel.Controls.Add(this.RenameListView);
            this.MainMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainMainPanel.Location = new System.Drawing.Point(0, 32);
            this.MainMainPanel.Name = "MainMainPanel";
            this.MainMainPanel.Size = new System.Drawing.Size(800, 371);
            this.MainMainPanel.TabIndex = 2;
            // 
            // RenameListView
            // 
            this.RenameListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.RenameListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenameListView.FullRowSelect = true;
            this.RenameListView.HideSelection = false;
            this.RenameListView.Location = new System.Drawing.Point(0, 0);
            this.RenameListView.Name = "RenameListView";
            this.RenameListView.Size = new System.Drawing.Size(800, 371);
            this.RenameListView.TabIndex = 0;
            this.RenameListView.UseCompatibleStateImageBehavior = false;
            this.RenameListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "原始";
            this.columnHeader1.Width = 390;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "预计";
            this.columnHeader2.Width = 390;
            // 
            // MainTopPanel
            // 
            this.MainTopPanel.Controls.Add(this.DescText);
            this.MainTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainTopPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTopPanel.Name = "MainTopPanel";
            this.MainTopPanel.Size = new System.Drawing.Size(800, 32);
            this.MainTopPanel.TabIndex = 1;
            // 
            // DescText
            // 
            this.DescText.Location = new System.Drawing.Point(3, 4);
            this.DescText.Name = "DescText";
            this.DescText.PlaceholderText = "点击选择重命名后目录";
            this.DescText.ReadOnly = true;
            this.DescText.Size = new System.Drawing.Size(794, 23);
            this.DescText.TabIndex = 0;
            this.DescText.Click += new System.EventHandler(this.DescText_Click);
            // 
            // RenamePb
            // 
            this.RenamePb.Name = "RenamePb";
            this.RenamePb.Size = new System.Drawing.Size(100, 16);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "重命名";
            this.Load += new System.EventHandler(this.Main_Load);
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.MainPanel.ResumeLayout(false);
            this.MainMainPanel.ResumeLayout(false);
            this.MainTopPanel.ResumeLayout(false);
            this.MainTopPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel InfoLabel;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowser;
        private System.Windows.Forms.ToolStripProgressBar InfoPb;
        private System.Windows.Forms.ToolStripMenuItem 执行ToolStripMenuItem;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.ListView RenameListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStripMenuItem 扫描ToolStripMenuItem1;
        private System.Windows.Forms.Panel MainTopPanel;
        private System.Windows.Forms.TextBox DescText;
        private System.Windows.Forms.Panel MainMainPanel;
        private System.Windows.Forms.ToolStripProgressBar RenamePb;
    }
}

