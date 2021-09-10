
namespace AvManager
{
    partial class MagnetItem
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.MagnetItemBottomPanel = new System.Windows.Forms.Panel();
            this.MagnetItemMainMainPanel = new System.Windows.Forms.Panel();
            this.MagnetItemListBox = new System.Windows.Forms.ListBox();
            this.MagnetItemMainUpperPanel = new System.Windows.Forms.Panel();
            this.MagnetItemMainUpperMainPanel = new System.Windows.Forms.Panel();
            this.MagnetItemTitleText = new System.Windows.Forms.TextBox();
            this.MagnetItemMainUpperRightPanel = new System.Windows.Forms.Panel();
            this.MagnetItemInfoLabel = new System.Windows.Forms.Label();
            this.MagnetItemMainPanel = new System.Windows.Forms.Panel();
            this.MagnetItemPicBox = new System.Windows.Forms.PictureBox();
            this.MagnetItemBottomPanel.SuspendLayout();
            this.MagnetItemMainMainPanel.SuspendLayout();
            this.MagnetItemMainUpperPanel.SuspendLayout();
            this.MagnetItemMainUpperMainPanel.SuspendLayout();
            this.MagnetItemMainUpperRightPanel.SuspendLayout();
            this.MagnetItemMainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MagnetItemPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // MagnetItemBottomPanel
            // 
            this.MagnetItemBottomPanel.Controls.Add(this.MagnetItemMainMainPanel);
            this.MagnetItemBottomPanel.Controls.Add(this.MagnetItemMainUpperPanel);
            this.MagnetItemBottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.MagnetItemBottomPanel.Location = new System.Drawing.Point(0, 304);
            this.MagnetItemBottomPanel.Margin = new System.Windows.Forms.Padding(2);
            this.MagnetItemBottomPanel.Name = "MagnetItemBottomPanel";
            this.MagnetItemBottomPanel.Size = new System.Drawing.Size(446, 165);
            this.MagnetItemBottomPanel.TabIndex = 0;
            // 
            // MagnetItemMainMainPanel
            // 
            this.MagnetItemMainMainPanel.Controls.Add(this.MagnetItemListBox);
            this.MagnetItemMainMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MagnetItemMainMainPanel.Location = new System.Drawing.Point(0, 23);
            this.MagnetItemMainMainPanel.Name = "MagnetItemMainMainPanel";
            this.MagnetItemMainMainPanel.Size = new System.Drawing.Size(446, 142);
            this.MagnetItemMainMainPanel.TabIndex = 2;
            // 
            // MagnetItemListBox
            // 
            this.MagnetItemListBox.BackColor = System.Drawing.Color.Teal;
            this.MagnetItemListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MagnetItemListBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MagnetItemListBox.FormattingEnabled = true;
            this.MagnetItemListBox.ItemHeight = 17;
            this.MagnetItemListBox.Location = new System.Drawing.Point(0, 0);
            this.MagnetItemListBox.Margin = new System.Windows.Forms.Padding(2);
            this.MagnetItemListBox.Name = "MagnetItemListBox";
            this.MagnetItemListBox.Size = new System.Drawing.Size(446, 142);
            this.MagnetItemListBox.TabIndex = 0;
            this.MagnetItemListBox.SelectedIndexChanged += new System.EventHandler(this.MagnetItemListBox_SelectedIndexChanged);
            // 
            // MagnetItemMainUpperPanel
            // 
            this.MagnetItemMainUpperPanel.Controls.Add(this.MagnetItemMainUpperMainPanel);
            this.MagnetItemMainUpperPanel.Controls.Add(this.MagnetItemMainUpperRightPanel);
            this.MagnetItemMainUpperPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MagnetItemMainUpperPanel.Location = new System.Drawing.Point(0, 0);
            this.MagnetItemMainUpperPanel.Name = "MagnetItemMainUpperPanel";
            this.MagnetItemMainUpperPanel.Size = new System.Drawing.Size(446, 23);
            this.MagnetItemMainUpperPanel.TabIndex = 1;
            // 
            // MagnetItemMainUpperMainPanel
            // 
            this.MagnetItemMainUpperMainPanel.Controls.Add(this.MagnetItemTitleText);
            this.MagnetItemMainUpperMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MagnetItemMainUpperMainPanel.Location = new System.Drawing.Point(0, 0);
            this.MagnetItemMainUpperMainPanel.Name = "MagnetItemMainUpperMainPanel";
            this.MagnetItemMainUpperMainPanel.Size = new System.Drawing.Size(385, 23);
            this.MagnetItemMainUpperMainPanel.TabIndex = 1;
            // 
            // MagnetItemTitleText
            // 
            this.MagnetItemTitleText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MagnetItemTitleText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MagnetItemTitleText.Location = new System.Drawing.Point(0, 0);
            this.MagnetItemTitleText.Multiline = true;
            this.MagnetItemTitleText.Name = "MagnetItemTitleText";
            this.MagnetItemTitleText.ReadOnly = true;
            this.MagnetItemTitleText.Size = new System.Drawing.Size(385, 23);
            this.MagnetItemTitleText.TabIndex = 0;
            // 
            // MagnetItemMainUpperRightPanel
            // 
            this.MagnetItemMainUpperRightPanel.Controls.Add(this.MagnetItemInfoLabel);
            this.MagnetItemMainUpperRightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.MagnetItemMainUpperRightPanel.Location = new System.Drawing.Point(385, 0);
            this.MagnetItemMainUpperRightPanel.Name = "MagnetItemMainUpperRightPanel";
            this.MagnetItemMainUpperRightPanel.Size = new System.Drawing.Size(61, 23);
            this.MagnetItemMainUpperRightPanel.TabIndex = 0;
            // 
            // MagnetItemInfoLabel
            // 
            this.MagnetItemInfoLabel.AutoSize = true;
            this.MagnetItemInfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MagnetItemInfoLabel.Location = new System.Drawing.Point(0, 0);
            this.MagnetItemInfoLabel.Name = "MagnetItemInfoLabel";
            this.MagnetItemInfoLabel.Size = new System.Drawing.Size(0, 17);
            this.MagnetItemInfoLabel.TabIndex = 1;
            // 
            // MagnetItemMainPanel
            // 
            this.MagnetItemMainPanel.Controls.Add(this.MagnetItemPicBox);
            this.MagnetItemMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MagnetItemMainPanel.Location = new System.Drawing.Point(0, 0);
            this.MagnetItemMainPanel.Margin = new System.Windows.Forms.Padding(2);
            this.MagnetItemMainPanel.Name = "MagnetItemMainPanel";
            this.MagnetItemMainPanel.Size = new System.Drawing.Size(446, 304);
            this.MagnetItemMainPanel.TabIndex = 1;
            // 
            // MagnetItemPicBox
            // 
            this.MagnetItemPicBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MagnetItemPicBox.Location = new System.Drawing.Point(0, 0);
            this.MagnetItemPicBox.Margin = new System.Windows.Forms.Padding(2);
            this.MagnetItemPicBox.Name = "MagnetItemPicBox";
            this.MagnetItemPicBox.Size = new System.Drawing.Size(446, 304);
            this.MagnetItemPicBox.TabIndex = 0;
            this.MagnetItemPicBox.TabStop = false;
            this.MagnetItemPicBox.Click += new System.EventHandler(this.MagnetItemPicBox_Click);
            // 
            // MagnetItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MagnetItemMainPanel);
            this.Controls.Add(this.MagnetItemBottomPanel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MagnetItem";
            this.Size = new System.Drawing.Size(446, 469);
            this.Load += new System.EventHandler(this.MagnetItem_Load);
            this.MagnetItemBottomPanel.ResumeLayout(false);
            this.MagnetItemMainMainPanel.ResumeLayout(false);
            this.MagnetItemMainUpperPanel.ResumeLayout(false);
            this.MagnetItemMainUpperMainPanel.ResumeLayout(false);
            this.MagnetItemMainUpperMainPanel.PerformLayout();
            this.MagnetItemMainUpperRightPanel.ResumeLayout(false);
            this.MagnetItemMainUpperRightPanel.PerformLayout();
            this.MagnetItemMainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MagnetItemPicBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MagnetItemBottomPanel;
        private System.Windows.Forms.Panel MagnetItemMainPanel;
        private System.Windows.Forms.PictureBox MagnetItemPicBox;
        private System.Windows.Forms.ListBox MagnetItemListBox;
        private System.Windows.Forms.Label MagnetItemInfoLabel;
        private System.Windows.Forms.Panel MagnetItemMainUpperPanel;
        private System.Windows.Forms.Panel MagnetItemMainUpperRightPanel;
        private System.Windows.Forms.Panel MagnetItemMainUpperMainPanel;
        private System.Windows.Forms.TextBox MagnetItemTitleText;
        private System.Windows.Forms.Panel MagnetItemMainMainPanel;
    }
}
