using System.Data;
using System.Linq;
using System.Windows.Forms;
using AGW.Base.Helper;
using Entities;

namespace AGW.Base.Components
{
    public class ComponentPurePanel : Panel
    {
        /// <summary>
        /// 树
        /// </summary>
        public ComponentTree Tree { get; private set; }

        /// <summary>
        /// 表格
        /// </summary>
        public ComponentDataGrid DataGrid { get; set; }

        /// <summary>
        /// 初始化一个新Page
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="name">key名称</param>
        /// <param name="dt">加载的数据</param>
        public ComponentDataGrid InitializeDataGrid(string name, DataTable dt)
        {
            var toolstrip = ControlsHelper.NewToolStrip(name);
            this.Controls.Add(toolstrip);
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

            if (hasTree)
            {
                var rows = columns.Where(x => x.IsTree).ToList();
                if (hasTree)
                {
                    Tree = new ComponentTree
                    {
                        Dock = DockStyle.Left,
                        Width = 200
                    };
                    this.Controls.Add(Tree);

                    ControlsHelper.CreateTree(Tree, rows, dt);

                    EventHelper helper = new EventHelper();
                    helper.BindingTreeNodeOnClick(Tree);

                    Tree.ExpandAll();
                    Tree.BringToFront();

                    Splitter splitter = new Splitter();
                    this.Controls.Add(splitter);
                    splitter.BringToFront();
                    splitter.Dock = DockStyle.Left;

                    Tree.BindingDataGrid(dgv);

                    dgv.BindingTree(Tree);

                    //Tree.SelectedNode = Tree.Nodes[0];
                }
            }

            this.Controls.Add(dgv);
            dgv.Dock = DockStyle.Fill;
            dgv.BringToFront();

            dgv.BindingToolBar(toolstrip);
            toolstrip.BindingDataGrid(dgv);

            return dgv;
        }
    }
}
