﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AvManager
{
    public partial class Thumnail : Form
    {
        public List<List<string>> pics = new List<List<string>>();

        public Thumnail()
        {
            InitializeComponent();
        }

        public Thumnail(List<List<string>> pics)
        {
            InitializeComponent();
            this.pics = pics;
        }

        private void Thumnail_Load(object sender, EventArgs e)
        {
            var row = pics.FirstOrDefault().Count;
            var col = pics.Count;

            TableLayoutPanel tlp = new TableLayoutPanel();
            tlp.RowCount = row;
            tlp.ColumnCount = col;
            tlp.Dock = DockStyle.Fill;

            for (int i = 0; i < pics.FirstOrDefault().Count; i++)
            {
                for (int j = 0; j < pics.Count; j++)
                {
                    PictureBox pb = new PictureBox();
                    pb.Width = 200;
                    pb.Height = 150;
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    pb.Image = Image.FromFile(pics[j][i]);

                    tlp.Controls.Add(pb, j, i);
                }
            }

            this.Height = row * 150 + 50;
            this.Width = col * 200 + 30;

            this.ThumnailPanel.Controls.Add(tlp);
        }
    }
}
