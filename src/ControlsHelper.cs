using AGW.Base.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGW.Base
{
    /// <summary>
    /// 控件帮助类
    /// </summary>
    public class ControlsHelper
    {
        /// <summary>
        /// 创建一个TabPage
        /// </summary>
        /// <param name="Title">标题</param>
        /// <param name="name">TabPageName</param>
        /// <returns>TabPage</returns>
        public static TabPage NewPage(string Title, string name)
        {
            TabPage page = new TabPage()
            {
                Text = Title,
                Name = name,
                BackColor = Color.White
            };
            return page;
        }

        /// <summary>
        /// 创建一个工具栏
        /// </summary>
        /// <param name="name">工具栏Name,一般是容器数据源</param>
        /// <returns>ComponentToolbar</returns>
        internal static ComponentToolbar NewToolStrip(string name)
        {
            var toolstrip = new ComponentToolbar();

            DataTable ButtonData = DBHelper.GetDataTable($@"
select *
from t_toolstrip a WITH(NOLOCK)
LEFT JOIN T_Button b on a.fToolName = b.fBtnName
where a.fInterFaceName = '{name}'");

            if (ButtonData == null || ButtonData.Rows.Count == 0) return toolstrip;

            foreach (DataRow dr in ButtonData.Rows)
            {
                AddButton(toolstrip, dr);
            }
            return toolstrip;
        }


        private static void AddButton(ComponentToolbar toolstrip, DataRow dr)
        {
            var button = new ToolStripButton()
            {
                Name = dr["fbtnname"] + "",
                Text = dr["fbtntext"] + ""
            };
            toolstrip.Items.Add(button);

            if (!string.IsNullOrEmpty(dr["fbtnimage"] + ""))
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(dr["fbtnimage"] + "")))
                {
                    Image image = Image.FromStream(ms);
                    button.Image = image;
                }
            }
            AddEvent(button);
        }
        private static void AddEvent(ToolStripButton button)
        {
            string name = button.Name;
            switch (name)
            {
                case "add":
                    break;
                case "refresh":
                    button.Click += RefreshClick;
                    break;
                default:
                    break;
            }
        }

        private static void RefreshClick(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            ComponentToolbar tool = button.GetCurrentParent() as ComponentToolbar;
            string sql = (tool.DataGrid.DataSource as DataTable).Namespace;
            string tablename = (tool.DataGrid.DataSource as DataTable).TableName;
            DataTable dt = DBHelper.GetDataTable(sql);
            dt.Namespace = sql;
            dt.TableName = tablename;
            tool.DataGrid.DataSource = dt;
        }

        internal static CompontentDataGrid NewDataGrid(DataTable gridData)
        {
            var grid = new CompontentDataGrid()
            {
                DataSource = gridData,
                BackgroundColor = Color.White
            };

            var menu = new ContextMenuStrip();
            menu.Items.Add("查看当前SQL");
            menu.Items.Add("查看当前Table");
            menu.ItemClicked += (s, e) =>
            {
                ContextMenuStrip toolstrip = e.ClickedItem.GetCurrentParent() as ContextMenuStrip;
                CompontentDataGrid getDgv = toolstrip.SourceControl as CompontentDataGrid;
                menu.Hide();

                if (e.ClickedItem.Text.Equals("查看当前SQL"))
                {
                    MessageBox.Show((getDgv.DataSource as DataTable).Namespace);
                }
                else if (e.ClickedItem.Text.Equals("查看当前Table"))
                {
                    MessageBox.Show((getDgv.DataSource as DataTable).TableName);
                }
            };

            grid.ContextMenuStrip = menu;
            return grid;
        }

        /// <summary>
        /// 初始化CompontentDataGrid样式
        /// </summary>
        /// <param name="MainDgv"></param>
        public static void InitStyle(CompontentDataGrid dgv)
        {
            dgv.ReadOnly = true;
            dgv.BackgroundColor = Color.White;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dgv.AllowUserToAddRows = false;
            dgv.MultiSelect = false;

            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedColumnHeadersBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedColumnHeadersBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedColumnHeadersBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
        }
    }
}
