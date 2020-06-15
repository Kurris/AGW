using AGW.Base.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AGW.Base.Components
{
    public partial class frmModule : FrmBase
    {
        public frmModule(ComponentDataGrid grid, bool Edit = false)
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.FixedSingle;
            ShowInTaskbar = false;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            _mbEdit = Edit;

            _mdataRow = grid.SelectedRows.Count > 0
                ? grid.SelectedRows[0]
                : null;

            flowLayout.FlowDirection = FlowDirection.TopDown;
            _mgrid = grid;

            this.btnOK.Click += BtnOK_Click;
            this.btnCancel.Click += BtnCancel_Click;

            if (_mbEdit)
            {
                this.Text = grid.TabPage.Text + "---编辑";
            }
            else
            {
                this.Text = grid.TabPage.Text + "---新增";
            }
        }

        private bool _mbEdit = false;

        private DataGridViewRow _mdataRow = null;

        public Action<object, EventArgs> atRefresh = null;

        void BtnOK_Click(object sender, EventArgs e)
        {
            DataTable dt = _mgrid.DataSource as DataTable;
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

            atRefresh?.Invoke(_mgrid.Toolbar, null);

            this.DialogResult = DialogResult.OK;
            this.Close();

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

        private ComponentDataGrid _mgrid = null;

        public void Initialize()
        {
            int ibottomHeight = this.panelBtn.Height;
            this.Height = ibottomHeight;

            string[] KeysValues = _mgrid.ParentKeyValues;

            string[] ChildKeys = _mgrid.PrimaryKey;


            int itenWidth = 0;
            foreach (DataGridViewColumn col in _mgrid.Columns)
            {
                ComponentLableAndControl lableAndControl = new ComponentLableAndControl();
                lableAndControl.Controls.Add(new Label() { Text = col.HeaderText, TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Left });

                ColumnInfo colinfo = col.Tag as ColumnInfo;


                Control ctrl = null;
                if (col.CellType.Equals(typeof(DataGridViewCheckBoxCell)))
                {
                    ctrl = new CheckBox() { Name = col.DataPropertyName, Dock = DockStyle.Right };
                }
                else
                {
                    ctrl = new TextBox() { Name = col.DataPropertyName, Dock = DockStyle.Right };
                    ctrl.Text = colinfo.DefaultValue;
                }

                if (_mbEdit)
                {
                    if (col.CellType.Equals(typeof(DataGridViewCheckBoxCell)))
                    {
                        CheckBox chk = ctrl as CheckBox;
                        chk.Checked = (bool)_mdataRow.Cells[ctrl.Name].Value;
                        chk.Enabled = !colinfo.ReadOnly;
                    }
                    else
                    {
                        TextBox txt = ctrl as TextBox;
                        txt.Text = _mdataRow.Cells[ctrl.Name].Value + "";
                        txt.ReadOnly = colinfo.ReadOnly;
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
                                    chk.Checked = Convert.ToBoolean(KeysValues[i]);
                                    chk.Enabled = !colinfo.ReadOnly;
                                }
                                else
                                {
                                    TextBox txt = ctrl as TextBox;
                                    txt.Text = KeysValues[i];
                                    txt.ReadOnly = colinfo.ReadOnly;
                                }
                            }
                        }
                    }
                }

                lableAndControl.Controls.Add(ctrl);

                lableAndControl.Visible = colinfo.Visible;

                flowLayout.Controls.Add(lableAndControl);
                this.Height += lableAndControl.Height * 2;
                itenWidth = lableAndControl.Width;
            }
            this.Width = itenWidth + 50;
        }
    }
}
