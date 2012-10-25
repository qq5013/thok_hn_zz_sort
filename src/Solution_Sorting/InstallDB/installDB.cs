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

        #region ����˽�г�Ա����Ӧ��װ��������յ����û�����

        private string strDBName;       //Ҫ�����ݿ������,Ĭ��ΪLogistics

        private string strDBServerName; //���ݿ����������

        private string strDBUser;       //���ݿ��¼��

        private string strDBPwd;        //���ݿ�����

        private string strPhysicalDir;  //��װ������·��

        private string strDBPath;       //�����ݿⴴ���ںδ�

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

        #region Install  �����￪ʼ������װ
        public override void Install(IDictionary stateSaver)
        {
            try
            {
                strPhysicalDir = this.Context.Parameters["targetdir"].ToString().Trim();                //��װ������·��                
                strDBPath = this.Context.Parameters["DBDIR"].ToString().Trim();                         //���ݿⰲװ·��
                strDBServerName = this.Context.Parameters["SERVERNAME"].ToString().Trim();              //���ݿ�����
                strDBName = "Logistics";                                                                //���ݿ������
                strDBUser = this.Context.Parameters["LOGINNAME"].ToString().Trim();                     //��¼���ݿ��û�
                strDBPwd = this.Context.Parameters["PASSWORD"].ToString().Trim();                       //��¼���ݿ�����
                strSetupContentIndex = this.Context.Parameters["BTNSETUPCONTENT"].ToString().Trim();    //Ҫ��װ������,1:��װ��Ϣϵͳ�����ݿ�;2:ֻ��װ��Ϣϵͳ(����װ���ݿ�);3:ֻ��װ���ݿ�
                if (string.IsNullOrEmpty(strDBPath.Trim()))
                {
                    MessageBox.Show("���ݿⰲװ��Ĭ�����ݿ�·���£�" + strPhysicalDir, "��װ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("��װ�����г���!", "��װ��ʾ", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
        
            if (strSetupContentIndex == "1")
            {
                base.Install(stateSaver);

                //�ָ����ݿ�
                RestoreDB(strDBName);
                //ִ����������SQL�ļ�
                ExecuteSQLFile();
                //ע��DOTNET��IIS
                RegistDotNetIIS();

            }
            if (strSetupContentIndex == "2")
            {
                base.Install(stateSaver);
                //ע��DOTNET��IIS
                RegistDotNetIIS();
            }

            if (strSetupContentIndex == "3")
            {
                //�ָ����ݿ�
                RestoreDB(strDBName);
                //ִ����������SQL�ļ�
                ExecuteSQLFile();
            }
            AddFavorites();
        }

        #endregion

        #region Uninstall ɾ��
        public override void Uninstall(IDictionary savedState)
        {
            if (savedState == null)
            {
                throw new ApplicationException("δ��ж�أ�");
            }
            else
            {
                base.Uninstall(savedState);
            }
        }

        #endregion

        #region RestoreDB �ӱ����ļ��ָ����ݿ⼰���ݿ��
        /// �ӱ����ļ��ָ����ݿ⼰���ݿ��
        protected bool RestoreDB(string strDBName)
        {           
            bool Restult = false;
            string MSQL1 = "RESTORE FILELISTONLY FROM DISK = '" + strPhysicalDir + @"" + strDBName + ".bak'";
            try
            {
                if (GetDbLogical(strDBName, MSQL1))
                {
                    ExecuteSql("master", "USE MASTER IF EXISTS (SELECT NAME FROM SYSDATABASES WHERE NAME='" + strDBName + "') DROP DATABASE " + strDBName);
                    ExecuteSql("master", MSQL);     //�ָ�backup�����ݿ�
                }
                Restult = true;
            }
            catch(Exception excp)
            {
                MessageBox.Show("���ݿ�ָ�����" + excp.Message, "��װ��ʾ", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
            return Restult;
        }

        #endregion

        #region ExecuteSql ִ��SQL���
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
                MessageBox.Show("���ݿ�ָ�����!!!" + e.Message, "��װ��ʾ", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
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
                    //��ȡSqlServerĬ�ϰ�װ·��
                    srv = new SQLDMO.SQLServerClass();
                    srv.Connect(strDBServerName, strDBUser, strDBPwd);
                    string SqlServerPath = Path.Combine(srv.Registry.SQLDataRoot, "DATA");             
                    if (string.IsNullOrEmpty(strDBPath.Trim()) || strDBPath.Equals("Ĭ�ϰ�װ·��"))
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
                                MessageBox.Show("�������ݿⰲװĿ¼ʧ�ܣ�", "��װ��ʾ", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
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
                    MessageBox.Show("���ݿⰲװ�ļ��д���"+excp.Message, "��װ��ʾ", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
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

        #region ConnectDatabase �������ݿ�
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
                    MessageBox.Show("�޷��������ݿ⣡"+excp.Message, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region ע��DOTNET��IIS
        private void RegistDotNetIIS()
        {
            try
            {
                Process.Start(@"C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\aspnet_regiis.exe", "-i");
                Process.Start(@"" + strPhysicalDir + "setup_DSDRIVER.exe");
            }
            catch
            { 
                MessageBox.Show("ע��.NET��IISʧ��,�밲װ��Ϻ��ֶ�ע�ᣡ","��װ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,0);
            }                    
        }
        #endregion

        #region ִ��SQL�ļ�
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
                MessageBox.Show("ִ��SQL�ļ�ʧ��,�밲װ��Ϻ��ֶ�ִ�У�"+excp.Message, "��װ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
        }
        #endregion

        #region ����ҳ��ӵ��ղؼк�����������ղؼ�
        private void AddFavorites()
        {
            try
            {
                System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Favorites) + @"\���̷ּ���Ϣ����ϵͳ.url");
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Favorites);
                IWshShell_ClassClass iwshshell = new IWshShell_ClassClass();
                IWshURLShortcut shortcut = null;
                shortcut = iwshshell.CreateShortcut(path + @"\���̷ּ���Ϣ����ϵͳ.url") as IWshURLShortcut;
                shortcut.TargetPath = @"http://localhost/SortingSystem/";
                shortcut.Save();


                System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\���̷ּ���Ϣ����ϵͳ.url");
                path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                iwshshell = new IWshShell_ClassClass();
                shortcut = null;
                shortcut = iwshshell.CreateShortcut(path + @"\���̷ּ���Ϣ����ϵͳ.url") as IWshURLShortcut;
                shortcut.TargetPath = @"http://localhost/SortingSystem/";
                shortcut.Save();

            }
            catch (Exception)
            {

            }
        }
        #endregion
    }
}