using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WX.Model
{
    public class VIPType:BaseModel
    {
        public int Id { get; set; }

        public string VIPName { get; set; }

        public string Notes { get; set; }
    }
}
