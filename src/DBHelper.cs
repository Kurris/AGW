using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace AGW.Base
{
    public sealed class DBHelper
    {
        /// <summary>
        /// 数据库连接字符串,取配置文件
        /// </summary>
        private static readonly string _msConnectionString = ConfigurationManager.ConnectionStrings["SqlConn"].ConnectionString;

        //ConfigurationManager.ConnectionStrings["SQLCONN"].ConnectionString;

        /// <summary>
        /// 数据库提供者工厂实例,取配置文件
        /// </summary>
        private static readonly DbProviderFactory providerFactory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["SqlConn"].ProviderName);

        //DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["SQLCONN"].ProviderName);

        /// <summary>
        /// 构造函数
        /// </summary>
        public DBHelper()
        {
        }

        #region CreateDbCommand

        /// <summary>
        /// 创建DbCommand对象,打开连接,打开事务
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="commandType">执行类型</param>
        /// <returns>DbCommand</returns>
        private static DbCommand CreateDbCommand(string sql, CommandType commandType = CommandType.Text, DbParameter[] parameters = null)
        {
            try
            {
                DbConnection dbConnection = providerFactory.CreateConnection();
                dbConnection.ConnectionString = _msConnectionString;
                DbCommand command = providerFactory.CreateCommand();

                command.Connection = dbConnection;
                command.CommandText = sql;
                command.CommandType = commandType;
                command.CommandTimeout = 60;//秒

                if (parameters != null && parameters.Count() != 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                command.Connection.Open();

                command.Transaction = dbConnection.BeginTransaction();

                return command;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 创建不带参数的Command对象，打开连接，打开事务
        /// </summary>
        /// <param name="commandType"></param>
        /// <returns></returns>
        private static DbCommand CreateDbCommand(CommandType commandType = CommandType.Text)
        {
            try
            {
                DbConnection dbConnection = providerFactory.CreateConnection();
                dbConnection.ConnectionString = _msConnectionString;
                DbCommand command = providerFactory.CreateCommand();

                command.Connection = dbConnection;
                command.CommandType = commandType;
                command.CommandTimeout = 60;//秒

                command.Connection.Open();

                command.Transaction = dbConnection.BeginTransaction();

                return command;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CreateDbCommand

        #region RunSql

        /// <summary>
        /// 执行查询语句
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <returns>受影响的行数</returns>
        public static int RunSql(string sql)
        {
            return RunSql(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 执行查询语句
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="commandType">执行命令类型</param>
        /// <returns>受影响的行数</returns>
        public static int RunSql(string sql, CommandType commandType, DbParameter[] parameters)
        {
            using (DbCommand command = CreateDbCommand(sql, commandType, parameters))
            {
                try
                {
                    int iRes = command.ExecuteNonQuery();

                    command.Transaction.Commit();

                    return iRes;
                }
                catch (Exception)
                {
                    command.Transaction.Rollback();
                    throw;
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        /// <summary>
        /// 同一个事务中执行Sql
        /// </summary>
        /// <param name="sqls">sql list</param>
        /// <param name="commandType">执行类型</param>
        /// <param name="parameters">paras llist</param>
        /// <returns></returns>
        public static bool RunSql(List<string> sqls, CommandType commandType, List<List<DbParameter>> parameters)
        {
            using (DbCommand command = CreateDbCommand(commandType))
            {
                try
                {
                    for (int i = 0; i < sqls.Count; i++)
                    {
                        string sql = sqls[i];

                        command.CommandText = sql;

                        if (parameters != null && parameters.Count > 0)
                        {
                            List<DbParameter> paras = parameters[i];
                            if (paras != null && paras.Count > 0)
                            {
                                command.Parameters.AddRange(paras.ToArray());
                            }
                        }
                        int iResult = command.ExecuteNonQuery();
                        if (iResult < 0)
                        {
                            throw new Exception("effected rows < 0");
                        }
                        command.Parameters.Clear();
                    }

                    command.Transaction.Commit();

                    return true;
                }
                catch (Exception)
                {
                    command.Transaction.Rollback();
                    throw;
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        #endregion RunSql

        #region GetDataReader

        /// <summary>
        /// 执行查询语句,单行
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <returns>DbDataReader对象</returns>
        public static DbDataReader GetDataReader(string sql)
        {
            return GetDataReader(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 执行查询语句,单行
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <param name="commandType">执行命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>DbDataReader对象</returns>
        public static DbDataReader GetDataReader(string sql, CommandType commandType, DbParameter[] parameters)
        {
            DbCommand command = CreateDbCommand(sql, commandType, parameters);

            try
            {
                DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow);

                return reader;
            }
            catch (Exception)
            {
                command.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 执行查询语句,自定义CommandBehavior
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <param name="commandType">执行命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>DbDataReader对象</returns>
        public static DbDataReader GetDataReader(string sql, CommandBehavior commandBehavior, DbParameter[] parameters)
        {
            DbCommand command = CreateDbCommand(sql, CommandType.Text, parameters);

            try
            {
                DbDataReader reader = command.ExecuteReader(commandBehavior);

                return reader;
            }
            catch (Exception)
            {
                command.Dispose();
                throw;
            }
        }

        #endregion GetDataReader

        #region GetDataSet

        /// <summary>
        /// 执行查询语句,返回一个包含查询结果的DataSet
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string sql)
        {
            return GetDataSet(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 执行查询语句,返回一个包含查询结果的DataSet
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="commandType">执行命令类型</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string sql, CommandType commandType, DbParameter[] parameters)
        {
            using (DbCommand command = CreateDbCommand(sql, commandType, parameters))
            {
                using (DbDataAdapter adapter = providerFactory.CreateDataAdapter())
                {
                    try
                    {
                        adapter.SelectCommand = command;
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);

                        command.Transaction.Commit();

                        return ds;
                    }
                    catch (Exception)
                    {
                        command.Transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        command.Connection.Close();
                    }
                }
            }
        }

        #endregion GetDataSet

        #region GetDataTable

        /// <summary>
        /// 执行查询语句,返回一个包含查询结果的DataTable
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string sql)
        {
            return GetDataTable(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 执行查询语句,返回一个包含查询结果的DataTable
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="commandType">执行命令类型</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string sql
            , CommandType commandType = CommandType.Text
            , DbParameter[] parameters = null)
        {
            DataSet ds = GetDataSet(sql, commandType, parameters);

            try
            {
                if (ds == null || ds.Tables.Count == 0)
                {
                    return null;
                }
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GetDataTable

        #region GetScalar

        /// <summary>
        /// 执行一个查询语句，返回查询结果的首行首列
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <returns>首行首列</returns>
        public static object GetScalar(string sql)
        {
            return GetScalar(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 执行一个查询语句，返回查询结果的首行首列
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="commandType">执行命令类型</param>
        /// <returns>首行首列object</returns>
        public static object GetScalar(string sql, CommandType commandType, DbParameter[] parameters)
        {
            using (DbCommand command = CreateDbCommand(sql, commandType, parameters))
            {
                try
                {
                    object obj = command.ExecuteScalar();

                    command.Transaction.Commit();
                    return obj;
                }
                catch (Exception)
                {
                    command.Transaction.Rollback();
                    throw;
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        #endregion GetScalar

        #region AddDbParameter

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="parameterDirection">参数类型,默认为输入参数</param>
        /// <returns>DbParameter对象</returns>
        public static DbParameter AddDbParameter(string name, object value, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            DbParameter parameter = providerFactory.CreateParameter();

            parameter.Direction = parameterDirection;
            if (!name.Contains("@"))
            {
                name = "@" + name;
            }
            parameter.ParameterName = name;
            parameter.Value = value;

            return parameter;
        }

        #endregion AddDbParameter
    }
}