using AGW.Base.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGW.Base
{
    public class EventHelper
    {
        public EventHelper(CompontentDataGrid dataGrid)
        {
            _mdataGrid = dataGrid;
        }

        private CompontentDataGrid _mdataGrid = null;


        public void BindingEvent()
        {
            _mdataGrid.CellClick += CommonCellClick;
            CommonCellClick(_mdataGrid, null);
        }

        private void CommonCellClick(object sender, DataGridViewCellEventArgs e)
        {
            CompontentDataGrid dataGrid = sender as CompontentDataGrid;

            List<CompontentDataGrid> children = dataGrid.GetChildrenDataGrid();


            var rows = dataGrid.SelectedRows;
            if (rows == null || rows.Count == 0)
            {
                foreach (CompontentDataGrid item in children)
                {
                    DataTable dt = (DataTable)item.DataSource;
                    dt.Rows.Clear();
                    item.DataSource = dt;
                    ClearRows(item);
                }
                return;
            }
            var row = rows[0];

            foreach (CompontentDataGrid item in children)
            {
                string[] childKeys = item.PrimaryKey;
                string[] parentKeys = item.ParentPrimaryKey;
                string swherestring = GetWhereString(row, parentKeys, childKeys);

                int iindex = (item.DataSource as DataTable).Namespace.IndexOf("where", StringComparison.OrdinalIgnoreCase);
                string sql = string.Empty;
                if (iindex < 0)
                {
                    sql = (item.DataSource as DataTable).Namespace + " where " + swherestring;
                }
                else
                {
                    sql = (item.DataSource as DataTable).Namespace.Substring(0, iindex).TrimEnd() + " where " + swherestring;
                }

                string tablename = (item.DataSource as DataTable).TableName;

                DataTable dt = DBHelper.GetDataTable(sql);
                dt.Namespace = sql;
                dt.TableName = tablename;
                item.DataSource = dt;
                item.CellClick += CommonCellClick;
                CommonCellClick(item, e);
            }

        }

        private string GetWhereString(DataGridViewRow drMain, string[] arrMain, string[] arrChild)
        {
            List<string> lisWhere = new List<string>();
            for (int i = 0; i < arrMain.Length; i++)
            {
                lisWhere.Add(arrChild[i] + "=" + "'" + drMain.Cells[arrMain[i]].Value + "" + "'");
            }

            return string.Join(" and ", lisWhere);
        }

        private void ClearRows(CompontentDataGrid dataGrid)
        {
            List<CompontentDataGrid> children = dataGrid.GetChildrenDataGrid();

            foreach (CompontentDataGrid item in children)
            {
                DataTable dt = (DataTable)item.DataSource;
                dt.Rows.Clear();
                item.DataSource = dt;
                ClearRows(item);
            }
        }
    }
}
