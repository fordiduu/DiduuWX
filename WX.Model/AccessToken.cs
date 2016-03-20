using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WX.Model
{
    public class AccessToken:BaseModel
    {
        public string access_token { set; get; }
        public int expires_in { set; get; }
    }
}
