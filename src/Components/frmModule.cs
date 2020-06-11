using AGW.Base.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AGW.Base.Components
{
    public partial class frmModule : Form
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
            _mdataRow = grid.SelectedRows[0];

            flowLayout.FlowDirection = FlowDirection.TopDown;
            _mgrid = grid;

            this.btnOK.Click += BtnOK_Click;
            this.btnCancel.Click += BtnCancel_Click;
        }

        private bool _mbEdit = false;

        private DataGridViewRow _mdataRow = null;

        public Action<object, EventArgs> atRefresh = null;

        private void BtnOK_Click(object sender, EventArgs e)
        {
            DataTable dt = _mgrid.DataSource as DataTable;
            string sTableName = dt.TableName;

            var reader = DBHelper.GetDataReader($"select top(1) * from {sTableName} with(nolock)");
            if (reader.Read())
            {
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
if exists(select 1 from {sTableName} where {sFid}={iFid})
begin
update {sTableName}
set {GetFields(Schema, true, true)}
where {sFid}={iFid}
end

else 
begin
 insert into { sTableName} ({ GetFields(Schema)}) values({ GetFields(Schema, true)})
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
                    var sqlctrl = lc.Where(x => x.Controls["fsql"] != null).FirstOrDefault();
                    Control sqlA = sqlctrl.Controls["fsql"];
                    var infoReader = DBHelper.GetDataReader(sqlA.Text);

                    DataTable infoschema = infoReader.GetSchemaTable();
                    List<string> listStr = new List<string>(20);

                    var name = lc.Where(x => x.Controls["fName"] != null).FirstOrDefault();
                    Control ctrname = name.Controls["fName"];

                    foreach (DataRow dr in infoschema.Rows)
                    {
                        string sqlCol = $@"
                INSERT INTO [dbo].[T_ProgramInfo]
                           ([fProgramName]
                           ,[fField]
                           ,[fDefaultValue]
                           ,[fVisiable]
                           ,[fEnable]
                           ,[fLength]
                           ,[fEmpty])
                     VALUES
                           ('{ctrname.Text}'
                           ,'{dr["ColumnName"]}'
                           ,''
                           ,1
                           ,1
                           ,{Convert.ToInt32(dr["ColumnSize"])}
                           ,{Convert.ToInt32(dr["AllowDBNull"])})";

                        listStr.Add(sqlCol);
                    }
                    DBHelper.RunSql(listStr, CommandType.Text, null);

                }

                atRefresh?.Invoke(_mgrid.Toolbar, null);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }

            string GetFields(DataTable collection, bool bGetValue = false, bool bEdit = false)
            {
                List<string> lisStr = new List<string>(10);
                var lc = flowLayout.Controls.OfType<ComponentLableAndControl>();
                foreach (DataRow dr in collection.Rows)
                {
                    var lcfind = lc.Where(x => x.Controls[dr["ColumnName"] + ""] != null).FirstOrDefault();
                    Control ctrl = lcfind.Controls[dr["ColumnName"] + ""];

                    if (bEdit)
                    {
                        lisStr.Add($"{dr["ColumnName"] + ""}='{ctrl.Text}'");
                    }
                    else
                    {
                        if (bGetValue)
                        {
                            lisStr.Add("'" + ctrl.Text + "'");
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

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private ComponentDataGrid _mgrid = null;

        public void Initialize()
        {
            int iclientHeight = this.Height;
            int ibottomHeight = this.panelBtn.Height;
            this.Height = ibottomHeight;

            string[] KeysValues = _mgrid.ParentKeyValues;

            string[] ChildKeys = _mgrid.PrimaryKey;

            if (_mgrid.Columns.Count < 10)
            {
                int itenWidth = 0;
                foreach (DataGridViewColumn col in _mgrid.Columns)
                {
                    ComponentLableAndControl lableAndControl = new ComponentLableAndControl();
                    lableAndControl.Controls.Add(new Label() { Text = col.HeaderText, TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Left });

                    Control ctrl = new TextBox() { Name = col.HeaderText, Dock = DockStyle.Right };

                    if (_mbEdit)
                    {
                        ctrl.Text = _mdataRow.Cells[ctrl.Name].Value + "";
                    }
                    else
                    {
                        if (ChildKeys != null)
                        {
                            for (int i = 0; i < ChildKeys.Length; i++)
                            {
                                if (ChildKeys[i].Equals(ctrl.Name, StringComparison.OrdinalIgnoreCase))
                                {
                                    ctrl.Text = KeysValues[i];
                                }
                            }
                        }
                    }

                    lableAndControl.Controls.Add(ctrl);
                    if (ctrl.Name.Equals("fid", StringComparison.OrdinalIgnoreCase)
                        || ctrl.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
                    {
                        lableAndControl.Visible = false;
                    }

                    flowLayout.Controls.Add(lableAndControl);
                    this.Height += lableAndControl.Height * 2;
                    itenWidth = lableAndControl.Width;
                }
                this.Width = itenWidth + 50;
            }
        }
    }
}
