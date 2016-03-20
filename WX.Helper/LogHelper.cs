using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace WX.Helper
{
    public class LogHelper
    {
        private const string br = "\r\n";
        private static ILog loger = LogManager.GetLogger("");

        /// <summary>
        /// 记录Log信息
        /// 附加信息记录在exception.Data中
        /// </summary>
        /// <param name="ex"></param>
        public static void Log(Exception ex, bool NoMail = false)
        {
            if (ex is System.Threading.ThreadAbortException)
                return;
            StringBuilder message = new StringBuilder("");
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                if (HttpContext.Current.Request.Url != null)
                    message.Append(string.Format("url:{0}{1}", HttpContext.Current.Request.Url.ToString(), br));
                if (HttpContext.Current.Request.Form != null)
                    message.Append(string.Format("Paras:{0}{1}", HttpContext.Current.Request.Form.ToString(), br));
            }
            message.Append(string.Format("datetime:{0}{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), br));
            message.Append(string.Format("message:{0}{1}", ex.Message, br));
            foreach (string key in ex.Data.Keys)
            {
                message.Append(string.Format("{0}:{1}{2}", key, ex.Data[key], br));
            }
            message.Append(string.Format("statack:{0}{1}", ex.StackTrace, br));
            message.Append(string.Format("---------------------------------------------------" + br + br));
            loger.Error(message);

            if (!NoMail)
            {
                string mails = ConfigHelper.GetSysConfigItem("SYSError_MailList", "MailAddress");
                if (!string.IsNullOrWhiteSpace(mails))
                {
                    List<string> mailList = new List<string>();
                    mailList.AddRange(mails.Split(';'));
                    //MailHelper.SendMail_Test(mailList, "ECLibrarySysErro", message.ToString());
                }
            }
        }

        public static void Log(string message)
        {
            loger.Error(DateTime.Now.ToString("yyyy-MM-dd:") + message + br + br);
        }

        ///// <summary>
        ///// 写入日志文件
        ///// </summary>
        ///// <param name="input"></param>
        //public static void WriteLogFile(string input)
        //{
        //    /**/
        //    ///指定日志文件的目录
        //    string fname = "D:\\ECLibrary\\User\\LogFile.txt";
        //    /**/
        //    ///定义文件信息对象

        //    FileInfo finfo = new FileInfo(fname);

        //    if (!finfo.Exists)
        //    {
        //        FileStream fs;
        //        fs = File.Create(fname);
        //        fs.Close();
        //        finfo = new FileInfo(fname);
        //    }

        //    /**/
        //    ///创建只写文件流

        //    using (FileStream fs = finfo.OpenWrite())
        //    {
        //        /**/
        //        ///根据上面创建的文件流创建写数据流
        //        StreamWriter w = new StreamWriter(fs);

        //        /**/
        //        ///设置写数据流的起始位置为文件流的末尾
        //        w.BaseStream.Seek(0, SeekOrigin.End);

        //        /**/
        //        ///写入“Log Entry : ”
        //        w.Write("Log Entry : ");

        //        /**/
        //        ///写入当前系统时间并换行
        //        w.Write("{0} {1} \n\r", DateTime.Now.ToLongTimeString(),
        //            DateTime.Now.ToLongDateString());

        //        /**/
        //        ///写入日志内容并换行
        //        w.Write(input + "\n\r");

        //        /**/
        //        ///写入------------------------------------“并换行
        //        w.Write("------------------------------------ \n\r");

        //        /**/
        //        ///清空缓冲区内容，并把缓冲区内容写入基础流
        //        w.Flush();

        //        /**/
        //        ///关闭写数据流
        //        w.Close();
        //    }

        //}
    }
}
