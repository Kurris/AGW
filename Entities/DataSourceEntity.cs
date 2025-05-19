using SqlSugar;

namespace Entities
{
    [SugarTable(TableName = "DataSources")]
    public class DataSourceEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string No { get; set; }

        public string Name { get; set; }

        public string Sql { get; set; }

        public string Table { get; set; }
    }
}
