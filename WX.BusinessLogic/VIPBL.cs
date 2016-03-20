using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WX.DataAccess;
using WX.Model;
using WX.Helper;

namespace WX.BusinessLogic
{
    public class VIPBL : BaseBL
    {
        VIPDA dal = new VIPDA();

        public FreeVIP GetVIPByWebName(VIPLog viplog, string WebName)
        {
            FreeVIP model = new FreeVIP();
            model.errcode = 0;
            model.errmsg = "Everything Is Ok!";
            //1.检查历史记录
            string HasLog = CheckUserTimes(WebName, viplog.IdentityType, viplog.IdentityNum, viplog.IPAddr);
            switch (HasLog)
            {
                case "true":
                    model = GenneralT<FreeVIP>(dal.GetNewAccount(WebName));//获取账号
                    if (model == null || model.Id <= 0)
                        return model;
                    viplog.VIPId = model.Id;
                    dal.WriteVIPLog(viplog);//日志记录账号使用状态
                    if (dal.GetLogTimes(model.Id) >= int.Parse(ConfigHelper.GetSysConfigItem("AccountCanUseTimes", WebName)))//更新账号状态
                        dal.UpdateAccountEnable(model.Id);
                    break;
                case "false":
                    model.errcode = 400;
                    model.errmsg = "今天的机会用完了，以后再来吧！";
                    break;
                default:
                    model = GetModelById(int.Parse(HasLog));
                    break;
            }
            return model;
        }

        public string CheckUserTimes(string accountType, string type, string num, string IPAddr)
        {
            DataTable dt = dal.CheckUserTimes(type, num, IPAddr);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return "true";
            }

            //检查使用记录中是否已经存在需要的账号记录
            DataRow[] dr = dt.Select("VIPName='" + accountType+"'");
            if (dr != null && dr.Count() >= 1)
            {
                return dr[0]["VIPId"].ToString();
            }
            //已有记录中不存在需要的账号
            return dt.Rows.Count >= 3 ? "false" : "true";
        }

        public FreeVIP GetModelById(int Id)
        {
            return GenneralT<FreeVIP>(dal.GetModelById(Id));
        }

    }
}
