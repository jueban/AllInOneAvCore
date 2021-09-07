using Models;
using System;
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
    public partial class MagnetList : UserControl
    {
        private List<ShowMagnetSearchResult> result;
        public TableLayoutPanel tableLayout;

        public MagnetList()
        {
            InitializeComponent();
        }

        public MagnetList(List<ShowMagnetSearchResult> result)
        {
            InitializeComponent();
            this.result = result;
        }

        private void MagnetList_Load(object sender, EventArgs e)
        {
            MagnetListTableLayout.ColumnCount = 4;
            MagnetListTableLayout.GrowStyle = TableLayoutPanelGrowStyle.AddRows;

            foreach(var res in result)
            {
                MagnetItem item = new(res);

                MagnetListTableLayout.Controls.Add(item);
            }

            MagnetListTableLayout.Dock = DockStyle.Fill;
            this.tableLayout = MagnetListTableLayout;
        }
    }
}
