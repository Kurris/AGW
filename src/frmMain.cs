﻿using AGW.Base;
using AGW.Base.Components;
using AGW.Base.Helper;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace AGW.Main
{
    /// <summary>
    /// 主窗体
    /// </summary>
    public partial class frmMain : FrmBase
    {
        /// <summary>
        /// 主窗体
        /// </summary>
        public frmMain()
        {
            InitializeComponent();

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

            int iIndex = MainTab.SelectedIndex;

            MainTab.TabPages.RemoveAt(MainTab.SelectedIndex);

            MainTab.SelectedIndex = iIndex - 1;
        }


        /// <summary>
        /// 初始化导航栏
        /// </summary>
        private void InitNavbar()
        {
            string sSql = @"SELECT * FROM t_Navigation with(nolock) ";
            DataTable dt = DBHelper.GetDataTable(sSql);

            MainNavBar.PaintStyleKind = NavBarViewKind.NavigationPane;
            MainNavBar.Groups.Clear();

            //导航栏
            DataRow[] drNavs = dt.Select("fNavType = 1");
            foreach (DataRow dr in drNavs)
            {
                //Nav容器
                NavBarGroupControlContainer Container = new NavBarGroupControlContainer();
                //Nav分组
                NavBarGroup Group = new NavBarGroup();
                Group.Caption = dr["fName"] + "";
                Group.Name = dr["fno"] + "";
                //分组的容器
                Group.ControlContainer = Container;
                Group.GroupStyle = NavBarGroupStyle.ControlContainer;
                //添加
                MainNavBar.Groups.Add(Group);
                MainNavBar.Controls.Add(Container);

                TreeView treeview = new TreeView();
                treeview.NodeMouseDoubleClick += Treeview_NodeMouseDoubleClick;
                treeview.Dock = DockStyle.Fill;

                Container.Controls.Add(treeview);

                //取导航栏同组的数据
                string sNo = dr["fGroup"] + "";
                DataRow[] drs = dt.Select(string.Format("fGroup = '{0}' and fPid={1}", sNo, 0));

                foreach (DataRow drr in drs)
                {
                    TreeNode fnode = new TreeNode();
                    fnode.Name = string.Empty;//DataHelper.ToString(drr["fNo"]);
                    fnode.Tag = Convert.ToInt32(drr["fCid"]);
                    fnode.Text = drr["fName"] + "";
                    treeview.Nodes.Add(fnode);
                    FindChildNode(dt, fnode, Convert.ToInt32(fnode.Tag));
                }

                void FindChildNode(DataTable dtAll, TreeNode fnode, int ifid)
                {
                    DataRow[] drsNode = dtAll.Select(string.Format("fPid = {0} and fgroup='{1}'", ifid, sNo));
                    foreach (DataRow drS in drsNode)
                    {
                        if (Convert.ToInt32(drS["fPid"]) == ifid)
                        {
                            TreeNode snode = new TreeNode();
                            snode.Tag = Convert.ToInt32(drS["fCid"]);
                            snode.Text = drS["fName"] + "";
                            snode.Name = drS["fAssembly"] + "";
                            fnode.Nodes.Add(snode);
                            FindChildNode(dtAll, snode, Convert.ToInt32(snode.Tag));
                        }
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
            string sSourceName = e.Node.Name;
            if (string.IsNullOrEmpty(sSourceName)) return;

            string sSourceText = e.Node.Text;

            //检查是否已打开
            foreach (TabPage ShowedPage in MainTab.TabPages)
            {
                if (ShowedPage.Name.Equals(sSourceName, StringComparison.OrdinalIgnoreCase))
                {
                    MainTab.SelectedTab = ShowedPage;
                    return;
                }
            }

            TabPage page = ControlsHelper.NewPage(sSourceText, sSourceName);
            MainTab.TabPages.Add(page);
            MainTab.SelectedTab = page;

            InitForm(sSourceName);
        }


        /// <summary>
        /// 初始化窗体
        /// </summary>
        /// <param name="SourceFormName">窗体Name</param>
        private void InitForm(string SourceFormName)
        {
            string smainSql = $@"select * from t_program where fname ='{SourceFormName}'";
            DataTable dtProgramInfo = DBHelper.GetDataTable(smainSql);
            DataRow drProgramInfo = dtProgramInfo.Rows[0];

            string smainSql1 = $"select * from (\r\n {drProgramInfo["fSql"] + ""}\r\n)t1 where 1=1";
            DataTable dtMain = DBHelper.GetDataTable(smainSql1);

            dtMain.TableName = drProgramInfo["fTable"] + "";
            dtMain.Namespace = smainSql1;

            ComponentPanel panel = new ComponentPanel(true);

            ComponentDataGrid grid = panel.InitializeNewTabPage(drProgramInfo["fCNName"] + "", SourceFormName, dtMain);

            panel.Dock = DockStyle.Top;
            MainTab.SelectedTab.Controls.Add(panel);
            panel.BringToFront();

            string srelationSql = $@"select * from T_FormRelationships with(nolock) where fmainname = '{SourceFormName}'";
            DataTable dtRelationShip = DBHelper.GetDataTable(srelationSql);

            //无从属关系
            if (dtRelationShip == null || dtRelationShip.Rows.Count == 0)
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

            RecursiveForm(grid, dtRelationShip, SourceFormName);


            #region 设置高度
            int ictrlCount = MainTab.SelectedTab.Controls.OfType<ComponentPanel>().Count();
            int ipageHeigh = MainTab.SelectedTab.Height;
            int iResult = ipageHeigh / ictrlCount;

            foreach (Control ctrl in MainTab.SelectedTab.Controls)
            {
                if (!(ctrl is ComponentPanel)) continue;

                ComponentPanel frm = ctrl as ComponentPanel;
                frm.Height = iResult;
            }
            #endregion

            #region 点击事件绑定

            EventHelper eventHelper = new EventHelper();

            eventHelper.BindingCellClickEvent(grid);

            #endregion
        }


        private void RecursiveForm(ComponentDataGrid parentGrid, DataTable dtRelation, string mainName)
        {
            DataRow[] drs = dtRelation.Select($"fFatherName ='{mainName}'");

            //第二级是否有多个gird
            bool bSecondOnly = drs?.Count() > 1
                ? true
                : false;

            ComponentPanel tmpMain = new ComponentPanel();


            var rows = parentGrid.SelectedRows;
            var row = rows[0];

            //二级存在多个
            if (bSecondOnly)
            {
                MainTab.SelectedTab.Controls.Add(tmpMain);
                tmpMain.Dock = DockStyle.Fill;
                tmpMain.BringToFront();

                foreach (DataRow dr in drs)
                {
                    string smainSql = $@"select * from t_program where fname ='{dr["fchildname"] + "" }'";
                    DataTable dtProgramInfo = DBHelper.GetDataTable(smainSql);
                    DataRow drProgramInfo = dtProgramInfo.Rows[0];

                    string smainSql1 = drProgramInfo["fSql"].ToString().TrimEnd();

                    DataTable dtMain = DBHelper.GetDataTable(smainSql1);
                    dtMain.TableName = drProgramInfo["fTable"] + "";
                    dtMain.Namespace = $"select * from (\r\n{smainSql1}\r\n) tfinal \r\nwhere 1=1";

                    ComponentDataGrid childGrid = tmpMain.InitializeNewTabPage(drProgramInfo["fcnname"] + "", drProgramInfo["fname"] + "", dtMain);

                    var Keys = GetPrimary(dr);
                    childGrid.PreGrid = parentGrid;
                    childGrid.PrimaryKey = Keys.ChildKeys;
                    childGrid.PrePrimaryKey = Keys.ParentKeys;

                    parentGrid.AddNextGrid(childGrid);

                }
            }
            else
            {
                //从属表数据
                DataRow drRelation = drs[0];

                var Keys = GetPrimary(drRelation);

                string smainSql = $@"select * from t_program where fname ='{drRelation["fchildname"] + "" }'";
                DataTable dtProgramInfo = DBHelper.GetDataTable(smainSql);
                DataRow drProgramInfo = dtProgramInfo.Rows[0];

                string smainSql1 = drProgramInfo["fSql"] + "";
                DataTable dtMain = DBHelper.GetDataTable(smainSql1);
                dtMain.TableName = drProgramInfo["fTable"] + "";
                dtMain.Namespace = $"select * from (\r\n{smainSql1}\r\n) tfinal \r\nwhere 1=1";

                ComponentDataGrid childGrid = tmpMain.InitializeNewTabPage(drProgramInfo["fcnname"] + "", drProgramInfo["fname"] + "", dtMain);
                MainTab.SelectedTab.Controls.Add(tmpMain);
                tmpMain.BringToFront();

                childGrid.PreGrid = parentGrid;
                childGrid.PrimaryKey = Keys.ChildKeys;
                childGrid.PrePrimaryKey = Keys.ParentKeys;

                parentGrid.AddNextGrid(childGrid);

                DataRow[] drNeedRecursive = dtRelation.Select($"ffathername='{drProgramInfo["fname"] + ""}'");

                if (drNeedRecursive.Count() > 0)
                {
                    tmpMain.Dock = DockStyle.Top;
                    SplitterControl splitter = new SplitterControl();
                    MainTab.SelectedTab.Controls.Add(splitter);
                    splitter.Dock = DockStyle.Top;
                    splitter.BringToFront();

                    RecursiveForm(childGrid, dtRelation, drProgramInfo["fname"] + "");
                }
                else
                {
                    tmpMain.Dock = DockStyle.Fill;
                }
            }
        }


        private (string[] ParentKeys, string[] ChildKeys) GetPrimary(DataRow drRelation)
        {

            string[] arrParentKeys = null;
            string[] arrChildKeys = null;

            string sParentKeys = drRelation["ffatherkeys"] + "";

            if (sParentKeys.Contains(","))
            {
                arrParentKeys = sParentKeys.Split(',');
            }
            else
            {
                arrParentKeys = new string[] { sParentKeys };
            }

            string sChildKeys = drRelation["fchildkeys"] + "";

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
