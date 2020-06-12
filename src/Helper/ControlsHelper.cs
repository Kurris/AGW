using AGW.Base.Components;
using System;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

/* function: ControlsHelper
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
                Text = dr["fbtntext"] + "",
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
            };
            if ("custom".Equals(dr["fbtnname"] + "", StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrEmpty(dr["fCustomName"] + ""))
            {
                button.Text = dr["fCustomName"] + "";
                button.Name = dr["fAssamblyName"] + "";
            }
            toolstrip.Items.Add(button);

            if (!string.IsNullOrEmpty(dr["fbtnimage"] + ""))
            {
                if (File.Exists(dr["fbtnimage"] + ""))
                {
                    Image image = Image.FromFile(dr["fbtnimage"] + "");
                    button.Image = image;
                }
            }
        }

        internal static ComponentDataGrid NewDataGrid(DataTable gridData, DataTable gridColInfo)
        {
            var grid = new ComponentDataGrid();
            grid.AutoGenerateColumns = false;
            grid.DataSource = gridData;


            var menu = new ContextMenuStrip();
            menu.Items.Add("查看当前SQL");
            menu.Items.Add("查看当前Table");
            menu.ItemClicked += (s, e) =>
            {
                ContextMenuStrip toolstrip = e.ClickedItem.GetCurrentParent() as ContextMenuStrip;
                ComponentDataGrid getDgv = toolstrip.SourceControl as ComponentDataGrid;
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

            InitStyle(grid, gridColInfo);

            return grid;
        }

        /// <summary>
        /// 初始化CompontentDataGrid样式
        /// </summary>
        /// <param name="MainDgv"></param>
        public static void InitStyle(ComponentDataGrid dgv, DataTable ColInfo)
        {
            dgv.ReadOnly = true;
            dgv.BackgroundColor = Color.White;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;


            dgv.AllowUserToAddRows = false;

            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedColumnHeadersBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedColumnHeadersBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedColumnHeadersBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            DataTable dt = dgv.DataSource as DataTable;
            foreach (DataColumn dc in dt.Columns)
            {
                Type type = dc.DataType;
                DataGridViewColumn col = null;
                if (type.Equals(typeof(bool)))
                {
                    col = new DataGridViewCheckBoxColumn();
                    col.CellTemplate = new DataGridViewCheckBoxCell();
                }
                else
                {
                    col = new DataGridViewTextBoxColumn();
                    col.CellTemplate = new DataGridViewTextBoxCell();
                }

                col.Name = dc.ColumnName;
                col.Visible = GetValue(ColInfo,"fvisiable" ,col.Name);
                col.Tag = GetValue(ColInfo, "freadonly", col.Name);
                col.DataPropertyName = dc.ColumnName;
                col.HeaderText = col.HeaderText = GlobalInvariant.GetLanguageByKey(dc.ColumnName);
                dgv.Columns.Add(col);
            }

            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            bool GetValue(DataTable Data, string MatchColumn,string ColumnName)
            {
                var row = Data.Select($"fcolname='{ColumnName}'").FirstOrDefault();
                if (row != null)
                {
                    return (bool)row[MatchColumn];
                }
                return false;
            }
        }
    }
}
