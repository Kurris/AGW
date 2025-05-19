using AGW.Base;
using AGW.Base.Components;
using AGW.Base.Helper;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Entities;
using System.Collections.Generic;

namespace AGW.Main
{
    /// <summary>
    /// 主窗体
    /// </summary>
    public partial class FormMain : FormBase
    {
        /// <summary>
        /// 主窗体
        /// </summary>
        public FormMain()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += FrmMain_Load;
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Load(object sender, EventArgs e)
        {
            InitNavbar();
            MainTab.DoubleClick += MainTab_DoubleClick;
        }

        /// <summary>
        /// 双击标题打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainTab_DoubleClick(object sender, EventArgs e)
        {
            if (MainTab.SelectedTab.Name.Equals("pagehome", StringComparison.OrdinalIgnoreCase)) return;

            int index = MainTab.SelectedIndex;
            MainTab.TabPages.RemoveAt(MainTab.SelectedIndex);
            MainTab.SelectedIndex = index - 1;
        }


        /// <summary>
        /// 初始化导航栏
        /// </summary>
        private void InitNavbar()
        {
            var navigations = Db.Queryable<NavigationEntity>().ToList();

            MainNavBar.PaintStyleKind = NavBarViewKind.NavigationPane;
            MainNavBar.Groups.Clear();

            //导航栏
            foreach (var navigation in navigations.Where(x => string.IsNullOrEmpty(x.PNo)))
            {
                //Nav分组
                NavBarGroup Group = new NavBarGroup
                {
                    Caption = navigation.Name,
                    Name = navigation.Name,
                    //分组的容器
                    ControlContainer = new NavBarGroupControlContainer(),
                    GroupStyle = NavBarGroupStyle.ControlContainer
                };
                //添加
                MainNavBar.Groups.Add(Group);
                MainNavBar.Controls.Add(Group.ControlContainer);

                TreeView treeview = new TreeView();
                treeview.NodeMouseDoubleClick += Treeview_NodeMouseDoubleClick;
                treeview.Dock = DockStyle.Fill;

                Group.ControlContainer.Controls.Add(treeview);

                //取导航栏同组的数据
                var next = navigations.Where(x => x.PNo == navigation.No).ToList();

                foreach (var item in next)
                {
                    var node = new TreeNode
                    {
                        Name = item.No,
                        //fnode.Tag = Convert.ToInt32(drr.);//todo
                        Text = item.DisplayName
                    };
                    treeview.Nodes.Add(node);
                    FindChildNode(navigations, node, node.Tag?.ToString());
                }

                void FindChildNode(List<NavigationEntity> dtAll, TreeNode fnode, string pno)
                {
                    var currentNext = dtAll.Where(x => x.PNo == pno);
                    foreach (var current in currentNext)
                    {
                        //if (current.FId == ifid)
                        //{
                        //    TreeNode snode = new TreeNode();
                        //    //snode.Tag = Convert.ToInt32(drS["fCid"]);//todo
                        //    snode.Text = current.Name;
                        //    snode.Name = current.Assembly;
                        //    fnode.Nodes.Add(snode);
                        //    FindChildNode(dtAll, snode, Convert.ToInt32(snode.Tag));
                        //}
                    }
                }
                treeview.ExpandAll();
            }
        }



        /// <summary>
        /// 双击导航栏树节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Treeview_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string programNo = e.Node.Name;

            //检查是否已打开
            foreach (TabPage item in MainTab.TabPages)
            {
                if (item.Name.Equals(programNo, StringComparison.OrdinalIgnoreCase))
                {
                    MainTab.SelectedTab = item;
                    return;
                }
            }

            CreateProgram(programNo);
        }


        /// <summary>
        /// 初始化窗体
        /// </summary>
        /// <param name="sourceFormName">窗体Name</param>
        private void CreateProgram(string programNo)
        {
            var program = Db.Queryable<ProgramEntity>().Where(x => x.No == programNo).Single();
            var dataSource = Db.Queryable<DataSourceEntity>().Where(x => x.No == program.DataSourceNo).Single();
            var relations = Db.Queryable<FormRelationshipEntity>().Where(x => x.MainProgramNo == programNo).ToList();

            TabPage page = ControlsHelper.NewPage(programNo, program.Title);
            MainTab.TabPages.Add(page);
            MainTab.SelectedTab = page;

            var panel = new ComponentPanel(true);
            MainTab.SelectedTab.Controls.Add(panel);

            string sql = $"select * from (\r\n {dataSource.Sql + ""}\r\n)t1 where 1=1";
            DataTable mainDt = Db.Ado.GetDataTable(sql);

            mainDt.TableName = dataSource.Table;
            mainDt.Namespace = sql;

            var purePanel = new ComponentPurePanel();
            panel.Controls.Add(purePanel);
            var grid = purePanel.InitializeDataGrid(programNo, mainDt);
            purePanel.Dock = DockStyle.Fill;
            purePanel.BringToFront();
            panel.Dock = DockStyle.Top;

            panel.BringToFront();

            //无从属关系
            if (!relations.Any())
            {
                panel.Dock = DockStyle.Fill;
                EventHelper eventHelperOnly = new EventHelper();

                eventHelperOnly.BindingCellClickEvent(grid);
                return;
            }

            SplitterControl splitter = new SplitterControl();
            MainTab.SelectedTab.Controls.Add(splitter);
            splitter.Dock = DockStyle.Top;
            splitter.BringToFront();

            RecursiveForm(grid, relations);


            #region 设置高度

            int countrolCount = MainTab.SelectedTab.Controls.OfType<Panel>().Count();
            int totalHeight = MainTab.SelectedTab.Height;
            int iResult = totalHeight / countrolCount;

            foreach (var item in MainTab.SelectedTab.Controls.OfType<Panel>())
            {
                item.Height = iResult;
            }

            #endregion

            #region 点击事件绑定

            EventHelper eventHelper = new EventHelper();
            eventHelper.BindingCellClickEvent(grid);

            #endregion
        }


        private void RecursiveForm(ComponentDataGrid parentGrid, List<FormRelationshipEntity> relations)
        {
            ComponentPanel panel = new ComponentPanel();

            var rows = parentGrid.SelectedRows;
            var row = rows[0];

            MainTab.SelectedTab.Controls.Add(panel);
            panel.BringToFront();
            panel.Dock = DockStyle.Fill;

            foreach (var item in relations)
            {
                HandleOneForm(item);
            }

            void HandleOneForm(FormRelationshipEntity item)
            {
                var program = Db.Queryable<ProgramEntity>().Where(x => x.No == item.ProgramNo).Single();
                var dataSource = Db.Queryable<DataSourceEntity>().Where(x => x.No == program.DataSourceNo).Single();

                string sql = dataSource.Sql.TrimEnd();
                DataTable mainDt = Db.Ado.GetDataTable(sql);
                mainDt.TableName = dataSource.Table;
                mainDt.Namespace = $"select * from (\r\n{sql}\r\n) tfinal \r\nwhere 1=1";

                var childGrid = panel.InitializeNewTabPage(program.No, program.Title, mainDt);

                var (parentKeys, childKeys) = GetPrimary(item);
                childGrid.PreGrid = parentGrid;
                childGrid.PrimaryKey = childKeys;
                childGrid.PrePrimaryKey = parentKeys;

                parentGrid.AddNextGrid(childGrid);
            }
        }


        private (string[] parentKeys, string[] childKeys) GetPrimary(FormRelationshipEntity relation)
        {

            string[] arrParentKeys = null;
            string[] arrChildKeys = null;

            string sParentKeys = relation.MainKeys;

            if (sParentKeys.Contains(","))
            {
                arrParentKeys = sParentKeys.Split(',');
            }
            else
            {
                arrParentKeys = new string[] { sParentKeys };
            }

            string sChildKeys = relation.ProgramKeys;

            if (sChildKeys.Contains(","))
            {
                arrChildKeys = sChildKeys.Split(',');
            }
            else
            {
                arrChildKeys = new string[] { sChildKeys };
            }

            return (arrParentKeys, arrChildKeys);
        }
    }
}
