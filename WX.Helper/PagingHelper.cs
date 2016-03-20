using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WX.Helper
{
    public class PagingHelper
    {
        public static string GenneralNumPagingHtml(int pageIndex, int totalPage, string buttonHtml, string buttonActiveHtml, string where = "", int numCount = 5)
        {
            //获取url参数
            StringBuilder paraUrl = new StringBuilder("");
            if (!string.IsNullOrEmpty(where))
            {
                string[] urlParas = where.Split(',');
                foreach (string para in urlParas)
                {
                    string pValue = RequestHelper.GeneralGetValue(para);
                    if (!string.IsNullOrWhiteSpace(pValue))
                        paraUrl.Append(string.Format("&{0}={1}", para, pValue));
                }
            }

            StringBuilder result = new StringBuilder("");
            int sideNum = (numCount - 1) / 2;
            result.Append(buttonHtml.Replace("{0}", "1").Replace("{1}", "«").Replace("{where}", paraUrl.ToString()));
            if (pageIndex - sideNum < 1)
            {
                //left
                #region
                for (int i = 1; i <= numCount; i++)
                {
                    if (i <= totalPage)
                    {
                        result.Append((pageIndex == i ? buttonActiveHtml : buttonHtml).Replace("{0}", i.ToString()).Replace("{1}", i.ToString()).Replace("{where}", paraUrl.ToString()));
                    }
                }
                #endregion
            }
            else if (pageIndex + sideNum > totalPage)
            {
                //right
                #region
                for (int i = totalPage - numCount + 1; i <= totalPage; i++)
                {
                    if (i > 0)
                    {
                        result.Append((pageIndex == i ? buttonActiveHtml : buttonHtml).Replace("{0}", i.ToString()).Replace("{1}", i.ToString()).Replace("{where}", paraUrl.ToString()));
                    }
                }
                #endregion
            }
            else
            {
                //min
                #region
                for (int i = pageIndex - sideNum; i <= pageIndex + sideNum; i++)
                {
                    result.Append((pageIndex == i ? buttonActiveHtml : buttonHtml).Replace("{0}", i.ToString()).Replace("{1}", i.ToString()).Replace("{where}", paraUrl.ToString()));
                }
                #endregion
            }
            result.Append(buttonHtml.Replace("{0}", totalPage.ToString()).Replace("{1}", "»").Replace("{where}", paraUrl.ToString()));
            return result.ToString();
        }
    }
}
