
namespace AvManager
{
    partial class MagnetList
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
            this.MagnetListTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // MagnetListTableLayout
            // 
            this.MagnetListTableLayout.AutoScroll = true;
            this.MagnetListTableLayout.AutoSize = true;
            this.MagnetListTableLayout.ColumnCount = 4;
            this.MagnetListTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.MagnetListTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.MagnetListTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.MagnetListTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.MagnetListTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MagnetListTableLayout.Location = new System.Drawing.Point(0, 0);
            this.MagnetListTableLayout.Margin = new System.Windows.Forms.Padding(2);
            this.MagnetListTableLayout.Name = "MagnetListTableLayout";
            this.MagnetListTableLayout.RowCount = 1;
            this.MagnetListTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MagnetListTableLayout.Size = new System.Drawing.Size(1221, 688);
            this.MagnetListTableLayout.TabIndex = 0;
            // 
            // MagnetList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MagnetListTableLayout);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MagnetList";
            this.Size = new System.Drawing.Size(1221, 688);
            this.Load += new System.EventHandler(this.MagnetList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MagnetListTableLayout;
    }
}
