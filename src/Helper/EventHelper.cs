using AGW.Base.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
        private List<string> _mlistEventRegist = new List<string>();

        /// <summary>
        /// bind cell on click event on data container
        /// </summary>
        /// <param name="dataGrid">container</param>
        public void BindingCellClickEvent(ComponentDataGrid Grid)
        {
            Grid.CellClick += CommonCellClick;
            Grid.CellDoubleClick += CommonCellDoubleClick;
            Grid.RowPostPaint += CommonRowPostPaint;
            BindMenuStrip(Grid);

            CommonCellClick(Grid, null);
        }

        public void BindingTreeNodeOnClick(ComponentTree Tree)
        {
            Tree.AfterSelect += Tree_AfterSelect;
        }

        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode CurrentNode = e.Node;

            string sqlFliter = string.Empty;
            if (e.Node.Name.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                sqlFliter = " 1=1 ";
            }
            else
            {
                if (e.Node.Nodes.Count > 0) return;

                string sCondition = CurrentNode.Name;

                string[] arrCondition = null;
                if (sCondition.Contains(","))
                    arrCondition = sCondition.Split(',');
                else
                    arrCondition = new string[] { sCondition };

                string[] arrValue = new string[arrCondition.Length];
                for (int i = 0; i < arrCondition.Length; i++)
                {
                    string sValue = CurrentNode.Text;
                    arrValue[arrCondition.Length - 1 - i] = sValue;

                    if (CurrentNode.Parent != null)
                    {
                        CurrentNode = CurrentNode.Parent;
                    }
                }

                sqlFliter = GetWhereString(arrCondition, arrValue);

                string GetWhereString(string[] ArrKey, string[] ArrValues)
                {
                    List<string> listwhere = new List<string>();
                    for (int i = 0; i < ArrKey.Length; i++)
                    {
                        listwhere.Add($"{ArrKey[i]}='{ArrValues[i]}'");
                    }
                    return string.Join(" and ", listwhere);
                }
            }

            ComponentTree Tree = CurrentNode.TreeView as ComponentTree;

            DataTable dt = Tree.DataGrid.DataSource as DataTable;

            string Sql = dt.Namespace;

            int iindex = Sql.LastIndexOf("where", StringComparison.OrdinalIgnoreCase);
            if (iindex < 0)
            {
                Sql = Sql + "\r\n where " + sqlFliter;
            }
            else
            {
                Sql = Sql.Substring(0, iindex).TrimEnd() + "\r\n where " + sqlFliter;
            }
            dt.Namespace = Sql;

            RefreshClick(Tree.DataGrid.Toolbar, null);
        }

        private void CommonRowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            StringFormat sf = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var RowBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, grid.Font, SystemBrushes.ControlText, RowBounds, sf);
        }

        /// <summary>
        /// bind cell on double click event on data container
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommonCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e != null && e.RowIndex < 0) return;

            EditClick(sender, null);
        }


        /// <summary>
        /// Data container's cell on click event
        /// </summary>
        /// <param name="sender">container</param>
        /// <param name="e">data args</param>
        private void CommonCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e != null && e.RowIndex < 0) return;

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

                if (!_mlistEventRegist.Contains(grid.Name))
                {
                    grid.CellClick += CommonCellClick;
                    grid.CellDoubleClick += CommonCellDoubleClick;
                    grid.RowPostPaint += CommonRowPostPaint;
                    BindMenuStrip(grid);

                    _mlistEventRegist.Add(grid.Name);
                }

                CommonCellClick(grid, e);
            }

        }

        private void BindMenuStrip(DataGridView grid)
        {
            var menu = new ContextMenuStrip();
            menu.Items.Add("复制");
            menu.Items.Add("查看当前数据源");

            menu.ItemClicked += (s, e) =>
            {
                ContextMenuStrip toolstrip = e.ClickedItem.GetCurrentParent() as ContextMenuStrip;

                ComponentDataGrid getDgv = toolstrip.SourceControl as ComponentDataGrid;
                toolstrip.Hide();

                if (e.ClickedItem.Text.Equals("查看当前数据源"))
                {
                    new frmDataSource(grid.Name, (getDgv.DataSource as DataTable).Namespace).ShowDialog();
                }
                else if (e.ClickedItem.Text.Equals("复制"))
                {
                    Clipboard.SetText(getDgv.CurrentCell.Value.ToString(), TextDataFormat.UnicodeText);
                }
            };

            grid.ContextMenuStrip = menu;
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
                    default:

                        string sassamblyFullName = button.Name;
                        string[] arrFullName = sassamblyFullName.Split(',');
                        string sfielName = Path.Combine(Application.StartupPath, "Locallib", arrFullName[0] + ".dll");

                        if (!File.Exists(sfielName))
                            button.Visible = false;
                        else
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
                assamblyHelper.LoadAssembly(button, RefreshClick, arrFullName[0] + "." + arrFullName[1]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

            ComponentToolbar tool = null;
            ComponentDataGrid grid = null;

            if (button != null)
            {
                tool = button.GetCurrentParent() as ComponentToolbar;
                grid = tool.DataGrid;
            }
            else
            {
                grid = sender as ComponentDataGrid;
            }

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
                int iIndex = rows[0].Index;

                // List<DataGridViewRow> lisRow = new List<DataGridViewRow>();

                List<string> lisStr = new List<string>(rows.Count);

                foreach (DataGridViewRow row in rows)
                {
                    var rowDelete = row.DataBoundItem as DataRowView;
                    rowDelete.Delete();
                    lisStr.Add($@"delete from {(grid.DataSource as DataTable).TableName} where fid ={(int)rowDelete["fid"]}");
                }

                DBHelper.RunSql(lisStr, CommandType.Text, null);

                //删除后自动点击
                if (grid.RowCount == iIndex && grid.RowCount != 0)
                {
                    int rowIndex = iIndex - 1;
                    grid.Rows[rowIndex].Selected = true;
                    CommonCellClick(grid, new DataGridViewCellEventArgs(0, rowIndex));
                }
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
