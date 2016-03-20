using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WX.Helper;
using System.Data;
using System.Data.SqlClient;
using WX.Model;

namespace WX.DataAccess
{
    public class VIPDA : BaseDA
    {
        CommandType text=CommandType.Text;
        string constr = sqlhelper.GetConnSting();
        string cmdstr = string.Empty;

        /// <summary>
        /// 根据用户请求来源以及标识取得用户是第几次请求
        /// </summary>
        /// <param name="type"></param>
        /// <param name="num"></param>
        public DataTable CheckUserTimes(string type, string num, string IPAddr)
        {
            cmdstr = GenneralSqlFromConfig(DBCommand.CheckUserVilid);
            SqlParameter[] spam ={
                                    new SqlParameter("@IdentityType",type),
                                    new SqlParameter("@IdentityNum",num),
                                    new SqlParameter("@IPAddr",IPAddr)
                                };
            return sqlhelper.ExecuteDataset(constr, text, cmdstr, spam).Tables[0];
        }
        
        public DataTable GetModelById(int Id)
        {
            cmdstr = GenneralSqlFromConfig(DBCommand.GetAccountById);
            SqlParameter spam = new SqlParameter("@Id", Id);
            return sqlhelper.ExecuteDataset(constr, text, cmdstr, spam).Tables[0];
        }

        public DataTable GetNewAccount(string WebName)
        {
            cmdstr = GenneralSqlFromConfig(DBCommand.GetNewAccount);
            SqlParameter spam = new SqlParameter("@VIPName", WebName);
            return sqlhelper.ExecuteDataset(constr, text, cmdstr,spam).Tables[0];
        }

        public int WriteVIPLog(VIPLog model)
        {
            cmdstr = GenneralSqlFromConfig(DBCommand.WriteVIPLog);
            SqlParameter[] spam ={
                                    new SqlParameter("@IdentityType",model.IdentityType),
                                    new SqlParameter("@IdentityNum",model.IdentityNum),
                                    new SqlParameter("@VIPId",model.VIPId),
                                    new SqlParameter("@IPAddr",model.IPAddr)
                                };
            return sqlhelper.ExecuteNonQuery(constr, text, cmdstr, spam);
        }

        public int GetLogTimes(int Id)
        {
            cmdstr = GenneralSqlFromConfig(DBCommand.CheckAccountHasUsedTimes);
            SqlParameter spam = new SqlParameter("@VIPId", Id);
            return sqlhelper.ExecuteNonQuery(constr, text, cmdstr, spam);
        }

        public void UpdateAccountEnable(int Id)
        {
            cmdstr = GenneralSqlFromConfig(DBCommand.UpdateAccountEnable);
            SqlParameter spam = new SqlParameter("@Id", Id);
            sqlhelper.ExecuteNonQuery(constr, text, cmdstr, spam);
        }

    }
}
