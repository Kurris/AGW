using SqlSugar;

namespace Entities
{
    [SugarTable(TableName = "ProgramColumns")]
    public class ProgramColumnEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string ProgramNo { get; set; }

        public string Name { get; set; }

        public bool IsTree { get; set; } = false;

        public bool IsReadonly { get; set; } = false;

        public int Sequence { get; set; } = 0;

        public bool IsVisible { get; set; } = true;

        public string DefaultValue { get; set; } = string.Empty;
    }
}
