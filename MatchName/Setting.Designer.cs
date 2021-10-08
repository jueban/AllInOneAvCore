
namespace MatchName
{
    partial class Setting
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
            this.SkipLabel = new System.Windows.Forms.Label();
            this.Number = new System.Windows.Forms.NumericUpDown();
            this.PrefixLabel = new System.Windows.Forms.Label();
            this.SavePrefix = new System.Windows.Forms.Button();
            this.PrefixText = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Number)).BeginInit();
            this.SuspendLayout();
            // 
            // SkipLabel
            // 
            this.SkipLabel.AutoSize = true;
            this.SkipLabel.Location = new System.Drawing.Point(12, 19);
            this.SkipLabel.Name = "SkipLabel";
            this.SkipLabel.Size = new System.Drawing.Size(151, 17);
            this.SkipLabel.TabIndex = 0;
            this.SkipLabel.Text = "跳过扫描大小<=GB的文件";
            // 
            // Number
            // 
            this.Number.DecimalPlaces = 1;
            this.Number.Location = new System.Drawing.Point(183, 17);
            this.Number.Name = "Number";
            this.Number.Size = new System.Drawing.Size(120, 23);
            this.Number.TabIndex = 1;
            // 
            // PrefixLabel
            // 
            this.PrefixLabel.AutoSize = true;
            this.PrefixLabel.Location = new System.Drawing.Point(12, 59);
            this.PrefixLabel.Name = "PrefixLabel";
            this.PrefixLabel.Size = new System.Drawing.Size(117, 17);
            this.PrefixLabel.TabIndex = 2;
            this.PrefixLabel.Text = "番号前缀列表 \',\'分隔";
            // 
            // SavePrefix
            // 
            this.SavePrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SavePrefix.Location = new System.Drawing.Point(350, 470);
            this.SavePrefix.Name = "SavePrefix";
            this.SavePrefix.Size = new System.Drawing.Size(75, 28);
            this.SavePrefix.TabIndex = 4;
            this.SavePrefix.Text = "保存";
            this.SavePrefix.UseVisualStyleBackColor = true;
            this.SavePrefix.Click += new System.EventHandler(this.SavePrefix_Click);
            // 
            // PrefixText
            // 
            this.PrefixText.Location = new System.Drawing.Point(13, 80);
            this.PrefixText.Name = "PrefixText";
            this.PrefixText.Size = new System.Drawing.Size(412, 384);
            this.PrefixText.TabIndex = 5;
            this.PrefixText.Text = "";
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 502);
            this.Controls.Add(this.PrefixText);
            this.Controls.Add(this.SavePrefix);
            this.Controls.Add(this.PrefixLabel);
            this.Controls.Add(this.Number);
            this.Controls.Add(this.SkipLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Setting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.Load += new System.EventHandler(this.Setting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Number)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SkipLabel;
        private System.Windows.Forms.NumericUpDown Number;
        private System.Windows.Forms.Label PrefixLabel;
        private System.Windows.Forms.Button SavePrefix;
        private System.Windows.Forms.RichTextBox PrefixText;
    }
}