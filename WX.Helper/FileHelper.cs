using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WX.Helper
{
    public class FileHelper
    {
        public static string AbsolutePath(string path)
        {
            return AppDomain.CurrentDomain.SetupInformation.ApplicationBase + path.Trim('~').Replace('/', '\\');
        }
    }
}
