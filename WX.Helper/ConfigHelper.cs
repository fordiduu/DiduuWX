using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using CM = System.Configuration.ConfigurationManager;

namespace WX.Helper
{
    public class ConfigHelper
    {
        private const string SysConfigPath = "~/Configuration/SYS.config";
        private const string DBConfigPath = "~/Configuration/DBCommand.config";
        private static DateTime SysConfigLastUpdateTime = new DateTime(1970, 1, 1);
        private static DateTime DBConfigLastUpdateTime = new DateTime(1970, 1, 1);
        private static Dictionary<string, Dictionary<string, string>> SysConfigItems = null;
        private static Dictionary<string, string> DBConfigItems = null;

        /// <summary>
        /// 获取系统配置config中的节点
        /// </summary>
        /// <param name="name">BaseInfo的name</param>
        /// <param name="nodeName">node名称</param>
        /// <returns></returns>
        public static string GetSysConfigItem(string name, string nodeName)
        {
            name = name.ToLower();
            nodeName = nodeName.ToLower();
            string absPath = FileHelper.AbsolutePath(SysConfigPath);
            if (SysConfigItems == null || File.GetLastWriteTime(absPath) > SysConfigLastUpdateTime)
            {
                //重新读取
                GeneralSysConfigItems();
            }
            if (SysConfigItems != null && SysConfigItems.Keys.Contains(name) && SysConfigItems[name].Keys.Contains(nodeName.ToLower()))
            {
                //read from cache
                return SysConfigItems[name][nodeName].ToString();
            }
            return "";
        }
        private static void GeneralSysConfigItems()
        {
            XmlDocument doc = new XmlDocument();
            bool loadXmlSuccess = false;
            string absPath = FileHelper.AbsolutePath(SysConfigPath);
            try
            {
                doc.Load(absPath);
                loadXmlSuccess = true;
            }
            catch (Exception ex)
            {
                ex.Data["Info"] = "Load System.config Error";
                LogHelper.Log(ex, true);
            }
            if (loadXmlSuccess)
            {
                XmlElement root = doc.DocumentElement;
                SysConfigItems = new Dictionary<string, Dictionary<string, string>>();
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        string nodeFirstName = node.Attributes["Name"] == null ? null : node.Attributes["Name"].Value.ToLower();
                        if (nodeFirstName == null)
                            nodeFirstName = node.Attributes["name"] == null ? null : node.Attributes["name"].Value.ToLower();
                        if (!string.IsNullOrWhiteSpace(nodeFirstName) && !SysConfigItems.Keys.Contains(nodeFirstName))
                        {
                            SysConfigItems.Add(nodeFirstName, new Dictionary<string, string>());
                            foreach (XmlNode n in node.ChildNodes)
                            {
                                if (node.NodeType == XmlNodeType.Element)
                                {
                                    if (!SysConfigItems[nodeFirstName].Keys.Contains(n.Name.ToLower()))
                                    {
                                        SysConfigItems[nodeFirstName].Add(n.Name.ToLower(), n.InnerText);
                                    }
                                }
                            }
                        }
                    }
                }
                SysConfigLastUpdateTime = File.GetLastWriteTime(absPath);
            }
        }

        /// <summary>
        /// 获取数据库配置config中的节点
        /// </summary>
        /// <param name="name">DataCommandName</param>
        /// <returns></returns>
        public static string GetDBConfigItem(string name)
        {
            name = name.ToLower();
            string absPath = FileHelper.AbsolutePath(DBConfigPath);
            if (SysConfigItems == null || File.GetLastWriteTime(absPath) > DBConfigLastUpdateTime)
            {
                //重新读取
                GeneralDBConfigItems();
            }
            if (DBConfigItems != null && DBConfigItems.Keys.Contains(name))
            {
                //read from cache
                return DBConfigItems[name].ToString();
            }
            return "";
        }
        private static void GeneralDBConfigItems()
        {
            XmlDocument doc = new XmlDocument();
            string absPath = FileHelper.AbsolutePath(DBConfigPath);
            bool loadXmlSuccess = false;
            try
            {
                doc.Load(absPath);
                loadXmlSuccess = true;
            }
            catch (Exception ex)
            {
                ex.Data["Info"] = "Load DB.config Error";
                LogHelper.Log(ex, true);
            }
            if (loadXmlSuccess)
            {
                XmlElement root = doc.DocumentElement;
                DBConfigItems = new Dictionary<string, string>();
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        string nodeFirstName = node.Attributes["Name"] == null ? null : node.Attributes["Name"].Value.ToLower();
                        if (nodeFirstName == null)
                            nodeFirstName = node.Attributes["name"] == null ? null : node.Attributes["name"].Value.ToLower();
                        if (!string.IsNullOrWhiteSpace(nodeFirstName) && !DBConfigItems.Keys.Contains(nodeFirstName))
                        {
                            DBConfigItems.Add(nodeFirstName, node.InnerText);
                        }
                    }
                }
                DBConfigLastUpdateTime = File.GetLastWriteTime(absPath);
            }
        }
        public static string GetAppSettingsItem(string key)
        {
            string result = CM.AppSettings[key];
            return result == null ? "" : result;
        }
    }
}
