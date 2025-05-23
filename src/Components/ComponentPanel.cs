﻿
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using AGW.Base.Helper;
using Entities;
using Entities.Model;

namespace AGW.Base.Components
{
    /// <summary>
    /// 基础面板
    /// </summary>
    [ToolboxItem(false)]
    public class ComponentPanel : Panel
    {
        /// <summary>
        /// 是否为主(第一个)
        /// </summary>
        private bool _isMain = false;

        /// <summary>
        /// 基础面板Ctor
        /// </summary>
        /// <param name="isMain"></param>
        public ComponentPanel(bool isMain = false)
        {
            _isMain = isMain;

            Tab = new TabControl
            {
                Dock = DockStyle.Fill
            };
            Tab.BringToFront();
            this.Controls.Add(Tab);
        }

        /// <summary>
        /// 当前面板的树
        /// </summary>
        public ComponentTree Tree { get; private set; }

        /// <summary>
        /// 当前面板的TabControl
        /// </summary>
        public TabControl Tab { get; private set; }


        /// <summary>
        /// 初始化一个新Page
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="name">key名称</param>
        /// <param name="dt">加载的数据</param>
        public ComponentDataGrid InitializeNewTabPage(string name, string title, DataTable dt)
        {
            //如果存在相同名称Page
            if (Tab.TabPages.ContainsKey(name))
            {
                ComponentDataGrid finGrid = Tab.TabPages[name].Controls.OfType<ComponentDataGrid>().FirstOrDefault();
                finGrid.DataSource = dt;
                return finGrid;
            }

            var page = ControlsHelper.NewPage(name, title);
            Tab.TabPages.Add(page);

            var toolstrip = ControlsHelper.NewToolStrip(name);
            page.Controls.Add(toolstrip);
            toolstrip.BringToFront();

            var columns = DBHelper.Db.Queryable<ProgramColumnEntity>().Where(x => x.ProgramNo == name).ToList();
            if (columns.Count == 0)
            {
                var dataSourceNo = DBHelper.Db.Queryable<ProgramEntity>().Where(x => x.No == name).Select(x => x.DataSourceNo).Single();
                columns = DBHelper.Db.Queryable<DataSourceDetailEntity>().Where(x => x.DataSourceNo == dataSourceNo)
                    .OrderBy(x => x.Field)
                    .Select(x => new ProgramColumnEntity
                    {
                        Sequence = 1,
                        IsVisible = true,
                        IsTree = false,
                        Name = x.Field,
                        IsReadonly = false,
                        DefaultValue = string.Empty,
                        ProgramNo = name
                    }).ToList();
            }

            var hasTree = columns.Any(x => x.IsTree);

            var dgv = ControlsHelper.NewDataGrid(name, dt, columns);

            if (_isMain && hasTree)
            {
                var rows = columns.Where(x => x.IsTree).ToList();
                if (hasTree)
                {
                    Tree = new ComponentTree
                    {
                        Dock = DockStyle.Left,
                        Width = 200
                    };
                    page.Controls.Add(Tree);

                    ControlsHelper.CreateTree(Tree, rows, dt);

                    EventHelper helper = new EventHelper();
                    helper.BindingTreeNodeOnClick(Tree);

                    Tree.ExpandAll();
                    Tree.BringToFront();

                    Splitter splitter = new Splitter();
                    page.Controls.Add(splitter);
                    splitter.BringToFront();
                    splitter.Dock = DockStyle.Left;

                    Tree.BindingDataGrid(dgv);
                    Tree.BindingTabPage(page);

                    dgv.BindingTree(Tree);

                    _isMain = false;
                }
            }


            page.Controls.Add(dgv);
            dgv.Dock = DockStyle.Fill;
            dgv.BringToFront();

            dgv.BindingToolBar(toolstrip);
            dgv.BindingTabPage(page);

            toolstrip.BindingDataGrid(dgv);
            toolstrip.BindingTabPage(page);

            return dgv;
        }
    }
}
