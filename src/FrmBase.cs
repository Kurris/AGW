using System.Drawing;
using System.Windows.Forms;

namespace AGW.Base
{
    /// <summary>
    /// 窗体基类
    /// </summary>
    public partial class FrmBase : Form
    {
        public FrmBase()
        {
            InitializeComponent();

            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterParent;
        }
    }
}
