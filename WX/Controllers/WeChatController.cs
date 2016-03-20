using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Xml;

namespace WX.Controllers
{
    public class WeChatController : Controller
    {
        //
        // GET: /WeChat/
        private const string Token = "DiduuWXToken";
        public ActionResult Wechat()
        {
            if (Request.HttpMethod.ToLower() == "post")
            {
                StreamReader str = new StreamReader(Request.InputStream, System.Text.Encoding.UTF8);
                XmlDocument xml = new XmlDocument();
                xml.Load(str);
                XmlNode xn = xml.SelectSingleNode("xml"); //返回标签名为“xml”的所有节点，返回的是数组

                string xmlDoc = "";
                for (int i = 0; i < xn.ChildNodes.Count; i++)
                {
                    xmlDoc += "\r\n" + xn.ChildNodes[i].Name + ":" + xn.ChildNodes[i].InnerText;
                }
                ToUserName = xn.SelectSingleNode("ToUserName").InnerText; //获取用户的OpenID;
                FromUserName = xn.SelectSingleNode("FromUserName").InnerText; //获取用户的OpenID
                MsgType = xn.SelectSingleNode("MsgType").InnerText; //获得用户发来的信息类型:text,image,location,link,event等
                if (!string.IsNullOrWhiteSpace(MsgType))
                {
                    switch (MsgType.ToLower())
                    {
                        case "text":
                            myContent = xn.SelectSingleNode("Content").InnerText;
                            if (myContent.Substring(0, 1).ToLower() == "v")
                                ToWXStr = ReplayText(myContent);
                            else
                                ToWXStr = Normal_Text(myContent);
                            break;
                        case "event":
                            switch (xn.SelectSingleNode("Event").InnerText)
                            {
                                case "subscribe":
                                    ToWXStr = ReplayText(SubscribeEvent());
                                    break;
                                case "unsubscribe":
                                    break;
                                case "click":
                                    string EventKey = xn.SelectSingleNode("EventKey").InnerText;
                                    ToWXStr = ReplayText("您好，我们的攻城狮们正在研发此功能，请期待！");
                                    break;
                            }
                            break;
                        default:
                            ToWXStr = ReplayText("您好，您的消息我们已经收到，客服看到后会在第一时间内给您回复！");
                            break;
                    }
                }
            }
            else
            {
                string echoStr = Request.QueryString["echoStr"];
                if (CheckSignature())
                {
                    if (!string.IsNullOrEmpty(echoStr))
                    {
                        return Content(echoStr);
                    }
                }
            }
            return Content(ToWXStr, "text/xml");
        }
        
        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns></returns>
        private bool CheckSignature()
        {
            string signature = Request.QueryString["signature"];
            string timestamp = Request.QueryString["timestamp"];
            string nonce = Request.QueryString["nonce"];
            string[] ArrTmp = { Token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 创建自定义菜单的方法
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateMenu()
        {
            //GetAccess_Token();
            //string menu = "{\"button\": [{\"name\": \"优惠活动\",\"sub_button\": [{\"type\": \"view\",\"name\": \"活动\",\"url\": \"http://m.enmuo.com/views/activity/index.html\"},{\"type\": \"view\",\"name\": \"优惠\",\"url\": \"http://m.enmuo.com/views/group/list.html?groupId=201311010002\"}]},{\"type\": \"view\",\"name\": \"会员卡\",\"url\": \"http://m.enmuo.com/views/weixin/log.html\"},{\"type\": \"view\",\"name\": \"么么社区\",\"url\": \"http://m.wsq.qq.com/263468374\"}]}";
            //string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + access_token;
            //return Content(HttpPost(url, menu));
            return Content("");
        }

        /// <summary>
        /// 获取用户IP信息
        /// </summary>
        /// <returns></returns>
        private string getIPV4()
        {
            string result = "";
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in nics)
                {
                    if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        IPInterfaceProperties ip = adapter.GetIPProperties();
                        UnicastIPAddressInformationCollection ipCollection = ip.UnicastAddresses;
                        foreach (UnicastIPAddressInformation ipadd in ipCollection)
                            if (ipadd.Address.AddressFamily == AddressFamily.InterNetwork)
                                result = ipadd.Address.ToString();
                    }
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 普通文本的消息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string Normal_Text(string value)
        {
            string result = "";
            switch (value)
            {
                case "1":
                case "活动":
                case "近期活动":
                    string _I_title = "【么么年货节】抢年货，就是这么任性！";
                    string _I_desc = "2015么么年货开枪啦！都是宝儿爱吃的，妈妈就要这么豪爽，就是这么任性！";
                    string _I_Picurl = "http://mmbiz.qpic.cn/mmbiz/xXv9zAWMrOYeVouyWmmSUzGBgEcxrELcFccwE5zyIamP2t9ic01cRKHib5P4fticFlR1iaN6lS72HrdGta1SchGhicg/640?wxfrom=5";
                    string _I_Url = "http://topic.enmuo.com/nianhuo/";
                    result = ReplaySingleNews(_I_title, _I_desc, _I_Picurl, _I_Url);
                    //result = ReplayMedia("z34chBrVCdxx5JIaxNrMVpBBwRBiCVKC55sp7xL_lz8", "news");
                    break;
                case "年货":
                    string _I_title1 = "【么么年货节】抢年货，就是这么任性！";
                    string _I_desc1 = "2015么么年货开枪啦！都是宝儿爱吃的，妈妈就要这么豪爽，就是这么任性！";
                    string _I_Picurl1 = "http://mmbiz.qpic.cn/mmbiz/xXv9zAWMrOYeVouyWmmSUzGBgEcxrELcFccwE5zyIamP2t9ic01cRKHib5P4fticFlR1iaN6lS72HrdGta1SchGhicg/640?wxfrom=5";
                    string _I_Url1 = "http://topic.enmuo.com/nianhuo/";
                    result = ReplaySingleNews(_I_title1, _I_desc1, _I_Picurl1, _I_Url1);
                    //result = ReplayMedia("z34chBrVCdxx5JIaxNrMVpBBwRBiCVKC55sp7xL_lz8", "news");
                    break;
                case "台历":
                    string _I_title2 = "么么2015超级福利 ！拿台历享免费，还不快来购！";
                    string _I_desc2 = "　　冬天已到，春天还会远么？　　说到春天，那么问题来了！　　2015年的日子你知道怎么过么？　　不要急不要慌";
                    string _I_Picurl2 = "http://mmbiz.qpic.cn/mmbiz/xXv9zAWMrOa8sVeavGXa3rZpFEic0b4juicyG1tngooAGWb6BsvE1XQNNXhq6z8mdZj8t7f2va9PEiaIibicwiakeLEw/640?wxfrom=5";
                    string _I_Url2 = "http://topic.enmuo.com/calendar/index.shtml";
                    result = ReplaySingleNews(_I_title2, _I_desc2, _I_Picurl2, _I_Url2);
                    //result = ReplayMedia("qQp9lchbnz8gkQJIakd1mMc1FmftEQIS6n1kCuVPCw4", "news");
                    break;
                case "童谣":
                case "目录":
                case "童谣目录":
                    string _I_title3 = "童谣目录";
                    string _I_desc3 = "么么收藏的童谣都在目录这里啦，只要在公众号中输入相应的童谣名字即可听到童谣哦～";
                    string _I_Picurl3 = "https://mmbiz.qlogo.cn/mmbiz/xXv9zAWMrOZd9ejwZBibojNHiajR5m4PU4icqmFiaSVtIpHFgkKLQRXG00OqDXXpAlXShqyhswIIBIyroZzjyzr5zA/0";
                    string _I_Url3 = "http://topic.enmuo.com/calendar/index.shtml";
                    result = ReplaySingleNews(_I_title3, _I_desc3, _I_Picurl3, _I_Url3);
                    //result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9LxZAfMlgONEtcP8IChQWwvo","news");
                    break;
                case "一分钱":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9LzseJsV4-Uh7BiYyOyWjCuI");
                    break;
                case "一只哈巴狗":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L7sLv-DX56rTwuhyWPAlcZk");
                    break;
                case "上学歌":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L45c37F5kfQ_FEfvEJM1G-w");
                    break;
                case "买菜":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L21hNWgcURjZfYbJ6G1m81o");
                    break;
                case "十指和十趾":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9Lz2CV--FOod_H-cwNzOVt3A");
                    break;
                case "吹泡泡":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9LyD9GFqOGsE7Qzffikb_2yk");
                    break;
                case "喇叭花":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L5pG6xfXufFoa0qO0nv2DWo");
                    break;
                case "大西瓜":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L9WZMsXmiWfDkGG841-Xq7A");
                    break;
                case "太阳":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L6FY1mDMhIh14otRLiqNM4c");
                    break;
                case "太阳当空照":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L6z2qb80Q3ze25AqMKx_JdI");
                    break;
                case "小宝贝":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9LxjCTfMAIv_Gu-SEN8ePeQc");
                    break;
                case "小气鬼":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L-G9cpCehQ4mFHtrkI_jQvo");
                    break;
                case "小猫":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L8vMEwNB8U8Kr9hqwewKt8s");
                    break;
                case "小白兔":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L3MDq9YRuH_L6j_Teijg3Hw");
                    break;
                case "小老鼠上灯台":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L2j7xn78YP95kpmqrJqk9d4");
                    break;
                case "小蘑菇":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L5u4wd28vQ9NbJGBebDvJRY");
                    break;
                case "小鸭子":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L_pHj0k99Ra6PfqaTJQFs_A");
                    break;
                case "我爱我的幼儿园":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L7A7ezu6_qUNDybPeIAdwAQ");
                    break;
                case "打电话":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L7MkYoV-AaW7ye6vCaAhgLo");
                    break;
                case "捡豆豆":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L2grgtTddShcEm0S7k9PHOM");
                    break;
                case "数星星":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L0znh9beYv_1A4B5wyU3N-Y");
                    break;
                case "数鸭子":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L_n9V58tK_Jte-K7MWdPFQw");
                    break;
                case "新年好":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L3f5cpXIBEjsHVArTw2ULe4");
                    break;
                case "月亮":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L5ISljD3CWVtyr1RecEKXTE");
                    break;
                case "毛毛虫":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9LyWvdn4Dl8YCA_1R0ufCj5c");
                    break;
                case "氢气球":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L3SsxUo3nxQe2Jdw-UVanfk");
                    break;
                case "牵牛花":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L6Mpccs4ysRYVPaFSOL7-1g");
                    break;
                case "睡觉":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L8_ELo9U5OqejPi9MAlJLeE");
                    break;
                case "礼貌歌":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L97RTuxb8jJUV10e9y8c3_o");
                    break;
                case "耗子长了一身毛":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L_ybSoKsylG1_lYGQWAg3GI");
                    break;
                case "莲花儿灯":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L8WxdkoB5y-yqIrxxLc4j9E");
                    break;
                case "虫儿飞":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9LzNetZya47T-tqVcZGPS-xY");
                    break;
                case "蜗牛与黄鹂鸟":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L-uQcniWYdYALyiYOufb7VY");
                    break;
                case "蝴蝶花":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L_DcfeDrU6ZaeE8P9tUaUQo");
                    break;
                case "过年了":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L51-lYw0DWKcl0ZvCu51hZs");
                    break;
                case "金箍噜棒":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9LwPWuS9cwth_3oONZaFbtLw");
                    break;
                case "钓鱼":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9L9zkws81wtUOXAGxX8udbtU");
                    break;
                case "风婆婆":
                    result = ReplayMedia("XtrNsfr2LWzNU5gRdyK9Ly9BP1Y32upMU7yuMlpm7gE");
                    break;
                case "激活码":
                case "求激活码":
                case "激活":
                case "验证码":
                    result = ReplayText("<a href=\"http://985.so/fmAQ \">点击下载</a>\r\n\r\n激活码：\r\n150+m0970000g0n8\r\n\r\n1、各位么友们，一定要 【先勾选日期】， 再激活哦~\r\n2、如果不能成功跳转，则表示网络信号较差，请换用较好的网络信号再次尝试。\r\n3、如提示“抢票已结束”则表示您预订的游玩日期接待人员已饱和，请重新选择其他游玩时间再次激活。");
                    break;
                case "冒险家门票":
                    result = ReplayText("点击开夺“冒险家乐园”门票，每天都送，都送哦~  【http://dwz.cn/GdFgy】");
                    break;
                case "Q":
                case "q":
                case "群":
                case "Q群":
                case "q群":
                case "qq群":
                case "qQ群":
                case "Qq群":
                case "QQ群":
                    result = ReplayText("育儿交流全国群   17809585\r\n郑州同城育儿群   154120628\r\n准妈准爸交流群  324432146\r\n加群请备注 来自“么么微信”");
                    break;
                case "客服":
                case "小么":
                    ToDkf();
                    break;
                default:
                    result = ReplayText(NewsAutoReplay());
                    break;
            }
            return result;
        }

        public ActionResult GetMediaList(string access, int start)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token={0}", access);
            string Json = "{\"type\":\"voice\",\"offset\":" + start + ",\"count\":20}";
            //string result = HttpPost(url, Json);
            //return Content(result);
            return Content("");
        }

        /// <summary>
        /// 当用户发送的文本消息处理不了时就推送给客服处理
        /// </summary>
        /// <returns></returns>
        private string ToDkf()
        {
            StringBuilder relayCustomMsg = new StringBuilder();
            relayCustomMsg.Append("<xml>");
            relayCustomMsg.Append("<ToUserName><![CDATA[" + FromUserName + "]]></ToUserName>");
            relayCustomMsg.Append("<FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>");
            relayCustomMsg.Append("<CreateTime>" + ConvertDateTimeInt() + "</CreateTime>");
            relayCustomMsg.Append("<MsgType><![CDATA[transfer_customer_service]]></MsgType>");
            relayCustomMsg.Append("<TransInfo><KfAccount>![CDATA[enmuo01@enmuobuy]]</KfAccount></TransInfo>");
            relayCustomMsg.Append("</xml>");
            return relayCustomMsg.ToString();
        }

        /// <summary>
        /// 订阅公众号的事件
        /// </summary>
        /// <returns></returns>
        private string SubscribeEvent()
        {
            return "亲，欢迎来到嘀嘟科技！mo-爱心mo-爱心mo-爱心\r\n嘀嘟公众号全新上线啦！mo-大兵mo-大兵mo-大兵\r\n这一次，嘀嘟机器人“嘟嘟”首次和大家见面，调戏、聊天、卖萌，统统都会；mo-呲牙\r\n这一次，嘀嘟可以帮助“资源小白”方便的找到资源，找不到的电影、音乐、软件，尽管来提；mo-呲牙\r\n这一次，我们开始为您免费提供各种VIP账号，迅雷、好莱坞、爱奇艺，让您免受广告之苦；mo-呲牙\r\n这一次，嘀嘟带给你不一样的惊喜；mo-玫瑰\r\n来嘀嘟，发现更多……";
        }

        /// <summary>
        /// 消息自动回复
        /// </summary>
        /// <returns></returns>
        private string NewsAutoReplay()
        {
            myContent = "您好~么么淘欢迎您的加入/可爱\r\n回复以下关键字可获得相关信息。\r\n回复“活动”查看么么最新活动。\r\n回复“童谣”查看童谣目录为宝宝点歌。\r\n回复“台历”查看2015么么台历详情。\r\n回复“年货”查看么么年货节特价商品。\r\n若您还有其他问题咨询，请留言，客服小么会及时回复您的问题/亲亲\r\n悄悄告诉你，回复“客服”就可以立即召唤小么来为你服务哦！";
            return myContent;
        }

        /// <summary>
        /// 回复文字消息给用户
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string ReplayText(string content)
        {
            StringBuilder relayCustomMsg = new StringBuilder();
            relayCustomMsg.Append("<xml>");
            relayCustomMsg.Append("<ToUserName><![CDATA[" + FromUserName + "]]></ToUserName>");
            relayCustomMsg.Append("<FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>");
            relayCustomMsg.Append("<CreateTime>" + ConvertDateTimeInt() + "</CreateTime>");
            relayCustomMsg.Append("<MsgType><![CDATA[text]]></MsgType>");
            relayCustomMsg.Append("<Content><![CDATA[" + content + "]]></Content>");
            relayCustomMsg.Append("</xml>");
            return relayCustomMsg.ToString();
        }

        /// <summary>
        /// 回复语音消息
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        private string ReplayMedia(string media_id)
        {
            StringBuilder relayCustomMsg = new StringBuilder();
            relayCustomMsg.Append("<xml>");
            relayCustomMsg.Append("<ToUserName><![CDATA[" + FromUserName + "]]></ToUserName>");
            relayCustomMsg.Append("<FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>");
            relayCustomMsg.Append("<CreateTime>" + ConvertDateTimeInt() + "</CreateTime>");
            relayCustomMsg.Append("<MsgType><![CDATA[voice]]></MsgType>");
            relayCustomMsg.Append("<Voice><MediaId><![CDATA[" + media_id + "]]></MediaId></Voice>");
            relayCustomMsg.Append("</xml>");
            return relayCustomMsg.ToString();
        }

        /// <summary>
        /// 回复图文消息给用户
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string ReplayVoteNews(int key)
        {
            //model = Voting_BLL.getVoteInfo(key);
            //if (string.IsNullOrEmpty(model.V_Name))
            //    return replayText("【text】投票失败，未能识别出您的投票编号，请检查后重试，如有疑问请回复“客服”以获取帮助。");
            //string I_Title = "感谢您的参与，您已为编号为V+" + AddZero(VoteNum) + "的选手投票成功，该选手目前" + 000 + "票，距离第一名" + 000 + "票~~~";
            //string I_Description = "偷偷告诉你，拉票是让票数直线上升的唯一法宝哦！";
            //string I_Picurl = "http://topic.enmuo.com/huabing/photo/1.jpg";
            //string I_Url = "http://topic.enmuo.com/huabing/show.shtml?id=" + VoteNum;
            StringBuilder relayCustomMsg = new StringBuilder();
            //relayCustomMsg.Append("<xml>");
            //relayCustomMsg.Append("<ToUserName><![CDATA[" + FromUserName + "]]></ToUserName>");
            //relayCustomMsg.Append("<FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>");
            //relayCustomMsg.Append("<CreateTime>" + ConvertDateTimeInt() + "</CreateTime>");
            //relayCustomMsg.Append("<MsgType><![CDATA[news]]></MsgType>");
            //relayCustomMsg.Append("<ArticleCount>1</ArticleCount>");
            //relayCustomMsg.Append("<Articles>");
            //relayCustomMsg.Append("<item>");
            //relayCustomMsg.Append("<Title><![CDATA[" + I_Title + "]]></Title>");
            //relayCustomMsg.Append("<Description><![CDATA[" + I_Description + "]]></Description>");
            //relayCustomMsg.Append("<PicUrl><![CDATA[" + I_Picurl + "]]></PicUrl>");
            //relayCustomMsg.Append("<Url><![CDATA[" + I_Url + "]]></Url>");
            //relayCustomMsg.Append("</item>");
            //relayCustomMsg.Append("</Articles>");
            //relayCustomMsg.Append("</xml> ");
            return relayCustomMsg.ToString();
        }

        /// <summary>
        /// 回复单图文消息给用户---普通图文消息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string ReplaySingleNews(string _Title, string _I_Description, string _I_Picurl, string _I_Url)
        {
            StringBuilder relayCustomMsg = new StringBuilder();
            relayCustomMsg.Append("<xml>");
            relayCustomMsg.Append("<ToUserName><![CDATA[" + FromUserName + "]]></ToUserName>");
            relayCustomMsg.Append("<FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>");
            relayCustomMsg.Append("<CreateTime>" + ConvertDateTimeInt() + "</CreateTime>");
            relayCustomMsg.Append("<MsgType><![CDATA[news]]></MsgType>");
            relayCustomMsg.Append("<ArticleCount>1</ArticleCount>");
            relayCustomMsg.Append("<Articles>");
            relayCustomMsg.Append("<item>");
            relayCustomMsg.Append("<Title><![CDATA[" + _Title + "]]></Title>");
            relayCustomMsg.Append("<Description><![CDATA[" + _I_Description + "]]></Description>");
            relayCustomMsg.Append("<PicUrl><![CDATA[" + _I_Picurl + "]]></PicUrl>");
            relayCustomMsg.Append("<Url><![CDATA[" + _I_Url + "]]></Url>");
            relayCustomMsg.Append("</item>");
            relayCustomMsg.Append("</Articles>");
            relayCustomMsg.Append("</xml> ");
            return relayCustomMsg.ToString();
        }

        /// <summary>
        /// 给传递过来的数字加0
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        string AddZero(int num)
        {
            string result = "";
            if (num < 10)
                result = "00" + num;
            if (num > 9 && num < 100)
                result = "0" + num;
            if (num > 99)
                result = num.ToString();
            return result;
        }

        /// <summary>
        /// 获得1970-1-1到现在的秒数
        /// datetime转换为unixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private int ConvertDateTimeInt()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(DateTime.Now - startTime).TotalSeconds;
        }

        void sendMail(string body)
        {
            string userEmail = "15093428263@126.com";  //发件人邮箱
            string userPswd = "231519149";    //发件人邮箱密码
            string toEmail = "15093428263@126.com";    //收件人邮箱
            string mailServer = "smtp.126.com";  //邮件服务器
            string my_title = "";
            string subject = "【\"微信投票\"】" + my_title;    //邮件标题
            string mailBody = body;   //邮件内容
            //string[] attachFiles //邮件附件
            //邮箱帐号的登录名
            string username = userEmail.Substring(0, userEmail.IndexOf('@'));
            //邮件发送者
            MailAddress from = new MailAddress(userEmail);
            //邮件接收者
            MailAddress to = new MailAddress(toEmail);
            MailMessage mailobj = new MailMessage(from, to);
            //邮件标题
            mailobj.Subject = subject;
            //邮件内容
            mailobj.Body = mailBody;
            //邮件不是html格式
            mailobj.IsBodyHtml = false;
            //邮件编码格式
            mailobj.BodyEncoding = System.Text.Encoding.GetEncoding("GB2312");
            //邮件优先级
            mailobj.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient(mailServer);
            //不使用默认凭据访问服务器
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(username, userPswd);
            //使用network发送到smtp服务器
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                //开始发送邮件
                smtp.Send(mailobj);
            }
            catch
            {
            }
        }
        

        #region 辅助变量

        string ToUserName = "";
        string FromUserName = "";
        string MsgType = "";
        /// <summary>
        /// 准备发送个微信的内容
        /// </summary>
        string ToWXStr = "";
        /// <summary>
        /// 如果MsgType是text的话，接收到的文本内容
        /// </summary>
        string myContent = "";
        #endregion

    }
}
