
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
            this.AvItemMainPanel = new System.Windows.Forms.Panel();
            this.AvItemPicBox = new System.Windows.Forms.PictureBox();
            this.AvItemInfoLabel = new System.Windows.Forms.Label();
            this.AvItemBottomPanel.SuspendLayout();
            this.AvItemMainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AvItemPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // AvItemBottomPanel
            // 
            this.AvItemBottomPanel.Controls.Add(this.AvItemInfoLabel);
            this.AvItemBottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.AvItemBottomPanel.Location = new System.Drawing.Point(0, 164);
            this.AvItemBottomPanel.Name = "AvItemBottomPanel";
            this.AvItemBottomPanel.Size = new System.Drawing.Size(244, 29);
            this.AvItemBottomPanel.TabIndex = 0;
            // 
            // AvItemMainPanel
            // 
            this.AvItemMainPanel.Controls.Add(this.AvItemPicBox);
            this.AvItemMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AvItemMainPanel.Location = new System.Drawing.Point(0, 0);
            this.AvItemMainPanel.Name = "AvItemMainPanel";
            this.AvItemMainPanel.Size = new System.Drawing.Size(244, 164);
            this.AvItemMainPanel.TabIndex = 0;
            // 
            // AvItemPicBox
            // 
            this.AvItemPicBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AvItemPicBox.Location = new System.Drawing.Point(0, 0);
            this.AvItemPicBox.Name = "AvItemPicBox";
            this.AvItemPicBox.Size = new System.Drawing.Size(244, 164);
            this.AvItemPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.AvItemPicBox.TabIndex = 0;
            this.AvItemPicBox.TabStop = false;
            this.AvItemPicBox.Click += new System.EventHandler(this.AvItemPicBox_Click);
            // 
            // AvItemInfoLabel
            // 
            this.AvItemInfoLabel.AutoSize = true;
            this.AvItemInfoLabel.Location = new System.Drawing.Point(3, 6);
            this.AvItemInfoLabel.Name = "AvItemInfoLabel";
            this.AvItemInfoLabel.Size = new System.Drawing.Size(43, 17);
            this.AvItemInfoLabel.TabIndex = 1;
            this.AvItemInfoLabel.Text = "label1";
            // 
            // AvItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.AvItemMainPanel);
            this.Controls.Add(this.AvItemBottomPanel);
            this.Name = "AvItem";
            this.Size = new System.Drawing.Size(244, 193);
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
