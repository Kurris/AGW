using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AGW.Base.Components;
using Entities;
using Entities.Model;

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
        /// <param name="Name">Name</param>
        /// <returns>TabPage</returns>
        public static TabPage NewPage(string name, string title)
        {
            TabPage page = new TabPage()
            {
                Text = title,
                Name = name,
                BackColor = Color.White
            };

            return page;
        }

        /// <summary>
        /// 创建一个工具栏
        /// </summary>
        /// <param name="Name">工具栏Name,一般是容器数据源</param>
        /// <returns>ComponentToolbar</returns>
        internal static ComponentToolbar NewToolStrip(string name)
        {
            var toolstrip = new ComponentToolbar();

            var buttons = DBHelper.Db.Queryable<ToolStripEntity>().LeftJoin<ButtonEntity>((a, b) => a.Name == b.Name)
                   .Where(a => a.ProgramNo == name)
                   .Select((a, b) => b)
                   .ToList();

            foreach (var dr in buttons)
            {
                AddButton(toolstrip, dr);
            }
            return toolstrip;
        }

        /// <summary>
        /// 对工具栏添加按钮
        /// </summary>
        /// <param name="toolstrip"></param>
        /// <param name="dr"></param>
        private static void AddButton(ComponentToolbar toolstrip, ButtonEntity dr)
        {

            var button = new ToolStripButton()
            {
                Name = dr.Name,
                Text = dr.Title,
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
            };
            if ("custom".Equals(dr.Name, StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrEmpty(dr.Name))
            {
                button.Text = dr.Name;
                button.Name = dr.Name;
            }
            toolstrip.Items.Add(button);

            if (!string.IsNullOrEmpty(dr.Image))
            {
                if (File.Exists(dr.Image))
                {
                    Image image = Image.FromFile(dr.Image);
                    button.Image = image;
                }
            }
        }

        /// <summary>
        /// 创建一个数据容器
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        internal static ComponentDataGrid NewDataGrid(string name, DataTable dt, List<ProgramColumnEntity> columns)
        {
            var grid = new ComponentDataGrid
            {
                AutoGenerateColumns = false,
                DataSource = dt,
                Name = name
            };

            InitStyle(grid, columns);

            grid.ProgramColumns = columns.Select(x => new ProgramColumn
            {
                Name = x.Name
            }).ToList();

            return grid;
        }

        /// <summary>
        /// 初始化CompontentDataGrid样式
        /// </summary>
        /// <param name="MainDgv"></param>
        private static void InitStyle(ComponentDataGrid dgv, List<ProgramColumnEntity> columns)
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
                    col = new DataGridViewCheckBoxColumn
                    {
                        CellTemplate = new DataGridViewCheckBoxCell()
                    };
                }
                else
                {
                    col = new DataGridViewTextBoxColumn
                    {
                        CellTemplate = new DataGridViewTextBoxCell()
                    };
                }

                var colinfo = new ColumnInfo()
                {
                    ReadOnly = GetValue<bool>(columns, nameof(ProgramColumnEntity.IsReadonly), dc.ColumnName),
                    Visible = GetValue<bool>(columns, nameof(ProgramColumnEntity.IsVisible), dc.ColumnName),
                    ColumnName = dc.ColumnName,
                    DefaultValue = GetValue<string>(columns, nameof(ProgramColumnEntity.DefaultValue), dc.ColumnName),
                    Translation = GlobalInvariant.GetLanguageByKey(dc.ColumnName),
                    DataPropertyName = dc.ColumnName,
                    Num = GetValue<int>(columns, nameof(ProgramColumnEntity.Sequence), dc.ColumnName),
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

            T GetValue<T>(List<ProgramColumnEntity> Data, string MatchColumn, string ColumnName)
            {
                var row = Data.Where(x => x.Name == ColumnName).FirstOrDefault();
                if (row != null)
                {
                    return (T)row.GetType().GetProperties().FirstOrDefault(x => x.Name == MatchColumn).GetValue(row);
                }
                return default(T);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tree"></param>
        /// <param name="ColumnInfoData"></param>
        /// <param name="GridData"></param>
        public static void CreateTree(TreeView Tree, List<ProgramColumnEntity> ColumnInfoData, DataTable GridData)
        {
            var tor = ColumnInfoData.GetEnumerator();

            if (tor.MoveNext())
            {
                string sColumnName = tor.Current.Name;

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
                    sColumnName = tor.Current.Name;

                    foreach (TreeNode tn in Tree.Nodes)
                    {
                        if (tn.Name.Equals("all", StringComparison.OrdinalIgnoreCase)) continue;

                        RecusionTree(tor, tn, GridData, sColumnName);

                        //tor.Reset();
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
