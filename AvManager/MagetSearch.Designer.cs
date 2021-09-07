
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
            this.ManetSearchMainPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.MagnetSearchAllRadio = new System.Windows.Forms.RadioButton();
            this.MagnetSearchNotExistRadio = new System.Windows.Forms.RadioButton();
            this.MagnetSearchGreaterRadio = new System.Windows.Forms.RadioButton();
            this.MagnetSearchCopyBtn = new System.Windows.Forms.Button();
            this.MagetnetSearchDownloadBtn = new System.Windows.Forms.Button();
            this.MagnetSearchInfoLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ManetSearchMainPanel
            // 
            this.ManetSearchMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ManetSearchMainPanel.Location = new System.Drawing.Point(0, 0);
            this.ManetSearchMainPanel.Margin = new System.Windows.Forms.Padding(2);
            this.ManetSearchMainPanel.Name = "ManetSearchMainPanel";
            this.ManetSearchMainPanel.Size = new System.Drawing.Size(1369, 624);
            this.ManetSearchMainPanel.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.MagnetSearchInfoLabel);
            this.panel1.Controls.Add(this.MagnetSearchAllRadio);
            this.panel1.Controls.Add(this.MagnetSearchNotExistRadio);
            this.panel1.Controls.Add(this.MagnetSearchGreaterRadio);
            this.panel1.Controls.Add(this.MagnetSearchCopyBtn);
            this.panel1.Controls.Add(this.MagetnetSearchDownloadBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1369, 51);
            this.panel1.TabIndex = 1;
            // 
            // MagnetSearchAllRadio
            // 
            this.MagnetSearchAllRadio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MagnetSearchAllRadio.AutoSize = true;
            this.MagnetSearchAllRadio.Checked = true;
            this.MagnetSearchAllRadio.Location = new System.Drawing.Point(11, 14);
            this.MagnetSearchAllRadio.Name = "MagnetSearchAllRadio";
            this.MagnetSearchAllRadio.Size = new System.Drawing.Size(50, 21);
            this.MagnetSearchAllRadio.TabIndex = 4;
            this.MagnetSearchAllRadio.TabStop = true;
            this.MagnetSearchAllRadio.Text = "全部";
            this.MagnetSearchAllRadio.UseVisualStyleBackColor = true;
            this.MagnetSearchAllRadio.CheckedChanged += new System.EventHandler(this.MagnetSearchAllRadio_CheckedChanged);
            // 
            // MagnetSearchNotExistRadio
            // 
            this.MagnetSearchNotExistRadio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MagnetSearchNotExistRadio.AutoSize = true;
            this.MagnetSearchNotExistRadio.Location = new System.Drawing.Point(159, 14);
            this.MagnetSearchNotExistRadio.Name = "MagnetSearchNotExistRadio";
            this.MagnetSearchNotExistRadio.Size = new System.Drawing.Size(86, 21);
            this.MagnetSearchNotExistRadio.TabIndex = 3;
            this.MagnetSearchNotExistRadio.Text = "大于不存在";
            this.MagnetSearchNotExistRadio.UseVisualStyleBackColor = true;
            this.MagnetSearchNotExistRadio.CheckedChanged += new System.EventHandler(this.MagnetSearchNotExistRadio_CheckedChanged);
            // 
            // MagnetSearchGreaterRadio
            // 
            this.MagnetSearchGreaterRadio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MagnetSearchGreaterRadio.AutoSize = true;
            this.MagnetSearchGreaterRadio.Location = new System.Drawing.Point(67, 14);
            this.MagnetSearchGreaterRadio.Name = "MagnetSearchGreaterRadio";
            this.MagnetSearchGreaterRadio.Size = new System.Drawing.Size(86, 21);
            this.MagnetSearchGreaterRadio.TabIndex = 2;
            this.MagnetSearchGreaterRadio.Text = "大于已存在";
            this.MagnetSearchGreaterRadio.UseVisualStyleBackColor = true;
            this.MagnetSearchGreaterRadio.CheckedChanged += new System.EventHandler(this.MagnetSearchGreaterRadio_CheckedChanged);
            // 
            // MagnetSearchCopyBtn
            // 
            this.MagnetSearchCopyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MagnetSearchCopyBtn.Location = new System.Drawing.Point(1209, 8);
            this.MagnetSearchCopyBtn.Name = "MagnetSearchCopyBtn";
            this.MagnetSearchCopyBtn.Size = new System.Drawing.Size(75, 33);
            this.MagnetSearchCopyBtn.TabIndex = 1;
            this.MagnetSearchCopyBtn.Text = "只复制";
            this.MagnetSearchCopyBtn.UseVisualStyleBackColor = true;
            this.MagnetSearchCopyBtn.Click += new System.EventHandler(this.MagnetSearchCopyBtn_Click);
            // 
            // MagetnetSearchDownloadBtn
            // 
            this.MagetnetSearchDownloadBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MagetnetSearchDownloadBtn.Location = new System.Drawing.Point(1290, 8);
            this.MagetnetSearchDownloadBtn.Name = "MagetnetSearchDownloadBtn";
            this.MagetnetSearchDownloadBtn.Size = new System.Drawing.Size(75, 33);
            this.MagetnetSearchDownloadBtn.TabIndex = 0;
            this.MagetnetSearchDownloadBtn.Text = "下载";
            this.MagetnetSearchDownloadBtn.UseVisualStyleBackColor = true;
            this.MagetnetSearchDownloadBtn.Click += new System.EventHandler(this.MagetnetSearchDownloadBtn_Click);
            // 
            // MagnetSearchInfoLabel
            // 
            this.MagnetSearchInfoLabel.AutoSize = true;
            this.MagnetSearchInfoLabel.Location = new System.Drawing.Point(252, 17);
            this.MagnetSearchInfoLabel.Name = "MagnetSearchInfoLabel";
            this.MagnetSearchInfoLabel.Size = new System.Drawing.Size(0, 17);
            this.MagnetSearchInfoLabel.TabIndex = 5;
            // 
            // MagetSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1369, 624);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ManetSearchMainPanel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MagetSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MagetSearch";
            this.Load += new System.EventHandler(this.MagetSearch_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ManetSearchMainPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button MagetnetSearchDownloadBtn;
        private System.Windows.Forms.Button MagnetSearchCopyBtn;
        private System.Windows.Forms.RadioButton MagnetSearchGreaterRadio;
        private System.Windows.Forms.RadioButton MagnetSearchNotExistRadio;
        private System.Windows.Forms.RadioButton MagnetSearchAllRadio;
        private System.Windows.Forms.Label MagnetSearchInfoLabel;
    }
}