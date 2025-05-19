using System.Drawing;
using System.Windows.Forms;
using Entities;
using SqlSugar;

namespace AGW.Base
{
    /// <summary>
    /// 窗体基类
    /// </summary>
    public partial class FormBase : Form
    {
        public FormBase()
        {
            InitializeComponent();

            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterParent;

        }

        public static SqlSugarClient Db => DBHelper.Db;
    }
}
