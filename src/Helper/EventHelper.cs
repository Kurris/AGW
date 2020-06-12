using AGW.Base.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

/* function: EventHelper
 * Date:2020 06 12
 * Creator:  ligy  
 *
 * Data                                     Modifier                                    Details
 * 
 * 
 *
 *************************************************************************************************************************/

namespace AGW.Base.Helper
{
    public class EventHelper
    {

        /// <summary>
        /// bind cell on click event on data container
        /// </summary>
        /// <param name="dataGrid">container</param>
        public void BindingCellClickEvent(ComponentDataGrid dataGrid)
        {
            dataGrid.CellClick += CommonCellClick;
            CommonCellClick(dataGrid, null);
        }

        /// <summary>
        /// Data container's cell on click event
        /// </summary>
        /// <param name="sender">container</param>
        /// <param name="e">data args</param>
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
        /// Get sql where character string 
        /// </summary>
        /// <param name="drParent">The single row whick parent container had selected </param>
        /// <param name="arrParent">Parent container's primary keys</param>
        /// <param name="arrChild">Child container's primary keys</param>
        /// <returns>where character string</returns>
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
        /// Clear child container's rows
        /// </summary>
        /// <param name="dataGrid">Child container</param>
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


        /// <summary>
        /// Add events for toolbar
        /// </summary>
        /// <param name="toolbar">Toolbar</param>
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

        /// <summary>
        /// Csutom button OnClick Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Edit Button OnClick Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditClick(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            ComponentToolbar tool = button.GetCurrentParent() as ComponentToolbar;
            var grid = tool.DataGrid;
            var rows = grid.SelectedRows;
            if (rows == null || rows.Count == 0)
            {
                MessageBox.Show("当前没有可以编辑的数据!", "title", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            frmModule form = new frmModule(grid, true);
            form.atRefresh = RefreshClick;
            form.Initialize();
            form.ShowDialog();
        }

        /// <summary>
        /// Delete Button OnClick Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteClick(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            ComponentToolbar tool = button.GetCurrentParent() as ComponentToolbar;
            var grid = tool.DataGrid;
            var rows = grid.SelectedRows;
            if (rows == null || rows.Count == 0)
            {
                MessageBox.Show("当前没有可以删除的数据!", "title", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DialogResult dr = MessageBox.Show($"是否删除选中的{rows.Count}行", "title", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

        /// <summary>
        /// Add Button OnClick Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// Refresh Button OnClick Event
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
