using System;

namespace AGW.Base.Components
{
    public partial class frmDataSource : FrmBase
    {
        public frmDataSource(string DataSource, string DataSql)
        {
            InitializeComponent();

            this.Load += FrmDataSource_Load;

            _msdataSource = DataSource;
            _mdataSql = DataSql;
        }

        private readonly string _msdataSource = string.Empty;
        private readonly string _mdataSql = string.Empty;

        private void FrmDataSource_Load(object sender, EventArgs e)
        {
            txtDataSource.ReadOnly = true;
            txtDataSource.Text = _msdataSource;

            txtDataSql.ReadOnly = true;
            txtDataSql.Text = _mdataSql;

            btnClose.Click += (s, eve) => this.Close();
        }
    }
}
