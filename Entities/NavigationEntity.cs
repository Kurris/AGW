
using SqlSugar;

namespace Entities
{
    [SugarTable(TableName = "Navigations")]
    public class NavigationEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string PNo { get; set; }

        public string No { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public int Sequence { get; set; }
    }
}
