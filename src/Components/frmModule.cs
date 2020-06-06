using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGW.Base.Components
{
    public partial class frmModule : Form
    {
        public frmModule(ComponentDataGrid grid)
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.FixedSingle;
            ShowInTaskbar = false;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            flowLayout.FlowDirection = FlowDirection.TopDown;
            _mgrid = grid;

            this.btnOK.Click += BtnOK_Click;
            this.btnCancel.Click += BtnCancel_Click;
        }

        public Action<object, EventArgs> atRefresh = null;

        private void BtnOK_Click(object sender, EventArgs e)
        {
            DataTable dt = _mgrid.DataSource as DataTable;
            string sTableName = dt.TableName;

            var reader = DBHelper.GetDataReader($"select * from {sTableName} with(nolock)");
            if (reader.Read())
            {
                DataTable Schema = reader.GetSchemaTable();
                Schema.Rows.Remove(Schema.Rows[0]);

                string sql = $@"insert into {sTableName}({GetFields(Schema)}) values({GetFields(Schema, true)})";
                DBHelper.RunSql(sql);

                atRefresh?.Invoke(_mgrid.Toolbar, null);
                if (sTableName.Equals("t_program", StringComparison.OrdinalIgnoreCase))
                {
                    var lc = flowLayout.Controls.OfType<ComponentLableAndControl>();
                    var lcfind = lc.Where(x => x.Controls["fsql"] != null).FirstOrDefault();
                    Control ctrl = lcfind.Controls["fsql"];
                    DataTable dtColumnsInfo = DBHelper.GetDataTable(ctrl.Text);
                    List<string> listStr = new List<string>(20);
                    foreach (DataRow dr in dtColumnsInfo.Rows)
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
           (<fProgramName, varchar(50),>
           ,<fField, varchar(20),>
           ,<fDefaultValue, varchar(50),>
           ,<fVisiable, bit,>
           ,<fEnable, bit,>
           ,<fLength, varchar(50),>
           ,<fEmpty, bit,>)"
;
                    }

                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

            string GetFields(DataTable collection, bool bGetValue = false)
            {
                List<string> lisStr = new List<string>(10);
                foreach (DataRow dr in collection.Rows)
                {
                    if (bGetValue)
                    {
                        var lc = flowLayout.Controls.OfType<ComponentLableAndControl>();
                        var lcfind = lc.Where(x => x.Controls[dr["ColumnName"] + ""] != null).FirstOrDefault();
                        Control ctrl = lcfind.Controls[dr["ColumnName"] + ""];
                        lisStr.Add("'" + ctrl.Text + "'");
                    }
                    else
                    {
                        lisStr.Add(dr["ColumnName"] + "");
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
