using System.ComponentModel;
using System.Windows.Forms;

namespace AGW.Base.Components
{
    /// <summary>
    /// 控件组合
    /// </summary>
    [ToolboxItem(false)]
    public partial class ComponentLableAndControl : UserControl
    {
        /// <summary>
        /// 控件组合
        /// </summary>
        public ComponentLableAndControl()
        {
            InitializeComponent();

            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName
        {
            get => this.label1.Text;
            private set => this.label1.Text = value;
        }

        /// <summary>
        /// 设置显示名称
        /// </summary>
        /// <param name="Name"></param>
        public void SetDisplayName(string Name)
        {
            DisplayName = Name;
        }

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="Ctrl"></param>
        public void AddControl(Control Ctrl)
        {
            Ctrl.Dock = DockStyle.Left;
            panel1.Controls.Add(Ctrl);
        }
    }
}
