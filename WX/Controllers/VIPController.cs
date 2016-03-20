using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WX.Helper;
using WX.BusinessLogic;
using WX.Model;

namespace WX.Controllers
{
    public class VIPController : Controller
    {
        JSBaseEngin Js_Engin = new JSBaseEngin();
        VIPBL bll = new VIPBL();

        public ActionResult Index()
        {
            CurrentUrl = Request.Url.ToString();
            string url = ViewBag.S_url = CurrentUrl;
            string second = ViewBag.S_Second = getSeconds().ToString();
            string ticket = ViewBag.JsApiTicket = Js_Engin.JsApiTicket;
            string noncestr = ViewBag.S_noncestr = JSBaseEngin.getMD5Str(second);
            string signature = ViewBag.signature = get_jssdk_signature(ticket, noncestr, second, url).ToLower();
            return View();
        }  

        public string Ext(string WebName, string type, string num, string VIPToken)
        {
            FreeVIP model = new FreeVIP();
            VIPLog log = new VIPLog();
            log.IdentityType = type;
            log.IdentityNum = num;
            log.IPAddr = HttpHelper.GetClientIP();
            model = bll.GetVIPByWebName(log, WebName);
            JavaScriptSerializer js = new JavaScriptSerializer();
            string content = js.Serialize(model);
            return content;
        }


        string CurrentUrl = string.Empty;

        string get_jssdk_signature(string ticket, string noncestr, string second, string url)
        {
            return JSBaseEngin.getSHA1Str("jsapi_ticket=" + ticket + "&noncestr=" + noncestr + "&timestamp=" + second + "&url=" + url);
        }

        int getSeconds()
        {
            TimeSpan timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            return (int)timeSpan.TotalSeconds;
        }


        public string GetName()
        {
            
            return string.Empty;
        }


    }
}
