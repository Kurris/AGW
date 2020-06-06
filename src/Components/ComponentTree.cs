using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGW.Base.Components
{
    /// <summary>
    /// 结构树
    /// </summary>
    internal class ComponentTree : TreeView, IComponentLink
    {
        /// <summary>
        /// 默认初始化一个Text:"全部";Name:"All"节点
        /// </summary>
        public ComponentTree()
        {
            this.Nodes.Add(new TreeNode()
            {
                Name = "All",
                Text = "全部"
            });
        }

        internal CompontentDataGrid DataGrid { get; private set; }
        internal TabPage TabPage { get; private set; }

        public void BindingDataGrid(CompontentDataGrid grid)
        {
            DataGrid = grid;
        }

        public void BindingTabPage(TabPage page)
        {
            TabPage = page;
        }

        public CompontentDataGrid GetBindingDataGrid() => DataGrid;

        public TabPage GetBindingTabPage() => TabPage;
    }
}
