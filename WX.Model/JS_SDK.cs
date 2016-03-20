using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WX.Model
{
    public class JS_SDK : BaseModel
    {
        public string ticket { get; set; }
        public string expires_in { get; set; }
    }
}
