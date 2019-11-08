using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace DevHelp
{
    //公共类，用于连接数据库操作，执行connection语句
   public class DBhelper
    {
        //数据库连接字符串
        //private  string str = ConfigurationManager.ConnectionStrings["DevConnection"].ToString();
        private static string str;
        /// <summary>
        /// 数据库连接词
        /// </summary>
        public string ConStr
        {
            get { return str; }
            set { str = value; }
        }

       private SqlConnection connection;
        /// <summary>
        /// Connection对象
        /// </summary>
       public SqlConnection Connection
       {
           get
           {
               if (connection == null)
               {
                   connection = new SqlConnection(str);
               }
               return connection;
           }
       }
        /// <summary>
        /// 打开数据库连接
        /// </summary>
       public void OpenConnection()
       {
           if (Connection.State == ConnectionState.Closed)
           {
               Connection.Open();
           }
           else if (Connection.State == ConnectionState.Broken)
           {
               Connection.Close();
               Connection.Open();
           }
       }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
       public void CloseConnection()
       {
           if (Connection.State == ConnectionState.Open || Connection.State == ConnectionState.Broken)
           {
               Connection.Close();
           }
       }
        /// <summary>
        /// 返回所有影响的行数的第一行第一列数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteScaler(string sql)   //返回所有影响的行数的第一行第一列数据
       {
           OpenConnection();
           SqlCommand comand = new SqlCommand(sql, Connection);
           int i=Convert.ToInt32(comand.ExecuteScalar());
           return i;
       }
        /// <summary>
        /// 逐行读取
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string sql)  //逐行读取
       {
           OpenConnection();
           SqlCommand command = new SqlCommand(sql, Connection);
           SqlDataReader reader = command.ExecuteReader();
           return reader;
       }
        /// <summary>
        /// 逐行读取   参数化SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string sql,SqlParameter[] para)  //逐行读取   参数化SQL
       {
           OpenConnection();
           SqlCommand command = new SqlCommand(sql, Connection);
           command.Parameters.AddRange(para);
           SqlDataReader reader = command.ExecuteReader();
           return reader;
       }
        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)       //增删改
       {
           OpenConnection();
           SqlCommand command = new SqlCommand(sql, Connection);
           int i = command.ExecuteNonQuery();
           return i;
       }
        /// <summary>
        ///增删改    参数化SQL 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql,SqlParameter[] para)       //增删改    参数化SQL
       {
           OpenConnection();
           SqlCommand command = new SqlCommand(sql, Connection);
           command.Parameters.AddRange(para);
           int i = command.ExecuteNonQuery();
           return i;
       }
        /// <summary>
        /// 查询，填充
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql) {   //查询，填充
           OpenConnection();
           SqlCommand cmd = new SqlCommand(sql,Connection);
           SqlDataAdapter sda = new SqlDataAdapter(cmd);
           DataSet ds = new DataSet();
           sda.Fill(ds);
           return ds;
       }
        /// <summary>
        /// 查询，填充   参数化SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql,SqlParameter[] para)
       {   //查询，填充   参数化SQL
           OpenConnection();
           SqlCommand cmd = new SqlCommand(sql, Connection);
           cmd.Parameters.AddRange(para);
           SqlDataAdapter sda = new SqlDataAdapter(cmd);
           DataSet ds = new DataSet();
           sda.Fill(ds);
           return ds;
       }
    }
}
