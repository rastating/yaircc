using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Yaircc
{
    public partial class FavouriteServersDialog : Form
    {
        public FavouriteServersDialog()
        {
            InitializeComponent();
        }

        private void FavouriteServersDialog_Shown(object sender, EventArgs e)
        {
            this.serverTreeView.ExpandAll();
        }
    }
}
