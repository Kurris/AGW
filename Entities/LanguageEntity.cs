using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Entities
{
    [SugarTable(TableName = "Languages")]
    public class LanguageEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string Key { get; set; }


        public string Cns { get; set; }

        public string Eng { get; set; }
    }
}
