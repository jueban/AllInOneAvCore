
namespace AvManager
{
    partial class AvItem
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
            this.AvItemBottomPanel = new System.Windows.Forms.Panel();
            this.AvItemInfoLabel = new System.Windows.Forms.Label();
            this.AvItemMainPanel = new System.Windows.Forms.Panel();
            this.AvItemPicBox = new System.Windows.Forms.PictureBox();
            this.AvItemBottomPanel.SuspendLayout();
            this.AvItemMainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AvItemPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // AvItemBottomPanel
            // 
            this.AvItemBottomPanel.Controls.Add(this.AvItemInfoLabel);
            this.AvItemBottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.AvItemBottomPanel.Location = new System.Drawing.Point(0, 270);
            this.AvItemBottomPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AvItemBottomPanel.Name = "AvItemBottomPanel";
            this.AvItemBottomPanel.Size = new System.Drawing.Size(453, 48);
            this.AvItemBottomPanel.TabIndex = 0;
            // 
            // AvItemInfoLabel
            // 
            this.AvItemInfoLabel.AutoSize = true;
            this.AvItemInfoLabel.Location = new System.Drawing.Point(6, 10);
            this.AvItemInfoLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.AvItemInfoLabel.Name = "AvItemInfoLabel";
            this.AvItemInfoLabel.Size = new System.Drawing.Size(73, 28);
            this.AvItemInfoLabel.TabIndex = 1;
            this.AvItemInfoLabel.Text = "label1";
            // 
            // AvItemMainPanel
            // 
            this.AvItemMainPanel.Controls.Add(this.AvItemPicBox);
            this.AvItemMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AvItemMainPanel.Location = new System.Drawing.Point(0, 0);
            this.AvItemMainPanel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AvItemMainPanel.Name = "AvItemMainPanel";
            this.AvItemMainPanel.Size = new System.Drawing.Size(453, 270);
            this.AvItemMainPanel.TabIndex = 0;
            // 
            // AvItemPicBox
            // 
            this.AvItemPicBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AvItemPicBox.Location = new System.Drawing.Point(0, 0);
            this.AvItemPicBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AvItemPicBox.Name = "AvItemPicBox";
            this.AvItemPicBox.Size = new System.Drawing.Size(453, 270);
            this.AvItemPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.AvItemPicBox.TabIndex = 0;
            this.AvItemPicBox.TabStop = false;
            this.AvItemPicBox.Click += new System.EventHandler(this.AvItemPicBox_Click);
            // 
            // AvItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.AvItemMainPanel);
            this.Controls.Add(this.AvItemBottomPanel);
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "AvItem";
            this.Size = new System.Drawing.Size(453, 318);
            this.Load += new System.EventHandler(this.AvItem_Load);
            this.AvItemBottomPanel.ResumeLayout(false);
            this.AvItemBottomPanel.PerformLayout();
            this.AvItemMainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AvItemPicBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel AvItemBottomPanel;
        private System.Windows.Forms.Panel AvItemMainPanel;
        private System.Windows.Forms.PictureBox AvItemPicBox;
        private System.Windows.Forms.Label AvItemInfoLabel;
    }
}
