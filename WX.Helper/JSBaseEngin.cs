using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using WX.Model;

namespace WX.Helper
{
    public class JSBaseEngin : WXBaseEngine
    {
        private string JS_SDK_URL = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={{AccessToken}}&type=jsapi";
        private const string JsApiTicketCacheKey = "JsApiTicketCacheKey";
        private const int JsApiTicketTimeOut = 7000;


        /// <summary>
        /// 得到JsApiTicket，当JsApiTicket失效时，自动调用NewJsApiTicket，并存入缓存
        /// </summary>
        public string JsApiTicket
        {
            get
            {
                try
                {
                    object jsapiticket = CacheHelper.Get<object>(JsApiTicketCacheKey);
                    if (jsapiticket == null)
                    {
                        string ticket = NewJsApiTicket;
                        CacheHelper.Update(JsApiTicketCacheKey, ticket, JsApiTicketTimeOut / 60);
                        return ticket;
                    }
                    return jsapiticket.ToString();
                }
                catch (Exception ex)
                {
                    ex.Data["From:"] = "JsApiTicket";
                    LogHelper.Log(ex);
                    return "JsApiTicket:" + ex.Message;
                }
            }
        }

        /// <summary>
        /// 获取JsApiTicket
        /// </summary>
        public string NewJsApiTicket
        {
            get
            {
                try
                {
                    JS_SDK jssdk = base.WXReTryGet<JS_SDK>(JS_SDK_URL);
                    return jssdk.ticket;
                }
                catch (Exception ex)
                {
                    ex.Data["From:"] = "JsApiTicket";
                    LogHelper.Log(ex);
                    return "NewJsApiTicket:" + ex.Message;
                }
            }
        }

        /// <summary>
        /// 获得签名是需要用到的timestamp
        /// </summary>
        /// <returns>int类型</returns>
        public static int getTimestamp()
        {
            TimeSpan timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            return (int)timeSpan.TotalSeconds;
        }

        /// <summary>
        /// 获取字符串的16位加密字符串，用于随机生成noncestr
        /// </summary>
        /// <param name="str">要进行加密的字符串</param>
        /// <returns></returns>
        public static string getMD5Str(string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(0, 16).ToUpper();
        }

        /// <summary>
        /// 获取字符串的SHA1方式的加密字符串，用于JS-SDK验证签名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string getSHA1Str(string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1").ToUpper();
        }

        public string GetSignature(string noncestr, int timestamp, string url, out string JsSDK_ticket)
        {
            JsSDK_ticket = JsApiTicket;
            return getSHA1Str("jsapi_ticket=" + JsApiTicket + "&noncestr=" + noncestr + "&timestamp=" + timestamp + "&url=" + url);
        }


    }
}
