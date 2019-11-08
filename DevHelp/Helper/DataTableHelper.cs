using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;

namespace DevHelp
{
    /// <summary>
    /// DataTable 辅助类
    /// </summary>
    public class DataTableHelper<T> where T : class, new()
    {
        public DataTableHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>  
        /// 填充对象列表：用DataTable转换成实体类
        /// </summary>  
        public List<T> FillModel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<T> modelList = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                //T model = (T)Activator.CreateInstance(typeof(T));  
                T model = new T();
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);
                    if (propertyInfo != null && dr[i] != DBNull.Value)
                        propertyInfo.SetValue(model, dr[i].ToString(), null);
                }

                modelList.Add(model);
            }
            return modelList;
        }
        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public DataTable FillDataTable(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            DataTable dt = CreateData(modelList[0]);

            foreach (T model in modelList)
            {
                DataRow dataRow = dt.NewRow();
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    object itemValue = propertyInfo.GetValue(model, null);
                    if (itemValue == null)///对空值的处理，如果为空值，就将值设置为DBNull.Value
                    {
                        itemValue = DBNull.Value;
                    }
                    dataRow[propertyInfo.Name] = itemValue;
                }
                dt.Rows.Add(dataRow);

            }
            return dt;
        }
        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public DataTable CreateData(T model)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                DataColumn dataColumn = new DataColumn();
                if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Type[] typeArray = propertyInfo.PropertyType.GetGenericArguments();
                    Type typebase = typeArray[0];
                    dataColumn = new DataColumn(propertyInfo.Name, typebase);
                }
                else
                {
                    dataColumn = new DataColumn(propertyInfo.Name, propertyInfo.PropertyType);
                }
                dataTable.Columns.Add(dataColumn);
            }
            return dataTable;
        }
        /// <summary>
        ///将datatable中数据批量插入到数据库
        /// </summary>
        /// <param name="ConnectionString">数据库连接词</param>
        /// <param name="TableName">数据库表名</param>
        /// <param name="Dt">datatable数据</param>
        public void SqlBulkCopyByDatatable(string ConnectionString, string TableName, DataTable Dt)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    try
                    {
                        sqlbulkcopy.BulkCopyTimeout = 5000;//指定超时时间 以秒为单位 默认超时时间是30秒,设置为0即不限时间
                        sqlbulkcopy.DestinationTableName = TableName;//目标数据库表名

                        for (int i = 0; i < Dt.Columns.Count; i++)
                        {
                            //映射字段名 DataTable列名 ,数据库对应列名 
                            sqlbulkcopy.ColumnMappings.Add(Dt.Columns[i].ColumnName, Dt.Columns[i].ColumnName);
                        }
                        /*               
                        //额外,可不写：设置一次性处理的行数。这个行数处理完后，会激发SqlRowsCopied()方法。默认为1
                        //这个可以用来提示用户S，qlBulkCopy的进度
                        sqlbulkcopy.NotifyAfter = 1;
                        //设置激发的SqlRowsCopied()方法，这里为sqlbulkcopy_SqlRowsCopied 
                        sqlbulkcopy.SqlRowsCopied += new SqlRowsCopiedEventHandler(bulkCopy_SqlRowsCopied);
                        */
                        sqlbulkcopy.WriteToServer(Dt);//将数据源拷备到目标数据库
                    }
                    catch (System.Exception e)
                    {
                        // throw e;
                        string eMessage = e.Message.ToString();
                        int indexLeft = eMessage.IndexOf("重复键值为 (") + 7;
                        int indexRight = eMessage.IndexOf(")。");
                        int strLength = indexRight - indexLeft;
                        if (indexLeft != -1)
                        {
                            throw new Exception("批量导入失败，存在重复记录：" + eMessage.Substring(indexLeft, strLength));
                        }
                        else
                        {
                            throw e;
                        }

                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}