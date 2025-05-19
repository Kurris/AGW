using SqlSugar;

namespace Entities
{

    [SugarTable(TableName = "Programs")]
    public class ProgramEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string No { get; set; }

        public string Title { get; set; }

        public string DataSourceNo { get; set; }
    }
}
