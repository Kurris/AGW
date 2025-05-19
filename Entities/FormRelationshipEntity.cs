using SqlSugar;

namespace Entities
{
    [SugarTable(TableName = "FormRelationships")]
    public class FormRelationshipEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string MainProgramNo { get; set; }

        public string ProgramNo { get; set; }

        public string MainKeys { get; set; }

        public string ProgramKeys { get; set; }
    }
}
