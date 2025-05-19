
using SqlSugar;

namespace Entities
{

    [SugarTable(TableName = "DataSourceDetails")]
    public class DataSourceDetailEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string DataSourceNo { get; set; }

        public string Field { get; set; }

        public string DefaultValue { get; set; }

        public bool Visiable { get; set; }

        public bool Enable { get; set; }

        public int Length { get; set; }

        public bool IsEmpty { get; set; }
    }
}
