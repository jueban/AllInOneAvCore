
namespace AvManager
{
    partial class Thumnail
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
            this.ThumnailPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // ThumnailPanel
            // 
            this.ThumnailPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThumnailPanel.Location = new System.Drawing.Point(0, 0);
            this.ThumnailPanel.Name = "ThumnailPanel";
            this.ThumnailPanel.Size = new System.Drawing.Size(800, 450);
            this.ThumnailPanel.TabIndex = 0;
            // 
            // Thumnail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ThumnailPanel);
            this.Name = "Thumnail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "截图";
            this.Load += new System.EventHandler(this.Thumnail_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ThumnailPanel;
    }
}