using SqlSugar;

namespace Entities
{
    [SugarTable(TableName = "Buttons")]
    public class ButtonEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public string Permission { get; set; }
    }
}
