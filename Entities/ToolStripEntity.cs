
using SqlSugar;

namespace Entities
{

    [SugarTable(TableName = "ToolStrips")]
    public class ToolStripEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string ProgramNo { get; set; }

        public string Name { get; set; }

        public string CustomName { get; set; }

        public string AssamblyName { get; set; }

        public string ExeName { get; set; }
    }
}
