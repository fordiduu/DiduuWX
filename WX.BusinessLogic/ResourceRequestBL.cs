using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WX.DataAccess;
using WX.Helper;
using WX.Model;

namespace WX.BusinessLogic
{
    public class ResourceRequestBL : BaseBL
    {
        ResourceRequestDA dal = new ResourceRequestDA();
        public string NewResourceRequest(ReSourceRequest model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Content))
                return null;
            model.IPAddr = HttpHelper.GetClientIP();
            #region 验证IP的每天发送量是否超标，暂定为10次/天
            if (GetTimesByIP(model.IPAddr) >= 10)
                return "请求次数超过限制，请明天再来，谢谢！";
            #endregion
            return dal.NewResourceRequest(model).ToString();
        }

        public int GetTimesByIP(string IPAddr)
        {
            DataTable dt = dal.GetTimesByIP(IPAddr);
            if (dt == null || dt.Rows.Count == 0)
                return 0;
            string filter = "IPAddr='" + IPAddr + "'";
            DataRow[] drList = dt.Select(filter);
            return drList.Count();
        }

        public int EnableResourceRequest(int RId)
        {
            if (RId <= 0)
                return 0;
            return dal.EnableResourceRequest(RId);
        }

        public int UpdateResourceRequest(ReSourceRequest model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Content))
                return 0;
            return dal.UpdateResourceRequest(model);
        }

        public int Useful(int RId)
        {
            if (RId <= 0)
                return 0;
            return dal.Useful(RId);
        }

        public ReSourceRequest GetSingleResourceRequest(int RId)
        {
            if (RId <= 0)
                return null;
            return GenneralT<ReSourceRequest>(dal.GetSingleResourceRequest(RId));
        }

        public List<ReSourceRequest> GetListResourceRequest(ViewType type)
        {
            string[] spam = null;
            string where = string.Empty;
            switch (type.ToString())
            {
                case "Hot":
                    where = " order by useful,AddTime";
                    break;
                default:
                case "Fresh":
                    where = " order by AddTime desc";
                    break;
                case "My":
                    where = " where OpenId={0} order by AddTime Desc";
                    spam[0] = CookieHelper.GetCookieValue("OpenId");
                    break;
            }
            DataTable dt = dal.GetListResourceRequest(where, spam);
            if (dt == null || dt.Rows.Count <= 0)
                return null;
            List<int> mylist = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                mylist.Add(int.Parse(dr["Id"].ToString()));
            }
            List<ReSourceRequest> list = new List<ReSourceRequest>();
            foreach (int RId in mylist)
            {
                ReSourceRequest model = GenneralT<ReSourceRequest>(dal.GetSingleResourceRequest(RId));
                list.Add(model);
            }
            return list;
        }

    }
}
