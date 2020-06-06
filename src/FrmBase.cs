using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGW.Base
{
    public partial class FrmBase : Form
    {
        public FrmBase()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.Name.Equals("frmMain", StringComparison.OrdinalIgnoreCase))
            {
                this.WindowState = FormWindowState.Maximized;
            }

            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterParent;

            base.OnLoad(e);
        }
    }
}
