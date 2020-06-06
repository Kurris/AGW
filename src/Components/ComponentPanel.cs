using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGW.Base.Components
{
    /// <summary>
    /// 基础面板
    /// </summary>
    public class ComponentPanel : Panel
    {
        public ComponentPanel() : this(false)
        {

        }

        public ComponentPanel(bool bGeneralTree)
        {
            _mbGeneralTree = bGeneralTree;

            TabCtrl = new TabControl();
            TabCtrl.Dock = DockStyle.Fill;
            TabCtrl.BringToFront();
            this.Controls.Add(TabCtrl);
        }

        public ComponentTree Tree { get; private set; }

        public TabControl TabCtrl { get; private set; }

        private bool _mbGeneralTree = false;

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="Title">标题</param>
        /// <param name="name">key名称</param>
        /// <param name="gridData">加载的数据</param>
        public ComponentDataGrid InitializeNewTabPage(string Title, string name, DataTable gridData)
        {
            if (TabCtrl.TabPages.ContainsKey(name))
            {
                ComponentDataGrid finGrid = TabCtrl.TabPages[name].Controls.OfType<ComponentDataGrid>().FirstOrDefault();
                finGrid.DataSource = gridData;
                return finGrid;
            }

            TabPage page = ControlsHelper.NewPage(Title, name);
            TabCtrl.TabPages.Add(page);

            ComponentToolbar toolstrip = ControlsHelper.NewToolStrip(name);
            page.Controls.Add(toolstrip);
            toolstrip.BringToFront();


            if (_mbGeneralTree)
            {
                Tree = new ComponentTree();
                Tree.Dock = DockStyle.Left;
                page.Controls.Add(Tree);
                Tree.BringToFront();
            }

            ComponentDataGrid dgv = ControlsHelper.NewDataGrid(gridData);
            dgv.Name = name;
            page.Controls.Add(dgv);
            dgv.Dock = DockStyle.Fill;
            dgv.BringToFront();
            ControlsHelper.InitStyle(dgv);

            dgv.BindingToolbar(toolstrip);

            toolstrip.BindingDataGrid(dgv);
            toolstrip.BindingTabPage(page);


            if (_mbGeneralTree)
            {
                Tree.BindingDataGrid(dgv);
                Tree.BindingTabPage(page);
                _mbGeneralTree = false;
            }

            return dgv;
        }
    }
}
