using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGW.Base.Components
{
    /// <summary>
    /// 组件实现接口
    /// </summary>
    public interface IComponentLink
    {
        /// <summary>
        /// 组件绑定数据容器
        /// </summary>
        /// <param name="grid">数据容器</param>
        void BindingDataGrid(ComponentDataGrid grid);

        /// <summary>
        /// 组件绑定当前页面
        /// </summary>
        /// <param name="page"></param>
        void BindingTabPage(TabPage page);

        /// <summary>
        /// 获取组件绑定的页面
        /// </summary>
        /// <returns></returns>
        TabPage GetBindingTabPage();

        /// <summary>
        /// 获取主键绑定的数据容器
        /// </summary>
        /// <returns></returns>
        ComponentDataGrid GetBindingDataGrid();
    }
}
