using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGW.Base.Components
{
    /// <summary>
    /// 数据容器
    /// </summary>
    public class CompontentDataGrid : DataGridView
    {

        /// <summary>
        /// 上一级容器
        /// </summary>
        public CompontentDataGrid ParentDataGrid { get; set; }

        /// <summary>
        /// 下一级容器
        /// </summary>
        private List<CompontentDataGrid> _mchildrenDataGrid = new List<CompontentDataGrid>();

        /// <summary>
        /// 下一级容器数量
        /// </summary>
        public int ChildrenCount { get => _mchildrenDataGrid.Count; }

        /// <summary>
        /// 容器主键
        /// </summary>
        public string[] PrimaryKey { get; set; }

        /// <summary>
        /// 上一级容器主键
        /// </summary>
        public string[] ParentPrimaryKey { get; set; }

        /// <summary>
        /// 添加下一级容器
        /// </summary>
        /// <param name="dataGrid">容器</param>
        public void AddChildDataGrid(CompontentDataGrid dataGrid)
        {
            bool bExists = _mchildrenDataGrid.Any(x => x.Name.Equals(dataGrid.Name, StringComparison.OrdinalIgnoreCase));
            if (bExists)
            {
                throw new ArgumentException($"Name:{dataGrid.Name} 已经存在!");
            }

            _mchildrenDataGrid.Add(dataGrid);
        }

        /// <summary>
        /// 移除匹配项的容器
        /// </summary>
        /// <param name="nameOfDataGrid">容器Name</param>
        /// <returns>移除个数</returns>
        public int RemoveChildDataGrid(string nameOfDataGrid)
        {
            return _mchildrenDataGrid.RemoveAll(x => x.Name.Equals(nameOfDataGrid, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 获取下一级容i去
        /// </summary>
        /// <returns>容器</returns>
        public List<CompontentDataGrid> GetChildrenDataGrid() => _mchildrenDataGrid;

    }
}
