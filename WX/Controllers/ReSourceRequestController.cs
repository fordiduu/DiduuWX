using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WX.BusinessLogic;
using WX.Helper;
using WX.Helper;
using WX.Model;

namespace WX.Controllers
{
    public class ReSourceRequestController : Controller
    {
        ResourceRequestBL RRBL = new ResourceRequestBL();
        /// <summary>
        /// 资源帮帮主页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 添加请求资源
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult AddRequest()
        {
            ReSourceRequest model = new ReSourceRequest();
            string content = Request["content"] == null ? "" : Request["content"].ToString();
            string contact = Request["contact"] == null ? "" : Request["contact"].ToString();
            string Code = Request["code"] == null ? "" : Request["code"].ToString();
            if (string.IsNullOrWhiteSpace(Code))
                return Content("验证码不能为空！");
            #region 处理验证码

            string CodeType = CookieHelper.GetCookieValue("CodeType");
            string CodeValue = CookieHelper.GetCookieValue("CodeValue").Replace(" ", "");
            switch (CodeType)
            { 
                case "Char":
                    if (Code != CodeValue)
                        return Content("验证码输入错误");
                    break;
                case "Num":
                    switch (CodeValue.Length.ToString())
                    { 
                        case "4":
                            int num1 = int.Parse(CodeValue.Substring(0, 2));
                            int num2 = int.Parse(CodeValue.Substring(3, 1));
                            string type = CodeValue.Substring(2, 1);
                            int result = 0;
                            switch (type)
                            { 
                                case "+":
                                    result = num1 + num2;
                                    break;
                                case "-":
                                    result = num1 - num2;
                                    break;
                                case "x":
                                    result = num1 * num2;
                                    break;
                            }
                            if (Code != result.ToString())
                                return Content("验证码输入错误");
                            break;
                        case "5":
                            int num3 = int.Parse(CodeValue.Substring(0, 2));
                            int num4 = int.Parse(CodeValue.Substring(3, 2));
                            string type2 = CodeValue.Substring(2, 1);
                            int result2 = 0;
                            switch (type2)
                            { 
                                case "+":
                                    result2 = num3 + num4;
                                    break;
                                case "-":
                                    result2 = num3 - num4;
                                    break;
                                case "x":
                                    result2 = num3 * num4;
                                    break;
                            }
                            if (Code != result2.ToString())
                                return Content("验证码输入错误");
                            break;
                    }
                    break;
            }
            #endregion
            model.Content = content;
            model.Contact = contact;
            return Content(RRBL.NewResourceRequest(model));
        }
                
        /// <summary>
        /// 最新提问
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRequest(string SortType)
        {
            return View();
        }

        public ActionResult CodeImage()
        {
            byte[] byData = CodeHelper.CreateValidateGraphic(CodeHelper.RandomCode(5));
            return File(byData, @"image/jpg");
        }

        public ActionResult NumImage()
        {
            byte[] byData = CodeHelper.CreateValidateGraphic(CodeHelper.RandomNum());
            return File(byData, @"image/jpg");
        }

    }
}
