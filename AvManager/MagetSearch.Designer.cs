
namespace AvManager
{
    partial class MagetSearch
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
            this.MagnetSearchMainMainPanel = new System.Windows.Forms.Panel();
            this.MagnetSearchUpperPanel = new System.Windows.Forms.Panel();
            this.MagetnetSearchDownloadBtn = new System.Windows.Forms.Button();
            this.MagnetSearchCopyBtn = new System.Windows.Forms.Button();
            this.MagnetSearchGreaterRadio = new System.Windows.Forms.RadioButton();
            this.MagnetSearchNotExistRadio = new System.Windows.Forms.RadioButton();
            this.MagnetSearchAllRadio = new System.Windows.Forms.RadioButton();
            this.MagnetSearchInfoLabel = new System.Windows.Forms.Label();
            this.MagnetSearchUpperPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MagnetSearchMainMainPanel
            // 
            this.MagnetSearchMainMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MagnetSearchMainMainPanel.Location = new System.Drawing.Point(0, 89);
            this.MagnetSearchMainMainPanel.Name = "MagnetSearchMainMainPanel";
            this.MagnetSearchMainMainPanel.Size = new System.Drawing.Size(2542, 939);
            this.MagnetSearchMainMainPanel.TabIndex = 0;
            // 
            // MagnetSearchUpperPanel
            // 
            this.MagnetSearchUpperPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MagnetSearchUpperPanel.Controls.Add(this.MagnetSearchInfoLabel);
            this.MagnetSearchUpperPanel.Controls.Add(this.MagnetSearchAllRadio);
            this.MagnetSearchUpperPanel.Controls.Add(this.MagnetSearchNotExistRadio);
            this.MagnetSearchUpperPanel.Controls.Add(this.MagnetSearchGreaterRadio);
            this.MagnetSearchUpperPanel.Controls.Add(this.MagnetSearchCopyBtn);
            this.MagnetSearchUpperPanel.Controls.Add(this.MagetnetSearchDownloadBtn);
            this.MagnetSearchUpperPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MagnetSearchUpperPanel.Location = new System.Drawing.Point(0, 0);
            this.MagnetSearchUpperPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MagnetSearchUpperPanel.Name = "MagnetSearchUpperPanel";
            this.MagnetSearchUpperPanel.Size = new System.Drawing.Size(2542, 89);
            this.MagnetSearchUpperPanel.TabIndex = 1;
            // 
            // MagetnetSearchDownloadBtn
            // 
            this.MagetnetSearchDownloadBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MagetnetSearchDownloadBtn.Location = new System.Drawing.Point(2394, 13);
            this.MagetnetSearchDownloadBtn.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MagetnetSearchDownloadBtn.Name = "MagetnetSearchDownloadBtn";
            this.MagetnetSearchDownloadBtn.Size = new System.Drawing.Size(139, 57);
            this.MagetnetSearchDownloadBtn.TabIndex = 0;
            this.MagetnetSearchDownloadBtn.Text = "下载";
            this.MagetnetSearchDownloadBtn.UseVisualStyleBackColor = true;
            this.MagetnetSearchDownloadBtn.Click += new System.EventHandler(this.MagetnetSearchDownloadBtn_Click);
            // 
            // MagnetSearchCopyBtn
            // 
            this.MagnetSearchCopyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MagnetSearchCopyBtn.Location = new System.Drawing.Point(2243, 13);
            this.MagnetSearchCopyBtn.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MagnetSearchCopyBtn.Name = "MagnetSearchCopyBtn";
            this.MagnetSearchCopyBtn.Size = new System.Drawing.Size(139, 57);
            this.MagnetSearchCopyBtn.TabIndex = 1;
            this.MagnetSearchCopyBtn.Text = "只复制";
            this.MagnetSearchCopyBtn.UseVisualStyleBackColor = true;
            this.MagnetSearchCopyBtn.Click += new System.EventHandler(this.MagnetSearchCopyBtn_Click);
            // 
            // MagnetSearchGreaterRadio
            // 
            this.MagnetSearchGreaterRadio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MagnetSearchGreaterRadio.AutoSize = true;
            this.MagnetSearchGreaterRadio.Location = new System.Drawing.Point(124, 23);
            this.MagnetSearchGreaterRadio.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MagnetSearchGreaterRadio.Name = "MagnetSearchGreaterRadio";
            this.MagnetSearchGreaterRadio.Size = new System.Drawing.Size(142, 32);
            this.MagnetSearchGreaterRadio.TabIndex = 2;
            this.MagnetSearchGreaterRadio.Text = "大于已存在";
            this.MagnetSearchGreaterRadio.UseVisualStyleBackColor = true;
            this.MagnetSearchGreaterRadio.CheckedChanged += new System.EventHandler(this.MagnetSearchGreaterRadio_CheckedChanged);
            // 
            // MagnetSearchNotExistRadio
            // 
            this.MagnetSearchNotExistRadio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MagnetSearchNotExistRadio.AutoSize = true;
            this.MagnetSearchNotExistRadio.Location = new System.Drawing.Point(295, 23);
            this.MagnetSearchNotExistRadio.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MagnetSearchNotExistRadio.Name = "MagnetSearchNotExistRadio";
            this.MagnetSearchNotExistRadio.Size = new System.Drawing.Size(142, 32);
            this.MagnetSearchNotExistRadio.TabIndex = 3;
            this.MagnetSearchNotExistRadio.Text = "大于不存在";
            this.MagnetSearchNotExistRadio.UseVisualStyleBackColor = true;
            this.MagnetSearchNotExistRadio.CheckedChanged += new System.EventHandler(this.MagnetSearchNotExistRadio_CheckedChanged);
            // 
            // MagnetSearchAllRadio
            // 
            this.MagnetSearchAllRadio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MagnetSearchAllRadio.AutoSize = true;
            this.MagnetSearchAllRadio.Checked = true;
            this.MagnetSearchAllRadio.Location = new System.Drawing.Point(20, 23);
            this.MagnetSearchAllRadio.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MagnetSearchAllRadio.Name = "MagnetSearchAllRadio";
            this.MagnetSearchAllRadio.Size = new System.Drawing.Size(79, 32);
            this.MagnetSearchAllRadio.TabIndex = 4;
            this.MagnetSearchAllRadio.TabStop = true;
            this.MagnetSearchAllRadio.Text = "全部";
            this.MagnetSearchAllRadio.UseVisualStyleBackColor = true;
            this.MagnetSearchAllRadio.CheckedChanged += new System.EventHandler(this.MagnetSearchAllRadio_CheckedChanged);
            // 
            // MagnetSearchInfoLabel
            // 
            this.MagnetSearchInfoLabel.AutoSize = true;
            this.MagnetSearchInfoLabel.Location = new System.Drawing.Point(468, 28);
            this.MagnetSearchInfoLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.MagnetSearchInfoLabel.Name = "MagnetSearchInfoLabel";
            this.MagnetSearchInfoLabel.Size = new System.Drawing.Size(0, 28);
            this.MagnetSearchInfoLabel.TabIndex = 5;
            // 
            // MagetSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2542, 1028);
            this.Controls.Add(this.MagnetSearchMainMainPanel);
            this.Controls.Add(this.MagnetSearchUpperPanel);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MagetSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MagetSearch";
            this.Load += new System.EventHandler(this.MagetSearch_Load);
            this.MagnetSearchUpperPanel.ResumeLayout(false);
            this.MagnetSearchUpperPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MagnetSearchMainMainPanel;
        private System.Windows.Forms.Panel MagnetSearchUpperPanel;
        private System.Windows.Forms.Label MagnetSearchInfoLabel;
        private System.Windows.Forms.RadioButton MagnetSearchAllRadio;
        private System.Windows.Forms.RadioButton MagnetSearchNotExistRadio;
        private System.Windows.Forms.RadioButton MagnetSearchGreaterRadio;
        private System.Windows.Forms.Button MagnetSearchCopyBtn;
        private System.Windows.Forms.Button MagetnetSearchDownloadBtn;
    }
}