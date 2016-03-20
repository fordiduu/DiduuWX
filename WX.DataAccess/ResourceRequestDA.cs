using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using WX.Helper;
using WX.Model;

namespace WX.DataAccess
{
    public class ResourceRequestDA:BaseDA
    {
        string constr = sqlhelper.GetConnSting();
        CommandType text = CommandType.Text;
        string cmdstr = string.Empty;
        public int NewResourceRequest(ReSourceRequest model)
        {
            cmdstr = GenneralSqlFromConfig(DBCommand.NewResourceRequest);
            SqlParameter[] spam ={
                                    new SqlParameter("@Content",model.Content),
                                    new SqlParameter("@Contact",model.Contact),
                                    new SqlParameter("@AddTime",DateTime.Now),
                                    new SqlParameter("@IPAddr",model.IPAddr)
                                };
            return sqlhelper.ExecuteNonQuery(constr, text, cmdstr, spam);
        }

        public int EnableResourceRequest(int RId)
        {
            cmdstr = GenneralSqlFromConfig(DBCommand.EnableResourceRequest);
            SqlParameter spam=new SqlParameter("@RId",RId);
            return sqlhelper.ExecuteNonQuery(constr, text, cmdstr, spam);
        }

        public int UpdateResourceRequest(ReSourceRequest model)
        {
            cmdstr = GenneralSqlFromConfig(DBCommand.UpdateResourceRequest);
            SqlParameter[] spam ={
                                    new SqlParameter("@Content",model.Content),
                                    new SqlParameter("@Contact",model.Contact),
                                    new SqlParameter("@UpdateTime",DateTime.Now),
                                    new SqlParameter("@RId",model.Id)
                                };
            return sqlhelper.ExecuteNonQuery(constr, text, cmdstr, spam);
        }

        public int Useful(int RId)
        {
            cmdstr = GenneralSqlFromConfig(DBCommand.UsefulRequest);
            SqlParameter spam = new SqlParameter("@RId", RId);
            return sqlhelper.ExecuteNonQuery(constr, text, cmdstr, spam);
        }

        public DataTable GetSingleResourceRequest(int RId)
        {
            cmdstr = GenneralSqlFromConfig(DBCommand.GetSingleResourceRequest);
            SqlParameter spam = new SqlParameter("@RId", RId);
            return sqlhelper.ExecuteDataset(constr, text, cmdstr, spam).Tables[0];
        }

        public DataTable GetListResourceRequest(string where,string[] spam = null)
        {
            cmdstr = GenneralSqlFromConfig(DBCommand.UsefulRequest) +" "+ where;
            cmdstr = spam == null ? cmdstr : string.Format(cmdstr, spam);
            return sqlhelper.ExecuteDataset(constr, text, cmdstr).Tables[0];
        }

        public DataTable GetTimesByIP(string IPAddr)
        {
            cmdstr = GenneralSqlFromConfig(DBCommand.GetTimesByIP);
            SqlParameter spam = new SqlParameter("@IPAddr", IPAddr);
            return sqlhelper.ExecuteDataset(constr, text, cmdstr, spam).Tables[0];
        }

    }
}
