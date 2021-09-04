
namespace AvManager
{
    partial class ManualRename
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ManualRenameLeftPanel = new System.Windows.Forms.Panel();
            this.ManualRenameListView = new System.Windows.Forms.ListView();
            this.ManualRenameListViewSize = new System.Windows.Forms.ColumnHeader();
            this.ManuanRenameListViewName = new System.Windows.Forms.ColumnHeader();
            this.ManualRenameMainPanel = new System.Windows.Forms.Panel();
            this.ManualRenameMainMainPanel = new System.Windows.Forms.Panel();
            this.ManualRenameMainUpperPanel = new System.Windows.Forms.Panel();
            this.ManualRenameEpisodeLabel = new System.Windows.Forms.Label();
            this.ManualRenameLocationLabel = new System.Windows.Forms.Label();
            this.ManualRenameFinalLabel = new System.Windows.Forms.Label();
            this.ManualRenameConfirmBtn = new System.Windows.Forms.Button();
            this.ManualRenameChineseCB = new System.Windows.Forms.CheckBox();
            this.ManualRenameEpisodeCombo = new System.Windows.Forms.ComboBox();
            this.ManualRenamePositionCombo = new System.Windows.Forms.ComboBox();
            this.ManualRenameSearchBtn = new System.Windows.Forms.Button();
            this.ManualRenameMatchBtn = new System.Windows.Forms.Button();
            this.ManualRenameFinalText = new System.Windows.Forms.TextBox();
            this.ManualRenameMatchLabel = new System.Windows.Forms.Label();
            this.ManualRenameMatchText = new System.Windows.Forms.TextBox();
            this.ManualRenameLeftPanel.SuspendLayout();
            this.ManualRenameMainPanel.SuspendLayout();
            this.ManualRenameMainUpperPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ManualRenameLeftPanel
            // 
            this.ManualRenameLeftPanel.Controls.Add(this.ManualRenameListView);
            this.ManualRenameLeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.ManualRenameLeftPanel.Location = new System.Drawing.Point(0, 0);
            this.ManualRenameLeftPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.ManualRenameLeftPanel.Name = "ManualRenameLeftPanel";
            this.ManualRenameLeftPanel.Size = new System.Drawing.Size(494, 1023);
            this.ManualRenameLeftPanel.TabIndex = 0;
            // 
            // ManualRenameListView
            // 
            this.ManualRenameListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ManualRenameListViewSize,
            this.ManuanRenameListViewName});
            this.ManualRenameListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ManualRenameListView.FullRowSelect = true;
            this.ManualRenameListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ManualRenameListView.HideSelection = false;
            this.ManualRenameListView.Location = new System.Drawing.Point(0, 0);
            this.ManualRenameListView.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.ManualRenameListView.MultiSelect = false;
            this.ManualRenameListView.Name = "ManualRenameListView";
            this.ManualRenameListView.Size = new System.Drawing.Size(494, 1023);
            this.ManualRenameListView.TabIndex = 0;
            this.ManualRenameListView.UseCompatibleStateImageBehavior = false;
            this.ManualRenameListView.View = System.Windows.Forms.View.Details;
            this.ManualRenameListView.SelectedIndexChanged += new System.EventHandler(this.ManualRenameListView_SelectedIndexChanged);
            // 
            // ManualRenameListViewSize
            // 
            this.ManualRenameListViewSize.Text = "大小";
            this.ManualRenameListViewSize.Width = 100;
            // 
            // ManuanRenameListViewName
            // 
            this.ManuanRenameListViewName.Text = "名称";
            this.ManuanRenameListViewName.Width = 300;
            // 
            // ManualRenameMainPanel
            // 
            this.ManualRenameMainPanel.Controls.Add(this.ManualRenameMainMainPanel);
            this.ManualRenameMainPanel.Controls.Add(this.ManualRenameMainUpperPanel);
            this.ManualRenameMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ManualRenameMainPanel.Location = new System.Drawing.Point(494, 0);
            this.ManualRenameMainPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.ManualRenameMainPanel.Name = "ManualRenameMainPanel";
            this.ManualRenameMainPanel.Size = new System.Drawing.Size(1733, 1023);
            this.ManualRenameMainPanel.TabIndex = 1;
            // 
            // ManualRenameMainMainPanel
            // 
            this.ManualRenameMainMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ManualRenameMainMainPanel.Location = new System.Drawing.Point(0, 165);
            this.ManualRenameMainMainPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.ManualRenameMainMainPanel.Name = "ManualRenameMainMainPanel";
            this.ManualRenameMainMainPanel.Size = new System.Drawing.Size(1733, 858);
            this.ManualRenameMainMainPanel.TabIndex = 1;
            // 
            // ManualRenameMainUpperPanel
            // 
            this.ManualRenameMainUpperPanel.Controls.Add(this.ManualRenameEpisodeLabel);
            this.ManualRenameMainUpperPanel.Controls.Add(this.ManualRenameLocationLabel);
            this.ManualRenameMainUpperPanel.Controls.Add(this.ManualRenameFinalLabel);
            this.ManualRenameMainUpperPanel.Controls.Add(this.ManualRenameConfirmBtn);
            this.ManualRenameMainUpperPanel.Controls.Add(this.ManualRenameChineseCB);
            this.ManualRenameMainUpperPanel.Controls.Add(this.ManualRenameEpisodeCombo);
            this.ManualRenameMainUpperPanel.Controls.Add(this.ManualRenamePositionCombo);
            this.ManualRenameMainUpperPanel.Controls.Add(this.ManualRenameSearchBtn);
            this.ManualRenameMainUpperPanel.Controls.Add(this.ManualRenameMatchBtn);
            this.ManualRenameMainUpperPanel.Controls.Add(this.ManualRenameFinalText);
            this.ManualRenameMainUpperPanel.Controls.Add(this.ManualRenameMatchLabel);
            this.ManualRenameMainUpperPanel.Controls.Add(this.ManualRenameMatchText);
            this.ManualRenameMainUpperPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ManualRenameMainUpperPanel.Location = new System.Drawing.Point(0, 0);
            this.ManualRenameMainUpperPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.ManualRenameMainUpperPanel.Name = "ManualRenameMainUpperPanel";
            this.ManualRenameMainUpperPanel.Size = new System.Drawing.Size(1733, 165);
            this.ManualRenameMainUpperPanel.TabIndex = 0;
            // 
            // ManualRenameEpisodeLabel
            // 
            this.ManualRenameEpisodeLabel.AutoSize = true;
            this.ManualRenameEpisodeLabel.Location = new System.Drawing.Point(431, 69);
            this.ManualRenameEpisodeLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.ManualRenameEpisodeLabel.Name = "ManualRenameEpisodeLabel";
            this.ManualRenameEpisodeLabel.Size = new System.Drawing.Size(42, 28);
            this.ManualRenameEpisodeLabel.TabIndex = 11;
            this.ManualRenameEpisodeLabel.Text = "Ep.";
            // 
            // ManualRenameLocationLabel
            // 
            this.ManualRenameLocationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ManualRenameLocationLabel.AutoSize = true;
            this.ManualRenameLocationLabel.Location = new System.Drawing.Point(82, 69);
            this.ManualRenameLocationLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.ManualRenameLocationLabel.Name = "ManualRenameLocationLabel";
            this.ManualRenameLocationLabel.Size = new System.Drawing.Size(54, 28);
            this.ManualRenameLocationLabel.TabIndex = 10;
            this.ManualRenameLocationLabel.Text = "位置";
            // 
            // ManualRenameFinalLabel
            // 
            this.ManualRenameFinalLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ManualRenameFinalLabel.AutoSize = true;
            this.ManualRenameFinalLabel.Location = new System.Drawing.Point(11, 120);
            this.ManualRenameFinalLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.ManualRenameFinalLabel.Name = "ManualRenameFinalLabel";
            this.ManualRenameFinalLabel.Size = new System.Drawing.Size(54, 28);
            this.ManualRenameFinalLabel.TabIndex = 9;
            this.ManualRenameFinalLabel.Text = "最终";
            // 
            // ManualRenameConfirmBtn
            // 
            this.ManualRenameConfirmBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ManualRenameConfirmBtn.Location = new System.Drawing.Point(1588, 112);
            this.ManualRenameConfirmBtn.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.ManualRenameConfirmBtn.Name = "ManualRenameConfirmBtn";
            this.ManualRenameConfirmBtn.Size = new System.Drawing.Size(139, 44);
            this.ManualRenameConfirmBtn.TabIndex = 8;
            this.ManualRenameConfirmBtn.Text = "确认";
            this.ManualRenameConfirmBtn.UseVisualStyleBackColor = true;
            this.ManualRenameConfirmBtn.Click += new System.EventHandler(this.ManualRenameConfirmBtn_Click);
            // 
            // ManualRenameChineseCB
            // 
            this.ManualRenameChineseCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ManualRenameChineseCB.AutoSize = true;
            this.ManualRenameChineseCB.Location = new System.Drawing.Point(767, 68);
            this.ManualRenameChineseCB.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.ManualRenameChineseCB.Name = "ManualRenameChineseCB";
            this.ManualRenameChineseCB.Size = new System.Drawing.Size(80, 32);
            this.ManualRenameChineseCB.TabIndex = 7;
            this.ManualRenameChineseCB.Text = "中文";
            this.ManualRenameChineseCB.UseVisualStyleBackColor = true;
            this.ManualRenameChineseCB.CheckedChanged += new System.EventHandler(this.ManualRenameChineseCB_CheckedChanged);
            // 
            // ManualRenameEpisodeCombo
            // 
            this.ManualRenameEpisodeCombo.FormattingEnabled = true;
            this.ManualRenameEpisodeCombo.Location = new System.Drawing.Point(490, 64);
            this.ManualRenameEpisodeCombo.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.ManualRenameEpisodeCombo.Name = "ManualRenameEpisodeCombo";
            this.ManualRenameEpisodeCombo.Size = new System.Drawing.Size(221, 36);
            this.ManualRenameEpisodeCombo.TabIndex = 6;
            this.ManualRenameEpisodeCombo.SelectedIndexChanged += new System.EventHandler(this.ManualRenameEpisodeCombo_SelectedIndexChanged);
            // 
            // ManualRenamePositionCombo
            // 
            this.ManualRenamePositionCombo.FormattingEnabled = true;
            this.ManualRenamePositionCombo.Location = new System.Drawing.Point(152, 64);
            this.ManualRenamePositionCombo.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.ManualRenamePositionCombo.Name = "ManualRenamePositionCombo";
            this.ManualRenamePositionCombo.Size = new System.Drawing.Size(221, 36);
            this.ManualRenamePositionCombo.TabIndex = 5;
            this.ManualRenamePositionCombo.SelectedIndexChanged += new System.EventHandler(this.ManualRenamePositionCombo_SelectedIndexChanged);
            // 
            // ManualRenameSearchBtn
            // 
            this.ManualRenameSearchBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ManualRenameSearchBtn.Location = new System.Drawing.Point(1588, 13);
            this.ManualRenameSearchBtn.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.ManualRenameSearchBtn.Name = "ManualRenameSearchBtn";
            this.ManualRenameSearchBtn.Size = new System.Drawing.Size(139, 44);
            this.ManualRenameSearchBtn.TabIndex = 4;
            this.ManualRenameSearchBtn.Text = "搜索";
            this.ManualRenameSearchBtn.UseVisualStyleBackColor = true;
            this.ManualRenameSearchBtn.Click += new System.EventHandler(this.ManualRenameSearchBtn_Click);
            // 
            // ManualRenameMatchBtn
            // 
            this.ManualRenameMatchBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ManualRenameMatchBtn.Location = new System.Drawing.Point(1437, 13);
            this.ManualRenameMatchBtn.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.ManualRenameMatchBtn.Name = "ManualRenameMatchBtn";
            this.ManualRenameMatchBtn.Size = new System.Drawing.Size(139, 44);
            this.ManualRenameMatchBtn.TabIndex = 3;
            this.ManualRenameMatchBtn.Text = "匹配";
            this.ManualRenameMatchBtn.UseVisualStyleBackColor = true;
            this.ManualRenameMatchBtn.Click += new System.EventHandler(this.ManualRenameMatchBtn_Click);
            // 
            // ManualRenameFinalText
            // 
            this.ManualRenameFinalText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ManualRenameFinalText.Enabled = false;
            this.ManualRenameFinalText.Location = new System.Drawing.Point(82, 115);
            this.ManualRenameFinalText.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.ManualRenameFinalText.Name = "ManualRenameFinalText";
            this.ManualRenameFinalText.Size = new System.Drawing.Size(1477, 34);
            this.ManualRenameFinalText.TabIndex = 2;
            // 
            // ManualRenameMatchLabel
            // 
            this.ManualRenameMatchLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ManualRenameMatchLabel.AutoSize = true;
            this.ManualRenameMatchLabel.Location = new System.Drawing.Point(11, 21);
            this.ManualRenameMatchLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.ManualRenameMatchLabel.Name = "ManualRenameMatchLabel";
            this.ManualRenameMatchLabel.Size = new System.Drawing.Size(54, 28);
            this.ManualRenameMatchLabel.TabIndex = 1;
            this.ManualRenameMatchLabel.Text = "匹配";
            // 
            // ManualRenameMatchText
            // 
            this.ManualRenameMatchText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ManualRenameMatchText.Location = new System.Drawing.Point(82, 15);
            this.ManualRenameMatchText.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.ManualRenameMatchText.Name = "ManualRenameMatchText";
            this.ManualRenameMatchText.Size = new System.Drawing.Size(1341, 34);
            this.ManualRenameMatchText.TabIndex = 0;
            // 
            // ManualRename
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2227, 1023);
            this.Controls.Add(this.ManualRenameMainPanel);
            this.Controls.Add(this.ManualRenameLeftPanel);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "ManualRename";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "手动重命名";
            this.Load += new System.EventHandler(this.ManualRename_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ManualRename_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ManualRename_KeyPress);
            this.ManualRenameLeftPanel.ResumeLayout(false);
            this.ManualRenameMainPanel.ResumeLayout(false);
            this.ManualRenameMainUpperPanel.ResumeLayout(false);
            this.ManualRenameMainUpperPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ManualRenameLeftPanel;
        private System.Windows.Forms.Panel ManualRenameMainPanel;
        private System.Windows.Forms.Panel ManualRenameMainMainPanel;
        private System.Windows.Forms.Panel ManualRenameMainUpperPanel;
        private System.Windows.Forms.ListView ManualRenameListView;
        private System.Windows.Forms.ColumnHeader ManuanRenameListViewName;
        private System.Windows.Forms.ColumnHeader ManualRenameListViewSize;
        private System.Windows.Forms.Button ManualRenameConfirmBtn;
        private System.Windows.Forms.CheckBox ManualRenameChineseCB;
        private System.Windows.Forms.ComboBox ManualRenameEpisodeCombo;
        private System.Windows.Forms.ComboBox ManualRenamePositionCombo;
        private System.Windows.Forms.Button ManualRenameSearchBtn;
        private System.Windows.Forms.Button ManualRenameMatchBtn;
        private System.Windows.Forms.TextBox ManualRenameFinalText;
        private System.Windows.Forms.Label ManualRenameMatchLabel;
        private System.Windows.Forms.TextBox ManualRenameMatchText;
        private System.Windows.Forms.Label ManualRenameFinalLabel;
        private System.Windows.Forms.Label ManualRenameLocationLabel;
        private System.Windows.Forms.Label ManualRenameEpisodeLabel;
    }
}