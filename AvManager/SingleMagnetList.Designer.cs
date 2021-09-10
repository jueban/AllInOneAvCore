
namespace AvManager
{
    partial class SingleMagnetList
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
            this.SingleMagnetListListView = new System.Windows.Forms.ListView();
            this.Name = new System.Windows.Forms.ColumnHeader();
            this.Size = new System.Windows.Forms.ColumnHeader();
            this.DateTime = new System.Windows.Forms.ColumnHeader();
            this.FinishCount = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // SingleMagnetListListView
            // 
            this.SingleMagnetListListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Name,
            this.Size,
            this.DateTime,
            this.FinishCount});
            this.SingleMagnetListListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SingleMagnetListListView.HideSelection = false;
            this.SingleMagnetListListView.Location = new System.Drawing.Point(0, 0);
            this.SingleMagnetListListView.MultiSelect = false;
            this.SingleMagnetListListView.Name = "SingleMagnetListListView";
            this.SingleMagnetListListView.Size = new System.Drawing.Size(800, 289);
            this.SingleMagnetListListView.TabIndex = 0;
            this.SingleMagnetListListView.UseCompatibleStateImageBehavior = false;
            this.SingleMagnetListListView.View = System.Windows.Forms.View.Details;
            this.SingleMagnetListListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SingleMagnetListListView_MouseClick);
            // 
            // Name
            // 
            this.Name.Text = "名称";
            this.Name.Width = 350;
            // 
            // Size
            // 
            this.Size.Text = "大小";
            this.Size.Width = 120;
            // 
            // DateTime
            // 
            this.DateTime.Text = "日期";
            this.DateTime.Width = 150;
            // 
            // FinishCount
            // 
            this.FinishCount.Text = "完成";
            this.FinishCount.Width = 80;
            // 
            // SingleMagnetList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 289);
            this.Controls.Add(this.SingleMagnetListListView);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SingleMagnetList";
            this.Load += new System.EventHandler(this.SingleMagnetList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView SingleMagnetListListView;
        private System.Windows.Forms.ColumnHeader Name;
        private System.Windows.Forms.ColumnHeader Size;
        private System.Windows.Forms.ColumnHeader DateTime;
        private System.Windows.Forms.ColumnHeader FinishCount;
    }
}