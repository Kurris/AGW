using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Entities
{
    public static class DBHelper
    {
        public readonly static SqlSugarClient Db;

        static DBHelper()
        {
            Db = new SqlSugarClient(new ConnectionConfig
            {
                ConfigId = "db-main",

                ConnectionString = "server=192.168.199.124;port=3306;userid=root;password=dlhis123;database=agw;charset=utf8mb4;",
                DbType = DbType.MySqlConnector,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,

                MoreSettings = new ConnMoreSettings
                {
                    DisableNvarchar = true,
                },
                ConfigureExternalServices = new ConfigureExternalServices
                {
                    EntityService = (c, p) =>
                    {
                        if (!p.IsPrimarykey && c.PropertyType.IsGenericType && c.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                            || (!p.IsPrimarykey && p.PropertyInfo.PropertyType == typeof(string)))
                        {
                            p.IsNullable = true;
                        }
                    }
                }
            });

            Db.DbMaintenance.CreateDatabase();
            Db.CodeFirst.InitTables(typeof(DataSourceEntity), typeof(ButtonEntity), typeof(FormRelationshipEntity), typeof(ProgramColumnEntity),
                typeof(NavigationEntity), typeof(ProgramEntity), typeof(DataSourceDetailEntity), typeof(ToolStripEntity), typeof(LanguageEntity));
        }
    }
}
