using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace WX.Helper
{
    public class UrlHelper
    {

        public static string HttpPath(string path, Page page)
        {
            return page.ResolveUrl(path);
        }

        /// <summary>
        /// 将项目文件转换为http路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string HttpUrl4Local(string path)
        {
            if (path.ToLower().StartsWith("http"))
                return path;
            return ConfigHelper.GetAppSettingsItem("WebSiteUrl").Trim('/') + (path.StartsWith("~") ? path.Trim('~') : path);
        }

    }
}
