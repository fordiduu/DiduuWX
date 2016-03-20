using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WX.Model
{
    public class BaseModel
    {
        public BaseModel() { }

        public string DBCommand { get; set; }

        /// <summary>
        /// 返回码,0表示成功
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 对返回码的文本描述内容
        /// </summary>
        public string errmsg { get; set; }
    }
}
