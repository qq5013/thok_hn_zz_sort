using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.Odd.View
{
    public partial class CigaretteForm : THOK.AF.View.ToolbarForm
    {
        private DataTable cigaretteTable = null;
        private THOK.Odd.Dal.CigaretteDal cigaretteDal = new THOK.Odd.Dal.CigaretteDal();

        public CigaretteForm()
        {
            InitializeComponent();
            Util.EnableFilter(dgvMain);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                cigaretteTable = cigaretteDal.GetCigarette();
                bsMain.DataSource = cigaretteTable;

            }
            catch (Exception exp)
            {
                MessageBox.Show("读入卷烟信息失败，原因：" + exp.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                cigaretteDal.SaveCigarette(cigaretteTable);
                Util.ShowInfo("保存卷烟信息成功");

            }
            catch (Exception exp)
            {
                MessageBox.Show("保存卷烟信息失败，原因：" + exp.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }
    }
}

