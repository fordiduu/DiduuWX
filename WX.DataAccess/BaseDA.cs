using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WX.Helper;

namespace WX.DataAccess
{
    public class BaseDA
    {
        /// <summary>
        /// 从Config获取需要执行的Sql语句或者存储过程
        /// </summary>
        /// <param name="commandName">唯一标示名</param>
        /// <returns></returns>
        protected string GenneralSqlFromConfig(DBCommand commandName)
        {
            return ConfigHelper.GetDBConfigItem(commandName.ToString());
        }

        protected SqlConnection SqlConn
        {
            get
            {
                SqlConnection conn = sqlhelper.GetConnection();
                return conn;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="where">查询条件</param>
        /// <param name="totalCount">总数量</param>
        /// <param name="cols">列</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        public DataTable SelectFromPaging(string tablename, int pageIndex, int pageSize, string where, out int totalCount, string cols = "*", string order = "ID DESC")
        {
            string sql = GenneralSqlFromConfig(DBCommand.Common_SP_Paging).Trim();
            SqlParameter totalRecordPara = new SqlParameter("@TotalRecord", 0);
            totalRecordPara.Direction = ParameterDirection.InputOutput;
            SqlParameter[] paras = {
                                       totalRecordPara,
                                       new SqlParameter("@TableName",(object)tablename),
                                       new SqlParameter("@ReFieldsStr",(object)cols),
                                       new SqlParameter("@OrderString",(object)order),
                                       new SqlParameter("@WhereString",(object)where),
                                       new SqlParameter("@PageSize",(object)pageSize),
                                       new SqlParameter("@PageIndex",(object)pageIndex)
                                   };
            DataTable resultDT = sqlhelper.ExecuteDataset(SqlConn, CommandType.StoredProcedure, sql, paras).Tables[0];
            totalCount = Convert.ToInt32(totalRecordPara.Value);
            return resultDT;
        }
    }
}
