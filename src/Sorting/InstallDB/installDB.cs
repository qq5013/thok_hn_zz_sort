using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.DirectoryServices;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Configuration.Install;
using System.Management;
using System.Collections;
using Microsoft.Win32;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Diagnostics;
using IWshRuntimeLibrary;

namespace InstallDB
{
    [RunInstaller(true)]
    public partial class installDB : Installer
    {

        #region 设置私有成员，对应安装程序里接收到的用户输入

        //要建数据库的名称。
        private string strDBName;       

        //数据库服务器名称。
        private string strDBServerName; 

        //数据库登录名。
        private string strDBUser;       

        //数据库密码。
        private string strDBPwd;        

        //安装的物理路径。
        private string strPhysicalDir;  

        //把数据库创建在何处。
        private string strDBPath;       

        //要安装的内容,1:安装监控系统和数据库;2:只安监控系统(不安装数据库);3:只安装数据库。
        private string strSetupContentIndex;

        public static string VirDirSchemaName = "IIsWebVirtualDir";

        private SqlConnection sqlConn;

        private SqlCommand sqlCmd;

        private String[] oValues = new String[2];

        private string MSQL = "";



        #endregion

        public installDB()
        {
            InitializeComponent();
        }

        #region Install  从这里开始启动安装
        public override void Install(IDictionary stateSaver)
        {
            try
            {
                strPhysicalDir = this.Context.Parameters["targetdir"].ToString().Trim();                //安装的物理路径                
                strDBPath = this.Context.Parameters["DBDIR"].ToString().Trim();                         //数据库安装路径
                strDBServerName = this.Context.Parameters["SERVERNAME"].ToString().Trim();              //数据库名称
                strDBName = "SortDB";                                                                //数据库服务器
                strDBUser = this.Context.Parameters["LOGINNAME"].ToString().Trim();                     //登录数据库用户
                strDBPwd = this.Context.Parameters["PASSWORD"].ToString().Trim();                       //登录数据库密码
                strSetupContentIndex = this.Context.Parameters["BTNSETUPCONTENT"].ToString().Trim();    //要安装的内容,1:安装系统程序和数据库;2:只安装系统程序(不安装数据库);3:只安装数据库
                if (string.IsNullOrEmpty(strDBPath.Trim()))
                {
                    MessageBox.Show("数据库安装到默认数据库路径下！" + strPhysicalDir, "安装提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("安装过程中出错!!!", "安装提示", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
        
            if (strSetupContentIndex == "1")
            {
                base.Install(stateSaver);

                //恢复数据库
                RestoreDB(strDBName);

                //执行所覆带的SQL文件
                ExecuteSQLFile();

            }
            if (strSetupContentIndex == "2")
            {
                base.Install(stateSaver);
            }

            if (strSetupContentIndex == "3")
            {
                //恢复数据库
                RestoreDB(strDBName);

                //执行所覆带的SQL文件
                ExecuteSQLFile();
            }
        }

        #endregion

        #region Uninstall 删除
        public override void Uninstall(IDictionary savedState)
        {
            if (savedState == null)
            {
                throw new ApplicationException("未能卸载！");
            }
            else
            {
                base.Uninstall(savedState);
            }
        }

        #endregion

        #region RestoreDB 从备份文件恢复数据库及数据库表
        /// 从备份文件恢复数据库及数据库表
        protected bool RestoreDB(string strDBName)
        {           
            bool Restult = false;
            string MSQL1 = "RESTORE FILELISTONLY FROM DISK = '" + strPhysicalDir + @"" + strDBName +".bak'";
            try
            {
                if (GetDbLogical(strDBName, MSQL1))
                {
                    ExecuteSql("master", "USE MASTER IF EXISTS (SELECT NAME FROM SYSDATABASES WHERE NAME='" + strDBName + "') DROP DATABASE " + strDBName);
                    ExecuteSql("master", MSQL);     //恢复backup的数据库
                }
                Restult = true;
            }
            catch(Exception excp)
            {
                MessageBox.Show("数据库恢复出错！" + excp.Message, "安装提示", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
            return Restult;
        }

        #endregion

        #region ExecuteSql 执行SQL语句
        private void ExecuteSql(string DataBaseName, string sqlstring)
        {       
            sqlCmd = new System.Data.SqlClient.SqlCommand(sqlstring, sqlConn);
            if (ConnectDatabase())
            {
                try
                {                 
                    sqlCmd.Connection.ChangeDatabase(DataBaseName);              
                    sqlCmd.ExecuteNonQuery();
                }
                finally
                {
                    sqlCmd.Connection.Close();
                }
            }
        }

        private bool GetDbLogical(string DataBaseName, string sqlstring)
        {           
            string sPathMdf = "";
            string sPathLdf = "";          
            if (sqlConn == null)
            {
                sqlConn = new SqlConnection();             
            }            
            sqlConn.ConnectionString = @"Data Source=" + strDBServerName + ";Initial Catalog=master;User ID=" + strDBUser + ";Password=" + strDBPwd;
           
            sqlConn.Open();

            try
            {
                ExecuteSql("master", "USE MASTER IF NOT EXISTS (SELECT NAME FROM SYSDATABASES WHERE NAME='" + strDBName + "') CREATE DATABASE [" + strDBName + "]");
            }
            catch (Exception e)
            {
                MessageBox.Show("数据库恢复出错!!!" + e.Message, "安装提示", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }

            SQLDMO.SQLServer srv = null;
            bool rs = false;           
            int index = 0;
            SqlDataReader oreader = null;         
            sqlCmd = new System.Data.SqlClient.SqlCommand(sqlstring, sqlConn);
            if (ConnectDatabase())
            {
                try
                {                    
                    //获取SqlServer默认安装路径
                    srv = new SQLDMO.SQLServerClass();
                    srv.Connect(strDBServerName, strDBUser, strDBPwd);
                    string SqlServerPath = Path.Combine(srv.Registry.SQLDataRoot, "DATA");             
                    if (string.IsNullOrEmpty(strDBPath.Trim()) || strDBPath.Equals("默认安装路径"))
                    {
                        sPathMdf = Path.Combine(SqlServerPath, DataBaseName + ".mdf");
                        sPathLdf = Path.Combine(SqlServerPath, DataBaseName + ".ldf");
                    }
                    else
                    {                    
                        if (!Directory.Exists(strDBPath))
                        {
                            try
                            {                             
                                Directory.CreateDirectory(strDBPath);
                            }
                            catch
                            {
                                MessageBox.Show("创建数据库安装目录失败！", "安装提示", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
                            }
                        }
                        sPathMdf = Path.Combine(strDBPath, DataBaseName + ".mdf");
                        sPathLdf = Path.Combine(strDBPath, DataBaseName + ".ldf");                     
                    }
                    sqlCmd.Connection.ChangeDatabase("master");
                    oreader = sqlCmd.ExecuteReader();
                    while (oreader.Read())
                    {
                        oValues[index] = oreader.GetString(0);
                        index++;
                    }                
                    string sMdf = oValues[0].ToString();
                    string sLdf = oValues[1].ToString();
                    MSQL = "RESTORE DATABASE " + DataBaseName +
                        " FROM DISK = '" + strPhysicalDir + @"" + strDBName + ".bak' " +
                        " WITH MOVE '" + sMdf + "' TO '" + sPathMdf + "', " +
                        " MOVE '" + sLdf + "' TO '" + sPathLdf + "'";

                    try
                    {
                        System.IO.File.Delete(sPathMdf);
                        System.IO.File.Delete(sPathLdf);
                    }
                    catch (Exception)
                    {
                    }

                }
                catch(Exception excp)
                {
                    MessageBox.Show("数据库安装文件有错误！"+excp.Message, "安装提示", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
                }
                finally
                {
                    oreader.Close();
                    if (oreader != null)
                    {
                        oreader.Dispose();
                    }
                    sqlCmd.Connection.Close();
                    if (sqlCmd != null)
                    {
                        sqlCmd.Dispose();
                    }
                    srv.DisConnect();
                }
                rs = true;
            }
            return rs;
        }
        #endregion

        #region ConnectDatabase 连接数据库
        private bool ConnectDatabase()
        {           
            if (sqlCmd.Connection.State != ConnectionState.Open)
            {              
                try
                {
                    sqlCmd.Connection.Open();
                }
                catch(Exception excp)
                {
                    MessageBox.Show("无法连接数据库！"+excp.Message, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 执行SQL文件
        private void ExecuteSQLFile()
        {           
            try
            {
                System.IO.FileInfo FileInfo = new System.IO.FileInfo(strPhysicalDir + "initialization.sql");
                if (!FileInfo.Exists)
                    return;
                Process.Start("osql.exe", "-E -S " + strDBServerName + " -d master -i " + strPhysicalDir + "initialization.sql");
            }
            catch(Exception excp)
            {
                MessageBox.Show("执行SQL文件失败,请安装完毕后手动执行！"+excp.Message, "安装提示", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
        }
        #endregion

    }
}