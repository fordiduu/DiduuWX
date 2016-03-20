using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WX.Helper
{
    public class RequestHelper
    {
        public static string GeneralPostValue(string paraName)
        {
            string v = HttpContext.Current.Request.Form.Get(paraName);
            return v;
        }

        public static string GeneralGetValue(string paraName)
        {
            return HttpContext.Current.Request.QueryString.Get(paraName);
        }
    }
}
