using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using THOK.ParamUtil;

namespace THOK.AS.Sorting
{
    public class Parameter: BaseObject
    {
        private string lineCode;
        [CategoryAttribute("ϵͳ����"), DescriptionAttribute("���ּ��ߴ���"), Chinese("�ּ��ߴ���")]
        public string LineCode
        {
            get { return lineCode; }
            set { lineCode = value; }
        }

        private string serverName;

        [CategoryAttribute("�������ݿ����Ӳ���"), DescriptionAttribute("���ݿ����������"), Chinese("����������")]
        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

        private string dbName;

        [CategoryAttribute("�������ݿ����Ӳ���"), DescriptionAttribute("���ݿ�����"), Chinese("���ݿ���")]
        public string DBName
        {
            get { return dbName; }
            set { dbName = value; }
        }

        private string dbUser;

        [CategoryAttribute("�������ݿ����Ӳ���"), DescriptionAttribute("���ݿ������û���"), Chinese("�û���")]
        public string DBUser
        {
            get { return dbUser; }
            set { dbUser = value; }
        }
        private string password;

        [CategoryAttribute("�������ݿ����Ӳ���"), DescriptionAttribute("���ݿ���������"), Chinese("����")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string remoteServerName;

        [CategoryAttribute("���������ݿ����Ӳ���"), DescriptionAttribute("���ݿ����������"), Chinese("����������")]
        public string RemoteServerName
        {
            get { return remoteServerName; }
            set { remoteServerName = value; }
        }

        private string remoteDBName;

        [CategoryAttribute("���������ݿ����Ӳ���"), DescriptionAttribute("���ݿ�����"), Chinese("���ݿ���")]
        public string RemoteDBName
        {
            get { return remoteDBName; }
            set { remoteDBName = value; }
        }

        private string remoteDBUser;

        [CategoryAttribute("���������ݿ����Ӳ���"), DescriptionAttribute("���ݿ������û���"), Chinese("�û���")]
        public string RemoteDBUser
        {
            get { return remoteDBUser; }
            set { remoteDBUser = value; }
        }
        private string remotePassword;

        [CategoryAttribute("���������ݿ����Ӳ���"), DescriptionAttribute("���ݿ���������"), Chinese("����")]
        public string RemotePassword
        {
            get { return remotePassword; }
            set { remotePassword = value; }
        }

        private string exportPort;

        [CategoryAttribute("�����ն�ͨ�Ų���"), DescriptionAttribute("�����ն˼����˿�"), Chinese("�����˿�")]
        public string ExportPort
        {
            get { return exportPort; }
            set { exportPort = value; }
        }

        private string exportIP;

        [CategoryAttribute("�����ն�ͨ�Ų���"), DescriptionAttribute("�����ն�IP��ַ"), Chinese("IP��ַ")]
        public string ExportIP
        {
            get { return exportIP; }
            set { exportIP = value; }
        }

        private string supplyIP;
        [CategoryAttribute("����ϵͳͨ�Ų���"), DescriptionAttribute("����ϵͳIP��ַ"), Chinese("IP��ַ")]
        public string SupplyIP
        {
            get { return supplyIP; }
            set { supplyIP = value; }
        }

        private string supplyPort;
        [CategoryAttribute("����ϵͳͨ�Ų���"), DescriptionAttribute("����ϵͳ�����˿�"), Chinese("�����˿�")]
        public string SupplyPort
        {
            get { return supplyPort; }
            set { supplyPort = value; }
        }

        private string sortLedIP;
        [CategoryAttribute("�ּ𳵼����ϵͳͨ�Ų���"), DescriptionAttribute("�ּ𳵼����ϵͳIP��ַ"), Chinese("IP��ַ")]
        public string SortLedIP
        {
            get { return sortLedIP; }
            set { sortLedIP = value; }
        }

        private string sortLedPort;
        [CategoryAttribute("�ּ𳵼����ϵͳͨ�Ų���"), DescriptionAttribute("�ּ𳵼����ϵͳ�����˿�"), Chinese("�����˿�")]
        public string SortLedPort
        {
            get { return sortLedPort; }
            set { sortLedPort = value; }
        }

        private string portName;

        [CategoryAttribute("��״��ͨ�Ų���"), DescriptionAttribute("��״�����ں�"), Chinese("���ں�")]
        public string PortName
        {
            get { return portName; }
            set { portName = value; }
        }

        private string baudRate;

        [CategoryAttribute("��״��ͨ�Ų���"), DescriptionAttribute("��״��������"), Chinese("������")]
        public string BaudRate
        {
            get { return baudRate; }
            set { baudRate = value; }
        }

        private string parity;

        [CategoryAttribute("��״��ͨ�Ų���"), DescriptionAttribute("��״������λ"), Chinese("����λ")]
        public string Parity
        {
            get { return parity; }
            set { parity = value; }
        }

        private string dataBits;

        [CategoryAttribute("��״��ͨ�Ų���"), DescriptionAttribute("��״������λ"), Chinese("����λ")]
        public string DataBits
        {
            get { return dataBits; }
            set { dataBits = value; }
        }

        private string stopBits;

        [CategoryAttribute("��״��ͨ�Ų���"), DescriptionAttribute("��״��ֹͣλ"), Chinese("ֹͣλ")]
        public string StopBits
        {
            get { return stopBits; }
            set { stopBits = value; }
        }
    }
}
