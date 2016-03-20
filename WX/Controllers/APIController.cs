using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WX.Model;

namespace WX.Controllers
{
    public class APIController : Controller
    {
        //
        // GET: /API/

        public ActionResult Mobile(int? ItemId,int? NextId,int? ItemsCount)
        {
            Mobile model = new Mobile();
            switch (ItemId.ToString())
            {
                case "1":
                    model.Title = "娱乐";
                    model.Description = "每天娱乐报道，满足你的八卦心";
                    model.BG_IMG = "http://7xin88.com1.z0.glb.clouddn.com/yule.jpg";
                    model.Next_Title = "要闻";
                    model.Next_Description = "时事要闻，最新获取，24H不重样";
                    model.Next_BG_IMG = "http://7xin88.com1.z0.glb.clouddn.com/yaowen.jpg";
                    break;
                case "2":
                    model.Title = "要闻";
                    model.Description = "时事要闻，最新获取，24H不重样";
                    model.BG_IMG = "http://7xin88.com1.z0.glb.clouddn.com/yaowen.jpg";
                    model.Next_Title = "体育";
                    model.Next_Description = "最新体育资讯，满足你的好奇心";
                    model.Next_BG_IMG = "http://7xin88.com1.z0.glb.clouddn.com/tiyu.jpg";
                    break;
                case "3":
                    model.Title = "体育";
                    model.Description = "最新体育资讯，满足你的好奇心";
                    model.BG_IMG = "http://7xin88.com1.z0.glb.clouddn.com/tiyu.jpg";
                    model.Next_Title = "八卦";
                    model.Next_Description = "每天娱乐报道，无厘头八卦满天飞";
                    model.Next_BG_IMG = "http://7xin88.com1.z0.glb.clouddn.com/bagua.jpg";
                    break;
                case "4":
                    model.Title = "八卦";
                    model.Description = "每天娱乐报道，无厘头八卦满天飞";
                    model.BG_IMG = "http://7xin88.com1.z0.glb.clouddn.com/bagua.jpg";
                    model.Next_Title = "头条";
                    model.Next_Description = "每日头条，看有没有汪星人";
                    model.Next_BG_IMG = "http://7xin88.com1.z0.glb.clouddn.com/toutiao.jpg";
                    break;
                default :
                    model.Title = "头条";
                    model.Description = "每日头条，看有没有汪星人";
                    model.BG_IMG = "http://7xin88.com1.z0.glb.clouddn.com/toutiao.jpg";
                    model.Next_Title = "嘀嘟";
                    model.Next_Description = "嘀嘟新闻，定制您需要的";
                    model.Next_BG_IMG = "http://7xin88.com1.z0.glb.clouddn.com/bg.png";
                    break;
            }
            model.Items = "<div class=\"news-list\"><div class=\"news-list-temp\"><div class=\"news-title\"><a href=\"#\"target=\"_self\">基辛格：习近平是最杰出的中国领导人之一</a></div><div class=\"news-desc-time\"><a href=\"#\"target=\"_self\">人民网记者专访基辛格：“我期待着习主席的访问将为世界和平作出重大贡献”</a><span>今天:15:23</span></div></div><div class=\"news-list-temp\"><div class=\"news-title\"><a href=\"#\"target=\"_self\">基辛格：习近平是最杰出的中国领导人之一</a></div><div class=\"news-desc-time\"><a href=\"#\"target=\"_self\">人民网记者专访基辛格：“我期待着习主席的访问将为世界和平作出重大贡献”</a><span>今天:15:23</span></div></div><div class=\"news-list-temp\"><div class=\"news-title\"><a href=\"#\"target=\"_self\">基辛格：习近平是最杰出的中国领导人之一</a></div><div class=\"news-desc-time\"><a href=\"#\"target=\"_self\">人民网记者专访基辛格：“我期待着习主席的访问将为世界和平作出重大贡献”</a><span>今天:15:23</span></div></div><div class=\"news-list-temp\"><div class=\"news-title\"><a href=\"#\"target=\"_self\">基辛格：习近平是最杰出的中国领导人之一</a></div><div class=\"news-desc-time\"><a href=\"#\"target=\"_self\">人民网记者专访基辛格：“我期待着习主席的访问将为世界和平作出重大贡献”</a><span>今天:15:23</span></div></div><div class=\"news-list-temp\"><div class=\"news-title\"><a href=\"#\"target=\"_self\">基辛格：习近平是最杰出的中国领导人之一</a></div><div class=\"news-desc-time\"><a href=\"#\"target=\"_self\">人民网记者专访基辛格：“我期待着习主席的访问将为世界和平作出重大贡献”</a><span>今天:15:23</span></div></div></div>";
            JavaScriptSerializer js = new JavaScriptSerializer();
            string content = js.Serialize(model);
            Response.AddHeader("Access-Control-Allow-Origin", "*");
            return Content(content);
        }

    }
}
