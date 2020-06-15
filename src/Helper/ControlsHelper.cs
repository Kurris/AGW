using AGW.Base.Components;
using System;
using System.Collections;
using System.Collections.Generic;
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

            InitStyle(grid, gridColInfo);

            return grid;
        }

        /// <summary>
        /// 初始化CompontentDataGrid样式
        /// </summary>
        /// <param name="MainDgv"></param>
        public static void InitStyle(ComponentDataGrid dgv, DataTable ColData)
        {
            dgv.ReadOnly = true;
            dgv.BackgroundColor = Color.White;

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgv.AllowUserToAddRows = false;

            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv.AdvancedColumnHeadersBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedColumnHeadersBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedColumnHeadersBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedColumnHeadersBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

            dgv.TopLeftHeaderCell.Value = "行号";


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

                var colinfo = new ColumnInfo()
                {
                    ReadOnly = GetValue<bool>(ColData, "freadonly", dc.ColumnName),
                    Visible = GetValue<bool>(ColData, "fvisiable", dc.ColumnName),
                    ColumnName = dc.ColumnName,
                    DefaultValue = GetValue<string>(ColData, "fdefaultvalue", dc.ColumnName),
                    Translation = GlobalInvariant.GetLanguageByKey(dc.ColumnName),
                    DataPropertyName = dc.ColumnName,
                    Num = GetValue<int>(ColData, "fnum", dc.ColumnName),
                };


                col.Name = colinfo.ColumnName;
                col.DisplayIndex = colinfo.Num;
                col.Visible = colinfo.Visible;
                col.ReadOnly = colinfo.ReadOnly;
                col.DataPropertyName = colinfo.ColumnName;
                col.HeaderText = colinfo.Translation;

                col.Tag = colinfo;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgv.Columns.Add(col);
            }

            T GetValue<T>(DataTable Data, string MatchColumn, string ColumnName)
            {
                var row = Data.Select($"fcolname='{ColumnName}'").FirstOrDefault();
                if (row != null)
                {
                    return (T)row[MatchColumn];
                }
                return default(T);
            }
        }


        public static void HandleTree(TreeView Tree, DataRow[] ColumnInfoData, DataTable GridData)
        {
            var tor = ColumnInfoData.GetEnumerator();

            if (tor.MoveNext())
            {
                string sColumnName = (tor.Current as DataRow)["fcolname"] + "";

                var distinctData = GridData.AsEnumerable()
               .Select(x => x[sColumnName].ToString())
               .Distinct(StringComparer.OrdinalIgnoreCase);

                foreach (string data in distinctData)
                {
                    TreeNode ctndata = new TreeNode()
                    {
                        Text = data,
                        Name = sColumnName,
                    };
                    Tree.Nodes.Add(ctndata);
                }

                if (tor.MoveNext())
                {
                    sColumnName = (tor.Current as DataRow)["fcolname"] + "";

                    foreach (TreeNode tn in Tree.Nodes)
                    {
                        if (tn.Name.Equals("all", StringComparison.OrdinalIgnoreCase)) continue;

                        RecusionTree(tor, tn, GridData, sColumnName);

                        tor.Reset();
                        tor.MoveNext();
                    }
                }
            }
        }

        private static void RecusionTree(IEnumerator Tor, TreeNode Node, DataTable GridData, string ColumnName, int MoveCount = 2)
        {
            string name = Node.Name;

            string[] arrKey = null;
            if (name.Contains(","))
                arrKey = name.Split(',');
            else
                arrKey = new string[] { name };

            string[] keyValues = new string[arrKey.Count()];

            TreeNode recursionnode = Node;

            for (int i = 0; i < arrKey.Length; i++)
            {
                string sValue = recursionnode.Text;
                keyValues[arrKey.Length - 1 - i] = sValue;
                recursionnode = Node.Parent;
            }


            var distinctData = GridData.Select(GetWhereString(arrKey, keyValues)).
                Select(x => x[ColumnName].ToString())
                .Distinct(StringComparer.OrdinalIgnoreCase);


            foreach (string data in distinctData)
            {
                TreeNode Ctn = new TreeNode()
                {
                    Text = data,
                    Name = Node.Name + "," + ColumnName,
                };

                Node.Nodes.Add(Ctn);
            }

            MoveCount++;

            foreach (TreeNode tn in Node.Nodes)
            {
                if (Tor.MoveNext())
                {
                    ColumnName = (Tor.Current as DataRow)["fcolname"] + "";

                    RecusionTree(Tor, tn, GridData, ColumnName, MoveCount);
                    Tor.Reset();
                    for (int i = 0; i < MoveCount - 1; i++)
                    {
                        Tor.MoveNext();
                    }
                }
            }

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
    }
}
