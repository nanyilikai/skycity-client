using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
    public class SqlAccess
    {
        public static MySqlConnection mySqlConnection;//连接类对象  
        private string host = "localhost";  //如果是局域网，那么写上本机的局域网IP
        private string database = "test";       
        private string userId = "root";
        private string pwd = "123";
        private string port = "3306";

        public SqlAccess()
        {
            OpenSql();
        }
        /// <summary>
        /// 设置要连接数据库得基础值
        /// </summary>
        /// <param name="host"></param>
        /// <param name="database"></param>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <param name="port"></param>
        public SqlAccess(string host,string database,string userId,string pwd,string port)
        {
            this.host = host;
            this.database = database;
            this.userId = userId;
            this.pwd = pwd;
            this.port = port;
            OpenSql();

        }
        /// <summary>  
                /// 打开数据库  
                /// </summary>  
        public void OpenSql()
        {
            try
            {
                //string.Format是将指定的 String类型的数据中的每个格式项替换为相应对象的值的文本等效项。  
                string sqlString = string.Format("Server = {0};port={4};Database = {1}; User ID = {2}; Password = {3};", host, database, userId, pwd, port);
                mySqlConnection = new MySqlConnection(sqlString);
                mySqlConnection.Open();
            }
            catch (Exception e)
            {
                throw new Exception("服务器连接失败....." + e.Message.ToString());
            }
        }
        /// <summary>  
                /// 创建表  
                /// </summary>  
                /// <param name="name">表名</param>  
                /// <param name="colName">属性列</param>  
                /// <param name="colType">属性类型</param>  
                /// <returns></returns>  
        public DataSet CreateTable(string name, string[] colName, string[] colType)
        {
            if (colName.Length != colType.Length)
            {
                throw new Exception("输入不正确：" + "columns.Length != colType.Length");
            }
            string query = "CREATE TABLE  " + name + "(" + colName[0] + " " + colType[0];
            for (int i = 1; i < colName.Length; i++)
            {
                query += "," + colName[i] + " " + colType[i];
            }
            query += ")";
            return ExecuteQuery(query);
        }
        /// <summary>  
                /// 创建具有id自增的表  
                /// </summary>  
                /// <param name="name">表名</param>  
                /// <param name="colName">属性列</param>  
                /// <param name="colType">属性列类型</param>  
                /// <returns></returns>  
        public DataSet CreateTableAutoID(string name, string[] colName, string[] colType)
        {
            if (colName.Length != colType.Length)
            {

                throw new Exception("columns.Length != colType.Length");

            }

            string query = "CREATE TABLE  " + name + " (ID INT NOT NULL AUTO_INCREMENT";

            for (int i = 0; i < colName.Length; ++i)
            {

                query += ", " + colName[i] + " " + colType[i];

            }

            query += ", PRIMARY KEY (" + colName[0] + ")" + ")";
            return ExecuteQuery(query);
        }

        /// <summary>  
                /// 查询  
                /// </summary>  
                /// <param name="tableName">表名</param>  
                /// <param name="items">需要查询的列</param>  
                /// <param name="whereColName">查询的条件列</param>  
                /// <param name="operation">条件操作符</param>  
                /// <param name="value">条件的值</param>  
                /// <returns></returns>  
        public DataSet Select(string tableName, string[] items, string[] whereColName, string[] operation, string[] value)
        {
            if (whereColName.Length != operation.Length || operation.Length != value.Length)
            {
                throw new Exception("输入不正确：" + "col.Length != operation.Length != values.Length");
            }
            string query = "SELECT " + items[0];
            for (int i = 1; i < items.Length; i++)
            {
                query += "," + items[i];
            }
            query += " FROM " + tableName;
        if(whereColName.Length>0)
            query += " WHERE " + whereColName[0] + operation[0] + "'" + value[0] + "'";
        for (int i = 1; i < whereColName.Length; i++)
            {
                query += " AND " + whereColName[i] + operation[i] + "'" + value[i] + "'";
            }
            return ExecuteQuery(query);
        }

        /// <summary>  
                /// 插入一条数据，包括所有，不适用自动累加ID。  
                /// </summary>  
                /// <param name="tableName">表名</param>  
                /// <param name="values">插入值</param>  
                /// <returns></returns>  
        public DataSet InsertInto(string tableName, string[] values)
        {
            string query = "INSERT INTO " + tableName + " VALUES ( '" + values[0] + "'";
            for (int i = 1; i < values.Length; ++i)
            {

                query += ", '" + values[i] + "'";

            }
            query += ")"; 
            return ExecuteQuery(query);
        }


        /// <summary>  
                /// 插入部分  
                /// </summary>  
                /// <param name="tableName">表名</param>  
                /// <param name="colName">属性列</param>  
                /// <param name="colValues">属性值</param>  
                /// <returns></returns>  
        public DataSet InsertInto(string tableName, string[] colName, string[] colValues)
        {

            if (colName.Length != colValues.Length)
            {

                throw new Exception("columns.Length != colType.Length");

            }

            string query = "INSERT INTO " + tableName + " (" + colName[0];
            for (int i = 1; i < colName.Length; ++i)
            {

                query += ", " + colName[i];

            }

            query += ") VALUES ( '" + colValues[0] + "'";
            for (int i = 1; i < colValues.Length; ++i)
            {

                query += ", '" + colValues[i] + "'";

            }

            query += ")"; 
            return ExecuteQuery(query);

        }

        /// <summary>  
                /// 删除  
                /// </summary>  
                /// <param name="tableName">表名</param>  
                /// <param name="colName">条件：删除列</param>  
                /// <param name="colValues">删除该列属性值所在得行</param>  
                /// <param name="flag">ture为or,false为and</param>
                /// <returns></returns>  
        public DataSet Delete(string tableName, string[] colName, string[] colValues,bool flag)
        {
            if (colName.Length != colValues.Length)
            {
                throw new Exception("输入不正确：" + "colName.Length != colValues.Length");
            }
            string operation = " AND";
            if (flag)
                operation = " OR";
            string query = "DELETE FROM " + tableName + " WHERE " + colName[0] + " = " + colValues[0];
            for (int i = 1; i < colValues.Length; ++i)
            {

                query += operation + colName[i] + " = " + colValues[i];
            }  
            return ExecuteQuery(query);
        }

        /// <summary>  
                /// 更新  
                /// </summary>  
                /// <param name="tableName">表名</param>  
                /// <param name="colName">更新列</param>  
                /// <param name="colValues">更新的值</param>  
                /// <param name="selectkey">条件：列</param>  
                /// <param name="selectvalue">条件：值</param>  
                /// <returns></returns>  
        public DataSet UpdateInto(string tableName, string[] colName, string[] colValues, string selectkey, string selectvalue)
        {
            if (colName.Length != colValues.Length)
            {
                throw new Exception("输入不正确：" + "colName.Length != colValues.Length");
            }

            string query = "UPDATE " + tableName + " SET " + colName[0] + " = " + colValues[0];

            for (int i = 1; i < colValues.Length; ++i)
            {

                query += ", " + colName[i] + " =" + colValues[i];
            }

            query += " WHERE " + selectkey + " = " + selectvalue;

            return ExecuteQuery(query);
        }

        /// <summary>  
                ///   
                /// 执行Sql语句  
                /// </summary>  
                /// <param name="sqlString">sql语句</param>  
                /// <returns></returns>  
        public DataSet ExecuteQuery(string sqlString)
        {
            if (mySqlConnection.State == ConnectionState.Open)
            {
                DataSet ds = new DataSet();
                try
                {

                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlString, mySqlConnection);
                    mySqlDataAdapter.Fill(ds);
                }
                catch (Exception e)
                {
                    throw new Exception("SQL:" + sqlString + "/n" + e.Message.ToString());
                }
                finally
                {

                }
                return ds;
            }
            return null;
        }

        public void Close()
        {

            if (mySqlConnection != null)
            {
                mySqlConnection.Close();
                mySqlConnection.Dispose();
                mySqlConnection = null;
            }

        }
    }