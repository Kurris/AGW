using AGW.Base.Components;
using AGW.Base.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGW.Base.Helper
{
    public class EventHelper
    {


        public void BindingCellClickEvent(ComponentDataGrid dataGrid)
        {
            dataGrid.CellClick += CommonCellClick;
            CommonCellClick(dataGrid, null);
        }

        /// <summary>
        /// 容器单元格点击事件
        /// </summary>
        /// <param name="sender">容器</param>
        /// <param name="e">事件参数(不适用)</param>
        private void CommonCellClick(object sender, DataGridViewCellEventArgs e)
        {
            ComponentDataGrid dataGrid = sender as ComponentDataGrid;

            if (e == null)
            {
                ToolbarAddEvent(dataGrid.Toolbar);
            }

            if (dataGrid == null) throw new ArgumentNullException("当前容器为空");

            List<ComponentDataGrid> childrenGrid = dataGrid.GetChildrenDataGrid();


            var rows = dataGrid.SelectedRows;
            if (rows == null || rows.Count == 0)
            {
                foreach (ComponentDataGrid grid in childrenGrid)
                {
                    DataTable dt = grid.DataSource as DataTable;

                    if (dt == null) continue;

                    dt.Rows.Clear();
                    grid.DataSource = dt;

                    ClearRows(grid);
                }
                return;
            }

            var row = rows[0];

            foreach (ComponentDataGrid grid in childrenGrid)
            {
                string[] childKeys = grid.PrimaryKey;
                string[] parentKeys = grid.ParentPrimaryKey;
                string swherestring = GetWhereString(row, parentKeys, childKeys);

                grid.ParentKeyValues = new string[parentKeys.Length];
                for (int i = 0; i < parentKeys.Length; i++)
                {
                    grid.ParentKeyValues[i] = row.Cells[parentKeys[i]].Value + "";
                }

                int iindex = (grid.DataSource as DataTable).Namespace.LastIndexOf("where", StringComparison.OrdinalIgnoreCase);
                string sql = string.Empty;
                if (iindex < 0)
                {
                    sql = (grid.DataSource as DataTable).Namespace + "\r\n where " + swherestring;
                }
                else
                {
                    sql = (grid.DataSource as DataTable).Namespace.Substring(0, iindex).TrimEnd() + "\r\n where " + swherestring;
                }

                string tablename = (grid.DataSource as DataTable).TableName;

                DataTable dt = DBHelper.GetDataTable(sql);
                dt.Namespace = sql;
                dt.TableName = tablename;
                grid.DataSource = dt;
                grid.CellClick += CommonCellClick;
                CommonCellClick(grid, e);
            }

        }

        /// <summary>
        /// 获取Where条件
        /// </summary>
        /// <param name="drParent">父容器点击的行数据</param>
        /// <param name="arrParent">父容器的主键</param>
        /// <param name="arrChild">子容器的主键</param>
        /// <returns>where sql</returns>
        private string GetWhereString(DataGridViewRow drParent, string[] arrParent, string[] arrChild)
        {
            List<string> lisWhere = new List<string>();
            for (int i = 0; i < arrParent.Length; i++)
            {
                lisWhere.Add("tfinal." + arrChild[i] + "=" + "'" + drParent.Cells[arrParent[i]].Value + "" + "'");
            }

            return string.Join(" and ", lisWhere);
        }

        /// <summary>
        /// 清楚容器行数据+
        /// </summary>
        /// <param name="dataGrid"></param>
        private void ClearRows(ComponentDataGrid dataGrid)
        {
            List<ComponentDataGrid> childrenGrid = dataGrid.GetChildrenDataGrid();

            foreach (ComponentDataGrid grid in childrenGrid)
            {
                DataTable dt = grid.DataSource as DataTable;

                if (dt == null) continue;

                dt.Rows.Clear();
                grid.DataSource = dt;

                ClearRows(grid);
            }
        }



        private void ToolbarAddEvent(ComponentToolbar toolbar)
        {
            foreach (ToolStripButton button in toolbar.Items)
            {
                string name = button.Name;
                switch (name)
                {
                    case "add":
                        button.Click += AddClick;
                        break;
                    case "refresh":
                        button.Click += RefreshClick;
                        break;
                    case "delete":
                        button.Click += DeleteClick;
                        break;
                    case "edit":
                        button.Click += EditClick;
                        break;
                    case "custom":
                        button.Click += CustomClick;
                        break;
                }
            }
        }

        private void CustomClick(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;

            string sassamblyFullName = button.Name;
            try
            {
                string[] arrFullName = sassamblyFullName.Split(',');
                string sfielName = Path.Combine(Application.StartupPath, "Locallib", arrFullName[0] + ".dll");

                if (!File.Exists(sfielName)) throw new FileNotFoundException(sfielName);

                AssamblyHelper assamblyHelper = new AssamblyHelper(sfielName);
                assamblyHelper.LoadAssembly(sassamblyFullName);

            }
            catch (Exception)
            {
                button.Visible = false;
            }
        }

        private void EditClick(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            ComponentToolbar tool = button.GetCurrentParent() as ComponentToolbar;
            var grid = tool.DataGrid;
            var rows = grid.SelectedRows;
            if (rows == null)
            {
                MessageBox.Show("当前没有可以编辑的数据!");
                return;
            }
            frmModule form = new frmModule(grid, true);
            form.atRefresh = RefreshClick;
            form.Initialize();
            form.ShowDialog();
        }

        private void DeleteClick(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            ComponentToolbar tool = button.GetCurrentParent() as ComponentToolbar;
            var grid = tool.DataGrid;
            var rows = grid.SelectedRows;
            if (rows == null)
            {
                MessageBox.Show("当前没有可以删除的数据!");
                return;
            }
            DialogResult dr = MessageBox.Show("是否删除选中行", "title", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                List<DataGridViewRow> lisRow = new List<DataGridViewRow>();
                foreach (DataGridViewRow row in rows)
                {
                    lisRow.Add(row);
                }
                List<string> lisStr = new List<string>(lisRow.Count);
                lisRow.ForEach(new Action<DataGridViewRow>(x =>
                {
                    lisStr.Add($@"delete from {(grid.DataSource as DataTable).TableName} where fid ={(int)x.Cells["fid"].Value}");
                    grid.Rows.Remove(x);
                }));
                DBHelper.RunSql(lisStr, CommandType.Text, null);
            }
        }

        private void AddClick(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            ComponentToolbar tool = button.GetCurrentParent() as ComponentToolbar;
            var grid = tool.DataGrid;

            frmModule form = new frmModule(grid);
            form.atRefresh = RefreshClick;
            form.Initialize();
            form.ShowDialog();
        }


        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshClick(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            ComponentToolbar tool = null;
            if (button == null)
            {
                tool = sender as ComponentToolbar;
            }
            else
            {
                tool = button.GetCurrentParent() as ComponentToolbar;
            }

            string sql = (tool.DataGrid.DataSource as DataTable).Namespace;
            string tablename = (tool.DataGrid.DataSource as DataTable).TableName;
            DataTable dt = DBHelper.GetDataTable(sql);
            dt.Namespace = sql;
            dt.TableName = tablename;
            tool.DataGrid.DataSource = dt;

            if (dt != null && dt.Rows.Count > 0)
            {
                tool.DataGrid.Rows[0].Cells[0].Selected = true;
                CommonCellClick(tool.DataGrid, new DataGridViewCellEventArgs(0, 0));
            }
        }
    }
}
