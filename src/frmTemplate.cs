using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AGW.Base.Components;

namespace AGW.Base
{
    public partial class frmTemplate : UserControl
    {
        public frmTemplate()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }

        public List<TabPage> Page
        {
            get
            {
                List<TabPage> pages = new List<TabPage>();
                foreach (TabPage page in MainTab.TabPages)
                {
                    pages.Add(page);
                }
                return pages;
            }
        }

        public TabPage SelectedTab { get => MainTab.SelectedTab; }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="Title">标题</param>
        /// <param name="name">key名称</param>
        /// <param name="gridData">加载的数据</param>
        public CompontentDataGrid InitializeNewTabPage(string Title, string name, DataTable gridData)
        {
            if (MainTab.TabPages.ContainsKey(name))
            {
                CompontentDataGrid finGrid = MainTab.TabPages[name].Controls.OfType<CompontentDataGrid>().FirstOrDefault();
                finGrid.DataSource = gridData;
                return finGrid;
            }

            TabPage page = ControlsHelper.NewPage(Title, name);
            MainTab.TabPages.Add(page);

            ComponentToolbar toolstrip = ControlsHelper.NewToolStrip(name);
            page.Controls.Add(toolstrip);
            toolstrip.BringToFront();

            CompontentDataGrid dgv = ControlsHelper.NewDataGrid(gridData);
            dgv.Name = name;
            page.Controls.Add(dgv);
            dgv.Dock = DockStyle.Fill;
            dgv.BringToFront();
            ControlsHelper.InitStyle(dgv);

            toolstrip.BindingDataGrid(dgv);
            toolstrip.BindingTabPage(page);

            return dgv;
        }
    }
}
