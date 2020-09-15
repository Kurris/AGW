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
        /// <summary>
        /// 基础面板Ctor
        /// </summary>
        /// <param name="Main"></param>
        public ComponentPanel(bool Main = false)
        {
            _mbMain = Main;

            TabCtrl = new TabControl();
            TabCtrl.Dock = DockStyle.Fill;
            TabCtrl.BringToFront();
            this.Controls.Add(TabCtrl);
        }

        /// <summary>
        /// 是否为主(第一个)
        /// </summary>
        private bool _mbMain = false;

        /// <summary>
        /// 当前面板的树
        /// </summary>
        public ComponentTree Tree { get; private set; }

        /// <summary>
        /// 当前面板的TabControl
        /// </summary>
        public TabControl TabCtrl { get; private set; }



        /// <summary>
        /// 初始化一个新Page
        /// </summary>
        /// <param name="Title">标题</param>
        /// <param name="Name">key名称</param>
        /// <param name="Grid">加载的数据</param>
        public ComponentDataGrid InitializeNewTabPage(string Title, string Name, DataTable Grid)
        {
            //如果存在相同名称Page
            if (TabCtrl.TabPages.ContainsKey(Name))
            {
                ComponentDataGrid finGrid = TabCtrl.TabPages[Name].Controls.OfType<ComponentDataGrid>().FirstOrDefault();
                finGrid.DataSource = Grid;
                return finGrid;
            }

            TabPage page = ControlsHelper.NewPage(Title, Name);
            TabCtrl.TabPages.Add(page);

            ComponentToolbar toolstrip = ControlsHelper.NewToolStrip(Name);
            page.Controls.Add(toolstrip);
            toolstrip.BringToFront();

            DataTable dtColInfo = DBHelper.GetDataTable($@"select * from T_InterfaceColums where fInterfacename='{Name}'");

            bool bgenTree = false;

            if (!_mbMain) goto NoTreeGeneral;

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

                    ControlsHelper.CreateTree(Tree, rows, Grid);

                    EventHelper helper = new EventHelper();
                    helper.BindingTreeNodeOnClick(Tree);

                    Tree.ExpandAll();

                    Tree.BringToFront();

                    Splitter splitter = new Splitter();
                    page.Controls.Add(splitter);
                    splitter.BringToFront();
                    splitter.Dock = DockStyle.Left;
                }
            }

        //没有树结构
        NoTreeGeneral:


            ComponentDataGrid dgv = ControlsHelper.NewDataGrid(Grid, dtColInfo);
            dgv.Name = Name;
            page.Controls.Add(dgv);
            dgv.Dock = DockStyle.Fill;
            dgv.BringToFront();

            dgv.BindingToolBar(toolstrip);
            dgv.BindingTabPage(page);

            toolstrip.BindingDataGrid(dgv);
            toolstrip.BindingTabPage(page);


            if (bgenTree)
            {
                Tree.BindingDataGrid(dgv);
                Tree.BindingTabPage(page);

                dgv.BindingTree(Tree);

                _mbMain = false;
            }

            return dgv;
        }
    }
}
