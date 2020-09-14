using AGW.Base.Extensions;
using AGW.Base.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace AGW.Base.Components
{
    /// <summary>
    /// 字段显示模板窗体
    /// </summary>
    public partial class frmModule : FrmBase
    {
        /// <summary>
        /// 字段显示模板窗体
        /// </summary>
        /// <param name="Grid">数据容器</param>
        /// <param name="Edit">是否为编辑模式</param>
        public frmModule(ComponentDataGrid Grid, bool Edit = false)
        {
            InitializeComponent();

            this.ShowInTaskbar = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            _mbEdit = Edit;
            _mGrid = Grid;

            //数据选择行
            _mDataRow = Grid.SelectedRows.Count > 0
                ? Grid.SelectedRows[0]
                : null;

            //控件排列方式
            flowLayout.FlowDirection = FlowDirection.TopDown;

            this.btnOK.Click += BtnOK_Click;
            this.btnCancel.Click += BtnCancel_Click;

            if (_mbEdit)
                this.Text = Grid.Page.Text + "---编辑";
            else
                this.Text = Grid.Page.Text + "---浏览";
        }

        /// <summary>
        /// 编辑模式
        /// </summary>
        private bool _mbEdit = false;

        /// <summary>
        /// 数据容器
        /// </summary>
        private ComponentDataGrid _mGrid = null;

        /// <summary>
        /// 当前数据行
        /// </summary>
        private DataGridViewRow _mDataRow = null;

        /// <summary>
        /// 刷新方法
        /// </summary>
        public Action<object, EventArgs> atRefresh = null;


        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitializeData()
        {
            try
            {
                this.Height = this.PanelBtnOKCancel.Height;

                string[] ParentKeysValues = _mGrid.PrePrimaryKeyValues;
                string[] ChildKeys = _mGrid.PrimaryKey;


                int itenWidth = 0;

                foreach (DataGridViewColumn col in _mGrid.Columns)
                {
                    ComponentLableAndControl lableAndControl = new ComponentLableAndControl();
                    lableAndControl.SetDisplayName(col.HeaderText);

                    ColumnInfo ColInfo = col.Tag as ColumnInfo;

                    Control ctrl = null;
                    if (col.CellType.Equals(typeof(DataGridViewCheckBoxCell)))
                    {
                        ctrl = new CheckBox() { Name = col.DataPropertyName, Dock = DockStyle.Right };
                    }
                    else
                    {
                        ctrl = new TextBox() { Name = col.DataPropertyName, Dock = DockStyle.Right };
                        ctrl.Text = ColInfo.DefaultValue;
                    }

                    if (_mbEdit)
                    {
                        if (col.CellType.Equals(typeof(DataGridViewCheckBoxCell)))
                        {
                            CheckBox chk = ctrl as CheckBox;
                            chk.Checked = (bool)_mDataRow.Cells[ctrl.Name].Value;
                            chk.Enabled = !ColInfo.ReadOnly;
                        }
                        else
                        {
                            TextBox txt = ctrl as TextBox;
                            txt.Text = _mDataRow.Cells[ctrl.Name].Value + "";
                            txt.ReadOnly = ColInfo.ReadOnly;
                        }
                    }
                    else
                    {
                        if (ChildKeys != null)
                        {
                            for (int i = 0; i < ChildKeys.Length; i++)
                            {
                                if (ChildKeys[i].Equals(ctrl.Name, StringComparison.OrdinalIgnoreCase))
                                {
                                    if (col.CellType.Equals(typeof(DataGridViewCheckBoxCell)))
                                    {
                                        CheckBox chk = ctrl as CheckBox;
                                        chk.Checked = Convert.ToBoolean(ParentKeysValues[i]);
                                        chk.Enabled = !ColInfo.ReadOnly;
                                    }
                                    else
                                    {
                                        TextBox txt = ctrl as TextBox;
                                        txt.Text = ParentKeysValues[i];
                                        txt.ReadOnly = ColInfo.ReadOnly;
                                    }
                                }
                            }
                        }
                    }

                    lableAndControl.AddControl(ctrl);

                    lableAndControl.Visible = ColInfo.Visible;

                    flowLayout.Controls.Add(lableAndControl);
                    this.Height += lableAndControl.Height * 2;
                    itenWidth = lableAndControl.Width;
                }
                this.Width = itenWidth + 50;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        /// <summary>
        /// 按钮确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                var dt = _mGrid.GetDataTable();
                string sTableName = dt.TableName;

                var reader = DBHelper.GetDataReader($"select top(1) * from {sTableName} with(nolock)");

                DataTable Schema = reader.GetSchemaTable();

                string sFid = Schema.Rows[0].Field<string>("ColumnName");

                var lc = flowLayout.Controls.OfType<ComponentLableAndControl>();

                var lcfind = lc.Where(x => x.Controls[sFid] != null).FirstOrDefault();
                Control ctrl = lcfind.Controls[sFid];

                Schema.Rows.Remove(Schema.Rows[0]);

                string sql = string.Empty;

                if (_mbEdit)
                {
                    int iFid = Convert.ToInt32(ctrl.Text);
                    sql = $@"
if  not exists(select 1 from {sTableName} where {sFid}={iFid})
begin
 insert into { sTableName} ({ GetFields(Schema)}) values({ GetFields(Schema, true)})
end
else
begin
update {sTableName}
set {GetFields(Schema, true, true)}
where {sFid}={iFid}
end
";
                }
                else
                {
                    sql = $@"insert into {sTableName}({GetFields(Schema)}) values({GetFields(Schema, true)})";
                }

                DBHelper.RunSql(sql);

                if (sTableName.Equals("t_program", StringComparison.OrdinalIgnoreCase))
                {
                    SpecialHandleDataSourceProgram();
                }

                atRefresh?.Invoke(_mGrid.ToolBar, null);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }

            string GetFields(DataTable collection, bool bGetValue = false, bool bEdit = false)
            {
                List<string> lisStr = new List<string>(10);
                var lc1 = flowLayout.Controls.OfType<ComponentLableAndControl>();
                foreach (DataRow dr in collection.Rows)
                {
                    var lcfind1 = lc1.Where(x => x.Controls[dr["ColumnName"] + ""] != null).FirstOrDefault();
                    Control ctrl1 = lcfind1.Controls[dr["ColumnName"] + ""];

                    if (bEdit)
                    {
                        if (ctrl1 is CheckBox)
                        {
                            lisStr.Add($"{dr["ColumnName"] + ""}='{((ctrl1 as CheckBox).Checked ? 1 : 0)}'");
                        }
                        else
                        {
                            lisStr.Add($"{dr["ColumnName"] + ""}='{ctrl1.Text}'");
                        }
                    }
                    else
                    {
                        if (bGetValue)
                        {
                            lisStr.Add("'" + ctrl1.Text + "'");
                        }
                        else
                        {
                            lisStr.Add(dr["ColumnName"] + "");
                        }
                    }
                }
                return string.Join(",", lisStr);
            }
        }

        void SpecialHandleDataSourceProgram()
        {
            ComponentLableAndControl nameOffSql = flowLayout.Controls
                .OfType<ComponentLableAndControl>()
                .Where(x => x.Controls["fsql"] != null)
                .FirstOrDefault();

            Control ctrlSql = nameOffSql.Controls["fsql"];

            ComponentLableAndControl nameOffName = flowLayout.Controls
                .OfType<ComponentLableAndControl>()
                .Where(x => x.Controls["fName"] != null)
                .FirstOrDefault();

            Control ctrlName = nameOffName.Controls["fName"];

            DataTable dtSchema = DBHelper.GetDataReader(ctrlSql.Text)?.GetSchemaTable();

            List<string> listStr = new List<string>(20);
            listStr.Add($@"
if not exists (select 1 from t_interface where finterfacename='{ctrlName.Text}')
begin
insert into t_interface(finterfacename) values('{ctrlName.Text}')
end

");


            foreach (DataRow dr in dtSchema.Rows)
            {
                string sSql = $@"
if not  exists (select 1 from T_InterfaceColums a where a.fInterfacename='{ctrlName.Text}' and a.fColName='{dr["ColumnName"] + ""}')
begin
INSERT INTO T_InterfaceColums (fColName,fInterFaceColIsTree,fInterfacename,fEmpty,fDefaultValue,fKey,fColType,fDataSource,fDataSourceCols,fDataMapCols,fNum,fVisiable)
     VALUES('{dr["ColumnName"] + ""}',0,'{ctrlName.Text}',{((bool)dr["AllowDBNull"] ? 1 : 0)},''
,{(bool.TryParse(dr["IsKey"] + "", out bool bres) ? bres ? 1 : 0 : 0)},'','','','',0,1)
end

";

                listStr.Add(sSql);
            }
            DBHelper.RunSql(listStr, CommandType.Text, null);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
