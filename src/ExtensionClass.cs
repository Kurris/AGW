using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AGW.Base.Components;

namespace AGW.Base.Extensions
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExtensionClass
    {
        /// <summary>
        /// 获取数据容器的DataTable
        /// </summary>
        /// <param name="Grid">数据容器</param>
        /// <returns></returns>
        public static DataTable GetDataTable(this ComponentDataGrid Grid)
        {
            return Grid.DataSource as DataTable;
        }
    }
}
