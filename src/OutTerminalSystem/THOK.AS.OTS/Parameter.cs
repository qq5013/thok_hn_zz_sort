using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using THOK.ParamUtil;

namespace THOK.AS.OTS
{
    public class Parameter: BaseObject
    {
        private string serverName;

        [CategoryAttribute("数据库连接参数"), DescriptionAttribute("数据库服务器名称"), Chinese("服务器名称")]
        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

        private string dbName;

        [CategoryAttribute("数据库连接参数"), DescriptionAttribute("数据库名称"), Chinese("数据库名")]
        public string DBName
        {
            get { return dbName; }
            set { dbName = value; }
        }

        private string dbUser;

        [CategoryAttribute("数据库连接参数"), DescriptionAttribute("数据库连接用户名"), Chinese("用户名")]
        public string DBUser
        {
            get { return dbUser; }
            set { dbUser = value; }
        }
        private string password;

        [CategoryAttribute("数据库连接参数"), DescriptionAttribute("数据库连接密码"), Chinese("密码")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        private bool printLabel;

        [CategoryAttribute("标签打印参数"), DescriptionAttribute("是否实时打印客户标签"), Chinese("实时打印标签")]
        public bool PrintLabel
        {
            get { return printLabel; }
            set { printLabel = value; }
        }

        private string ip;
        [CategoryAttribute("通信参数"), DescriptionAttribute("监听地址"), Chinese("IP地址")]
        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        private int port;
        [CategoryAttribute("通信参数"), DescriptionAttribute("监听端口"), Chinese("端口")]
        public int Port
        {
            get { return port; }
            set { port = value; }
        }
    }
}
