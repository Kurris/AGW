using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGW.Base.Components
{
    /// <summary>
    /// 工具栏
    /// </summary>
    public class ComponentToolbar : ToolStrip, IComponentLink
    {

        /// <summary>
        /// 数据容器
        /// </summary>
        internal ComponentDataGrid DataGrid { get; private set; }

        /// <summary>
        /// 容器页面
        /// </summary>
        internal TabPage TabPage { get; private set; }

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
