using System.ComponentModel;
using System.Windows.Forms;

namespace AGW.Base.Components
{
    /// <summary>
    /// 结构树
    /// </summary>
    [ToolboxItem(false)]
    public class ComponentTree : TreeView, IComponentLink
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
            this.Width = 300;
        }

        public ComponentDataGrid DataGrid { get; private set; }
        public TabPage TabPage { get; private set; }

        public void BindingDataGrid(ComponentDataGrid grid)
        {
            DataGrid = grid;
        }

        public void BindingTabPage(TabPage page)
        {
            TabPage = page;
        }

        public ComponentDataGrid GetBindingDataGrid() => DataGrid;

        public TabPage GetBindingTabPage() => TabPage;
    }
}
