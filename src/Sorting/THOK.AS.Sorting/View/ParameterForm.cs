using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.MCP.Config;
using THOK.Util;
using THOK.ParamUtil;

namespace THOK.AS.Sorting.View
{
    public partial class ParameterForm : THOK.AF.View.ToolbarForm
    {
        private Parameter parameter = new Parameter();
        private DBConfigUtil config = new DBConfigUtil("DefaultConnection", "SQLSERVER");
        private DBConfigUtil serverConfig = new DBConfigUtil("ServerConnection", "SQLSERVER");
        private THOK.MCP.Service.Package.Config.Configuration packageConfig = new THOK.MCP.Service.Package.Config.Configuration("PackPLC.xml");
        private Dictionary<string, string> attributes = null;

        public ParameterForm()
        {
            InitializeComponent();
            ReadParameter();
        }

        private void ReadParameter()
        {
            //�������ݿ����Ӳ���
            parameter.ServerName = config.Parameters["server"].ToString();
            parameter.DBName = config.Parameters["database"].ToString();
            parameter.DBUser = config.Parameters["uid"].ToString();
            parameter.Password = config.Parameters["password"].ToString();

            //���������ݿ����Ӳ���
            parameter.RemoteServerName = serverConfig.Parameters["server"].ToString();
            parameter.RemoteDBName = serverConfig.Parameters["database"].ToString();
            parameter.RemoteDBUser = serverConfig.Parameters["uid"].ToString();
            parameter.RemotePassword = serverConfig.Parameters["password"].ToString();

            //��ȡContext�����ļ�����
            ConfigUtil configUtil = new ConfigUtil();
            attributes = configUtil.GetAttribute();
            parameter.ExportIP = attributes["ExportIP"];
            parameter.ExportPort = attributes["ExportPort"];
            parameter.LineCode = attributes["LineCode"];
            parameter.SupplyIP = attributes["SupplyIP"];
            parameter.SupplyPort = attributes["SupplyPort"];
            parameter.SortLedIP = attributes["SortLedIP"];
            parameter.SortLedPort = attributes["SortLedPort"];

            //��ȡ��װ������
            parameter.PortName = packageConfig.Port.ToString();
            parameter.BaudRate = packageConfig.BaudRate.ToString();
            parameter.DataBits = packageConfig.DataBits.ToString();
            parameter.Parity = packageConfig.Parity.ToString();
            parameter.StopBits = packageConfig.StopBits.ToString();   

            propertyGrid.SelectedObject = parameter;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //���汾�����ݿ����Ӳ���
                config.Parameters["server"] = parameter.ServerName;
                config.Parameters["database"] = parameter.DBName;
                config.Parameters["uid"] = parameter.DBUser;
                config.Parameters["Password"] = config.Parameters["Password"].ToString() == parameter.Password ? parameter.Password : THOK.Util.Coding.Encoding(parameter.Password);
                config.Save();

                //������������ݿ����Ӳ���
                serverConfig.Parameters["server"] = parameter.RemoteServerName;
                serverConfig.Parameters["database"] = parameter.RemoteDBName;
                serverConfig.Parameters["uid"] = parameter.RemoteDBUser;
                serverConfig.Parameters["Password"] = serverConfig.Parameters["Password"].ToString() == parameter.RemotePassword ? parameter.RemotePassword : THOK.Util.Coding.Encoding(parameter.RemotePassword);
                serverConfig.Save();   

                //����Context����
                attributes["ExportIP"] = parameter.ExportIP;
                attributes["ExportPort"] = parameter.ExportPort;
                attributes["LineCode"] = parameter.LineCode;
                attributes["SupplyIP"] = parameter.SupplyIP;
                attributes["SupplyPort"] = parameter.SupplyPort;
                attributes["SortLedIP"] = parameter.SortLedIP;
                attributes["SortLedPort"] = parameter.SortLedPort;
                ConfigUtil configUtil = new ConfigUtil();
                configUtil.Save(attributes);

                //�����״������
                packageConfig.Port = parameter.PortName;
                packageConfig.BaudRate = Convert.ToInt32(parameter.BaudRate);
                packageConfig.DataBits = Convert.ToInt32(parameter.DataBits) > 8 || Convert.ToInt32(parameter.DataBits) < 5 ? 8 : Convert.ToInt32(parameter.DataBits);
                packageConfig.Parity = (System.IO.Ports.Parity)Enum.Parse(typeof(System.IO.Ports.Parity), parameter.Parity);
                packageConfig.StopBits = (System.IO.Ports.StopBits)Enum.Parse(typeof(System.IO.Ports.StopBits), parameter.StopBits);
                packageConfig.Save();



                MessageBox.Show("ϵͳ��������ɹ���������������ϵͳ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exp)
            {
                MessageBox.Show("����ϵͳ���������г����쳣��ԭ��" + exp.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }
    }
}

