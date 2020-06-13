using AGW.Base.Helper;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace AGW.Base.Components
{
    /// <summary>
    /// 基础面板
    /// </summary>
    [ToolboxItem(false)]
    public class ComponentPanel : Panel
    {

        public ComponentPanel(bool Main = false)
        {
            _mbMain = Main;

            TabCtrl = new TabControl();
            TabCtrl.Dock = DockStyle.Fill;
            TabCtrl.BringToFront();
            this.Controls.Add(TabCtrl);
        }

        public ComponentTree Tree { get; private set; }

        public TabControl TabCtrl { get; private set; }

        private bool _mbMain = false;

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

            DataTable dtColInfo = DBHelper.GetDataTable($@"select * from T_InterfaceColums where fInterfacename='{name}'");
            bool bgenTree = false;
            if (_mbMain)
            {
                if (dtColInfo != null && dtColInfo.Rows.Count > 0)
                {
                    DataRow[] rows = dtColInfo.Select("fInterFaceColIsTree = 1", "fnum");

                    bgenTree = rows.Count() > 0
                      ? true
                      : false;

                    if (bgenTree)
                    {
                        Tree = new ComponentTree();
                        Tree.Dock = DockStyle.Left;
                        page.Controls.Add(Tree);

                        ControlsHelper.HandleTree(Tree, rows, gridData);

                        Tree.ExpandAll();

                        Tree.BringToFront();

                        Splitter splitter = new Splitter();
                        page.Controls.Add(splitter);
                        splitter.BringToFront();
                        splitter.Dock = DockStyle.Left;
                    }
                }
            }



            ComponentDataGrid dgv = ControlsHelper.NewDataGrid(gridData, dtColInfo);
            dgv.Name = name;
            page.Controls.Add(dgv);
            dgv.Dock = DockStyle.Fill;
            dgv.BringToFront();

            dgv.BindingToolbar(toolstrip);
            dgv.BindingTabPage(page);

            toolstrip.BindingDataGrid(dgv);
            toolstrip.BindingTabPage(page);


            if (bgenTree)
            {
                Tree.BindingDataGrid(dgv);
                Tree.BindingTabPage(page);

                _mbMain = false;
            }

            return dgv;
        }
    }
}
