using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.Util;
using THOK.MCP.Config;

namespace THOK.AS.OTS.View
{
    public partial class ParameterForm : Form
    {
        private Parameter parameter = new Parameter();
        private DBConfigUtil config = new DBConfigUtil("DefaultConnection", "SQLSERVER");
        private ConfigUtil configUtil = new ConfigUtil();
        private THOK.MCP.Service.UDP.Config.Configuration udpConfig = new THOK.MCP.Service.UDP.Config.Configuration("UDP.xml");

        Dictionary<string, string> sysParam = null;

        public ParameterForm()
        {
            InitializeComponent();
            ReadParameter();
        }

        private void ReadParameter()
        {
            parameter.ServerName = config.Parameters["server"].ToString();
            parameter.DBName = config.Parameters["database"].ToString();
            parameter.DBUser = config.Parameters["uid"].ToString();
            parameter.Password = config.Parameters["password"].ToString();

            sysParam = configUtil.GetAttribute();
            parameter.PrintLabel = Convert.ToBoolean(sysParam["PrintLabel"]);

            parameter.IP = udpConfig.IP;
            parameter.Port = udpConfig.Port;

            propertyGrid.SelectedObject = parameter;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                config.Parameters["server"] = parameter.ServerName;
                config.Parameters["database"] = parameter.DBName;
                config.Parameters["uid"] = parameter.DBUser;
                config.Parameters["password"] = config.Parameters["Password"].ToString() == parameter.Password ? parameter.Password : THOK.Util.Coding.Encoding(parameter.Password);
                config.Save();

                sysParam["PrintLabel"] = parameter.PrintLabel.ToString();
                configUtil.Save(sysParam);

                udpConfig.IP = parameter.IP;
                udpConfig.Port = parameter.Port;
                udpConfig.Save();
                MessageBox.Show("系统参数保存成功，请重新启动本系统。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exp)
            {
                MessageBox.Show("保存系统参数过程中出现异常，原因：" + exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}