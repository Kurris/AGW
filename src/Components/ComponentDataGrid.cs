using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace AGW.Base.Components
{
    /// <summary>
    /// 数据容器
    /// </summary>
    [ToolboxItem(false)]
    public class ComponentDataGrid : DataGridView
    {

        /// <summary>
        /// 上一级容器
        /// </summary>
        public ComponentDataGrid PreGrid { get; set; }

        /// <summary>
        /// 下一级容器
        /// </summary>
        private readonly List<ComponentDataGrid> _mNextGrids = new List<ComponentDataGrid>();

        /// <summary>
        /// 当前页面
        /// </summary>
        public TabPage Page { get; private set; }

        /// <summary>
        /// 工具栏
        /// </summary>
        public ComponentToolbar ToolBar { get; private set; }

        /// <summary>
        /// 树
        /// </summary>
        public ComponentTree Tree { get; set; }

        /// <summary>
        /// 下一级容器数量
        /// </summary>
        public int ChildrenCount { get => _mNextGrids.Count; }

        /// <summary>
        /// 容器主键
        /// </summary>
        public string[] PrimaryKey { get; set; }

        /// <summary>
        /// 上一级容器主键
        /// </summary>
        public string[] PrePrimaryKey { get; set; }

        /// <summary>
        /// 上一级容器主键值
        /// </summary>
        public string[] PrePrimaryKeyValues { get; set; }

        /// <summary>
        /// 获取子数据控件
        /// </summary>
        /// <returns></returns>
        public List<ComponentDataGrid> GetChildrenGrid() => _mNextGrids;

        /// <summary>
        /// 添加下一级容器
        /// </summary>
        /// <param name="Grid">容器</param>
        public void AddNextGrid(ComponentDataGrid Grid)
        {
            bool bExists = _mNextGrids.Any(x => x.Name.Equals(Grid.Name, StringComparison.OrdinalIgnoreCase));
            if (bExists)
            {
                throw new ArgumentException($"Name:{Grid.Name} 已经存在!");
            }

            _mNextGrids.Add(Grid);
        }

        /// <summary>
        /// 移除匹配项的容器
        /// </summary>
        /// <param name="Name">容器Name</param>
        /// <returns>移除个数</returns>
        public int RemoveNextGrid(string Name) => _mNextGrids.RemoveAll(x => x.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// 组件绑定工具栏
        /// </summary>
        /// <param name="ToolBar">工具栏</param>
        public void BindingToolBar(ComponentToolbar ToolBar) => this.ToolBar = ToolBar;

        /// <summary>
        /// 组件绑定当前页面
        /// </summary>
        /// <param name="Page"></param>
        public void BindingTabPage(TabPage Page) => this.Page = Page;

        /// <summary>
        /// 绑定树
        /// </summary>
        /// <param name="Tree"></param>
        public void BindingTree(ComponentTree Tree) => this.Tree = Tree;
    }
}
